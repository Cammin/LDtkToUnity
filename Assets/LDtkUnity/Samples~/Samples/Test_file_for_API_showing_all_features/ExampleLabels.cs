using LDtkUnity;
using UnityEngine;

namespace Samples.Test_file_for_API_showing_all_features
{
    public class ExampleLabels : MonoBehaviour
    {
        [SerializeField] private TextMesh _textMesh = null;
        
        public void UpdateValues()
        {
            LDtkFields fields = GetComponent<LDtkFields>();
            
            _textMesh.text = fields.GetString("text");
            _textMesh.color = fields.GetColor("color");
        }
    }
}
