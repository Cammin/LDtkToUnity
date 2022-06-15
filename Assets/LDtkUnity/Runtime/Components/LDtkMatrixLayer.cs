using System;
using UnityEngine;

namespace LDtkUnity
{
    [AddComponentMenu("")] //todo this is for a future feature
    internal class LDtkMatrixLayer : MonoBehaviour
    {
        internal LDtkMatrixColumn[] columns;

        public IntGridMetaData GetIntGridValue(int x, int y)
        {
            return columns[x].rowValues[y];
        }
    }
    
    [Serializable]
    internal class LDtkMatrixColumn
    {
        public IntGridMetaData[] rowValues;
    }
    
    [Serializable]
    internal class IntGridMetaData
    {
        public int value;
    }
}
