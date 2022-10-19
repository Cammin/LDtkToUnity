using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkEntityIconFactory
    {
        private readonly EntityDefinition _data;
        private readonly LDtkProjectImporter _importer;
        
        public LDtkEntityIconFactory(EntityDefinition def, LDtkProjectImporter importer)
        {
            _data = def;
            _importer = importer;
        }

        public Texture2D GetIcon() //todo get image icons in eventually, partially finished. probably add in once we can find out about setting this up during onenable
        {
            Profiler.BeginSample("GetShapeForRenderMode");
            Texture2D srcBackground = GetShapeForRenderMode();
            Profiler.EndSample();
            
            if (srcBackground == null)
            {
                LDtkDebug.LogError("Did not get source icon for entity icon");
                return null;
            }
            
            Profiler.BeginSample("CopySrcBackground");
            Texture2D tex = srcBackground.Copy();
            Profiler.EndSample();

            Profiler.BeginSample("TintTexture");
            TintTexture(tex, _data.UnityColor);
            Profiler.EndSample();
            
            
            //Profiler.BeginSample("ResizeStretch");
            //LDtkTextureUtil.ResizeStretch(tex, 12, 12);
            //Profiler.EndSample();
            
            //Profiler.BeginSample("ReframeTexture");
            //ReframeTexture(tex);
            //Profiler.EndSample();
            

            
            
            //Profiler.BeginSample("ApplyOverlay");
            //ApplyOverlay(tex);
            //Profiler.EndSample();
            
            
            //Profiler.BeginSample("SquarifyTexture");
            SquarifyTexture(tex);
            //Profiler.EndSample();
            
            Profiler.BeginSample("Apply");
            tex.filterMode = FilterMode.Point;
            tex.Apply(true);
            Profiler.EndSample();
            
            return tex;
        }

        private void ApplyOverlay(Texture2D tex)
        {
            Profiler.BeginSample("CreateSourceOverlayTexture");
            Texture2D overlayTex = CreateSourceOverlayTexture(); //
            Profiler.EndSample();
            if (!overlayTex)
            {
                //Debug.Log($"not overaly for {_data.Identifier}");
                return;
            }

            //Debug.Log("overlay");
            
            Profiler.BeginSample("ModifyOverlayForRenderType");
            ModifyOverlayForRenderType(tex, overlayTex);
            Profiler.EndSample();

            Profiler.BeginSample("ApplyOverlay");
            AddOverlayToBackground(tex, overlayTex);
            Profiler.EndSample();
        }

        private void TintTexture(Texture2D tex, Color color)
        {
            Color[] pixels = LDtkTextureUtility.TintPixels(tex, color);
            tex.SetPixels(pixels);
        }

        private Texture2D CreateSourceOverlayTexture()
        {
            TilesetRectangle mine = _data.TileRect;


            TilesetRectangle tile = mine;//new TilesetRectangle();
            //HACK test: (x:16, y:128, width:16, height:16)
            //tile.X = 16;
            //tile.Y = 128;
            //tile.W = 16;
            //tile.H = 16;
            //tile.TilesetUid = 60;
            

            if (tile == null)
            {
                //none defined
                //Debug.Log($"no tile for {_data.Identifier}");
                return null;
            }
            
            //Debug.Log(tile.TilesetUid);
            //Debug.Log(tile.UnityRect);
            

            TilesetDefinition tileset = tile.Tileset;
            if (tileset == null)
            {
                LDtkDebug.LogError("Issue getting tileset definition data");
                return null;
            }

            LDtkRelativeGetterTilesetTexture getter = new LDtkRelativeGetterTilesetTexture();
            Texture2D srcOverlay = getter.GetRelativeAsset(tileset, _importer.assetPath);
            if (srcOverlay == null)
            {
                return null;
            }

            //todo this is done for later
            //LDtkTextureSpriteSlicer slicer = new LDtkTextureSpriteSlicer(srcOverlay, tile.UnityRect, _importer.PixelsPerUnit);
            //RectInt slice = slicer.ImageSlice.ToRectInt();

            Profiler.BeginSample("CreateSlice");
            //Texture2D assetPreview = LDtkTextureUtility.CreateSlice(srcOverlay, slice);
            Profiler.EndSample();
            //Texture2D assetPreview = AssetPreview.GetAssetPreview(sprite);
            //Texture2D tex = sprite.ToTexture2D();

            //return assetPreview;
            return null;
        }

        public void AddOverlayToBackground(Texture2D background, Texture2D overlay)
        {
            //Debug.Log(background.width);
            //Debug.Log(background.height);
            //Debug.Log(overlay.width);
            //Debug.Log(overlay.height);
            
            int startY = background.height - overlay.height;
            
            for (int x = 0; x < overlay.width; x++)
            {
                for (int y = 0; y < overlay.height; y++)
                {
                    int yCoordToBg = startY + y;

                    Color bgColor = background.GetPixel(x, yCoordToBg);
                    Color overlayColor = overlay.GetPixel(x, y);

                    Color finalColor = Color.Lerp(bgColor, overlayColor, overlayColor.a);
                    background.SetPixel(x, yCoordToBg, finalColor);
                }
            }
        }
        
        private Texture2D GetShapeForRenderMode()
        {
            Texture2D icon = null;
            switch (_data.RenderMode)
            {
                case RenderMode.Cross:
                    icon = LDtkIconUtility.LoadCrossIcon();
                    break;
                case RenderMode.Ellipse:
                    icon = LDtkIconUtility.LoadCircleIcon();
                    break;
                case RenderMode.Rectangle:
                case RenderMode.Tile:
                    icon = LDtkIconUtility.LoadSquareIcon();
                    break;
            }
            return icon;
        }
        
        private void SquarifyTexture(Texture2D tex)
        {
            Vector2Int newSize = GetSquareTexSize(tex);
            Vector2Int srcSize = new Vector2Int(tex.width, tex.height);
            
            Color[] clearColors = GetClearPixels(newSize.x * newSize.y);
            Color[] pixels = tex.GetPixels();
            
            LDtkTextureUtility.Resize(tex, newSize.x, newSize.y);
            tex.SetPixels(clearColors);
                
            Vector2Int padding = (newSize - srcSize) / 2;
            tex.SetPixels(padding.x, padding.y, srcSize.x, srcSize.y, pixels);
        }
        
        private void ReframeTexture(Texture2D tex)
        {
            Vector2Int newSize = GetSquareTexSize(tex);
            LDtkTextureUtility.ResizeStretch(tex, newSize.x, newSize.y);
        }

        private static Color[] GetClearPixels(int pixelAmount)
        {
            Color[] clearColors = new Color[pixelAmount];
            for (int i = 0; i < clearColors.Length; i++)
            {
                clearColors[i] = Color.clear;
            }

            return clearColors;
        }

        private Vector2Int GetSquareTexSize(Texture2D tex)
        {
            int finalX = tex.width;
            int finalY = tex.height;

            //expand width
            if (_data.Height > _data.Width)
            {
                float scaleUp = (float)_data.Height / _data.Width;
                finalX = Mathf.FloorToInt(finalX * scaleUp);
            }
            
            //expand height
            else if (_data.Height < _data.Width)
            {
                float scaleUp = _data.Width / (float)_data.Height;
                finalY = Mathf.FloorToInt(finalY * scaleUp);
            }

            return new Vector2Int(finalX, finalY);
        }

        private void ModifyOverlayForRenderType(Texture2D background, Texture2D overlay)
        {
            switch (_data.TileRenderMode)
            {
                case TileRenderMode.FitInside:
                case TileRenderMode.Repeat:
                case TileRenderMode.Cover: 
                    //live in the top left, scaling both axis to the background's smallest axis
                    int min = Mathf.Min(background.width, background.height);
                    LDtkTextureUtility.ResizeStretch(overlay, min, min);
                    break;

                case TileRenderMode.Stretch:
                    //stretch the image so that it's the same resolution as the background
                    LDtkTextureUtility.ResizeStretch(overlay, background.width, background.height);
                    break;
                
                
                case TileRenderMode.FullSizeCropped: 
                case TileRenderMode.FullSizeUncropped:
                    //draw the image overlay, but only the first 24x24 pixels of the overlay, then stretched to fit the background

                    for (int x = 0; x < Mathf.Min(overlay.width, background.width); x++)
                    {
                        for (int y = 0; y < Mathf.Min(overlay.height, background.height); y++)
                        {
                            //todo setup some handling here
                        }
                    }
                    
                    LDtkTextureUtility.ResizeStretch(overlay, 24, 24);
                    break;
                

            }
        }
        
        
    }
}