using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// use this to inherit and serialize/deserialize custom little things. we store the json string so that we can deserialize it into the organized data
    /// </summary>
    public abstract class LDtkDataReferenceComponent<TData, TFile> : LDtkJsonComponent<TData> where TFile : LDtkJsonFile<TData>
    {
        public const string PROP_PROJECT = nameof(_file);
        
        [SerializeField] private TFile _file;

        public override TData FromJson()
        {
            if (_file == null)
            {
                Debug.LogError("LDtk: Json File is null");
                return default;
            }

            TData json = _file.FromJson;
            if (json != null)
            {
                return json;
            }
            
            Debug.LogError("LDtk: Json File had a deserialization problem");
            return default;

        }
    }
}