using LDtkUnity.Runtime.FieldInjection;
using UnityEngine;

namespace LDtkUnity.Samples.Example.Scripts
{
    public class LDtkEntityFieldsExample : MonoBehaviour, ILDtkFieldInjectedEvent
    {
        [LDtkField("TheInt")] public int _theInt = default;
        [LDtkField("TheFloat")] public float _theFloat = default;
        [LDtkField("TheBool")] public bool _theBool = default;
        [LDtkField("TheString")] public string _theString = default;
        [LDtkField("TheMultiLines")] public string _theMultiLine = default;
        [LDtkField("TheEnum")] public Item _theEnum = default;
        [LDtkField("TheColor")] public Color _theColor = default;
        [LDtkField("ThePoint")] public Vector2Int _thePoint = default;
        
        [LDtkField("TheIntArray")] public int[] _theInts = default;
        [LDtkField("TheFloatArray")] public float[] _theFloats = default;
        [LDtkField("TheBoolArray")] public bool[] _theBools = default;
        [LDtkField("TheStringArray")] public string[] _theStrings = default;
        [LDtkField("TheMultiLinesArray")] public string[] _theMultiLines = default;
        [LDtkField("TheEnumArray")] public Item[] _theEnums = default;
        [LDtkField("TheColorArray")] public Color[] _theColors = default;
        [LDtkField("ThePointArray")] public Vector2Int[] _thePoints = default;

        //order of execution is these functions:
        private void Awake()
        {
            Debug.Log("LDtkEntityFieldsExample.Awake");
        }

        private void OnEnable()
        {
            Debug.Log("LDtkEntityFieldsExample.OnEnable");
        }
        
        public void OnLDtkFieldsInjected()
        {
            Debug.Log("LDtkEntityFieldsExample.OnLDtkFieldsInjected");
        }

        private void Start()
        {
            Debug.Log("LDtkEntityFieldsExample.Start");
        }
    }
}
