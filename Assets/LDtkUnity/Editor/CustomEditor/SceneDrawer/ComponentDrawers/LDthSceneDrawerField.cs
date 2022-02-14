using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public static class LDtkSceneDrawerField
    {
        public static void Draw(List<LDtkEntityDrawerComponent> entityComponents)
        {
            List<LDtkFieldDrawerData> datas = entityComponents.SelectMany(component => component.FieldDrawers).ToList();

            foreach (LDtkFieldDrawerData data in datas)
            {
                ProcessData(data);
            }
        }
        private static void ProcessData(LDtkFieldDrawerData data)
        {
            if (!data.Enabled)
            {
                return;
            }
            
            Handles.color = data.GizmoColor;
            ILDtkHandleDrawer drawer = DrawField(data);
            drawer?.OnDrawHandles();
        }

        private static ILDtkHandleDrawer DrawField(LDtkFieldDrawerData data)
        {
            if (data.Fields == null)
            {
                Debug.LogError("LDtk: Source is null, not drawing");
                return null;
            }
            
            switch (data.FieldMode)
            {
                case EditorDisplayMode.Hidden: 
                    //show nothing
                    break;
                    
                case EditorDisplayMode.ValueOnly: //display value (enum value could show image) todo
                case EditorDisplayMode.NameAndValue: //display identifier then value (enum value could show image) //todo
                    //todo choose to show more later? like an icon in a smaller size maybe?
                    return new LDtkFieldDrawerValue(data.Fields.transform.position + Vector3.up, data.Identifier);
                    
                case EditorDisplayMode.EntityTile: //If this is the case, then it simply overrides the data in the root entity. not here. so we draw from the entity data instead
                    break;//return new LDtkEntityDrawerIcon(data.Fields.transform, data.IconTex, data.IconRect);

                case EditorDisplayMode.PointPath:
                case EditorDisplayMode.PointStar:
                case EditorDisplayMode.PointPathLoop:
                case EditorDisplayMode.Points:
                    return new LDtkFieldDrawerPoints(data.Fields, data.Identifier, data.FieldMode, data.MiddleCenter, data.GridSize);
                    
                case EditorDisplayMode.RadiusPx:
                case EditorDisplayMode.RadiusGrid:
                    return new LDtkFieldDrawerRadius(data.Fields, data.Identifier, data.FieldMode, data.GridSize);

                case EditorDisplayMode.ArrayCountNoLabel:
                case EditorDisplayMode.ArrayCountWithLabel:
                    //todo address this. draw the array or array size
                    break;
                    
                case EditorDisplayMode.RefLinkBetweenCenters:
                case EditorDisplayMode.RefLinkBetweenPivots:
                    return new LDtkFieldDrawerEntityRef(data.Fields, data.Identifier, data.FieldMode, data.GridSize);

                default:
                    return null;
            }

            return null;
        }
    }
}