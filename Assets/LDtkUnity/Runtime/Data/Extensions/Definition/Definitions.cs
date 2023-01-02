using System.Linq;
using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class Definitions
    {
        /// <value>
        /// All IntGrid layer definitions. (Empty if none are defined)
        /// </value>
        [IgnoreDataMember] public LayerDefinition[] EntityLayers => Layers.Where(p => p.IsEntitiesLayer).ToArray();
        
        /// <value>
        /// All IntGrid layer definitions. (Empty if none are defined)
        /// </value>
        [IgnoreDataMember] public LayerDefinition[] IntGridLayers => Layers.Where(p => p.IsIntGridLayer).ToArray();
        
        /// <value>
        /// All Auto-Layer definitions. (Empty if none are defined)
        /// </value>
        [IgnoreDataMember] public LayerDefinition[] AutoLayers => Layers.Where(p => p.IsAutoLayer).ToArray();

        /// <value>
        /// All Tile layer definitions. (Empty if none are defined)
        /// </value>
        [IgnoreDataMember] public LayerDefinition[] TilesLayers => Layers.Where(p => p.IsTilesLayer).ToArray();
    }
}