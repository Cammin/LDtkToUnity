using JetBrains.Annotations;
using LDtkUnity.Editor;
using UnityEngine;
using UnityEngine.Profiling;

namespace LDtkUnity.Tests.TestPostProcessor
{
    [UsedImplicitly]
    public class TestPostProcessor : LDtkPostprocessor
    {
        protected override void OnPostprocessProject(GameObject projectRoot)
        {
            Test(projectRoot.gameObject, "Test_file_for_API_showing_all_features");
            

            //BenchmarkTextureLoading();
        }

        private static void BenchmarkTextureLoading()
        {
            int count = 1000000;

            LDtkProfiler.BeginSample("Benchmarks/TextureTest");
            Profiler.BeginSample("whiteTexture");
            for (int i = 0; i < count; i++)
            {
                Texture2D whiteTexture = Texture2D.whiteTexture;
                if (whiteTexture.isReadable)
                {
                }
            }

            Profiler.EndSample();
            Profiler.BeginSample("LoadDefaultTileTexture");
            for (int i = 0; i < count; i++)
            {
                Texture2D tex = LDtkResourcesLoader.LoadDefaultTileTexture();
                if (tex.isReadable)
                {
                }
            }

            Profiler.EndSample();
            LDtkProfiler.EndSample();
            Debug.Log("benchmark printed");
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
    }
}
