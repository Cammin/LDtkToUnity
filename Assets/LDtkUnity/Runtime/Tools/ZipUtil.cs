namespace LDtkUnity
{
    /// <summary>
    /// Because in older unity versions, this zip class only exists in runtime
    /// </summary>
    internal static class ZipUtil
    {
        #if UNITY_EDITOR
        public static void Extract(string from, string to)
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(from, to);
        }
        #endif
    }
}