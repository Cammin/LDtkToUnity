using System.Collections.Generic;

namespace LDtkUnity.Editor
{
    internal interface ILDtkSectionDataDrawer : ILDtkSectionDrawer
    {
        void Draw(IEnumerable<ILDtkIdentifier> datas);
    }
}