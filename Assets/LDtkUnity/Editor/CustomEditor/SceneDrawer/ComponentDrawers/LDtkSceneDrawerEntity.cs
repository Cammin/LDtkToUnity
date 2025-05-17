using System;
using System.Linq;
using UnityEngine;

namespace LDtkUnity.Editor
{
    /// <summary>
    /// Draws entity stuff, no field stuff
    /// </summary>
    internal static class LDtkSceneDrawerEntity
    {
        public static void DrawEntityDrawer(LDtkComponentEntity entityComponent)
        {
            Vector2 offset = Vector2.zero;

            if (!entityComponent.Def)
            {
                return;
            }
            
            switch (entityComponent.Def.RenderMode)
            {
                case RenderMode.Cross:
                case RenderMode.Ellipse:
                case RenderMode.Rectangle:
                case RenderMode.Tile:
                    LDtkEntityDrawerShapes entityDrawer = new LDtkEntityDrawerShapes(entityComponent);
                    entityDrawer.OnDrawHandles();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            offset = TryDrawTile(entityComponent, offset);
            TryDrawName(entityComponent, offset);

            return;
        }

        private static void TryDrawName(LDtkComponentEntity entity, Vector2 offset)
        {
            if (entity.Def.ShowName && LDtkPrefs.ShowEntityIdentifier)
            {
                HandleUtil.DrawText(entity.Identifier, entity.transform.position, entity.SmartColor, offset);
            }
        }

        private static Vector2 TryDrawTile(LDtkComponentEntity entityComponent, Vector2 offset)
        {
            bool entityDrawsTile = entityComponent.Def.RenderMode == RenderMode.Tile;
            bool fieldDrawsTile = entityComponent.Def.FieldDefs.Any(field => field.EditorDisplayMode == EditorDisplayMode.EntityTile);
            
            bool drawsTile = entityDrawsTile || fieldDrawsTile;
            
            if (!drawsTile)
            {
                return offset;
            }
            
            Sprite tile = entityComponent.Tile;
            if (!tile)
            {
                return offset;
            }
            
            LDtkEntityDrawerIcon iconDrawer = new LDtkEntityDrawerIcon(entityComponent.transform, tile.texture, tile.rect);

            iconDrawer.PrecalculateValues();
            offset += iconDrawer.OffsetToNextUI;
            iconDrawer.OnDrawHandles();

            return offset;
        }
    }
}