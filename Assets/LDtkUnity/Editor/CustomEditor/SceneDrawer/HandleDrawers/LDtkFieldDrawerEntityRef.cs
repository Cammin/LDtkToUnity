using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkFieldDrawerEntityRef : ILDtkHandleDrawer
    {
        private readonly LDtkFields _fields;
        private readonly string _identifier;
        private readonly EditorDisplayMode _mode;
        private readonly int _pixelsPerUnit;
        private readonly Vector2 _middleCenter;
        private readonly Color _smartColor;

        public LDtkFieldDrawerEntityRef(LDtkFields fields, string identifier, EditorDisplayMode mode, int pixelsPerUnit, Vector2 middleCenter, Color smartColor)
        {
            _fields = fields;
            _identifier = identifier;
            _mode = mode;
            _pixelsPerUnit = pixelsPerUnit;
            _middleCenter = middleCenter;
            _smartColor = smartColor;
        }

        public void OnDrawHandles()
        {
            if (!LDtkPrefs.ShowFieldEntityRef)
            {
                return;
            }
            
            GameObject[] refs = GetEntityRefs();


            Handles.color = _smartColor;
            Vector3 pos = _fields.transform.position;

            foreach (GameObject dest in refs)
            {
                if (dest == null)
                {
                    continue;
                }

                //_mode = EditorDisplayMode.RefLinkBetweenCenters;
                
                switch (_mode)
                {
                    case EditorDisplayMode.RefLinkBetweenCenters:
                        //pos -= (Vector3)_middleCenter; //todo get on this soon
                        break;
                    case EditorDisplayMode.RefLinkBetweenPivots:
                        break;
                }
                
                LDtkHandleDrawerUtil.RenderRefLink(pos, dest.transform.position, _pixelsPerUnit);
            }
        }
        
        private GameObject[] GetEntityRefs()
        {
            if (!_fields.ContainsField(_identifier))
            {
                LDtkDebug.LogWarning($"Fields component doesn't contain a field called {_identifier}, this should never happen. Try reverting prefab changes", _fields.gameObject);
                return Array.Empty<GameObject>();
            }
            
            if (_fields.IsFieldArray(_identifier))
            {
                return _fields.GetEntityReferenceArray(_identifier).Select(p =>
                {
                    LDtkIid foundEntity = p.FindEntity();
                    return foundEntity == null ? null : foundEntity.gameObject;
                }).ToArray();
            }
            
            LDtkIid iid = _fields.GetEntityReference(_identifier).FindEntity();
            
            //it's possible that the object doesnt exist if the entity was in another level for example.
            if (iid == null)
            {
                return Array.Empty<GameObject>();
            }
            
            GameObject entityRef = iid.gameObject;
            return new[] { entityRef };
        }
    }
}