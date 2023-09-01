using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal sealed class LDtkParsedEntityRef : ILDtkValueParser
    {
        public bool TypeName(FieldInstance instance)
        {
            return instance.IsEntityRef;
        }

        public object ImportString(LDtkFieldParseContext ctx)
        {
            object input = ctx.Input;
            if (input == null)
            {
                return null;
            }

            ReferenceToAnEntityInstance jsonReference = ConvertDict(input);
            if (jsonReference != null)
            {
                return new LDtkReferenceToAnEntityInstance(jsonReference);
            }

            LDtkDebug.LogError($"Issue here. input was {input}");
            return null;
        }
        
        public static ReferenceToAnEntityInstance ConvertDict(object input)
        {
            if (input == null)
            {
                return null;
            }

            if (input is Dictionary<string, object> dict)
            {
                string entityIid = (string)dict["entityIid"];
                string layerIid = (string)dict["layerIid"];
                string levelIid = (string)dict["levelIid"];
                string worldIid = (string)dict["worldIid"];

                return new ReferenceToAnEntityInstance()
                {
                    EntityIid = entityIid,
                    LayerIid = layerIid,
                    LevelIid = levelIid,
                    WorldIid = worldIid,
                };
            }
            Debug.LogError("The parsed object was not a dictionary?");
            return null;
        }
    }
}