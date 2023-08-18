using UnityEditor;
using UnityEditor.Compilation;

namespace LDtkUnity.Editor
{
    internal static class ForceRecompileAll
    {
        [MenuItem("LDtkUnity/Recompile", false)]
        private static void Recompile()
        {
            CompilationPipeline.RequestScriptCompilation(RequestScriptCompilationOptions.CleanBuildCache);
        }
    }
}