using System;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Any 'inactive' objects would have:
    /// GameObject inactive
    /// All hideflags (probably)
    /// And EditorOnly tag
    /// </summary>
    public class LDtkLevelToggles : MonoBehaviour
    {
        public const string TOGGLES = nameof(_toggles);
        public const string LEVELS = nameof(_lvls);

        [SerializeField] private bool[] _toggles;
        [SerializeField] private LDtkComponentLevel[] _lvls;

        public void SetLevels(LDtkComponentLevel[] lvls)
        {
            _lvls = lvls;

            _toggles = new bool[_lvls.Length];
            for (int i = 0; i < _toggles.Length; i++)
            {
                _toggles[i] = true;
            }
        }

    }
}