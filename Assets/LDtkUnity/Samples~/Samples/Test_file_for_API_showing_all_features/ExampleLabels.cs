using LDtkUnity;
using UnityEngine;

namespace Samples.Test_file_for_API_showing_all_features
{
    public class ExampleLabels : MonoBehaviour, ILDtkImportedFields
    {
        [SerializeField] private TextMesh _textMesh = null;
        
        public void OnLDtkImportFields(LDtkFields fields)
        {
            _textMesh.text = fields.GetString("text");
            _textMesh.color = fields.GetColor("color");
        }
    }
}
