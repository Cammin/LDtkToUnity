﻿using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class Level : ILDtkUid, ILDtkIdentifier, ILDtkIid
    {
        /// <summary>
        /// Used for deserializing ".ldtkl" files.
        /// </summary>
        /// <param name="json">
        /// The LDtk json string to deserialize.
        /// </param>
        /// <returns>
        /// The deserialized level.
        /// </returns>
        public static Level FromJson(string json)
        {
            return JsonSerializer.Deserialize<Level>(json, Converter.Settings);
        }

        /// <value>
        /// Background color of the level (same as `bgColor`, except the default value is
        /// automatically used here if its value is `null`)
        /// </value>
        [JsonIgnore] public Color UnityBgColor => BgColor.ToColor();
        
        /// <value>
        /// Background image pivot (0-1)
        /// </value>
        [JsonIgnore] public Vector2 UnityBgPivot => new Vector2((float)BgPivotX, (float)BgPivotY);
        
        /// <value>
        /// Size of the level in pixels
        /// </value>
        [JsonIgnore] public Vector2Int UnityPxSize => new Vector2Int((int)PxWid, (int)PxHei);
        
        /// <value>
        /// World coordinate in pixels.<br/>  Only relevant for world layouts where level spatial
        /// positioning is manual (ie. GridVania, Free). For Horizontal and Vertical layouts, the
        /// value is always (-1, -1) here.
        /// </value>
        [JsonIgnore] public Vector2Int UnityWorldCoord => new Vector2Int((int)WorldX, (int)WorldY);
        
        /// <value>
        /// Rect of the level in pixels
        /// </value>
        [JsonIgnore] public RectInt UnityWorldRect => new RectInt((int)WorldX, (int)WorldY, (int)PxWid, (int)PxHei);

        /// <value>
        /// The "guessed" color for this level in the editor, decided using either the background
        /// color or an existing custom field.
        /// </value>
        [JsonIgnore] public Color UnitySmartColor => SmartColor.ToColor();
        
        /// <summary>
        /// A special Vector2 position that solves where the layer's position should be in Unity's world space based off of LDtk's top-left origin.
        /// If the world layout is not free style, then the positioning of the level will be automatically arranged like they do in LDtk.
        /// </summary>
        /// <param name="layout">
        /// Layout of the world. This affects how the level would be naturally positioned.
        /// </param>
        /// <param name="pixelsPerUnit">
        /// Main pixels per unit.
        /// </param>
        /// <param name="vector">
        /// Vector of the level for when creating the position for the level's layout as LinearHorizontal or LinearVertical. Otherwise the value is 0.
        /// </param>
        /// <returns>
        /// The bottom left corner of the level's position in world space.
        /// </returns>
        public Vector2 UnityWorldSpaceCoord(WorldLayout layout, int pixelsPerUnit, int vector = 0)
        {
            Vector2Int pxCoord = Vector2Int.zero;
            switch (layout)
            {
                case WorldLayout.Free:
                case WorldLayout.GridVania:
                    pxCoord = UnityWorldCoord;
                    break;

                case WorldLayout.LinearHorizontal:
                    pxCoord.x = vector;
                    break;
                
                case WorldLayout.LinearVertical:
                    pxCoord.y = vector;
                    break;
            }
            
            return LDtkCoordConverter.LevelPosition(pxCoord, (int) PxHei, pixelsPerUnit);
        }
        
        /// <summary>
        /// A Rect of the level's bounds in Unity's world space.
        /// </summary>
        /// <param name="layout">
        /// Layout of the world. This affects how the level would be naturally positioned.
        /// </param>
        /// <param name="pixelsPerUnit">
        /// Main pixels per unit.
        /// </param>
        /// <param name="vector">
        /// Vector of the level for when creating the position for the level's layout as LinearHorizontal or LinearVertical. Otherwise the value is 0.
        /// </param>
        /// <returns>
        /// World space bounds of this level.
        /// </returns>
        public Rect UnityWorldSpaceBounds(WorldLayout layout, int pixelsPerUnit, int vector = 0)
        {
            return new Rect(UnityWorldSpaceCoord(layout, pixelsPerUnit, vector), new Vector3(PxWid, PxHei, 0) / pixelsPerUnit);
        }
    }
}
