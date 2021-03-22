using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class Level : ILDtkUid, ILDtkIdentifier
    {
        /// <summary>
        /// Used for deserializing ".ldtkl" files
        /// </summary>
        public static Level FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Level>(json, Converter.Settings);
        }

        /// <summary>
        /// The "__bgColor" field converted for use with Unity
        /// </summary>
        public Color UnityBgColor => BgColor.ToColor();
        
        /// <summary>
        /// Size of the level in pixels
        /// </summary>
        public Vector2Int UnityPxSize => new Vector2Int((int)PxWid, (int)PxHei);
        
        /// <summary>
        /// World coordinate in pixels
        /// </summary>
        public Vector2Int UnityWorldCoord => new Vector2Int((int)WorldX, (int)WorldY);
        
        /// <summary>
        /// A special Vector2 position that solves where the layer's position should be in Unity's world space based off of LDtk's top-left origin
        /// </summary>
        public Vector2 UnityWorldSpaceCoord(int pixelsPerUnit)
        {
            return LDtkToolOriginCoordConverter.LevelPosition(UnityWorldCoord, (int) PxHei, pixelsPerUnit);
        }
        
        /// <summary>
        /// A Rect of the level's bounds in Unity's world space.
        /// </summary>
        public Rect UnityWorldSpaceBounds(int pixelsPerUnit)
        {
            return new Rect(UnityWorldSpaceCoord(pixelsPerUnit), UnityPxSize / pixelsPerUnit);
        }
        
        //todo add handling for getting next neigbours depending on linearity or gridvania, neighbour handling essentially
    }
}
