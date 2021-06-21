using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Editor
{
    [InitializeOnLoad]
    public class LDtkSceneDrawer
    {
        static LDtkSceneDrawer()
        {
            SceneView.duringSceneGui += CustomOnSceneGUI;
        }
        
        private static void CustomOnSceneGUI(SceneView view)
        {
            DrawEntityDrawers();
            DrawLevelDrawers();
        }

        private static void DrawLevelDrawers()
        {
            List<LDtkComponentLevel> components = LDtkFindInScenes.FindInAllScenes<LDtkComponentLevel>();
            List<LDtkLevelDrawer> drawers = components.ConvertAll(p => new LDtkLevelDrawer(p));

            //borders, then labels, so that borders are never in front of labels
            foreach (LDtkLevelDrawer drawer in drawers)
            {
                drawer.OnDrawHandles();
            }
            
            foreach (LDtkLevelDrawer drawer in drawers)
            {
                drawer.DrawLabel();
            }
        }



        private static void DrawEntityDrawers()
        {
            List<LDtkEntityDrawerComponent> components = LDtkFindInScenes.FindInAllScenes<LDtkEntityDrawerComponent>();

            List<LDtkSceneDrawerBase> datas = new List<LDtkSceneDrawerBase>();

            foreach (LDtkEntityDrawerComponent component in components)
            {
                datas.Add(component.EntityDrawer);
                foreach (LDtkFieldDrawerData drawer in component.FieldDrawers)
                {
                    datas.Add(drawer);
                }
            }

            foreach (LDtkSceneDrawerBase data in datas)
            {
                ProcessData(data);
            }
        }


        private static void ProcessData(LDtkSceneDrawerBase data)
        {
            if (!data.Enabled)
            {
                return;
            }
            
            Handles.color = data.GizmoColor;
            ILDtkHandleDrawer drawer = GetDrawer(data);
            drawer?.OnDrawHandles();
        }

        private static ILDtkHandleDrawer GetDrawer(LDtkSceneDrawerBase data)
        {
            if (data is LDtkFieldDrawerData field)
            {
                return DrawField(field);
            }
            if (data is LDtkEntityDrawerData entity)
            {
                return DrawEntity(entity);
            }

            return null;
        }


        private static ILDtkHandleDrawer DrawEntity(LDtkEntityDrawerData entity)
        {
            Vector2 offset = Vector2.down;
            
            switch (entity.EntityMode)
            {
                case RenderMode.Cross:
                case RenderMode.Ellipse:
                case RenderMode.Rectangle:
                    LDtkEntityDrawerShapes.Data shapeData = new LDtkEntityDrawerShapes.Data()
                    {
                        EntityMode = entity.EntityMode,
                        FillOpacity = entity.FillOpacity,
                        LineOpacity = entity.LineOpacity,
                        Hollow = entity.Hollow,
                        Pivot = entity.Pivot,
                        Size = entity.Size
                    };
                    LDtkEntityDrawerShapes entityDrawer = new LDtkEntityDrawerShapes(entity.Transform, shapeData);
                    entityDrawer.OnDrawHandles();
                    break;
            }

            if (entity.DrawTile)
            {
                LDtkEntityDrawerIcon iconDrawer = new LDtkEntityDrawerIcon(entity.Transform, entity.Tex, entity.TexRect);
                iconDrawer.PrecalculateValues();
                offset = iconDrawer.OffsetToNextUI;
                iconDrawer.OnDrawHandles();
            }

            if (entity.ShowName && LDtkPrefs.ShowEntityIdentifier)
            {
                HandleUtil.DrawText(entity.Identifier, entity.Transform.position, entity.GizmoColor, offset, () => Selection.activeGameObject = entity.Transform.gameObject);
            }

            return null;
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
                    return new LDtkFieldDrawerPoints(data.Fields, data.Identifier, data.FieldMode, data.MiddleCenter);
                    
                case EditorDisplayMode.RadiusPx:
                case EditorDisplayMode.RadiusGrid:
                    return new LDtkFieldDrawerRadius(data.Fields, data.Identifier, data.FieldMode, data.GridSize);

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }
    }
}