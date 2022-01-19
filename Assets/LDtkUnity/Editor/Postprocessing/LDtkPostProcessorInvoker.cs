using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkPostProcessorInvoker
    {
        private static List<LDtkPostprocessor> _postprocessors;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            _postprocessors = null; //todo consider if this should even be used
        }
        
        private static void InitPostprocessors()
        {
            IEnumerable<Type> postprocessors = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(LDtkPostprocessor)));
            
            foreach (Type assetPostprocessorClass in postprocessors)
            {
                try
                {
                    LDtkPostprocessor assetPostprocessor = Activator.CreateInstance(assetPostprocessorClass) as LDtkPostprocessor;
                    _postprocessors.Add(assetPostprocessor);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            
            _postprocessors.Sort(new LDtkPostprocessorImportOrderComparer());
        }
        
        private static void CallPostProcessMethods(string methodName, object[] args)
        {
            if (_postprocessors == null)
            {
                _postprocessors = new List<LDtkPostprocessor>();
                InitPostprocessors();
            }
            
            foreach (LDtkPostprocessor target in _postprocessors)
            {
                //Debug.Log($"LDtk: Postprocess {target.GetType().Name}.{methodName}");
                InvokeMethodIfAvailable(target, methodName, args);
            }
        }
        
        private static bool InvokeMethodIfAvailable(object target, string methodName, object[] args)
        {
            Type targetType = target.GetType();
            MethodInfo method = targetType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
            {
                return false;
            }

            try
            {
                method.Invoke(target, args);
            }
            catch (Exception e)
            {
                int length = "System.Reflection.TargetInvocationException: Exception has been thrown by the target of an invocation. ---> ".Length;
                string s = e.ToString().Substring(length);
                
                GameObject gameObject = (GameObject)args[0];
                
                string gameObjectContext = "";
                if (gameObject)
                {
                    //there could be no parent either because we are the project, or are a level imported by the .ldtkl file
                    //we could have a parent only if we are a level beneath the project
                    Transform parent = gameObject.transform.parent;
                    string detail = parent ? $"level \"{gameObject.name}\" of project \"{parent.name}\"" : $"{gameObject.name}";
                    gameObjectContext = $" on {detail}";
                }

                MonoScript script = FindMonoScript(targetType);
                Debug.LogError($"LDtk: An exception occurred while postprocessing \"{targetType.Name}.{methodName}\" on {gameObjectContext}\n{s}", script);

                return false;
            }
            
            return true;
        }

        private static MonoScript FindMonoScript(Type type)
        {
            string[] findAssets = AssetDatabase.FindAssets($"t:MonoScript {type.Name}");
            if (findAssets.IsNullOrEmpty())
            {
                return null;
            }
            
            foreach (string find in findAssets)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(find);
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
                if (script)
                {
                    return script;
                }
            }

            return null;
        }

        public static void PostProcessProject(GameObject projectObj) => CallPostProcessMethods(LDtkPostprocessor.METHOD_PROJECT, new object[]{projectObj});
        public static void PostProcessLevel(GameObject levelObj, LdtkJson projectJson) => CallPostProcessMethods(LDtkPostprocessor.METHOD_LEVEL, new object[]{levelObj, projectJson});
    }
}