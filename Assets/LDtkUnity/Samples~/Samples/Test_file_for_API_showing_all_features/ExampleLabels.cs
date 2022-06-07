using LDtkUnity;
using UnityEngine;

namespace Samples.Test_file_for_API_showing_all_features
{
    public class ExampleLabels : MonoBehaviour, ILDtkImportedFields
    {
        [SerializeField] private TextMesh _textMesh = null;
        
        public void OnLDtkImportFields(LDtkFields fields)
        {
            if (_textMesh == null)
            {
                Debug.LogWarning($"LDtk Sample: An entity's referenced component was null. This can happen when importing the examples for the first time. Try reimporting again to fix the samples.\n{name}", gameObject);
                return;
            }
            
            _textMesh.text = fields.GetMultiline("text");
            _textMesh.color = fields.GetColor("color");
        }
    }
}
