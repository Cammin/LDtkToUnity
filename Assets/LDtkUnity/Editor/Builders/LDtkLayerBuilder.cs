namespace LDtkUnity.Editor.Builders
{
    public abstract class LDtkLayerBuilder
    {
        protected readonly LDtkProjectImporter Importer;
        protected readonly LayerInstance Layer;

        public LDtkLayerBuilder(LayerInstance layer, LDtkProjectImporter importer)
        {
            Layer = layer;
            Importer = importer;
        }
    }
}