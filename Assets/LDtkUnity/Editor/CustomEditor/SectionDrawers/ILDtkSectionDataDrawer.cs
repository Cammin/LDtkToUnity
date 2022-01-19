using System.Collections.Generic;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    internal interface ILDtkSectionDataDrawer : ILDtkSectionDrawer
    {
        void Draw(IEnumerable<ILDtkIdentifier> datas);
    }
}