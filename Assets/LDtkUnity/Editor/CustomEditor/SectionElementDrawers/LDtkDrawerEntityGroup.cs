using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Similar to the int grid value groups, this would draw groups with a mini header, and display many serialized values. 
    /// </summary>
    internal class LDtkDrawerEntityGroup : LDtkGroupDrawer<EntityDefinition, EntityDefinition, LDtkDrawerEntity>
    {
        private readonly string _tag;
        private readonly List<LDtkDrawerEntity> _drawers;
        
        public LDtkDrawerEntityGroup(EntityDefinition data, SerializedProperty arrayProp, string tag, List<LDtkDrawerEntity> drawers) : base(data, arrayProp)
        {
            _tag = tag;
            _drawers = drawers;
            Drawers = GetDrawers().ToList();
        }
        
        protected override List<LDtkDrawerEntity> GetDrawers()
        {
            return _drawers;
        }

        public override void Draw()
        {
            DrawGroupLabel(_tag);
            DrawItems();
        }
    }
}