using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    internal sealed partial class LDtkTilesetImporter :
        ISpriteEditorDataProvider, 
        ITextureDataProvider, 
        ISpritePhysicsOutlineDataProvider, 
        ISecondaryTextureDataProvider
    {
        SpriteImportMode ISpriteEditorDataProvider.spriteImportMode => SpriteImportMode.Multiple;
        float ISpriteEditorDataProvider.pixelsPerUnit => _pixelsPerUnit;
        Object ISpriteEditorDataProvider.targetObject => this;

        /// <summary>
        /// Called when the sprite editor window wants to initially collect rects to show the slices in the window.
        /// </summary>
        /// <returns></returns>
        SpriteRect[] ISpriteEditorDataProvider.GetSpriteRects()
        {
            return _sprites.Select(x => new LDtkSpriteRect(x) as SpriteRect).ToArray();
        }

        /// <summary>
        /// Called when we hit apply in the sprite editor window. these new sprites will be what was in the sprite editor window at the time of applying.
        /// This process is separate from the actual importer process.
        /// We'd select "apply", and that attempts to populate new sprites through this function;
        /// For our tileset importer needs, we do not want to add/remove any tiles. if any new ones are made from the sprite editor window, we IGNORE them.
        /// Only the importer will have agency over what's added/removed.
        /// With that said, we should have complete freedom over modifying these sprite's pivot, and physics shape.  
        /// </summary>
        void ISpriteEditorDataProvider.SetSpriteRects(SpriteRect[] spritesToWrite)
        {
            //when we apply new data. we should never want to add any new stuff
            
            //remove those that have become null or otherwise deleted and/or irrelevant
            //_sprites.RemoveAll(data => newSprites.FirstOrDefault(x => x.spriteID == data.spriteID) == null);

            Profiler.BeginSample("ToDictionary");
            var dict = _sprites.ToDictionary(x => x.spriteID, x => x);
            Profiler.EndSample();

            foreach (SpriteRect writeSprite in spritesToWrite)
            {
                if (!dict.TryGetValue(writeSprite.spriteID, out LDtkSpriteRect metaSprite))
                {
                    //if the new sprite is foreign to our current list of tiles (aka,newly made from the sprite editor window, 
                    continue;
                }
                
                //overwrite just these
                metaSprite.alignment = writeSprite.alignment;
                metaSprite.border = writeSprite.border;
                metaSprite.pivot = writeSprite.pivot;
            }
        }


        #region ReferenceFromAseprite

        /// <summary>
        /// Compared 
        /// </summary>
        /// <param name="newRects"></param>
        /// <param name="oldMetaRects"></param>
        /// <returns></returns>
        private static List<LDtkSpriteRect> UpdateLayers(in List<LDtkSpriteRect> newRects, in List<LDtkSpriteRect> oldMetaRects)
        {
            if (oldMetaRects.Count == 0)
            {
                return new List<LDtkSpriteRect>(newRects);
            }

            var finalLayers = new List<LDtkSpriteRect>(oldMetaRects);
            
            // Remove old layers
            foreach (LDtkSpriteRect oldLayer in oldMetaRects)
            {
                if (!newRects.Exists(x => x.name == oldLayer.name))
                {
                    finalLayers.Remove(oldLayer);
                }
            }
            
            // Add new layers
            foreach (LDtkSpriteRect newLayer in newRects)
            {
                if (!finalLayers.Exists(x => x.name == newLayer.name))
                {
                    finalLayers.Add(newLayer);
                }
            }
            
            // Retain guids for any existing meta fiels
            foreach (LDtkSpriteRect finalLayer in finalLayers)
            {
                var i = newRects.FindIndex(x => x.name == finalLayer.name);
                if (i == -1)
                {
                    continue;
                }

                newRects[i].spriteID = finalLayer.spriteID;
            }

            return finalLayers;
        }
        
        /// <summary>
        /// The main rewrite of the meta data, NO additional sprites.
        ///
        /// the sprite editor only modifies existing tiles.
        ///
        /// In here, out goal is to have every tile that exists.
        ///
        ///
        /// If the metadata had tiles that the importer no longer makes, remove them. 
        /// If the importer made some tiles that weren't metadata yet, make them.
        /// don't make any modifications from inside here. the user can configure what they want later.
        /// 
        /// </summary>
        /// <param name="srcRects">The rectangles from the deserialized file. they should always overwrite the rects that we had at hand</param>
        /// <returns></returns>
        private bool ReformatRectMetaData(List<TilesetRectangle> srcRects)
        {
            bool changed = false;

            int jsonTileCount = srcRects.Count;

            // trim metas off the end of the list to match the new src count.
            // LDtk handles this in the exact same way where if the tile count decreased, then any old tiles are complete
            if (_sprites.Count > jsonTileCount)
            {
                _sprites.RemoveRange(jsonTileCount, _sprites.Count - jsonTileCount);
                changed = true;
            }

            //add new blank ones to the end of the sprites list with new src rects 
            if (_sprites.Count < jsonTileCount)
            {
                for (int tileId = _sprites.Count; tileId < jsonTileCount; tileId++)
                {
                    LDtkSpriteRect newRect = new LDtkSpriteRect
                    {
                        border = Vector4.zero,
                        pivot = new Vector2(0.5f, 0.5f),
                        alignment = SpriteAlignment.Center,
                        rect = srcRects[tileId].UnityRect,
                        spriteID = GUID.Generate(),
                        name = $"{_definition.Def.Identifier}_{tileId.ToString()}",
                    };
                    _sprites.Add(newRect);
                }
                changed = true;
            }
            
            Debug.Assert(_sprites.Count == jsonTileCount, "Sprite counts were not equal!");

            //force rects to what they should really be.
            for (int i = 0; i < _sprites.Count; i++)
            {
                _sprites[i].rect = srcRects[i].UnityRect;
            }

            return changed;
        }


        #endregion
        
        //GOAL: Find a way to get tiles in an optimized manner. should we do this by looking for a tile via it's id? it's a lot better than checking via a string.
        //But at the same time, tiles could be abnormally shaped, and the only way to get those would be through giving then an id ourselves, or index via a rectint.  
        
        /// <summary>
        /// This may be used in the actual sprite generation step instead of here.
        /// IMPORTANT NOTE: The list is ordered like the tile order in ldtk from top left in rows.
        /// </summary>
        private List<TilesetRectangle> ReadSourceRectsFromJsonDefinition(TilesetDefinition def)
        {
            List<TilesetRectangle> rects = new List<TilesetRectangle>();
            //Debug.Log($"The tileset {def.Identifier} uses {usedTiles.Count} unique tiles");
            //int id = -1;
            int padding = def.Padding;
            int gridSize = def.TileGridSize;
            int spacing = def.Spacing;
            
            for (int y = 0; y < def.CHei; y++)
            {
                for (int x = 0; x < def.CWid; x++)
                {
                    //id++;

                    int pxX = (x * (gridSize+spacing)) + padding;
                    //Debug.Log(pxX);
                    int pxY = (y * (gridSize+spacing)) + padding;
                    int wid = Mathf.Min(gridSize, def.PxWid - pxX - padding);
                    int hei = Mathf.Min(gridSize, def.PxHei - pxY - padding);
                    
                    TilesetRectangle slice = new TilesetRectangle()
                    {
                        X = pxX,
                        Y = LDtkCoordConverter.ImageSliceY(pxY, hei, def.PxHei),
                        W = wid,
                        H = hei,
                        TilesetUid = def.Uid,
                    };
                    rects.Add(slice);
                }
            }
            return rects;
        }

        #region ISpriteEditorDataProvider
        void ISpriteEditorDataProvider.Apply()
        {
            // Do this so that asset change save dialog will not show
            var originalValue = EditorPrefs.GetBool("VerifySavingAssets", false);
            EditorPrefs.SetBool("VerifySavingAssets", false);
            AssetDatabase.ForceReserializeAssets(new string[] { assetPath }, ForceReserializeAssetsOptions.ReserializeMetadata);
            EditorPrefs.SetBool("VerifySavingAssets", originalValue);
        }

        void ISpriteEditorDataProvider.InitSpriteEditorDataProvider() 
        { }

        T ISpriteEditorDataProvider.GetDataProvider<T>() => 
            this as T;

        bool ISpriteEditorDataProvider.HasDataProvider(Type type) => 
            type.IsAssignableFrom(GetType());
        
        #endregion

        #region ITextureDataProvider
        void ITextureDataProvider.GetTextureActualWidthAndHeight(out int width, out int height)
        {
            Texture2D tex = LoadTex();
            width = tex != null ? tex.width : 0;
            height = tex != null ? tex.height : 0;
        }

        Texture2D ITextureDataProvider.GetReadableTexture2D() => LoadTex();
        Texture2D ITextureDataProvider.texture => LoadTex();
        Texture2D ITextureDataProvider.previewTexture => LoadTex();
        
        #endregion

        #region ISpritePhysicsOutlineDataProvider

        List<Vector2[]> ISpritePhysicsOutlineDataProvider.GetOutlines(GUID guid) =>
            GetSpriteData(guid).GetOutlines();

        void ISpritePhysicsOutlineDataProvider.SetOutlines(GUID guid, List<Vector2[]> data) =>
            GetSpriteData(guid).SetOutlines(data);

        float ISpritePhysicsOutlineDataProvider.GetTessellationDetail(GUID guid) => 
            GetSpriteData(guid).tessellationDetail;

        void ISpritePhysicsOutlineDataProvider.SetTessellationDetail(GUID guid, float value) => 
            GetSpriteData(guid).tessellationDetail = value;
        #endregion

        SecondarySpriteTexture[] ISecondaryTextureDataProvider.textures
        {
            get => _secondaryTextures;
            set => _secondaryTextures = value;
        }
        
        /*
     *
     * byte[] bytes = File.ReadAllBytes(texturePath);
        NativeArray<byte> native = new NativeArray<byte>(bytes, Allocator.Temp);

        var encoded = ImageConversion.EncodeNativeArrayToPNG(native, GraphicsFormat.R8G8B8A8_UNorm, width, height);
        NativeArray<Color32> bytesToColors = ByteToColorArray(native);

        static NativeArray<Color32> ByteToColorArray(in NativeArray<byte> data)
        {
            NativeArray<Color32> image = new NativeArray<Color32>(data.Length / 4, Allocator.Persistent);
            for (var i = 0; i < image.Length; ++i)
            {
                var dataIndex = i * 4;
                image[i] = new Color32(
                    data[dataIndex],
                    data[dataIndex + 1],
                    data[dataIndex + 2],
                    data[dataIndex + 3]);
            }
            return image;
        }

        Debug.Log($"native: {native.Length}");
        Debug.Log($"encoded: {encoded.Length}");
        Debug.Log($"bytesToColors: {bytesToColors.Length}");
        Debug.Log($"expect: {width*height}");
     */
        
    }
}