using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /*public class LDtkDrawerGridPrefab : LDtkAssetDrawer<LayerDefinition, GameObject>
    {
        private string _errorText = "";

        public LDtkDrawerGridPrefab(LayerDefinition data, SerializedProperty prop, string key) : base(data, prop, key)
        {
        }
        
        public override bool HasProblem()
        {
            //actively validate the prefab if it has the correct component setup in root and children
            if (Asset == null)
            {
                return false;
            }
            
            return !LDtkGridPrefabValidator.ValidateGridPrefabComponents(Asset, _data, out _errorText);
        }

        public override void Draw()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            Texture image = GetTex();
            
            DrawField(controlRect, tex: image);
            
            
            if (HasProblem())
            {
                CacheWarning(_errorText);
                DrawCachedProblem(controlRect);
            }
        }
        
        
        private Texture GetTex()
        {
            if (_data.IsIntGridLayer)
            {
                return LDtkIconUtility.LoadIntGridIcon();
            }

            if (_data.IsAutoLayer)
            {
                return LDtkIconUtility.LoadAutoLayerIcon();
            }

            if (_data.IsTilesLayer)
            {
                return LDtkIconUtility.LoadTilesetIcon();
            }

            return null;
        }
    }*/
}