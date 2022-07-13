using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    internal class LDtkTileArtifactFactory<T> : LDtkArtifactFactory where T : TileBase
    {
        private readonly Action<T, Sprite> _creationAction;

        public LDtkTileArtifactFactory(AssetImportContext ctx, LDtkArtifactAssets assets, string assetName, Action<T, Sprite> creationAction) : base(ctx, assets, assetName)
        {
            _creationAction = creationAction;
        }
        
        public void TryCreateTile() => TryCreateAsset(Artifacts.HasIndexedTile, CreateTile);

        private T CreateTile()
        {
            Sprite sprite = Artifacts.GetIndexedSprite(AssetName);
            if (sprite == null)
            {
                LDtkDebug.LogError($"Failed to get sprite to create LDtkArtTile; sprite was null for \"{Ctx.assetPath}\"");
                return null;
            }
                
            T newTile = ScriptableObject.CreateInstance<T>();
            newTile.name = AssetName;
            _creationAction?.Invoke(newTile, sprite);

            return newTile;
        }

        protected override bool AddArtifactAction(Object obj)
        {
            return Artifacts.AddTile((TileBase)obj);
        }
    }
}