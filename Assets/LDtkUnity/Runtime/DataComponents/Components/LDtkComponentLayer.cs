namespace LDtkUnity
{
    public class LDtkComponentLayer : LDtkDataComponent<LayerInstance>
    {
        protected override LayerInstance DeserializeJson()
        {
            return LayerInstance.FromJson(_json);
        }

        protected override string SerializeJson(LayerInstance data)
        {
            throw new System.NotImplementedException();
        }
    }
}