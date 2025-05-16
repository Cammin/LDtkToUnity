using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkFieldDrawerEntityRef : ILDtkHandleDrawer
    {
        private LDtkComponentEntity _entity;
        private LDtkField _field;

        public void OnDrawHandles(LDtkComponentEntity entity, LDtkField field)
        {
            if (!LDtkPrefs.ShowFieldEntityRef)
            {
                return;
            }
            
            _entity = entity;
            _field = field;
            
            GameObject[] refs = GetEntityRefs();
            
            Vector3 pos = _entity.transform.position;

            foreach (GameObject dest in refs)
            {
                if (dest == null)
                {
                    continue;
                }

                //_mode = EditorDisplayMode.RefLinkBetweenCenters;
                
                switch (_field.Definition.EditorDisplayMode)
                {
                    case EditorDisplayMode.RefLinkBetweenCenters:
                        //pos -= (Vector3)_middleCenter; //todo get on this soon
                        break;
                    case EditorDisplayMode.RefLinkBetweenPivots:
                        break;
                }
                
                LDtkHandleDrawerUtil.RenderRefLink(pos, dest.transform.position, _entity.PixelsPerUnit);
            }
        }
        
        private GameObject[] GetEntityRefs()
        {
            if (_field.IsArray)
            {
                LDtkFieldElement[] array = _field.GetArray();
                GameObject[] result = new GameObject[array.Length];
        
                for (int i = 0; i < array.Length; i++)
                {
                    LDtkIid entity = array[i].GetEntityReference().FindEntity();
                    result[i] = entity?.gameObject;
                }
        
                return result;
            }
    
            LDtkIid singleEntity = _field.GetSingle().GetEntityReference().FindEntity();
            return singleEntity 
                ? new[] { singleEntity.gameObject } 
                : Array.Empty<GameObject>();
        }

    }
}