using JetBrains.Annotations;
using LDtkUnity.Editor;
using UnityEngine;

namespace LDtkUnity.Tests.TestPostProcessor
{
    [UsedImplicitly]
    public class TestPostProcessor : LDtkPostprocessor
    {
        protected override void OnPostprocessProject(GameObject projectRoot)
        {
            Debug.Assert(ImportContext != null, "Should always have asset import context");
            
            //Debug.Log($"Postprocess project \"{projectRoot}\" at path \"{ImportContext.assetPath}\"");
            
            //Test(projectRoot.gameObject, "Test_file_for_API_showing_all_features");
        }

        protected override void OnPostprocessLevel(GameObject levelRoot, LdtkJson projectJson)
        {
            Test(levelRoot.gameObject, "Ossuary");
        }
        
        private void Test(GameObject projectObj, string text)
        {
            if (projectObj.name == text)
            {
                //Debug.Log(text, projectObj);
                //Debug.Log($"CUSTOM post process for {projectObj.name}\n");
                projectObj.AddComponent<SpriteRenderer>();
            }
        }

        public override int GetPostprocessOrder()
        {
            return 2;
        }
    }
}
