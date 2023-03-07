using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    public sealed class LDtkTableOfContents : ScriptableObject
    {
        [SerializeField] internal List<TocEntry> _entries;
        
        [Serializable]
        internal sealed class TocEntry
        {
            public string _identifier;
            public LDtkReferenceToAnEntityInstance[] _instances;

            public TocEntry(LdtkTableOfContentEntry entry)
            {
                _identifier = entry.Identifier;
                _instances = entry.Instances.Select(p => new LDtkReferenceToAnEntityInstance(p)).ToArray();
            }
        }
        
        internal void Initialize(LdtkJson json)
        {
            _entries = json.Toc.Select(p => new TocEntry(p)).ToList();
        }

        /// <summary>
        /// Gets all of the entity references in this table of contents of an entity type.
        /// </summary>
        /// <param name="identifier">
        /// The identifier of the entity type to look for.
        /// </param>
        public LDtkReferenceToAnEntityInstance[] GetEntities(string identifier)
        {
            foreach (TocEntry entry in _entries)
            {
                if (entry._identifier == identifier)
                {
                    return entry._instances;
                }
            }
            LDtkDebug.LogWarning($"A table of contents entry doesn't exist for \"{identifier}\"");
            return Array.Empty<LDtkReferenceToAnEntityInstance>();
        }
    }
}