using System;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    internal interface ILDtkSectionDrawer : IDisposable
    {
        void Init();
        void Draw();
        bool HasProblem { get; }
        bool HasResizedArrayPropThisUpdate { get; }
    }
}