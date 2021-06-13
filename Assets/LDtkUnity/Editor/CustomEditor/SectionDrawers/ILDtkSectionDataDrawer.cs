using System.Collections.Generic;

namespace LDtkUnity.Editor
{
    public interface ILDtkSectionDataDrawer : ILDtkSectionDrawer
    {
        void Draw(IEnumerable<ILDtkIdentifier> datas);
    }
}