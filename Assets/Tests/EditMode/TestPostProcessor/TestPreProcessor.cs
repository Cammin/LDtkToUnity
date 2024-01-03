using System.Linq;
using JetBrains.Annotations;
using LDtkUnity.Editor;
using UnityEngine;

namespace LDtkUnity.Tests.TestPostProcessor
{
    [UsedImplicitly]
    public class TestPreProcessor : LDtkPreprocessor
    {
        protected override void OnPreprocessProject(LdtkJson projectJson, string projectName)
        {
            if (projectName != "Test_file_for_API_showing_all_features")
            {
                return;
            }
            
            LayerDefinition layer = projectJson.Defs.Layers.FirstOrDefault();

            if (layer != null)
            {
                Debug.Log(layer.Identifier);
            }
        }

        protected override void OnPreprocessLevel(Level level, LdtkJson projectJson, string projectName)
        {
            if (projectName != "Test_file_for_API_showing_all_features")
            {
                return;
            }   
            
            if (level != null)
            {
                Debug.Log(level.Identifier);
            }
        }
        
        public override int GetPreprocessOrder()
        {
            return 2;
        }
    }
}
