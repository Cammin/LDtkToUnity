using Newtonsoft.Json;

namespace LDtkUnity
{
    public partial class LdtkCustomCommand
    {
        [JsonProperty("command")]
        public string Command { get; set; }

        /// <summary>
        /// Possible values: `Manual`, `AfterLoad`, `BeforeSave`, `AfterSave`
        /// </summary>
        [JsonProperty("when")]
        public When When { get; set; }
    }
}