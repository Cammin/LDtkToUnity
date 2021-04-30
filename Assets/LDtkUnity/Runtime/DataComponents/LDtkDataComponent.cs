using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// use this to inherit and serialize/deserialize custom little things. we store the json string so that we can deserialize it into the organized data
    /// todo turn out this is not the best idea in retrospect as it could make the size of the assets be pretty large
    /// </summary>
    /*public abstract class LDtkDataComponent<T> : LDtkJsonComponent<T>
    {
        public const string PROP_JSON = nameof(_json);
        
        [SerializeField] protected string _json;


        public override T FromJson()
        {
            if (_json == null)
            {
                Debug.LogError("LDtk: Project is null");
                return default;
            }

            T json = DeserializeJson();
            if (json != null)
            {
                return json;
            }
            
            Debug.LogError("LDtk: Project json had a deserialization problem");
            return default;

        }

        protected abstract T DeserializeJson();
        protected abstract string SerializeJson(T data);
    }*/
}