using System.Runtime.Serialization;
using UnityEngine;

namespace LDtkUnity
{
    public partial class FieldDefinition : ILDtkUid, ILDtkIdentifier
    {
        /// <value>
        /// Reference to the tileset that uses this icon. <br/>
        /// Make sure to call <see cref="LDtkUidBank"/>.<see cref="LDtkUidBank.CacheUidData"/> first!
        /// </value>
        [IgnoreDataMember] public TilesetDefinition Tileset => TilesetUid == null ? null : LDtkUidBank.GetUidData<TilesetDefinition>(TilesetUid.Value);
        
        [IgnoreDataMember] public Color UnityEditorDisplayColor => EditorDisplayColor.ToColor();
        
        /// <value>
        /// Returns if this field (or array element) type is an Int. 
        /// </value>
        [IgnoreDataMember] public bool IsInt => Type == "Int" || Type == "Array<Int>";
        
        /// <value>
        /// Returns if this field (or array element) type is a Float. 
        /// </value>
        [IgnoreDataMember] public bool IsFloat => Type == "Float" || Type == "Array<Float>";
        
        /// <value>
        /// Returns if this field (or array element) type is a Bool. 
        /// </value>
        [IgnoreDataMember] public bool IsBool => Type == "Bool" || Type == "Array<Bool>";
        
        /// <value>
        /// Returns if this field (or array element) type is a String. 
        /// </value>
        [IgnoreDataMember] public bool IsString => Type == "String" || Type == "Array<String>";

        /// <value>
        /// Returns if this field (or array element) type is MultiLines. 
        /// </value>
        [IgnoreDataMember] public bool IsMultilines => Type == "Multilines" || Type == "Array<Multilines>";
        
        /// <value>
        /// Returns if this field (or array element) type is a FilePath. 
        /// </value>
        [IgnoreDataMember] public bool IsFilePath => Type == "FilePath" || Type == "Array<FilePath>";
        
        /// <value>
        /// Returns if this field (or array element) type is a Color. 
        /// </value>
        [IgnoreDataMember] public bool IsColor => Type == "Color" || Type == "Array<Color>";

        /// <value>
        /// Returns if this field (or array element) type is an Enum. 
        /// </value>
        [IgnoreDataMember]
        public bool IsEnum => Type.Contains("LocalEnum.") || Type.Contains("ExternEnum.");

        /// <value>
        /// Returns if this field (or array element) type is a Tile. 
        /// </value>
        [IgnoreDataMember] public bool IsTile => Type == "Tile" || Type == "Array<Tile>";    
        
        /// <value>
        /// Returns if this field (or array element) type is a EntityRef. 
        /// </value>
        [IgnoreDataMember] public bool IsEntityRef => Type == "EntityRef" || Type == "Array<EntityRef>";    
        
        /// <value>
        /// Returns if this field (or array element) type is a Point. 
        /// </value>
        [IgnoreDataMember] public bool IsPoint => Type == "Point" || Type == "Array<Point>";

    }
}