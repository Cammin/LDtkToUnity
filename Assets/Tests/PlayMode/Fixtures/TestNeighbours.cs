using System.Linq;
using UnityEngine;

namespace LDtkUnity.Tests
{
    public class TestNeighbours : MonoBehaviour
    {
        public LDtkComponentLevel Top;
        public LDtkComponentLevel Mid;
        public LDtkComponentLevel Bottom;

        private void Start()
        {
            bool top = Mid.Neighbours.First(p => p.IsNorth).GetLevel().GetComponent<LDtkComponentLevel>() == Top;
            bool bot = Mid.Neighbours.First(p => p.IsSouth).GetLevel().GetComponent<LDtkComponentLevel>() == Bottom;
            Debug.Assert(top && bot);
        }
    }
}
