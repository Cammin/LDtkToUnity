using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_EnumDefJson)]
    public sealed class LDtkDefinitionObjectEnum : ScriptableObject
    {
        [field: Tooltip("Relative path to the external file providing this Enum")]
        [field: SerializeField] public string ExternalRelPath { get; private set; }
        
        [field: Tooltip("Tileset UID if provided")]
        [field: SerializeField] public LDtkDefinitionObjectTileset IconTileset { get; private set; }
        
        [field: Tooltip("User defined unique identifier")]
        [field: SerializeField] public string Identifier { get; private set; }
        
        [field: Tooltip("An array of user-defined tags to organize the Enums")]
        [field: SerializeField] public string[] Tags { get; private set; }
        
        [field: Tooltip("Unique Int identifier")]
        [field: SerializeField] public int Uid { get; private set; }
        
        [field: Tooltip("All possible enum values, with their optional Tile infos.")]
        [field: SerializeField] public LDtkDefinitionObjectEnumValue[] Values { get; private set; }

        #region EditorOnly

        [field: SerializeField] public string ExternalFileChecksum { get; private set; }

        #endregion
        
        internal void Populate(LDtkDefinitionObjectsCache cache, EnumDefinition def)
        {
            ExternalRelPath = def.ExternalRelPath;
            IconTileset = cache.GetObject(cache.Tilesets, def.IconTilesetUid);
            Identifier = def.Identifier;
            Tags = def.Tags;
            Uid = def.Uid;
            Values = def.Values.Select(p => new LDtkDefinitionObjectEnumValue(cache, p)).ToArray();
            
            //EditorOnly
            ExternalFileChecksum = def.ExternalFileChecksum;
        }
    }
}