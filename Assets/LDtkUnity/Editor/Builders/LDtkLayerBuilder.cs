using UnityEditor;

namespace LDtkUnity.Editor
{
    public abstract class LDtkLayerBuilder
    {
        protected readonly SerializedObject Project;
        protected readonly LayerInstance Layer;

        public LDtkLayerBuilder(LayerInstance layer, SerializedObject project)
        {
            Layer = layer;
            Project = project;
        }
    }
}