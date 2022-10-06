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
            bool top = Mid.NeighboursNorth.First().GetComponent<LDtkComponentLevel>() == Top;
            bool bot = Mid.NeighboursSouth.First().GetComponent<LDtkComponentLevel>() == Bottom;
            Debug.Assert(top && bot);
        }
    }
}
