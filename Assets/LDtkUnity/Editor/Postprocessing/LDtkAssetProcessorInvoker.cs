using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// This class gets all types derived from pre/post processors. When gathered types, creates instances of them and adds the actions to the action cache
    /// </summary>
    internal static class LDtkAssetProcessorInvoker
    {
        private static List<LDtkPreprocessor> _preprocessors;
        private static List<LDtkPostprocessor> _postprocessors;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            _preprocessors = null;
            _postprocessors = null;
        }
        
        public static void AddPreProcessProject(LDtkAssetProcessorActionCache cache, LdtkJson projectJson, string projectName) => 
            AddPreprocessActions(cache, LDtkPreprocessor.METHOD_PROJECT, new object[]{projectJson, projectName});
        
        public static void AddPreProcessLevel(LDtkAssetProcessorActionCache cache, Level levelJson, LdtkJson projectJson, string projectName) => 
            AddPreprocessActions(cache, LDtkPreprocessor.METHOD_LEVEL, new object[]{levelJson, projectJson, projectName});
        
        public static void AddPostProcessProject(LDtkAssetProcessorActionCache cache, LDtkJsonImporter importer, GameObject projectObj) => 
            AddPostprocessAction(cache, importer, LDtkPostprocessor.METHOD_PROJECT, new object[]{projectObj});
        
        public static void AddPostProcessLevel(LDtkAssetProcessorActionCache cache, LDtkJsonImporter importer, GameObject levelObj, LdtkJson projectJson) => 
            AddPostprocessAction(cache, importer, LDtkPostprocessor.METHOD_LEVEL, new object[]{levelObj, projectJson});

        private static void AddPreprocessActions(LDtkAssetProcessorActionCache cache, string methodName, object[] args)
        {
            TryInstantiatePreprocessors();
            
            foreach (LDtkPreprocessor preprocessor in _preprocessors)
            {
                cache.AddProcessAction(preprocessor.GetPreprocessOrder(), () =>
                {
                    InvokeMethodIfAvailable(preprocessor, methodName, args);
                }, $"Preprocessor\t<{methodName}>\t({preprocessor.GetType().Name})");
            }
        }
        
        private static void AddPostprocessAction(LDtkAssetProcessorActionCache cache, LDtkJsonImporter importer, string methodName, object[] args)
        {
            TryInstantiatePostprocessors();
            
            foreach (LDtkPostprocessor postprocessor in _postprocessors)
            {
                cache.AddProcessAction(postprocessor.GetPostprocessOrder(), () =>
                { 
                    postprocessor._importContext = importer.ImportContext;
                    InvokeMethodIfAvailable(postprocessor, methodName, args);
                    postprocessor._importContext = null;
                }, $"Postprocessor\t<{methodName}>\t({postprocessor.GetType().Name})");
            }
        }

        private static void TryInstantiatePreprocessors()
        {
            if (_preprocessors != null)
            {
                return;
            }
            
            _preprocessors = new List<LDtkPreprocessor>();
            TypeCache.TypeCollection preprocessors = TypeCache.GetTypesDerivedFrom<LDtkPreprocessor>();
            foreach (Type pre in preprocessors)
            {
                try
                {
                    LDtkPreprocessor preprocessor = Activator.CreateInstance(pre) as LDtkPreprocessor;
                    _preprocessors.Add(preprocessor);
                }
                catch (Exception e)
                {
                    LDtkDebug.LogError(e.ToString());
                }
            }
        }

        private static void TryInstantiatePostprocessors()
        {
            if (_postprocessors != null)
            {
                return;
            }
            
            _postprocessors = new List<LDtkPostprocessor>();
            TypeCache.TypeCollection postprocessors = TypeCache.GetTypesDerivedFrom<LDtkPostprocessor>();
            foreach (Type post in postprocessors)
            {
                try
                {
                    LDtkPostprocessor assetPostprocessor = Activator.CreateInstance(post) as LDtkPostprocessor;
                    _postprocessors.Add(assetPostprocessor);
                }
                catch (Exception e)
                {
                    LDtkDebug.LogError(e.ToString());
                }
            }
        }
        
        private static bool InvokeMethodIfAvailable(object processor, string methodName, object[] args)
        {
            Type targetType = processor.GetType();
            MethodInfo method = targetType.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
            {
                return false;
            }

            try
            {
                method.Invoke(processor, args);
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
                LDtkDebug.LogError($"An exception occurred while pre/post processing \"{targetType.Name}.{methodName}\" on {gameObjectContext}\n{s}", script);

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
    }
}