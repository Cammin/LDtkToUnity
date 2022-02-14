using System;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkFieldDrawerEntityRef : ILDtkHandleDrawer
    {
        private readonly LDtkFields _fields;
        private readonly string _identifier;
        private readonly EditorDisplayMode _mode;
        private readonly int _gridSize;

        public LDtkFieldDrawerEntityRef(LDtkFields fields, string identifier, EditorDisplayMode mode, int gridSize)
        {
            _fields = fields;
            _identifier = identifier;
            _mode = mode;
            _gridSize = gridSize;
        }

        public void OnDrawHandles()
        {
            if (!LDtkPrefs.ShowFieldEntityRef)
            {
                return;
            }
            
            GameObject[] refs = GetEntityRefs();

            _fields.GetSmartColor(out Color smartColor);
            Vector3 pos = _fields.transform.position;

            foreach (GameObject dest in refs)
            {
                if (dest == null)
                {
                    continue;
                }

                float thickness = LDtkPrefs.FieldEntityRefThickness;
                
                LDtkHandleDrawerUtil.RenderRefLink(pos, dest.transform.position, _gridSize);
                //DrawRefLink(pos, dest.transform.position, smartColor, width: thickness);
            }       
            
            //for now, we're not worrying about the precise points. but will look at later //todo
            /*switch (_mode)
            {
                case EditorDisplayMode.RefLinkBetweenCenters:
 
                    break;
                case EditorDisplayMode.RefLinkBetweenPivots:
                    //LDtkCoordConverter.EntityPivotOffset()
                    break;
            }*/
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

        //todo cleanup this file and other folder contents
        //Original code from: https://github.com/deepnight/ldtk/blob/51819b99e0aa83e20d56500569657b03bd3e54c1/src/electron.renderer/display/FieldInstanceRender.hx#L21
        private static void DrawRefLink(Vector3 from, Vector3 to, Color color, float dashLen = 0.3f, float off = 0.15f, float width = 10)
        {
            float fx = from.x;
            float fy = from.y;
            float tx = to.x;
            float ty = to.y;
        
            float angle = Mathf.Atan2(ty - fy, tx - fx);
            float length = Vector2.Distance((Vector2)from, (Vector2)to);
            int count = Mathf.CeilToInt(length / dashLen);
            dashLen = length / count;
        
            int sign = -1;
            float x = fx;
            float y = fy;
            float z = from.z;
        
            for (int n = 0; n < count; n++)
            {
                float r = (float)n/(count-1);

                Vector3 subStart = new Vector3(x, y, z);
                x = fx + Mathf.Cos(angle) * (n * dashLen) + Mathf.Cos(angle + Mathf.PI / 2) * sign * off * (1 - r);
                y = fy + Mathf.Sin(angle) * (n * dashLen) + Mathf.Sin(angle + Mathf.PI / 2) * sign * off * (1 - r);
                z = Vector3.Lerp(from, to, r).z;
                Vector3 subEnd = new Vector3(x, y, z);
            
                Color subColor = color;
                subColor.a = (0.15f + 0.85f * (1 - r)) * color.a;
            
                DrawLine(subStart, subEnd, subColor);
                sign = -sign;
            }
        
            //final line
            Vector3 lastStart = new Vector3(x, y, z);
            Color lastColor = color;
            lastColor.a = 0.1f * color.a;
            DrawLine(lastStart, to, lastColor);
            
            void DrawLine(Vector3 subFrom, Vector3 subTo, Color subColor)
            {
                Handles.color = subColor;
                HandleAAUtil.DrawAALine(subFrom, subTo, width);
            }
        }
    }
}