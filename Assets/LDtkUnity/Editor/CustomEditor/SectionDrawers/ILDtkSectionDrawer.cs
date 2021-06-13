using System;

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