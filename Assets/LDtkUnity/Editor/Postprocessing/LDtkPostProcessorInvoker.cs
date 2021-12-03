using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public static class LDtkPostProcessorInvoker
    {
        private static List<LDtkPostprocessor> _postprocessors;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset()
        {
            //_postprocessors = null; //todo consider if this should even be used
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
            
            _postprocessors.Sort(new CompareAssetImportPriority());
        }
        
        private class CompareAssetImportPriority : IComparer<LDtkPostprocessor>
        {
            /*int IComparer.Compare(System.Object xo, System.Object yo)
            {
                int x = ((LDtkPostprocessor)xo).GetPostprocessOrder();
                int y = ((LDtkPostprocessor)yo).GetPostprocessOrder();
                return x.CompareTo(y);
            }*/

            public int Compare(LDtkPostprocessor xo, LDtkPostprocessor yo)
            {
                int x = xo.GetPostprocessOrder();
                int y = yo.GetPostprocessOrder();
                return x.CompareTo(y);
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
                Debug.Log($"LDtk: Postprocess {methodName} via {inst.GetType()}");
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
        public static void PostProcessBackgroundColor(GameObject backgroundObj) => CallPostProcessMethods(LDtkPostprocessor.METHOD_BACKGROUND_COLOR, new object[]{backgroundObj});
        public static void PostProcessBackgroundTexture(GameObject backgroundObj) => CallPostProcessMethods(LDtkPostprocessor.METHOD_BACKGROUND_TEXTURE, new object[]{backgroundObj});
        public static void PostProcessEntity(GameObject entityObj, EntityInstance entity) => CallPostProcessMethods(LDtkPostprocessor.METHOD_ENTITY, new object[]{entityObj, entity});
        public static void PostProcessIntGridLayer(GameObject layerObj, LayerInstance layer, Tilemap[] tilemaps) => CallPostProcessMethods(LDtkPostprocessor.METHOD_INT_GRID_LAYER, new object[]{layerObj, layer, tilemaps});
        public static void PostProcessTilesetLayer(GameObject layerObj, LayerInstance layer, Tilemap[] tilemaps) => CallPostProcessMethods(LDtkPostprocessor.METHOD_AUTO_LAYER, new object[]{layerObj, layer, tilemaps});
    }
}