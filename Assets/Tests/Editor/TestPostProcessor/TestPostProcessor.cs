using LDtkUnity;
using LDtkUnity.Editor;
using UnityEngine;

namespace Tests.Editor
{
    public class TestPostProcessor : LDtkPostprocessor
    {
        protected override void OnPostProcessProject(GameObject projectObj, LdtkJson project)
        {
            Debug.Log($"CUSTOM post process for {projectObj.name}", projectObj);
        
            if (projectObj.name == "Test_file_for_API_showing_all_features")
            {
                Debug.Log("IS TEST_API");
                projectObj.AddComponent<SpriteRenderer>();
            }
        
        }

        protected override void OnPostProcessEntity(GameObject entityObj, EntityInstance entity)
        {
        }
    }
}
