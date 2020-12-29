using UnityEngine;

namespace LDtkUnity.BuildEvents.EntityEvents
{
    public interface ILDtkSettableColor
    {
        void OnLDtkSetEntityColor(Color newColor);
    }
}