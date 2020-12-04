using System;
using System.IO;
using System.Runtime.InteropServices;
using LDtkUnity.Editor.AssetManagement.EditorAssetLoading;
using LDtkUnity.Runtime.Data.Definition;
using LDtkUnity.Runtime.UnityAssets.Tileset;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.Drawers
{
    public class LDtkReferenceDrawerTileset : LDtkAssetReferenceDrawer<LDtkDefinitionTileset>
    {
        private bool _failedSpriteGet;
        private string _failedSpritePath;
        private readonly string _projectPath;
        
        LDtkTilesetAsset Asset => (LDtkTilesetAsset)Property.objectReferenceValue;

        public LDtkReferenceDrawerTileset(SerializedProperty asset, string projectProjectPath) : base(asset)
        {
            _projectPath = projectProjectPath;
        }

        
        protected override void DrawInternal(Rect controlRect, LDtkDefinitionTileset data)
        {
            DrawLeftIcon(controlRect, LDtkIconLoader.LoadTilesetIcon());
            DrawSelfSimple(controlRect, LDtkIconLoader.LoadTilesetIcon(), data);
            
            /*if (Asset != null && DrawRightFieldIconButton(controlRect, "Refresh"))
            {
                RefreshSpritePathAssignment(data);
            }*/

            
            
            if (Asset != null && _failedSpriteGet)
            {
                GUIStyle miniLabel = EditorStyles.miniLabel;
                miniLabel.normal.textColor = Color.red;

                
                
                //SerializedObject serializedAssetObject = new SerializedObject(Property.objectReferenceValue);

                
                //SerializedProperty spriteProp = serializedAssetObject.FindProperty("_asset");

                if (!Asset.AssetExists)
                {
                    EditorGUILayout.LabelField($"Tileset could not be found in path {_projectPath}", miniLabel);
                }
                else
                {
                    if (!Asset.ReferencedAsset.texture.isReadable)
                    {
                        EditorGUILayout.LabelField($"Texture is not set to Read/Write enabled", miniLabel);
                    }
                }
            }
        }

        public void RefreshSpritePathAssignment(LDtkDefinitionTileset data)
        {
            string assetsPath = $"{_projectPath}\\{data.relPath}";
            
            //Debug.Log($"trying to get {assetsPath}");
            

            string absolutePath = Path.GetFullPath(assetsPath);
            //if (absolutePath.StartsWith(Application.dataPath)) 
            {
                //Debug.Log("substringed");
                //assetsPath = "Assets" + absolutePath.Substring(Application.dataPath.Length);
            }
            /*assetsPath = assetsPath.Replace("~", "");
            assetsPath = assetsPath.Replace("\\", "/");*/

            byte[] byteData = File.ReadAllBytes(absolutePath);
            
            Guid guid = new Guid(byteData);

            Debug.Log(guid.ToString());
            string guidToAssetPath = AssetDatabase.GUIDToAssetPath(guid.ToString());
            
            Debug.Log(guidToAssetPath);
            Texture2D tileset = AssetDatabase.LoadAssetAtPath<Texture2D>(guidToAssetPath);
            
            
            
            //_failedSpriteGet = tileset == null;

            if (tileset != null)
            {
                Debug.Log($"Got it!");
                Debug.Log(tileset.name);
                
                //Property
                //Asset.ReferencedAsset = tileset;
            }
            else
            {
                Debug.Log("was null");
            }
        }


    }
}