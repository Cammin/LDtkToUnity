using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkFieldDrawerEntityRef : ILDtkHandleDrawer
    {
        private readonly LDtkFields _fields;
        private readonly string _identifier;
        private EditorDisplayMode _mode;
        private readonly int _gridSize;
        private readonly Vector2 _middleCenter;

        public LDtkFieldDrawerEntityRef(LDtkFields fields, string identifier, EditorDisplayMode mode, int gridSize, Vector2 middleCenter)
        {
            _fields = fields;
            _identifier = identifier;
            _mode = mode;
            _gridSize = gridSize;
            _middleCenter = middleCenter;
        }

        public void OnDrawHandles()
        {
            if (!LDtkPrefs.ShowFieldEntityRef)
            {
                return;
            }
            
            GameObject[] refs = GetEntityRefs();

            _fields.GetSmartColor(out Color smartColor); //todo this does not account for the entity's default color, add at a later date.
            Handles.color = smartColor;
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
            if (_fields.IsFieldArray(_identifier))
            {
                return _fields.GetEntityReferenceArray(_identifier);
            }

            GameObject entityRef = _fields.GetEntityReference(_identifier);
            return new[] { entityRef };
        }
    }
}