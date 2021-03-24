using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity
{
    public static class LDtkGridPrefabValidator
    {
        public static bool ValidateGridPrefabComponents(Grid tilemapPrefab, LayerDefinition layer, out string errorMsg)
        {
            if (tilemapPrefab == null)
            {
                errorMsg = $"Grid prefab is null. For \"{layer.Identifier}\"";
                return false;
            }
            
            if (tilemapPrefab.transform.childCount <= 0)
            {
                errorMsg = $"Grid prefab has no children; a child with a Tilemap component is required. For \"{layer.Identifier}\"";
                return false;
            }

            Transform child = tilemapPrefab.transform.GetChild(0);
            
            if (!child.GetComponent<Tilemap>())
            {
                errorMsg = $"Grid prefab's first child has no Tilemap component. For \"{layer.Identifier}\"";
                return false;
            }

            if ((layer.IsTilesLayer || layer.IsAutoLayer) && !child.GetComponent<TilemapRenderer>())
            {
                errorMsg = $"Grid prefab's first child has no TilemapRenderer component for the Tile/Auto Layer \"{layer.Identifier}\"";
                return false;
            }
            
            if (layer.IsIntGridLayer && !child.GetComponent<TilemapCollider2D>())
            {
                errorMsg = $"Grid prefab's first child has no TilemapCollider2D component for the IntGrid Layer \"{layer.Identifier}\"";
                return false;
            }

            errorMsg = "Success";
            return true;
        }
    }
}