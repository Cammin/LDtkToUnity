using System.Collections.Generic;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public interface ILDtkSectionDataDrawer : ILDtkSectionDrawer
    {
        void Draw(IEnumerable<ILDtkIdentifier> datas);
    }
}