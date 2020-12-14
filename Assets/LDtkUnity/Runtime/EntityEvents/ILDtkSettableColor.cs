using UnityEngine;

namespace LDtkUnity.EntityEvents
{
    public interface ILDtkSettableColor
    {
        void OnLDtkSetEntityColor(Color newColor);
    }
}