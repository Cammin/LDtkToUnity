using System;
using System.Collections.Generic;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// toc fields are only keyed by the entity's identifier and then the field's identifier. no uid.
    /// So we should build up a dictionary by identifier for easy lookup
    /// </summary>
    internal sealed class LDtkTocFieldFactory
    {
        private LdtkJson _json;
        private readonly LDtkJsonImporter _importer;
        private readonly LDtkProjectImporter _project;
        
        private Dictionary<string, EntityFieldDefsByIdentifier> _entityIdentifierToFieldDefs;
        private Dictionary<string, LDtkDefinitionObjectEntity> _entityIdentifierToEntityDefinition;

        public class TocFieldFactoryOutput
        {
            public LDtkDefinitionObjectEntity Definition;
            public List<LDtkField[]> Fields;
        }
            
        
        private class EntityFieldDefsByIdentifier : Dictionary<string, FieldDefinition>
        {
            public EntityFieldDefsByIdentifier(int capacity) : base(capacity) { }
        }
        
        public LDtkTocFieldFactory(LdtkJson json, LDtkJsonImporter importer, LDtkProjectImporter project)
        {
            _json = json;
            _importer = importer;
            _project = project;
        }

        public void IndexEntitiesAndFieldsByIdentifiers()
        {
            //no preallocate because we aren't necessarily covering all entities
            _entityIdentifierToFieldDefs = new Dictionary<string, EntityFieldDefsByIdentifier>();
            _entityIdentifierToEntityDefinition = new Dictionary<string, LDtkDefinitionObjectEntity>();
            
            foreach (EntityDefinition entityDef in _json.Defs.Entities)
            {
                //skip caching any that don't export to toc
                if (!entityDef.ExportToToc) continue;
                
                _entityIdentifierToEntityDefinition.Add(entityDef.Identifier, _importer.DefinitionObjects.GetObject<LDtkDefinitionObjectEntity>(entityDef.Uid));
                
                EntityFieldDefsByIdentifier fields = new EntityFieldDefsByIdentifier(entityDef.FieldDefs.Length);
                foreach (FieldDefinition fieldDef in entityDef.FieldDefs)
                {
                    //skip caching fields that don't export to toc
                    if (!fieldDef.ExportToToc) continue;
                    
                    fields.Add(fieldDef.Identifier, fieldDef);
                }
                _entityIdentifierToFieldDefs.Add(entityDef.Identifier, fields);
            }
        }
        
        /// <summary>
        /// One Toc entry can have multiple instances. so a list for each eneityt, and the element is an array of fields that the entity would possess.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public TocFieldFactoryOutput GenerateFieldsFromTocEntry(LdtkTableOfContentEntry entry)
        {
            string entityIdentifier = entry.Identifier;

            List<LDtkField[]> fieldsList = new List<LDtkField[]>();
            
            LdtkTocInstanceData[] instances = entry.UnityInstances();
            foreach (LdtkTocInstanceData instance in instances)
            {
                LDtkField[] fields = GetFields(entityIdentifier, instance.Fields);
                fieldsList.Add(fields);
            }

            TocFieldFactoryOutput output = new TocFieldFactoryOutput()
            {
                Definition = _entityIdentifierToEntityDefinition[entityIdentifier],
                Fields = fieldsList
            };

            return output;
        }

        private LDtkField[] GetFields(string entityIdentifier, Dictionary<string,object> srcFields)
        {
            LDtkFieldFactory fieldFactory = new LDtkFieldFactory(_project, _importer);

            LDtkField[] fields = new LDtkField[srcFields.Count];
            
            int i = 0;
            foreach (var fieldIdentifier in srcFields.Keys)
            {
                FieldDefinition def = GetFieldDefByFieldIdentifier(entityIdentifier, fieldIdentifier);
                fields[i] = fieldFactory.GetFieldFromInstance(def, srcFields[fieldIdentifier]);
                i++;
            }
            
            return fields;
        }
        
        /// <summary>
        /// The important part about this:
        /// A Toc field is only identified by its name. use the entity's name and the field name to figure out the type of the field to get it's object and then parse it.
        /// </summary>
        private FieldDefinition GetFieldDefByFieldIdentifier(string entityIdentifier, string fieldIdentifier)
        {
            if (_entityIdentifierToFieldDefs.TryGetValue(entityIdentifier, out EntityFieldDefsByIdentifier fields))
            {
                if (fields.TryGetValue(fieldIdentifier, out FieldDefinition fieldDef))
                {
                    return fieldDef;
                }

                LDtkDebug.LogError($"The field identifier \"{fieldIdentifier}\" does not exist in the entity \"{entityIdentifier}\"");
            }
            else
            {
                LDtkDebug.LogWarning($"The entity identifier \"{entityIdentifier}\" does not exist in the table of contents");
            }

            return null;
        }
    }
}