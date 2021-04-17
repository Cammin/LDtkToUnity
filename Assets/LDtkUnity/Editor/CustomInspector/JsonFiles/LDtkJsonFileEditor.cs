using UnityEngine.Assertions;

namespace LDtkUnity.Editor
{
    public abstract class LDtkJsonFileEditor<T> : UnityEditor.Editor
    {
        private T _cachedData = default;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            LDtkJsonComponent<T> file = (LDtkJsonComponent<T>) target;
            Assert.IsNotNull(file);

            if (_cachedData == null)
            {
                _cachedData = file.FromJson;
            }
            Assert.AreNotEqual(_cachedData, default);
            
            DrawInspectorGUI(_cachedData);
            
        }

        protected abstract void DrawInspectorGUI(T data);
    }
}