using System.Collections.Generic;
using UnityEditor;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Similar to the int grid value groups, this would draw groups with a mini header, and display many serialized values. 
    /// </summary>
    internal sealed class LDtkGroupDrawerEntity : LDtkGroupDrawer<EntityDefinition, EntityDefinition, LDtkDrawerEntity>
    {
        private readonly List<LDtkDrawerEntity> _drawers;
        
        public LDtkGroupDrawerEntity(EntityDefinition data, SerializedProperty arrayProp, string tag, List<LDtkDrawerEntity> drawers) : base(data, arrayProp)
        {
            _drawers = drawers;
            Tag = tag;
        }
        
        protected override List<LDtkDrawerEntity> GetDrawersForGroup()
        {
            return _drawers;
        }
    }
}