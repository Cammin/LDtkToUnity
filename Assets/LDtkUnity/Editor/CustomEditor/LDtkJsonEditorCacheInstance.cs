using System.Text;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkJsonEditorCacheInstance
    {
        public bool ShouldReconstruct;
        public LdtkJson Json;
        public byte[] Hash;

        public override string ToString()
        {
            return ByteArrayToString(Hash);
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length -1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}