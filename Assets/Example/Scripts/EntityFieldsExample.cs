using LDtkUnity.Runtime.LayerConstruction.EntityFieldInjection;
using UnityEngine;

namespace DevTesting.Scripts
{
    public class EntityFieldsExample : MonoBehaviour, ILDtkInjectedFieldEvent
    {
        [LDtkInjectableField("TheInt")] public int _theInt = default;
        [LDtkInjectableField("TheFloat")] public float _theFloat = default;
        [LDtkInjectableField("TheBool")] public bool _theBool = default;
        [LDtkInjectableField("TheString")] public string _theString = default;
        [LDtkInjectableField("TheMultiLines")] public string _theMultiLine = default;
        [LDtkInjectableField("TheEnum")] public Item _theEnum = default;
        [LDtkInjectableField("TheColor")] public Color _theColor = default;
        [LDtkInjectableField("ThePoint")] public Vector2Int _thePoint = default;
        
        [LDtkInjectableField("TheIntArray")] public int[] _theInts = default;
        [LDtkInjectableField("TheFloatArray")] public float[] _theFloats = default;
        [LDtkInjectableField("TheBoolArray")] public bool[] _theBools = default;
        [LDtkInjectableField("TheStringArray")] public string[] _theStrings = default;
        [LDtkInjectableField("TheMultiLinesArray")] public string[] _theMultiLines = default;
        [LDtkInjectableField("TheEnumArray")] public Item[] _theEnums = default;
        [LDtkInjectableField("TheColorArray")] public Color[] _theColors = default;
        [LDtkInjectableField("ThePointArray")] public Vector2Int[] _thePoints = default;
        
        public void OnLDtkFieldsInjected()
        {
            Debug.Log("Finished Injecting Fields");
        }
    }
}
