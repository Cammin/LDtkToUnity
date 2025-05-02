using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Field conversion for the LDtkFields component specifically.
    /// </summary>
    internal sealed class LDtkFieldsFactory
    {
        private readonly GameObject _instance;
        private readonly FieldInstance[] _fieldInstances;
        private readonly LDtkProjectImporter _project;
        private readonly LDtkJsonImporter _importer;
        
        public LDtkFields FieldsComponent { get; private set; }
        
        public LDtkFieldsFactory(GameObject instance, FieldInstance[] fieldInstances, LDtkProjectImporter project, LDtkJsonImporter importer)
        {
            _instance = instance;
            _fieldInstances = fieldInstances;
            _project = project;
            _importer = importer;
        }

        public void SetEntityFieldsComponent()
        {
            if (_fieldInstances.IsNullOrEmpty())
            {
                return;
            }
            
            if (!_instance.TryGetComponent(out LDtkFields fields))
            {
                fields = _instance.AddComponent<LDtkFields>();
            }
            
            LDtkProfiler.BeginSample("GetFields");
            LDtkField[] fieldData = GetFields();
            LDtkProfiler.EndSample();
            
            fields.SetFieldData(fieldData);

            FieldsComponent = fields;
        }

        private LDtkField[] GetFields()
        {
            LDtkFieldFactory fieldFactory = new LDtkFieldFactory(_project, _importer);

            LDtkField[] fields = new LDtkField[_fieldInstances.Length];
            for (int i = 0; i < _fieldInstances.Length; i++)
            {
                fields[i] = fieldFactory.GetFieldFromInstance(_fieldInstances[i].Definition, _fieldInstances[i].Value);
                
                //setup transforms for the points so that they are easy to follow along.
                LDtkProfiler.BeginSample($"Setup point transforms");
                LDtkField field = fields[i];
                if (field.Type == LDtkFieldType.Point)
                {
                    for (int ii = 0; ii < field._data.Length; ii++)
                    {
                        LDtkFieldElement element = field._data[ii];
                        Transform newPoint = new GameObject($"{_fieldInstances[i].Identifier}_{ii}").transform;
                        element.SetPointLocalTransform(newPoint);
                        newPoint.SetParent(_instance.transform, true);
                    }
                }
                LDtkProfiler.EndSample();
            }
            return fields;
        }
    }
}