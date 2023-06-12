using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
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

        SpriteRect[] ISpriteEditorDataProvider.GetSpriteRects()
        {
            return _sprites.Select(x => new LDtkSpriteRect(x) as SpriteRect).ToArray();
        }

        //this would be run as the file is being created
        public void PopulateSpriteRects(TilesetRectangle[] rects)
        {
            //SpriteRect
            
            //_sprites.Add(new LDtkSpriteRect(newSprite));
            //newSprite.name = ForceUpdateSpriteDataName(newSprite);
        }
        
        void ISpriteEditorDataProvider.SetSpriteRects(SpriteRect[] newSprites)
        {
            //remove those that have become null or otherwise deleted and/or irrelevant
            _sprites.RemoveAll(data => newSprites.FirstOrDefault(x => x.spriteID == data.spriteID) == null);
            
            //create any sprite rects that don't exist, but keep the existing ones if we have em. Grid Time!

            Debug.Log($"SetSpriteRects {newSprites.Length}");
            TilesetDefinition def = FromJson<TilesetDefinition>(); //todo we could cache this instead?
            List<RectInt> slices = new List<RectInt>();
            //Debug.Log($"The tileset {def.Identifier} uses {usedTiles.Count} unique tiles");
            int padding = def.Padding;
            int gridSize = def.TileGridSize;
            int spacing = def.Spacing;
            for (int y = padding; y < def.PxHei - padding; y += gridSize + spacing)
            {
                for (int x = padding; x < def.PxWid - padding; x += gridSize + spacing)
                {
                    if (x + gridSize > def.PxWid || y + gridSize > def.PxHei)
                    {
                        continue;
                    }

                    RectInt slice = new RectInt(x, y, gridSize, gridSize);
                    slices.Add(slice);
                }
            }

            foreach (RectInt slice in slices)
            {
                Rect rect = new Rect(slice.position, slice.size);
                LDtkSpriteRect foundRect = _sprites.FirstOrDefault(p => p.rect == rect);
                if (foundRect == null)
                {
                    _sprites.Add(new LDtkSpriteRect()
                    {
                        alignment = SpriteAlignment.Center, 
                        rect = rect,
                    });
                }
                
            }
            
            
            foreach (var newSprite in newSprites)
            {
                LDtkSpriteRect importData = _sprites.FirstOrDefault(x => x.spriteID == newSprite.spriteID);
                
                //if a new foreign sprite rect was trying to be made, don't allow it. only permit changing some values
                if (importData != null)
                {
                    importData.alignment = newSprite.alignment;
                    importData.border = newSprite.border;
                    //importData.name = newSprite.name; //never change name to a new one
                    importData.pivot = newSprite.pivot;
                    //importData.rect = newSprite.rect; //always maintain rect and never change
                    //Debug.Log($"Add {rect}");
                    
                    
                }
            }
        }
        
        //aseprite importer reference
        /*List<SpriteMetaData> UpdateSpriteImportData(List<Cell> cellLookup, RectInt[] spriteRects, Vector2Int[] packOffsets, Vector2Int[] uvTransforms)
        {
            var spriteImportData = GetSpriteImportData();
            
            //if none existed prior at all, it's safe to just add a bunch!
            if (spriteImportData.Count <= 0)
            {
                var newSpriteMeta = new List<SpriteMetaData>();

                for (var i = 0; i < spriteRects.Length; ++i)
                {
                    var cell = cellLookup[i];
                    var spriteData = CreateNewSpriteMetaData(in cell, in spriteRects[i], packOffsets[i], in uvTransforms[i]);
                    newSpriteMeta.Add(spriteData);
                }
                spriteImportData.Clear();
                spriteImportData.AddRange(newSpriteMeta);
            }
            else
            {
                // Remove old cells
                
                //if there was old data that now doesnt exist in the new data, get rid of it; it probably ceased to exist
                for (var i = spriteImportData.Count - 1; i >= 0; --i)
                {
                    var spriteData = spriteImportData[i];
                    
                    //check if the new data contains the old data, remove it 
                    if (cellLookup.FindIndex(x => x.spriteId == spriteData.spriteID) == -1)
                    {
                        spriteImportData.RemoveAt(i);
                    }
                }                
                
                // Add new cells
                for (var i = 0; i < cellLookup.Count; ++i)
                {
                    var cell = cellLookup[i];
                    // add cells that were missing or simply didn't exist 
                    if (spriteImportData.FindIndex(x => x.spriteID == cell.spriteId) == -1)
                    {
                        var spriteData = CreateNewSpriteMetaData(in cell, spriteRects[i], packOffsets[i], uvTransforms[i]);
                        spriteImportData.Add(spriteData);
                    }
                }
                
                // Update with new pack data
                //OVERWRITE any matches. this means forcing their rect position back to what it was
                for (var i = 0; i < cellLookup.Count; ++i)
                {
                    var cell = cellLookup[i];
                    var spriteData = spriteImportData.Find(x => x.spriteID == cell.spriteId);
                    if (spriteData != null)
                    {
                        var areSettingsUpdated = !m_PreviousAsepriteImporterSettings.IsDefault() &&
                                                 (pivotAlignment != m_PreviousAsepriteImporterSettings.defaultPivotAlignment ||
                                                  pivotSpace != m_PreviousAsepriteImporterSettings.defaultPivotSpace ||
                                                  customPivotPosition != m_PreviousAsepriteImporterSettings.customPivotPosition);
                        
                        // Update pivot if either the importer settings are updated
                        // or the source files rect has been changed (Only for Canvas, as rect position doesn't matter in local). 
                        if (pivotSpace == PivotSpaces.Canvas && 
                            (areSettingsUpdated || cell.updatedCellRect))
                        {
                            spriteData.alignment = SpriteAlignment.Custom;

                            var cellRect = cell.cellRect;
                            cellRect.x += packOffsets[i].x;
                            cellRect.y += packOffsets[i].y;
                            cellRect.width = spriteRects[i].width;
                            cellRect.height = spriteRects[i].height;
                            
                            spriteData.pivot = ImportUtilities.CalculateCellPivot(cellRect, m_CanvasSize, pivotAlignment, customPivotPosition);
                        }
                        else if (pivotSpace == PivotSpaces.Local && areSettingsUpdated)
                        {
                            spriteData.alignment = pivotAlignment;
                            spriteData.pivot = customPivotPosition;
                        }

                        spriteData.rect = new Rect(spriteRects[i].x, spriteRects[i].y, spriteRects[i].width, spriteRects[i].height);
                        spriteData.uvTransform = uvTransforms[i];
                    }
                }                
            }

            return spriteImportData;
        }*/
        
        
        /// <summary>
        /// This may be used in the actual sprite generation step instead of here
        /// </summary>
        private List<RectInt> GetStandardSpriteRectsForDefinition(TilesetDefinition def)
        {
            List<RectInt> rects = new List<RectInt>();
            //Debug.Log($"The tileset {def.Identifier} uses {usedTiles.Count} unique tiles");
            int padding = def.Padding;
            int gridSize = def.TileGridSize;
            int spacing = def.Spacing;
            
            for (int y = padding; y < def.PxHei - padding; y += gridSize + spacing)
            {
                for (int x = padding; x < def.PxWid - padding; x += gridSize + spacing)
                {
                    if (x + gridSize > def.PxWid || y + gridSize > def.PxHei)
                    {
                        continue;
                    }
                    
                    RectInt slice = new RectInt(x, y, gridSize, gridSize);
                    rects.Add(slice);
                }
            }

            return rects;
        }

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

        void ITextureDataProvider.GetTextureActualWidthAndHeight(out int width, out int height)
        {
            Texture2D tex = LoadTex();
            width = tex != null ? tex.width : 0;
            height = tex != null ? tex.height : 0;
        }

        Texture2D ITextureDataProvider.GetReadableTexture2D() => LoadTex();
        Texture2D ITextureDataProvider.texture => LoadTex();
        Texture2D ITextureDataProvider.previewTexture => LoadTex();

        
        List<Vector2[]> ISpritePhysicsOutlineDataProvider.GetOutlines(GUID guid) =>
            GetSpriteData(guid).GetOutlines();

        void ISpritePhysicsOutlineDataProvider.SetOutlines(GUID guid, List<Vector2[]> data) =>
            GetSpriteData(guid).SetOutlines(data);

        float ISpritePhysicsOutlineDataProvider.GetTessellationDetail(GUID guid) => 
            GetSpriteData(guid).tessellationDetail;

        void ISpritePhysicsOutlineDataProvider.SetTessellationDetail(GUID guid, float value) => 
            GetSpriteData(guid).tessellationDetail = value;

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