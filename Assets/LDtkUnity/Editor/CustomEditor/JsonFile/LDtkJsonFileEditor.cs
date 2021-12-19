using UnityEngine.Assertions;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public abstract class LDtkJsonFileEditor<T> : UnityEditor.Editor
    {
        protected LDtkTreeViewWrapper Tree;
        protected T JsonData = default;

        public void OnDisable()
        {
            Tree?.Dispose();
        }
        
        public override void OnInspectorGUI()
        {
            TryCacheJson();

            if (JsonData == null)
            {
                Assert.AreNotEqual(JsonData, default);
                return;
            }
            
            DrawInspectorGUI();
            
        }

        protected void TryCacheJson()
        {
            LDtkJsonFile<T> file = (LDtkJsonFile<T>)target;
            Assert.IsNotNull(file);

            if (JsonData == null)
            {
                JsonData = file.FromJson;
            }
        }

        protected abstract void DrawInspectorGUI();
    }
}