using System;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal struct InfiniteLoopInsurance
    {
        private const int DEFAULT_MAX = 10000000;
        private int _maxLoops;
        public int Loops;
        
        
        public InfiniteLoopInsurance(int max = DEFAULT_MAX)
        {
            _maxLoops = max;
            Loops = 0;
        }
        
        public void Insure()
        {
            if (_maxLoops == 0)
            {
                _maxLoops = DEFAULT_MAX;
            }
            
            if (Loops >= _maxLoops)
            {
                Debug.LogError("A while loop was infinite");
                throw new OverflowException("A while loop was infinite");
            }
            Loops++;
        }
    }
}