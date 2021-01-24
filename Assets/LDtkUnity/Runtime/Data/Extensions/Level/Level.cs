using LDtkUnity.Tools;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class Level : ILDtkUid, ILDtkIdentifier
    {
        public Color UnityBgColor => BgColor.ToColor();
        public Vector2Int UnityPxSize => new Vector2Int((int)PxWid, (int)PxHei);
        public Vector2Int UnityWorldCoord => new Vector2Int((int)WorldX, (int)WorldY);
        
        public Rect UnityWorldBounds(int pixelsPerUnit)
        {
            Vector3 size = new Vector3(PxWid, PxHei, 0) / pixelsPerUnit;
            return new Rect(UnityWorldSpaceCoord(pixelsPerUnit), size);
        }

        public Vector2 UnityWorldSpaceCoord(int pixelsPerUnit) => LDtkToolOriginCoordConverter.LevelPosition(UnityWorldCoord, (int)PxHei, pixelsPerUnit);
    }
    
    public partial class Level
    {
        public static Level FromJson(string json) => JsonConvert.DeserializeObject<Level>(json, LDtkUnity.Converter.Settings);
    }
}
