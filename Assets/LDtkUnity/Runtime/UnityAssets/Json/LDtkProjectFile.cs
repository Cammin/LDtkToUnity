namespace LDtkUnity
{
    public class LDtkProjectFile : LDtkJsonFile<LdtkJson>
    {
        public override LdtkJson FromJson => LdtkJson.FromJson(_json);
    }
}