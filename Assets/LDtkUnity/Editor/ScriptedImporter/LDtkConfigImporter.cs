using UnityEngine;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    [ScriptedImporter(LDtkImporterConsts.CONFIG_VERSION, LDtkImporterConsts.CONFIG_EXT, LDtkImporterConsts.CONFIG_ORDER)]
    internal sealed class LDtkConfigImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            LDtkConfigData data = LDtkConfigData.ReadJson(assetPath);

            LDtkConfig obj = ScriptableObject.CreateInstance<LDtkConfig>();
            obj._data = data;
            
            ctx.AddObjectToAsset("main", obj, LDtkIconUtility.LoadListIcon());
            ctx.SetMainObject(obj);
        }
    }
}