using System.Linq;
using Newtonsoft.Json;

namespace LDtkUnity
{
    /// <summary>
    /// Json Definition Data
    /// </summary>
    public partial class Definitions
    {
        /// <summary>
        /// All IntGrid layer definitions. (Empty if none are defined)
        /// </summary>
        [JsonIgnore] public LayerDefinition[] EntityLayers => Layers.Where(p => p.IsEntitiesLayer).ToArray();
        
        /// <summary>
        /// All IntGrid layer definitions. (Empty if none are defined)
        /// </summary>
        [JsonIgnore] public LayerDefinition[] IntGridLayers => Layers.Where(p => p.IsIntGridLayer).ToArray();
        
        /// <summary>
        /// All Auto-Layer definitions. (Empty if none are defined)
        /// </summary>
        [JsonIgnore] public LayerDefinition[] AutoLayers => Layers.Where(p => p.IsAutoLayer).ToArray();

        /// <summary>
        /// All Tile layer definitions. (Empty if none are defined)
        /// </summary>
        [JsonIgnore] public LayerDefinition[] TilesLayers => Layers.Where(p => p.IsTilesLayer).ToArray();
    }
}