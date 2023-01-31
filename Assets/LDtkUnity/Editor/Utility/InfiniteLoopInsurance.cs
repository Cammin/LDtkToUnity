using System;
using System.Text;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal struct InfiniteLoopInsurance
    {
        private const int DEFAULT_MAX = 100000;
        private int _maxLoops;
        private int _i;
        
        
        public InfiniteLoopInsurance(int max = DEFAULT_MAX)
        {
            _maxLoops = max;
            _i = 0;
        }
        
        public void Insure()
        {
            if (_maxLoops == 0)
            {
                _maxLoops = DEFAULT_MAX;
            }
            
            if (_i >= _maxLoops)
            {
                Debug.LogError("A while loop was infinite");
                throw new OverflowException("A while loop was infinite");
            }
            _i++;
        }
    }
}