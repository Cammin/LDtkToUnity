using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_EnumDefJson)]
    public sealed class LDtkDefinitionObjectEnum : ScriptableObject
    {
        /// <summary>
        /// Relative path to the external file providing this Enum
        /// </summary>
        [field: SerializeField] public string ExternalRelPath { get; private set; }

        /// <summary>
        /// Tileset UID if provided
        /// </summary>
        [field: SerializeField] public LDtkDefinitionObjectTileset IconTileset { get; private set; }

        /// <summary>
        /// User defined unique identifier
        /// </summary>
        [field: SerializeField] public string Identifier { get; private set; }

        /// <summary>
        /// An array of user-defined tags to organize the Enums
        /// </summary>
        [field: SerializeField] public string[] Tags { get; private set; }

        /// <summary>
        /// Unique Int identifier
        /// </summary>
        [field: SerializeField] public int Uid { get; private set; }

        /// <summary>
        /// All possible enum values, with their optional Tile infos.
        /// </summary>
        [field: SerializeField] public LDtkDefinitionObjectEnumValue[] Values { get; private set; }

        #region EditorOnly

        [field: SerializeField] public string ExternalFileChecksum { get; private set; }

        #endregion
        
        internal void Populate(LDtkDefinitionObjectsCache cache, EnumDefinition def)
        {
            name = $"Enum_{def.Identifier}";
            
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