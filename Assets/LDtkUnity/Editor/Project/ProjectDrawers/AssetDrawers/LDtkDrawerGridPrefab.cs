using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkDrawerGridPrefab : LDtkAssetDrawer<LayerDefinition, Grid>
    {
        public LDtkDrawerGridPrefab(LayerDefinition data, SerializedProperty prop, string key) : base(data, prop, key)
        {
        }

        protected override string AssetUnassignedText => "";

        public override bool HasProblem()
        {
            //TODO actively validate the prefab if it has the correct component setup in root and children
            return false;
        }

        public override void Draw()
        {
            Rect controlRect = EditorGUILayout.GetControlRect();
            Texture image = GetTex();
            DrawField(controlRect, tex: image);
        }

        private Texture GetTex()
        {
            if (_data.IsIntGridLayer)
            {
                return LDtkIconLoader.LoadIntGridIcon();
            }

            if (_data.IsAutoLayer)
            {
                return LDtkIconLoader.LoadAutoLayerIcon();
            }

            if (_data.IsTilesLayer)
            {
                return LDtkIconLoader.LoadTilesetIcon();
            }

            return null;
        }
    }
}