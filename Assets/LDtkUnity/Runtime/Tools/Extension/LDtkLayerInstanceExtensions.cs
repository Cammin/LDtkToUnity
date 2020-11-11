using LDtkUnity.Runtime.Data.Level;

namespace LDtkUnity.Runtime.Tools.Extension
{
    public static class LDtkLayerInstanceExtensions
    {
        public static bool IsIntGridLayer(this LDtkDataLayerInstance instance) => !instance.intGrid.NullOrEmpty();
        public static bool IsAutoTilesLayer(this LDtkDataLayerInstance instance) => !instance.autoLayerTiles.NullOrEmpty();
        public static bool IsGridTilesLayer(this LDtkDataLayerInstance instance) => !instance.gridTiles.NullOrEmpty();
        public static bool IsEntityInstancesLayer(this LDtkDataLayerInstance instance) => !instance.entityInstances.NullOrEmpty();
    }
}