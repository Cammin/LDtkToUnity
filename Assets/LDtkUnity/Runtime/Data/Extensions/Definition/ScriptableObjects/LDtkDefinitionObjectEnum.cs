using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_ENUM_DEF_JSON)]
    public sealed class LDtkDefinitionObjectEnum : LDtkDefinitionObject<EnumDefinition>, ILDtkUid
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

        [field: Tooltip("Internal")]
        [field: SerializeField] public string ExternalFileChecksum { get; private set; }

        #endregion
        
        internal override void SetAssetName()
        {
            name = $"Enum_{Uid}_{Identifier}";
        }
        
        internal override void Populate(LDtkDefinitionObjectsCache cache, EnumDefinition def)
        {
            ExternalRelPath = def.ExternalRelPath;
            IconTileset = cache.GetObject<LDtkDefinitionObjectTileset>(def.IconTilesetUid);
            Identifier = def.Identifier;
            Tags = def.Tags;
            Uid = def.Uid;
            Values = def.Values.Select(p => new LDtkDefinitionObjectEnumValue()).ToArray();
            
            //EditorOnly
            ExternalFileChecksum = def.ExternalFileChecksum;
            
            //populate ScriptableObjects that are arrays
            for (int i = 0; i < Values.Length; i++)
            {
                Values[i].Populate(cache, def.Values[i]);
            }
        }
    }
}
