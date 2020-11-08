using System;
using System.Diagnostics;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.LayerConstruction.UnityAssets.Colliders;
using LDtkUnity.Runtime.LayerConstruction.UnityAssets.Entity;
using LDtkUnity.Runtime.Tools;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Runtime
{
    public class LDtkLevelBuilder : MonoBehaviour
    {
        [SerializeField] private LDtkIntGridTileCollection _collisionTiles = null;
        [SerializeField] private LDtkEntityInstanceCollection _lDtkEntity = null;
        [HorizontalLine]
        [SerializeField] private Grid _collisionTilemapPrefab = null;
        
        public static event Action<LDtkDataLevel> OnLevelBuilt;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            OnLevelBuilt = null;
        }

        public void BuildLevel(LDtkDataLevel lvl)
        {
            string debugName = $"\"{lvl.identifier}\"";
            Stopwatch levelBuildTimer = Stopwatch.StartNew();
            Debug.Log($"LEd: Building level: {debugName}");

            BuildProcess(lvl);
            
            levelBuildTimer.Stop();
            long elapsedMs = levelBuildTimer.ElapsedMilliseconds;
            Debug.Log($"LEd: Built level {debugName} in {(double)elapsedMs/1000} seconds");
            
            OnLevelBuilt?.Invoke(lvl);
        }

        private void BuildProcess(LDtkDataLevel lvl)
        {
            InitStaticTools();
            BuildLayerInstances(lvl);
            DisposeStaticTools();
        }

        private static void InitStaticTools()
        {
            LDtkEnumGetter.Init();
            LDtkTileSpriteGetter.Init();
        }
        private static void DisposeStaticTools()
        {
            LDtkEnumGetter.Dispose();
            LDtkTileSpriteGetter.Dispose();
        }

        private void BuildLayerInstances(LDtkDataLevel lvl)
        {
            foreach (LDtkDataLayerInstance layerInstance in lvl.layerInstances)
            {
                BuildLayerInstance(lvl, layerInstance);
            }
        }

        private void BuildLayerInstance(LDtkDataLevel lvl, LDtkDataLayerInstance layerInstance)
        {
            if (layerInstance.IsIntGridLayer())
            {
                BuildIntGridLayer(lvl, layerInstance);
            }

            if (layerInstance.IsAutoTilesLayer())
            {
                //todo
                //TileBuilder.BuildTileLayerInstances(layerInstance.autoLayerTiles, _tilesets, layerinstance);
            }

            if (layerInstance.IsGridTilesLayer())
            {
                //todo
                //TileBuilder.BuildTileLayerInstances(layerInstance.gridTiles, _tilesets);
            }

            if (layerInstance.IsEntityInstancesLayer())
            {
                BuildEntityInstanceLayer(layerInstance);
            }
        }

        private void BuildEntityInstanceLayer(LDtkDataLayerInstance layerInstance)
        {
            Vector2Int layerSize = new Vector2Int(layerInstance.__cWid, layerInstance.__cHei);
            
            LDtkEntityInstanceBuilder.BuildEntityLayerInstances(layerInstance.entityInstances, _lDtkEntity, layerSize,
                layerInstance.__gridSize);
        }

        private void BuildIntGridLayer(LDtkDataLevel lvl, LDtkDataLayerInstance layerInstance)
        {
            GameObject tileMapObj = CreateNewTilemapLayer(_collisionTilemapPrefab.gameObject, lvl.identifier);
            Tilemap collisionTilemap = tileMapObj.GetComponentInChildren<Tilemap>();

            Vector2Int layerSize = new Vector2Int(layerInstance.__cWid, layerInstance.__cHei);
            LDtkIntGridBuilder.BuildIntGrid(collisionTilemap, _collisionTiles, layerInstance.intGrid, layerSize);
        }


        private GameObject CreateNewTilemapLayer(GameObject prefab, string objName)
        {
            GameObject grid = Instantiate(prefab, transform, true);
            grid.gameObject.name = objName;
            return grid;
        }
        
    }
}
