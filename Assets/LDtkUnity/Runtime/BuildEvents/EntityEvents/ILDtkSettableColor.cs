using UnityEngine;

namespace LDtkUnity.BuildEvents
{
    public interface ILDtkSettableColor
    {
        void OnLDtkSetEntityColor(Color newColor);
    }
}