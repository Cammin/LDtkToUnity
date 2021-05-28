using System.Linq;
using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class Definitions
    {
        /// <value>
        /// All IntGrid layer definitions. (Empty if none are defined)
        /// </value>
        [JsonIgnore] public LayerDefinition[] EntityLayers => Layers.Where(p => p.IsEntitiesLayer).ToArray();
        
        /// <value>
        /// All IntGrid layer definitions. (Empty if none are defined)
        /// </value>
        [JsonIgnore] public LayerDefinition[] IntGridLayers => Layers.Where(p => p.IsIntGridLayer).ToArray();
        
        /// <value>
        /// All Auto-Layer definitions. (Empty if none are defined)
        /// </value>
        [JsonIgnore] public LayerDefinition[] AutoLayers => Layers.Where(p => p.IsAutoLayer).ToArray();

        /// <value>
        /// All Tile layer definitions. (Empty if none are defined)
        /// </value>
        [JsonIgnore] public LayerDefinition[] TilesLayers => Layers.Where(p => p.IsTilesLayer).ToArray();
    }
}