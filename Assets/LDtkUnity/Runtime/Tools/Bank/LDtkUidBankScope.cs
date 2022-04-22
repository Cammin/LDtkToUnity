using System;

namespace LDtkUnity
{
    public class LDtkUidBankScope : IDisposable
    {
        public LDtkUidBankScope(LdtkJson json)
        {
            LDtkUidBank.CacheUidData(json);
        }

        public void Dispose()
        {
            LDtkUidBank.ReleaseDefinitions();
        }
    }
}