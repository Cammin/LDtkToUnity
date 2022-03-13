using System;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace LDtkUnity.Editor
{
    [UsedImplicitly]
    internal class LDtkParsedPoint : ILDtkValueParser, ILDtkPostParser
    {
        private ILDtkPostParseProcess<Vector2> _process;
        
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsPoint;

        public object ImportString(object input)
        {
            //Point can be legally null. for the purposes of the scene drawer, a null point in LDtk will translate to a negative infinity vector2 that tells the scene drawer not to draw
            if (input == null)
            {
                return default;
            }
            
            string stringInput = Convert.ToString(input);
            if (string.IsNullOrEmpty(stringInput))
            {
                return default;
            }

            GridPoint pointData = GridPoint.FromJson(stringInput);
            
            Vector2Int cellPos = pointData.UnityCoord;
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
    }
}