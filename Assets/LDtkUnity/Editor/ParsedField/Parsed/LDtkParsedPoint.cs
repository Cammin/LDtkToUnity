using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal sealed class LDtkParsedPoint : ILDtkValueParser, ILDtkPostParser
    {
        private ILDtkPostParseProcess<Vector2> _process;
        
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsPoint;

        public object ImportString(LDtkFieldParseContext ctx)
        {
            object input = ctx.Input;
            //Point can be legally null. for the purposes of the scene drawer, a null point in LDtk will translate to a negative infinity vector2 that tells the scene drawer not to draw
            if (input == null)
            {
                return default;
            }

            GridPoint pt = ConvertDict(input);
            
            //turn it into a vector2
            Vector2Int cellPos = pt.UnityCoord;
            Vector2 point = cellPos;
            
            if (_process != null)
            {
                point = _process.Postprocess(point);
            }

            return point;
        }

        public void SupplyPostProcessorData(LDtkBuilderEntity builder, FieldInstance field)
        {
            PointParseData data = builder.GetParsedPointData();
            _process = new LDtkPostParserPoint(data);
        }
        
        public static GridPoint ConvertDict(object input)
        {
            if (input == null)
            {
                return null;
            }

            if (input is Dictionary<string, object> dict)
            {
                double cx = (double)dict["cx"];
                double cy = (double)dict["cy"];

                return new GridPoint()
                {
                    Cx = (int)cx,
                    Cy = (int)cy,
                };
            }
            LDtkDebug.LogError("The parsed object was not a dictionary?");
            return null;
        }
    }
}