using System;
using System.Collections.Generic;

namespace LDtkUnity.Editor
{
    public interface ILDtkSectionDrawer : IDisposable
    {
        void Init();
        void Draw();
        bool HasProblem { get; }
        bool HasResizedArrayPropThisUpdate { get; }
    }
}