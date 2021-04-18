using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public abstract class LDtkRelativeAssetFinder<TData, TAsset> where TAsset : Object
    {
        protected abstract string GetRelPath(TData definition);

        public TAsset[] GetRelativeAssets(TData[] defs, Object relativeTo)
        {
            return defs.Select(def => LDtkPathUtil.GetAssetRelativeToAsset<TAsset>(relativeTo, GetRelPath(def))).ToArray();
        }
        public TAsset[] GetRelativeAssets(TData[] defs, string assetPath)
        {
            return defs.Select(def => LDtkPathUtil.GetAssetRelativeToAssetPath<TAsset>(assetPath, GetRelPath(def))).ToArray();
        }
    }
}