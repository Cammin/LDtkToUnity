using LDtkUnity;
using UnityEngine;

namespace Samples.Test_file_for_API_showing_all_features
{
    public class ExampleLabels : MonoBehaviour, ILDtkImportedFields
    {
        [SerializeField] private TextMesh _textMesh = null;
        [SerializeField] private SpriteRenderer _textBg = null;
        
        public void OnLDtkImportFields(LDtkFields fields)
        {
            TextMesh(fields);
            Background(fields);
        }

        private void Background(LDtkFields fields)
        {
            if (_textBg == null)
            {
                Debug.LogWarning($"LDtk Sample: An entity's referenced component was null. This can happen when importing the examples for the first time. Try reimporting again to fix the samples.\n{name}",
                    gameObject);
                return;
            }
            
            _textBg.color = fields.GetColor("color");
        }

        private void TextMesh(LDtkFields fields)
        {
            if (_textMesh == null)
            {
                Debug.LogWarning($"LDtk Sample: An entity's referenced component was null. This can happen when importing the examples for the first time. Try reimporting again to fix the samples.\n{name}",
                    gameObject);
                return;
            }

            _textMesh.text = fields.GetMultiline("text");
        }

        public int GetPostprocessOrder()
        {
            return 0;
        }
    }
}
