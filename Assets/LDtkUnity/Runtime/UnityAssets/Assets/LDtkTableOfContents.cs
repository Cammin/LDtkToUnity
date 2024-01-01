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
        
        internal void Initialize(LdtkJson json)
        {
            _entries = new List<LDtkTableOfContentsEntry>(json.Toc.Length);
            foreach (LdtkTableOfContentEntry p in json.Toc)
            {
                _entries.Add(new LDtkTableOfContentsEntry(p));
            }
        }

        [PublicAPI, Obsolete("Use " + nameof(GetEntry) + " instead")]
        public LDtkReferenceToAnEntityInstance[] GetEntities(string identifier)
        {
            return GetEntry(identifier)._entries.Select(p => p.EntityRef).ToArray();
        }
        
        /// <summary>
        /// Gets all of the entity references in this table of contents of an entity type.
        /// </summary>
        /// <param name="identifier">
        /// The identifier of the entity type to look for.
        /// </param>
        public LDtkTableOfContentsEntry GetEntry(string identifier)
        {
            foreach (LDtkTableOfContentsEntry entry in _entries)
            {
                if (entry._identifier == identifier)
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
        public string _identifier;
        public List<LDtkTableOfContentsEntryData> _entries;
            
        public LDtkTableOfContentsEntry(LdtkTableOfContentEntry entry)
        {
            _identifier = entry.Identifier;

            LdtkTocInstanceData[] datas = entry.UnityInstances();
            
            _entries = new List<LDtkTableOfContentsEntryData>(datas.Length);
            foreach (LdtkTocInstanceData p in datas)
            {
                _entries.Add(new LDtkTableOfContentsEntryData(p));
            }
        }
    }
        
    [Serializable]
    public sealed class LDtkTableOfContentsEntryData
    {
        public LDtkReferenceToAnEntityInstance EntityRef;
        //public LDtkFields Field; //todo
        public Vector2Int WorldPos;
        public Vector2Int SizePx;

        public LDtkTableOfContentsEntryData(LdtkTocInstanceData data)
        {
            EntityRef = new LDtkReferenceToAnEntityInstance(data.Iids);
            //Field = data.Fields; todo
            WorldPos = data.UnityWorld;
            SizePx = data.UnitySizePx;
        }
    }
}