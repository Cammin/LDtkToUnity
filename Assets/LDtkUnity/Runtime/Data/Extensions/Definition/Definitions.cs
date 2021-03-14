using System.Linq;

namespace LDtkUnity
{
    public partial class Definitions
    {
        public LayerDefinition[] EntityLayers => Layers.Where(p => p.IsEntitiesLayer).ToArray();
        public LayerDefinition[] IntGridLayers => Layers.Where(p => p.IsIntGridLayer).ToArray();
        public LayerDefinition[] AutoLayers => Layers.Where(p => p.IsAutoLayer).ToArray();
        public LayerDefinition[] TilesLayers => Layers.Where(p => p.IsTilesLayer).ToArray();
    }
}