using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkProjectSettings : ScriptableObject
    {
        public const string PROPERTY_INTERAL_ICONS_TEXTURE = nameof(_internalIconsTexture);
        public const string PROPERTY_REVERT_OVERRIDES_IN_SCENE = nameof(_revertOverridesInScene);

        [SerializeField] private Texture2D _internalIconsTexture = null;
        [SerializeField] private bool _revertOverridesInScene = false;

        private static LDtkProjectSettings Instance => LDtkProjectSettingsProvider.Instance;
        
        public static Texture2D InternalIconsTexture => Instance._internalIconsTexture;
        public static string InternalIconsTexturePath => Instance._internalIconsTexture ? AssetDatabase.GetAssetPath(Instance._internalIconsTexture) : null;
        public static bool RevertOverridesInScene => Instance._revertOverridesInScene;
    }
}