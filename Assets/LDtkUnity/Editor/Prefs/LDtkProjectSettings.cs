using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkProjectSettings : ScriptableObject
    {
        public const string PROPERTY_INTERAL_ICONS_TEXTURE = nameof(_internalIconsTexture);

        [SerializeField] private Texture2D _internalIconsTexture = null;

        private static LDtkProjectSettings Instance => LDtkProjectSettingsProvider.Instance;
        
        public static Texture2D InternalIconsTexture => Instance._internalIconsTexture;
        public static string InternalIconsTexturePath => Instance._internalIconsTexture ? AssetDatabase.GetAssetPath(Instance._internalIconsTexture) : null;
    }
}