using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class EntityReferenceInfos
    {
        //todo add functionality to get level/entity references
        
        internal static EntityReferenceInfos FromJson(string json)
        {
            return JsonConvert.DeserializeObject<EntityReferenceInfos>(json, Converter.Settings);
        }
    }
}