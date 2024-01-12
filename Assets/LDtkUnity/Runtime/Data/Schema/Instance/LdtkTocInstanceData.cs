using System.Runtime.Serialization;

namespace LDtkUnity
{
    public partial class LdtkTocInstanceData
    {
        /// <summary>
        /// An object containing the values of all entity fields with the `exportToToc` option
        /// enabled. This object typing depends on actual field value types.
        /// </summary>
        [DataMember(Name = "fields")]
        public object Fields { get; set; }

        [DataMember(Name = "heiPx")]
        public int HeiPx { get; set; }

        /// <summary>
        /// IID information of this instance
        /// </summary>
        [DataMember(Name = "iids")]
        public ReferenceToAnEntityInstance Iids { get; set; }

        [DataMember(Name = "widPx")]
        public int WidPx { get; set; }

        [DataMember(Name = "worldX")]
        public int WorldX { get; set; }

        [DataMember(Name = "worldY")]
        public int WorldY { get; set; }
    }
}