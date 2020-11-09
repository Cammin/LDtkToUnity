using System;
using System.Diagnostics;
using LDtkUnity.Runtime.Data.Level;
using LDtkUnity.Runtime.LayerConstruction.UnityAssets.Colliders;
using LDtkUnity.Runtime.LayerConstruction.UnityAssets.Entity;
using LDtkUnity.Runtime.LayerConstruction.UnityAssets.Settings;
using LDtkUnity.Runtime.LayerConstruction.UnityAssets.Tileset;
using LDtkUnity.Runtime.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Runtime
{
    public class LDtkLevelBuilder : MonoBehaviour
    {
        [SerializeField] private LDtkProjectAssets _projectAssets;
        
        
        public static event Action<LDtkDataLevel> OnLevelBuilt;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            OnLevelBuilt = null;
        }

        public void BuildLevel(LDtkDataLevel lvl)
        {
            if (_projectAssets == null)
            {
                Debug.LogError("Project Assets Object is null; not building level.");
                return;
            }
            
            string debugName = $"\"{lvl.identifier}\"";
            Stopwatch levelBuildTimer = Stopwatch.StartNew();
            Debug.Log($"LDtk: Building level: {debugName}");

            BuildProcess(lvl);
            
            levelBuildTimer.Stop();
            long elapsedMs = levelBuildTimer.ElapsedMilliseconds;
            Debug.Log($"LDtk: Built level {debugName} in {(double)elapsedMs/1000} seconds");
            
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
                BuildLayerInstance(layerInstance);
            }
        }

        private void BuildLayerInstance(LDtkDataLayerInstance layerInstance)
        {
            if (layerInstance.IsIntGridLayer())
            {
                BuildIntGridLayer(layerInstance);
            }

            if (layerInstance.IsAutoTilesLayer())
            {
                BuildTilesetLayer(layerInstance.autoLayerTiles);
            }

            if (layerInstance.IsGridTilesLayer())
            {
                BuildTilesetLayer(layerInstance.gridTiles);
            }

            if (layerInstance.IsEntityInstancesLayer())
            {
                BuildEntityInstanceLayer(layerInstance);
            }
        }

        private void BuildIntGridLayer(LDtkDataLayerInstance intGridLayer)
        {
            GameObject tileMapObj = CreateNewTilemapComponent(_projectAssets.CollisionTilemapPrefab.gameObject, intGridLayer.__identifier);
            Tilemap collisionTilemap = tileMapObj.GetComponentInChildren<Tilemap>();

            Vector2Int layerSize = new Vector2Int(intGridLayer.__cWid, intGridLayer.__cHei);
            LDtkIntGridBuilder.BuildIntGrid(collisionTilemap, _projectAssets.CollisionTiles, intGridLayer.intGrid, layerSize);
        }

        private void BuildTilesetLayer(LDtkDataTileInstance[] tiles)
        {
            LDtkTileBuilder.BuildTileLayerInstances(tiles, _projectAssets.Tilesets);
        }
        
        private void BuildEntityInstanceLayer(LDtkDataLayerInstance entityInstanceLayer)
        {
            Vector2Int layerSize = new Vector2Int(entityInstanceLayer.__cWid, entityInstanceLayer.__cHei);
            
            LDtkEntityInstanceBuilder.BuildEntityLayerInstances(entityInstanceLayer.entityInstances, _projectAssets.EntityInstances, layerSize,
                entityInstanceLayer.__gridSize);
        }
        

        private GameObject CreateNewTilemapComponent(GameObject prefab, string objName)
        {
            GameObject grid = Instantiate(prefab, transform, true);
            grid.gameObject.name = objName;
            return grid;
        }
        
    }
}
