using System;
using UnityEngine;

namespace LDtkUnity
{
    public class LDtkBuilderEntity : LDtkLayerBuilder
    {
        public LDtkBuilderEntity(LayerInstance layer, LDtkProject project) : base(layer, project)
        {
        }
        
        public GameObject BuildEntityLayerInstances(int layerSortingOrder)
        {
            LDtkParsedPoint.InformOfRecentLayerVerticalCellCount(Layer.UnityWorldPosition, (int)Layer.CHei);
            GameObject layerObj = new GameObject(Layer.Identifier);
            
            foreach (EntityInstance entityData in Layer.EntityInstances)
            {
                GameObject entityPrefab = Project.GetEntity(entityData.Identifier);
                if (entityPrefab == null)
                {
                    continue;
                }

                BuildEntityInstance(entityData, entityPrefab, layerObj, layerSortingOrder);
            }

            return layerObj;
        }

        private GameObject BuildEntityInstance(EntityInstance entityData, GameObject entityPrefab, GameObject layerObj, int layerSortingOrder)
        {
            GameObject entityObj = LDtkPrefabFactory.Instantiate(entityPrefab);
            entityObj.name = entityPrefab.name;
            
            entityObj.transform.parent = layerObj.transform;
            entityObj.transform.localPosition = LDtkToolOriginCoordConverter.EntityLocalPosition(entityData.UnityPx, (int)Layer.LevelReference.PxHei, (int)Layer.GridSize);
            
            //modify by the resized entity scaling from LDtk
            Vector3 newScale = entityObj.transform.localScale;
            newScale.Scale(entityData.UnityScale);
            entityObj.transform.localScale = newScale;

            LDtkFieldInjector.InjectEntityFields(entityData, entityObj, (int)Layer.GridSize);

            MonoBehaviour[] behaviors = entityObj.GetComponents<MonoBehaviour>();
            
            PostEntityInterfaceEvent<ILDtkFieldInjectedEvent>(behaviors, e => e.OnLDtkFieldsInjected());
            PostEntityInterfaceEvent<ILDtkSettableSortingOrder>(behaviors, e => e.OnLDtkSetSortingOrder(layerSortingOrder));
            PostEntityInterfaceEvent<ILDtkSettableColor>(behaviors, e => e.OnLDtkSetEntityColor(entityData.Definition.UnityColor));
            PostEntityInterfaceEvent<ILDtkSettableOpacity>(behaviors, e => e.OnLDtkSetOpacity((float)Layer.Opacity));

            foreach (MonoBehaviour monoBehaviour in behaviors)
            {
                LDtkEditorUtil.Dirty(monoBehaviour);
            }
            
            //record transform for the object's scale
            LDtkEditorUtil.Dirty(entityObj.transform);
            LDtkEditorUtil.Dirty(entityObj);
            
            return entityObj;
        }



        private void PostEntityInterfaceEvent<T>(MonoBehaviour[] behaviors, Action<T> action)
        {
            foreach (MonoBehaviour component in behaviors)
            {
                if (!(component is T thing)) continue;
                
                try
                {
                    action.Invoke(thing);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
    }
}