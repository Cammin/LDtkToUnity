using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Starting class from where scene drawing is started
    /// </summary>
    internal static class LDtkSceneDrawer
    {
        private static readonly LDtkLevelDrawer LevelDrawer = new LDtkLevelDrawer();
        
        private static readonly LDtkFieldDrawerPoints FieldDrawerPoints = new LDtkFieldDrawerPoints();
        private static readonly LDtkFieldDrawerRadius FieldDrawerRadius = new LDtkFieldDrawerRadius();
        private static readonly LDtkFieldDrawerEntityRef FieldDrawerEntityRef = new LDtkFieldDrawerEntityRef();
        
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
        private static void DrawGizmos(Transform objectTransform, GizmoType gizmoType)
        {
            LDtkProfiler.BeginSample("LDtkSceneGizmos");
            DrawSceneGizmos(objectTransform);
            LDtkProfiler.EndSample();
        }

        private static void DrawSceneGizmos(Transform objectTransform)
        {
            if (objectTransform.TryGetComponent<LDtkComponentLevel>(out var lvl))
            {
                LDtkProfiler.BeginSample("LDtkSceneDrawerLevel");
                LevelDrawer.DrawHandles(lvl);
                LDtkProfiler.EndSample();
                return;
            }

            if (objectTransform.TryGetComponent<LDtkComponentEntity>(out var entity))
            {
                //handles color applies for the whole entity including fields
                Handles.color = AdjustGizmoColor(entity.SmartColor);
                
                LDtkProfiler.BeginSample("LDtkSceneDrawerEntity");
                LDtkSceneDrawerEntity.DrawEntityDrawer(entity);
                LDtkProfiler.EndSample();
                
                if (entity.FieldInstances)
                {
                    LDtkProfiler.BeginSample("LDtkSceneDrawerField");
                    foreach (LDtkField field in entity.FieldInstances.Fields)
                    {
                        if (!ShouldDraw(field)) continue;

                        ILDtkHandleDrawer fieldDrawer = GetFieldDrawer(field);
                        fieldDrawer?.OnDrawHandles(entity, field);
                    }
                    LDtkProfiler.EndSample();
                }
            }
        }
        
        private static Color AdjustGizmoColor(Color color)
        {
            Color c = color;
            c.a = 0.66f;
            const float incrementDifference = -0.1f;
            c.r += incrementDifference;
            c.g += incrementDifference;
            c.b += incrementDifference;
            return c;
        }
        
        /// <summary>
        /// An early return to not draw.
        /// </summary>
        //todo should think about this differently. assert in the specific drawing sections?
        private static bool ShouldDraw(LDtkField field)
        {
            EditorDisplayMode? mode = field.Definition.EditorDisplayMode;
            
            switch (mode)
            {
                case EditorDisplayMode.Hidden: //do not show
                    return false;
                
                case EditorDisplayMode.ValueOnly: //all but point/point array
                    return field.Type != LDtkFieldType.Point;
                    
                case EditorDisplayMode.NameAndValue: //all
                    return true;
                    
                case EditorDisplayMode.EntityTile: //enum/enum array, tile/tile array
                    return field.Type == LDtkFieldType.Enum || field.Type == LDtkFieldType.Tile;

                case EditorDisplayMode.RadiusGrid: //int, float
                case EditorDisplayMode.RadiusPx: //int, float
                    return field.Type == LDtkFieldType.Int || field.Type == LDtkFieldType.Float;

                case EditorDisplayMode.PointStar: //point, point array
                case EditorDisplayMode.Points: //point, point array
                    return field.Type == LDtkFieldType.Point;
                
                case EditorDisplayMode.PointPath: //point array only
                case EditorDisplayMode.PointPathLoop: //point array only
                    return field.Type == LDtkFieldType.Point && field.Definition.IsArray;
                
                case EditorDisplayMode.ArrayCountNoLabel: //any arrays
                case EditorDisplayMode.ArrayCountWithLabel: //any arrays
                    return field.Definition.IsArray;
                    
                case EditorDisplayMode.RefLinkBetweenCenters: //entity ref, entity ref array
                case EditorDisplayMode.RefLinkBetweenPivots: //entity ref, entity ref array
                    return field.Type == LDtkFieldType.EntityRef;
                    
                default:
                    LDtkDebug.LogError("No Drawer eligibility found!");
                    return false;
            }
        }

        private static ILDtkHandleDrawer GetFieldDrawer(LDtkField field)
        {
            switch (field.Definition.EditorDisplayMode)
            {
                case EditorDisplayMode.Hidden: 
                    //show nothing
                    break;
                    
                case EditorDisplayMode.ValueOnly: //display value (enum value could show image) todo
                    break;
                    
                case EditorDisplayMode.NameAndValue: //display identifier then value (enum value could show image) //todo
                    //todo choose to show more later? like an icon in a smaller size maybe?
                    //return new LDtkFieldDrawerValue(data.Entity.transform.position + Vector3.up, data.Field.Identifier);
                    
                case EditorDisplayMode.EntityTile: 
                    //this is actually handled in the entity drawer, not here. The only reason why is because we want the entity identifier to render a little bit lower if the icon exists. also certain responsibilities handled there
                    break;

                case EditorDisplayMode.PointPath:
                case EditorDisplayMode.PointStar:
                case EditorDisplayMode.PointPathLoop:
                case EditorDisplayMode.Points:
                    return FieldDrawerPoints;
                    
                case EditorDisplayMode.RadiusPx:
                case EditorDisplayMode.RadiusGrid:
                    return FieldDrawerRadius;

                case EditorDisplayMode.ArrayCountNoLabel:
                case EditorDisplayMode.ArrayCountWithLabel:
                    //todo address this. draw the array or array size
                    break;
                    
                case EditorDisplayMode.RefLinkBetweenCenters:
                case EditorDisplayMode.RefLinkBetweenPivots:
                    return FieldDrawerEntityRef;

                case EditorDisplayMode.LevelTile:
                    //todo
                    break;
                
                default:
                    return null;
            }

            return null;
        }
    }
}