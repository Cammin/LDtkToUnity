using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class FieldInstanceEntityReference
    {
        /// <value>
        /// The referenced entity. <br/>
        /// Make sure to call <see cref="LDtkIidBank"/>.<see cref="LDtkIidBank.CacheIidData"/> first!
        /// </value>
        [JsonIgnore] public EntityInstance Entity => EntityIid == null ? null : LDtkIidBank.GetIidData<EntityInstance>(EntityIid);
        
        /// <value>
        /// The layer that this entity is referenced from. <br/>
        /// Make sure to call <see cref="LDtkIidBank"/>.<see cref="LDtkIidBank.CacheIidData"/> first!
        /// </value>
        [JsonIgnore] public LayerInstance Layer => LayerIid == null ? null : LDtkIidBank.GetIidData<LayerInstance>(LayerIid);
        
        /// <value>
        /// The level that this entity is referenced from. <br/>
        /// Make sure to call <see cref="LDtkIidBank"/>.<see cref="LDtkIidBank.CacheIidData"/> first!
        /// </value>
        [JsonIgnore] public Level Level => LevelIid == null ? null : LDtkIidBank.GetIidData<Level>(LevelIid);
        
        /// <value>
        /// The world that this entity is referenced from. <br/>
        /// Make sure to call <see cref="LDtkIidBank"/>.<see cref="LDtkIidBank.CacheIidData"/> first!
        /// </value>
        [JsonIgnore] public World World => WorldIid == null ? null : LDtkIidBank.GetIidData<World>(WorldIid);

        internal static FieldInstanceEntityReference FromJson(string json)
        {
            return JsonConvert.DeserializeObject<FieldInstanceEntityReference>(json, Converter.Settings);
        }
    }
}