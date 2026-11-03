using System;
using UnityEngine;

namespace LDtkUnity
{
    [Serializable]
    public struct LDtkIntPattern
    {
        [SerializeField] public int[] data;

        public int Length => data?.Length ?? 0;
        public int this[int i]
        {
            get => data[i];
            set => data[i] = value;
        }

        public int[] ToArray() => data;
        
        public LDtkIntPattern(int[] inputData)
        {
            data = inputData;
        }
    }
}
