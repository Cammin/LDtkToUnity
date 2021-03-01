using LDtkUnity.UnityAssets;

namespace LDtkUnity.Builders
{
    public abstract class LDtkLayerBuilder
    {
        protected readonly LDtkProject Project;
        protected readonly LayerInstance Layer;

        public LDtkLayerBuilder(LayerInstance layer, LDtkProject project)
        {
            Layer = layer;
            Project = project;
        }
    }
}