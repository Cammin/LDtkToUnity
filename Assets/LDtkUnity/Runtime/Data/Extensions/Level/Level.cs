using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Json Level Data
    /// </summary>
    public partial class Level : ILDtkUid, ILDtkIdentifier
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
            return JsonConvert.DeserializeObject<Level>(json, Converter.Settings);
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
        /// World coordinate in pixels
        /// </value>
        [JsonIgnore] public Vector2Int UnityWorldCoord => new Vector2Int((int)WorldX, (int)WorldY);
        
        /// <summary>
        /// A special Vector2 position that solves where the layer's position should be in Unity's world space based off of LDtk's top-left origin
        /// </summary>
        /// <param name="pixelsPerUnit">
        /// Main pixels per unit.
        /// </param>
        /// <returns>
        /// The bottom left corner of the level's position in world space.
        /// </returns>
        public Vector2 UnityWorldSpaceCoord(int pixelsPerUnit)
        {
            return LDtkCoordConverter.LevelPosition(UnityWorldCoord, (int) PxHei, pixelsPerUnit);
        }
        
        /// <summary>
        /// A Rect of the level's bounds in Unity's world space.
        /// </summary>
        /// <param name="pixelsPerUnit">
        /// Main pixels per unit.
        /// </param>
        /// <returns>
        /// World space bounds of this level.
        /// </returns>
        public Rect UnityWorldSpaceBounds(int pixelsPerUnit)
        {
            return new Rect(UnityWorldSpaceCoord(pixelsPerUnit), new Vector3(PxWid, PxHei, 0) / pixelsPerUnit);
        }

        /// <summary>
        /// Gets the next level in a linear level sequence.
        /// Only valid for LinearHorizontal and LinearVertical layouts.
        /// Returns null if this is the last level.
        /// </summary>
        /// <param name="worldLayout">
        /// The current world layout being used.
        /// </param>
        /// <returns>
        /// The previous neighbor.
        /// </returns>
        public Level GetNextNeighbour(WorldLayout worldLayout)
        {
            switch (worldLayout)
            {
                case WorldLayout.LinearHorizontal:
                    return Neighbours.FirstOrDefault(level => level.IsEast);
                    
                case WorldLayout.LinearVertical:
                    return Neighbours.FirstOrDefault(level => level.IsSouth);
            }
            Debug.LogWarning($"LDtk: Tried getting next neighbour with an invalid world layout: {worldLayout}");
            return null;
        }
        
        /// <summary>
        /// Gets the previous level in a linear level sequence.
        /// Only valid for LinearHorizontal and LinearVertical layouts.
        /// Returns null if this is the first level.
        /// </summary>
        /// <param name="worldLayout">
        /// The current world layout being used.
        /// </param>
        /// <returns>
        /// The previous neighbor.
        /// </returns>
        public Level GetPreviousNeighbour(WorldLayout worldLayout)
        {
            switch (worldLayout)
            {
                case WorldLayout.LinearHorizontal:
                    return Neighbours.FirstOrDefault(level => level.IsWest);
                    
                case WorldLayout.LinearVertical:
                    return Neighbours.FirstOrDefault(level => level.IsNorth);
            }
            Debug.LogWarning($"LDtk: Tried getting previous neighbour with an invalid world layout: {worldLayout}");
            return null;
        }
    }
}
