using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            foreach (LDtkComponentLevel level in components)
            {
                ProcessLevel(level);
            }
        }

        private static void ProcessLevel(LDtkComponentLevel level)
        {
            Handles.color = level.BgColor;
            LDtkLevelDrawer drawer = new LDtkLevelDrawer(level.transform.position, level.Size, level.gameObject.name);
            drawer.OnDrawHandles();
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


        private static ILDtkHandleDrawer DrawEntity(LDtkEntityDrawerData data)
        {
            if (LDtkPrefs.ShowEntityIdentifier && data.ShowName)
            {
                GizmoUtil.DrawText(data.Transform.position, data.Identifier);
            }
            
            switch (data.EntityMode)
            {
                case RenderMode.Cross:
                case RenderMode.Ellipse:
                case RenderMode.Rectangle:
                    LDtkEntityDrawerShapes.Data shapeData = new LDtkEntityDrawerShapes.Data()
                    {
                        EntityMode = data.EntityMode,
                        FillOpacity = data.FillOpacity,
                        LineOpacity = data.LineOpacity,
                        Hollow = data.Hollow,
                        Pivot = data.Pivot,
                        Size = data.Size
                    };
                    return new LDtkEntityDrawerShapes(data.Transform, shapeData);
                
                case RenderMode.Tile:
                    return new LDtkEntityDrawerIcon(data.Transform, data.Tex, data.TexRect);
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
                    
                case EditorDisplayMode.ValueOnly:
                case EditorDisplayMode.NameAndValue:
                    //todo choose to show more later? like an icon in a smaller size maybe?
                    return new LDtkFieldDrawerValue(data.Fields.transform.position + Vector3.up, data.Identifier);
                    
                case EditorDisplayMode.EntityTile: //REPLACE ENTITY TILE WITH ENUM DEFINITION TILE, so it's special todo target this eventually
                    return new LDtkEntityDrawerIcon(data.Fields.transform, data.IconTex, data.IconRect);

                case EditorDisplayMode.PointPath:
                case EditorDisplayMode.PointStar:
                case EditorDisplayMode.PointPathLoop:
                case EditorDisplayMode.Points:
                    return new LDtkFieldDrawerPoints(data.Fields, data.Identifier, data.FieldMode);
                    
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