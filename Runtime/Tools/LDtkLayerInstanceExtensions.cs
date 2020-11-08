using LDtkUnity.Runtime.Data.Level;

namespace LDtkUnity.Runtime.Tools
{
    public static class LDtkLayerInstanceExtensions
    {
        public static bool IsIntGridLayer(this LDtkDataLayerInstance instance) => !instance.intGrid.IsNullOrEmpty();
        public static bool IsAutoTilesLayer(this LDtkDataLayerInstance instance) => !instance.autoLayerTiles.IsNullOrEmpty();
        public static bool IsGridTilesLayer(this LDtkDataLayerInstance instance) => !instance.gridTiles.IsNullOrEmpty();
        public static bool IsEntityInstancesLayer(this LDtkDataLayerInstance instance) => !instance.entityInstances.IsNullOrEmpty();
    }
}