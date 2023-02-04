using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace LDtkUnity.Tests
{
    [InitializeOnLoad]
    public static class RunTestsOnRecompile
    {
        static RunTestsOnRecompile()
        {
            AssemblyReloadEvents.afterAssemblyReload += OnRecompile;
        }
        private static void OnRecompile()
        {
            RunTests();
        }

        private static void RunTests()
        {
            TestRunnerApi runner = ScriptableObject.CreateInstance<TestRunnerApi>();
            
            //LogTests(runner);
            
            
            Filter filter = new Filter()
            {
                testNames = new []
                {
                    //"LDtkUnity.Tests.TestJsonDigging.GetTilesetRelPaths",
                    "LDtkUnity.Tests.TestJsonDigging.GetUsedEntities(\"Assets/Samples/Samples/Entities.ldtk\")",
                },
                testMode = TestMode.EditMode
            };

            //runner.Execute(new ExecutionSettings(filter));
        }

        private static void LogTests(TestRunnerApi runner)
        {
            runner.RetrieveTestList(TestMode.EditMode, (testRoot) =>
            {
                Debug.Log($"Tree contains {testRoot.TestCaseCount} tests.");
                SearchChildren(testRoot);

                void SearchChildren(ITestAdaptor testObject)
                {
                    if (!testObject.HasChildren)
                    {
                        Debug.Log(testObject.FullName);
                        return;
                    }

                    foreach (ITestAdaptor child in testObject.Children)
                    {
                        SearchChildren(child);
                    }
                }
            });
        }
    }
}