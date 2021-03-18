using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity
{
    public partial class Level : ILDtkUid, ILDtkIdentifier
    {

        //todo add handling for getting next neigbours depending on linearity or gridvania, neighbour handling essentially

        public Color UnityBgColor => BgColor.ToColor();
        public Vector2Int UnityPxSize => new Vector2Int((int)PxWid, (int)PxHei);
        public Vector2Int UnityWorldCoord => new Vector2Int((int)WorldX, (int)WorldY);
        
        public Vector2 UnityWorldSpaceCoord(int pixelsPerUnit)
        {
            return LDtkToolOriginCoordConverter.LevelPosition(UnityWorldCoord, (int) PxHei, pixelsPerUnit);
        }
        
        public Rect UnityWorldSpaceBounds(int pixelsPerUnit)
        {
            return new Rect(UnityWorldSpaceCoord(pixelsPerUnit), new Vector3(PxWid, PxHei, 0) / pixelsPerUnit);
        }
        
        public static Level FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Level>(json, Converter.Settings);
        }
    }
}
