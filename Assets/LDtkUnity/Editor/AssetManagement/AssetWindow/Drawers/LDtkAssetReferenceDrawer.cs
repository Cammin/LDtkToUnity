using LDtkUnity.Runtime.Data;
using UnityEngine;

namespace LDtkUnity.Editor.AssetManagement.AssetWindow.Drawers
{
    public abstract class LDtkAssetReferenceDrawer<TData, TAsset> : LDtkReferenceDrawer<TData> where TData : ILDtkIdentifier
    {
        protected TAsset Asset { get; set; }

        protected LDtkAssetReferenceDrawer(TData data, TAsset asset)
        {
            Asset = asset;
        }
    }
}