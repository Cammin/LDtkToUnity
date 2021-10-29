using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity.Editor
{
    [ExcludeFromDocs]
    public class LDtkParsedPoint : ILDtkValueParser, ILDtkPostParser
    {
        private struct LDtkPoint
        {
            [JsonProperty("cx")]
            public int Cx { get; set; }
            
            [JsonProperty("cy")]
            public int Cy { get; set; }
        }
        
        private ILDtkPostParseProcess<Vector2> _process;
        
        bool ILDtkValueParser.TypeName(FieldInstance instance) => instance.IsPoint;

        public object ImportString(object input)
        {
            //Point can be legally null. for the purposes of the scene drawer, a null point in LDtk will translate to a magic vector2 that tells the scene drawer not to draw
            if (input == null)
            {
                return Vector2.negativeInfinity;
            }
            
            string stringInput = Convert.ToString(input);
            
            if (string.IsNullOrEmpty(stringInput))
            {
                return default;
            }

            LDtkPoint pointData = JsonConvert.DeserializeObject<LDtkPoint>(stringInput);
            
            int x = pointData.Cx;
            int y = pointData.Cy;

            Vector2Int cellPos = new Vector2Int(x, y);
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