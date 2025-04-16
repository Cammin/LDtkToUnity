namespace LDtkUnity.Editor
{
    internal sealed class LDtkFieldParseContext
    {
        public object Input;
        
        /// <summary>
        /// Needed by the parsed tile to get a sprite from it
        /// </summary>
        public LDtkProjectImporter Project;
        
        public LDtkJsonImporter Importer;
    }
}