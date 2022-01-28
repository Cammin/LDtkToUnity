namespace LDtkUnity.Editor
{
    public class LDtkParsedEntityRef : ILDtkValueParser
    {
        public bool TypeName(FieldInstance instance)
        {
            return instance.IsEntityRef;
        }

        public object ImportString(object input)
        {
             //tile = (FieldInstanceTile)input;

            //if (tile == null)
            {
                //Debug.LogError("LDtk: parse error for tile"); //todo come back to this when the schema data is available
                return null;
            }
        }
    }
}