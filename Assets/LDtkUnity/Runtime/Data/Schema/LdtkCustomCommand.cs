using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class LdtkCustomCommand
    {
        [DataMember(Name = "command")]
        public string Command { get; set; }

        /// <summary>
        /// Possible values: `Manual`, `AfterLoad`, `BeforeSave`, `AfterSave`
        /// </summary>
        [DataMember(Name = "when")]
        public When When { get; set; }
    }
}