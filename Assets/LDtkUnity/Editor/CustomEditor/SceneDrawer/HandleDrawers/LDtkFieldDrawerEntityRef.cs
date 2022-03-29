using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal class LDtkFieldDrawerEntityRef : ILDtkHandleDrawer
    {
        private readonly LDtkFields _fields;
        private readonly string _identifier;
        private readonly EditorDisplayMode _mode;
        private readonly int _gridSize;
        private readonly Vector2 _middleCenter;
        private readonly Color _smartColor;

        public LDtkFieldDrawerEntityRef(LDtkFields fields, string identifier, EditorDisplayMode mode, int gridSize, Vector2 middleCenter, Color smartColor)
        {
            _fields = fields;
            _identifier = identifier;
            _mode = mode;
            _gridSize = gridSize;
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
                
                LDtkHandleDrawerUtil.RenderRefLink(pos, dest.transform.position, _gridSize);
            }
        }
        
        private GameObject[] GetEntityRefs()
        {
            if (!_fields.ContainsField(_identifier))
            {
                Debug.LogWarning($"Fields component doesn't contain a field called {_identifier}, this should never happen. Try reverting prefab changes", _fields.gameObject);
                return Array.Empty<GameObject>();
            }
            
            if (_fields.IsFieldArray(_identifier))
            {
                return _fields.GetEntityReferenceArray(_identifier);
            }

            GameObject entityRef = _fields.GetEntityReference(_identifier);
            return new[] { entityRef };
        }
    }
}