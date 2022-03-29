using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal static class LDtkSceneDrawerEntity
    {
        public static void Draw(List<LDtkEntityDrawerComponent> entityComponents)
        {
            List<LDtkEntityDrawerData> datas = entityComponents.Select(component => component.EntityDrawer).ToList();

            foreach (LDtkEntityDrawerData data in datas)
            {
                ProcessData(data);
            }
        }
        private static void ProcessData(LDtkEntityDrawerData data)
        {
            if (!data.Enabled)
            {
                return;
            }
            
            Handles.color = data.SmartColor;
            ILDtkHandleDrawer drawer = DrawEntity(data);
            drawer?.OnDrawHandles();
        }

        private static ILDtkHandleDrawer DrawEntity(LDtkEntityDrawerData entity)
        {
            Vector2 offset = Vector2.zero;

            if (entity.Transform == null)
            {
                return null;
            }
            
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

            offset = TryDrawTile(entity, offset);
            TryDrawName(entity, offset);

            return null;
        }

        private static void TryDrawName(LDtkEntityDrawerData entity, Vector2 offset)
        {
            if (entity.ShowName && LDtkPrefs.ShowEntityIdentifier)
            {
                HandleUtil.DrawText(entity.Identifier, entity.Transform.position, entity.SmartColor, offset, () => HandleUtil.SelectIfNotAlreadySelected(entity.Transform.gameObject));
            }
        }

        private static Vector2 TryDrawTile(LDtkEntityDrawerData entity, Vector2 offset)
        {
            if (!entity.DrawTile)
            {
                return offset;
            }
            
            string path = entity.TexPath;
            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (tex == null)
            {
                return offset;
            }
            
            LDtkEntityDrawerIcon iconDrawer = new LDtkEntityDrawerIcon(entity.Transform, tex, entity.TexRect);

            iconDrawer.PrecalculateValues();
            offset += iconDrawer.OffsetToNextUI;
            iconDrawer.OnDrawHandles();

            return offset;
        }
    }
}