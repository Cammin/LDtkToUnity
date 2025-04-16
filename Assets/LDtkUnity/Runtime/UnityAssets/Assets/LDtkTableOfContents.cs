using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// The table of contents is generated when any entities are included in it from LDtk.
    /// Use it to quickly refer to a certain entity type in a level.
    /// </summary>
    [HelpURL(LDtkHelpURL.SO_TOC)]
    public sealed class LDtkTableOfContents : ScriptableObject
    {
        [SerializeField] internal List<LDtkTableOfContentsEntry> _entries;
        
        internal void InitializeList(LdtkJson json)
        {
            _entries = new List<LDtkTableOfContentsEntry>(json.Toc.Length);
        }

        internal void AddEntry(LdtkTableOfContentEntry entry, LDtkDefinitionObjectEntity def, List<LDtkField[]> fields)
        {
            LDtkTableOfContentsEntry newEntry = new LDtkTableOfContentsEntry(entry, def, fields);
            _entries.Add(newEntry);
        }

        [PublicAPI, Obsolete("Use " + nameof(GetEntry) + " instead")]
        public LDtkReferenceToAnEntityInstance[] GetEntities(string identifier)
        {
            return GetEntry(identifier).Entries.Select(p => p.EntityRef).ToArray();
        }
        
        /// <summary>
        /// Gets all the entity references in this table of contents of an entity type.
        /// </summary>
        /// <param name="identifier">
        /// The identifier of the entity type to look for.
        /// </param>
        public LDtkTableOfContentsEntry GetEntry(string identifier)
        {
            foreach (LDtkTableOfContentsEntry entry in _entries)
            {
                if (entry.Definition.Identifier == identifier)
                {
                    return entry;
                }
            }
            LDtkDebug.LogWarning($"A table of contents entry doesn't exist for \"{identifier}\"");
            return null;
        }
    }
    
    [Serializable]
    public sealed class LDtkTableOfContentsEntry
    {
        [SerializeField] private LDtkDefinitionObjectEntity _def;
        [SerializeField] private List<LDtkTableOfContentsEntryData> _entries;
        
        public LDtkDefinitionObjectEntity Definition => _def;
        
        /// <summary>
        /// The list of entity instances in this table of contents entry.
        /// </summary>
        public List<LDtkTableOfContentsEntryData> Entries => _entries;
        
        internal LDtkTableOfContentsEntry(LdtkTableOfContentEntry entry, LDtkDefinitionObjectEntity def, List<LDtkField[]> fields)
        {
            _def = def;

            LdtkTocInstanceData[] datas = entry.UnityInstances();
            
            _entries = new List<LDtkTableOfContentsEntryData>(datas.Length);
            for (int i = 0; i < datas.Length; i++)
            {
                _entries.Add(new LDtkTableOfContentsEntryData(datas[i], fields[i]));
            }
        }
    }
        
    [Serializable]
    public sealed class LDtkTableOfContentsEntryData
    {
        [SerializeField] private LDtkReferenceToAnEntityInstance _entityRef;
        [SerializeField] private Vector2Int _worldPos;
        [SerializeField] private Vector2Int _sizePx;
        [SerializeField] private LDtkField[] _fields;
        
        public LDtkReferenceToAnEntityInstance EntityRef => _entityRef;
        public Vector2Int WorldPosition => _worldPos;
        public Vector2Int Size => _sizePx;
        public LDtkField[] Fields => _fields;
        
        /// <summary>
        /// Finds a field by its identifier.
        /// </summary>
        public LDtkField GetField(string identifier)
        {
            return _fields.FirstOrDefault(p => p.Identifier == identifier);
        }
        
        internal LDtkTableOfContentsEntryData(LdtkTocInstanceData data, LDtkField[] fields)
        {
            _entityRef = new LDtkReferenceToAnEntityInstance(data.Iids);
            _fields = fields;
            _worldPos = data.UnityWorld;
            _sizePx = data.UnitySizePx;
        }
    }
}