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
        /// Background color of the level (same as `bgColor`, except the default value is
        /// automatically used here if its value is `null`)
        /// </summary>
        public Color UnityBgColor => BgColor.ToColor();
        
        /// <summary>
        /// Background image pivot (0-1)
        /// </summary>
        public Vector2 UnityBgPivot => new Vector2((float)BgPivotX, (float)BgPivotY);
        
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
            return new Rect(UnityWorldSpaceCoord(pixelsPerUnit), new Vector3(PxWid, PxHei, 0) / pixelsPerUnit);
        }
        
        //todo add handling for getting next neigbours depending on linearity or gridvania, neighbour handling essentially
    }
}
