using System;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// Serializable wrapper for tile rect IDs.
    /// </summary>
    [Serializable]
    public sealed class LDtkDefinitionObjectAutoLayerRuleTileRect
    {
        [SerializeField] private int[] _values = Array.Empty<int>();

        internal void Populate(int[] source)
        {
            if (source == null)
            {
                _values = null;
                return;
            }

            if (source.Length == 0)
            {
                _values = Array.Empty<int>();
                return;
            }

            var copy = new int[source.Length];
            Array.Copy(source, copy, source.Length);
            _values = copy;
        }

        internal int[] ToArray()
        {
            if (_values == null)
            {
                return null;
            }

            if (_values.Length == 0)
            {
                return Array.Empty<int>();
            }

            var copy = new int[_values.Length];
            Array.Copy(_values, copy, _values.Length);
            return copy;
        }

        internal static LDtkDefinitionObjectAutoLayerRuleTileRect[] Build(int[][] source)
        {
            if (source == null)
            {
                return null;
            }

            var result = new LDtkDefinitionObjectAutoLayerRuleTileRect[source.Length];
            for (int i = 0; i < source.Length; i++)
            {
                var rect = new LDtkDefinitionObjectAutoLayerRuleTileRect();
                rect.Populate(source[i]);
                result[i] = rect;
            }

            return result;
        }

        internal static int[][] ToJaggedArray(LDtkDefinitionObjectAutoLayerRuleTileRect[] source)
        {
            if (source == null)
            {
                return null;
            }

            var result = new int[source.Length][];
            for (int i = 0; i < source.Length; i++)
            {
                result[i] = source[i]?.ToArray();
            }

            return result;
        }

        internal static int[][] CloneJaggedArray(int?[][] source)
        {
            if (source == null)
            {
                return null;
            }

            var clone = new int[source.Length][];
            for (int i = 0; i < source.Length; i++)
            {
                var row = source[i];
                if (row == null || row.Length == 0)
                {
                    clone[i] = Array.Empty<int>();
                    continue;
                }

                var list = new int[row.Length];
                int count = 0;
                for (int j = 0; j < row.Length; j++)
                {
                    int? value = row[j];
                    if (value.HasValue)
                    {
                        list[count++] = value.Value;
                    }
                }

                if (count == 0)
                {
                    clone[i] = Array.Empty<int>();
                }
                else if (count == list.Length)
                {
                    clone[i] = list;
                }
                else
                {
                    var trimmed = new int[count];
                    Array.Copy(list, trimmed, count);
                    clone[i] = trimmed;
                }
            }

            return clone;
        }
    }
}
