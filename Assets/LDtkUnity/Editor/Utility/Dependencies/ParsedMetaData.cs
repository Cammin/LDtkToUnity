using UnityEditor;

namespace LDtkUnity.Editor
{
    internal struct ParsedMetaData
    {
        /// <summary>
        /// Serialized field variable name
        /// </summary>
        public string Name;
        public string Guid;
            
        public string GetAssetPath() => AssetDatabase.GUIDToAssetPath(Guid);
        public override string ToString() => $"\"{Name}\" {Guid}";
    }
}