using System.Collections.Generic;
using System.Linq;

namespace LDtkUnity.Editor
{
    public static class LDtkExtensionMethods
    {
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}