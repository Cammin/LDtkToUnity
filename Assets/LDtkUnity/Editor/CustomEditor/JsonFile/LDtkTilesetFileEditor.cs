using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkTilesetFile))]
    internal sealed class LDtkTilesetFileEditor : LDtkJsonFileEditor<LDtkTilesetDefinition>
    {
        private int? _uid = null;

        protected override Texture2D StaticPreview => LDtkIconUtility.LoadTilesetIcon();

        private void OnEnable()
        {
            TryCacheJson();
            InitValues();
        }
        
        private void InitValues()
        {
            TilesetDefinition def = JsonData.Def;
            
            //def.Uid
            

        }
        
        protected override void DrawInspectorGUI()
        {
            
            //DrawText("Uid");
            //DrawCountOfItems(_layerCount, "Uid", "Layers", LDtkIconUtility.LoadLayerIcon());
        }
    }
}