using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LDtkUnity.Editor
{
    public static class LDtkPostProcessorInvoker
    {
        private static List<LDtkPostprocessor> _postprocessors;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            _postprocessors = null;
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
        }

        private static void CallPostProcessMethods(string methodName, object[] args)
        {
            if (_postprocessors == null)
            {
                _postprocessors = new List<LDtkPostprocessor>();
                InitPostprocessors();
            }
            
            foreach (LDtkPostprocessor inst in _postprocessors)
            {
                InvokeMethodIfAvailable(inst, methodName, args);
            }
        }
        
        private static bool InvokeMethodIfAvailable(object target, string methodName, object[] args)
        {
            MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
            {
                return false;
            }
            
            method.Invoke(target, args);
            return true;
        }
        
        public static void PostProcessProject(GameObject projectObj, LdtkJson project) => CallPostProcessMethods(LDtkPostprocessor.METHOD_PROJECT, new object[]{projectObj, project});
        public static void PostProcessLevel(GameObject levelObj, Level level) => CallPostProcessMethods(LDtkPostprocessor.METHOD_LEVEL, new object[]{levelObj, level});
        public static void PostProcessBackground(GameObject backgroundObj) => CallPostProcessMethods(LDtkPostprocessor.METHOD_BACKGROUND, new object[]{backgroundObj});
        public static void PostProcessEntity(GameObject entityObj, EntityInstance entity) => CallPostProcessMethods(LDtkPostprocessor.METHOD_ENTITY, new object[]{entityObj, entity});
        public static void PostProcessIntGridLayer(GameObject layerObj, LayerInstance layer, Tilemap[] tilemaps) => CallPostProcessMethods(LDtkPostprocessor.METHOD_INT_GRID_LAYER, new object[]{layerObj, layer, tilemaps});
        public static void PostProcessAutoLayer(GameObject layerObj, LayerInstance layer, Tilemap[] tilemaps) => CallPostProcessMethods(LDtkPostprocessor.METHOD_AUTO_LAYER, new object[]{layerObj, layer, tilemaps});
    }
}