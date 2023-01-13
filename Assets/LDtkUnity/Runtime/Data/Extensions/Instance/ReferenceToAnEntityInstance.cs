using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class ReferenceToAnEntityInstance
    {
        /// <value>
        /// The referenced entity. <br/>
        /// Make sure to call <see cref="LDtkIidBank"/>.<see cref="LDtkIidBank.CacheIidData"/> first!
        /// </value>
        [IgnoreDataMember] public EntityInstance Entity => EntityIid == null ? null : LDtkIidBank.GetIidData<EntityInstance>(EntityIid);
        
        /// <value>
        /// The layer that this entity is referenced from. <br/>
        /// Make sure to call <see cref="LDtkIidBank"/>.<see cref="LDtkIidBank.CacheIidData"/> first!
        /// </value>
        [IgnoreDataMember] public LayerInstance Layer => LayerIid == null ? null : LDtkIidBank.GetIidData<LayerInstance>(LayerIid);
        
        /// <value>
        /// The level that this entity is referenced from. <br/>
        /// Make sure to call <see cref="LDtkIidBank"/>.<see cref="LDtkIidBank.CacheIidData"/> first!
        /// </value>
        [IgnoreDataMember] public Level Level => LevelIid == null ? null : LDtkIidBank.GetIidData<Level>(LevelIid);
        
        /// <value>
        /// The world that this entity is referenced from. <br/>
        /// Make sure to call <see cref="LDtkIidBank"/>.<see cref="LDtkIidBank.CacheIidData"/> first!
        /// </value>
        [IgnoreDataMember] public World World => WorldIid == null ? null : LDtkIidBank.GetIidData<World>(WorldIid);
    }
}