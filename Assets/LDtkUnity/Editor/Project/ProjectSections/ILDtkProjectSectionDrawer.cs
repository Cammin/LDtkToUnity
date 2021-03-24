using System;
using System.Collections.Generic;

namespace LDtkUnity.Editor
{
    public interface ILDtkProjectSectionDrawer : IDisposable
    {
        void Init();
        void Draw(IEnumerable<ILDtkIdentifier> datas);
        bool HasProblem { get; }
    }
}