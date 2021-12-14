using LDtkUnity;
using LDtkUnity.Editor;
using UnityEngine;

namespace Tests.Editor
{
    public class TestPostProcessorOrder2 : LDtkPostprocessor
    {
        protected override void OnPostProcessProject(GameObject projectRoot)
        {
            Test(projectRoot.gameObject, "Test_file_for_API_showing_all_features");
        }

        protected override void OnPostProcessLevel(GameObject levelRoot, LdtkJson projectJson)
        {
            Test(levelRoot.gameObject, "Ossuary");
        }
        
        private void Test(GameObject gameObject, string text)
        {
            //return;
            if (gameObject.name == text)
            {
                //Debug.Log(text, projectObj);
                Debug.Log($"CUSTOM post process for 2nd {gameObject.name}\n");
                //gameObject.AddComponent<SpriteRenderer>();
            }
        }

        public override int GetPostprocessOrder()
        {
            return 2;
        }
    }
}
