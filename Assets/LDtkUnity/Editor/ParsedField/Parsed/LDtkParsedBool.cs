using System;
using JetBrains.Annotations;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal sealed class LDtkParsedBool : ILDtkValueParser
    {
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsBool;

        public object ImportString(LDtkFieldParseContext ctx)
        {
            //bool can never be null but just in case
            if (ctx.Input == null)
            {
                ctx.Importer.Logger.LogWarning("Bool field was unexpectedly null");
                return false;
            }
            
            return Convert.ToBoolean(ctx.Input);
        }
    }
}