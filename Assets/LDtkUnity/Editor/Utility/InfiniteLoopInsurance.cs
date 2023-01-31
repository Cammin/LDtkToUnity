using System;
using System.Text;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal struct InfiniteLoopInsurance
    {
        private const int DEFAULT_MAX = 50000;
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
                Debug.LogError("The while loop was infinite");
                throw new OverflowException("The while loop was infinite");
            }
            _i++;
        }

        public void LogSb(StringBuilder sb)
        {
            if (_i >= _maxLoops)
            {
                Debug.LogError($"The while loop was infinite. sb:\n{sb}");
            }
        }
    }
}