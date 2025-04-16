using UnityEditor;

namespace LDtkUnity.Editor
{
    // This class merely existing makes the inspector draw with no scroll rects
    [CustomEditor(typeof(LDtkTableOfContents))]
    internal sealed class LDtkTableOfContentsEditor : UnityEditor.Editor
    {
        
    }
}