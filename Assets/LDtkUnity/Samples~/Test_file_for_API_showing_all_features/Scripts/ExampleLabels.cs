using LDtkUnity;
using UnityEngine;

namespace Samples.Test_file_for_API_showing_all_features
{
    public class ExampleLabels : MonoBehaviour
    {
        [SerializeField] private TextMesh _textMesh = null;
        
        [LDtkField("text")] public string _desc;
        [LDtkField("color")] public Color _color;

        public void UpdateValues()
        {
            _textMesh.text = _desc;
            _textMesh.color = _color;
        }
    }
}
