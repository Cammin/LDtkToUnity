using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class PathDrawer
    {
        private const float BUTTON_WIDTH = 26;
        
        private readonly SerializedProperty _pathProp;
        private readonly GUIContent _labelContent;
        private readonly string _originalPath;
        private readonly string _extension;
        private readonly string _filePanelDescription;

        private readonly GUIContent _folderButtonContent;

        public PathDrawer(SerializedProperty pathProp, GUIContent labelContent, string originalPath, string folderButtonTooltip, string extension = "", string filePanelDescription = "Select location")
        {
            _labelContent = labelContent;
            _pathProp = pathProp;
            _originalPath = originalPath;
            _extension = extension;
            _filePanelDescription = filePanelDescription;

            _folderButtonContent = new GUIContent()
            {
                tooltip = folderButtonTooltip,
                image = LDtkIconUtility.GetUnityIcon("Folder"),
            };
        }

        public string DrawPathField()
        {
            string assetPath = Path.GetFullPath(_originalPath);
            string csPath = Path.ChangeExtension(assetPath, $".{_extension}");

            string defaultRelPath = LDtkPathUtility.GetRelativePath(assetPath, csPath);
            
            if (!DrawFieldAndButton(defaultRelPath))
            {
                string propStringValue = _pathProp.stringValue;
                if (!string.IsNullOrEmpty(propStringValue))
                {
                    return _pathProp.stringValue;
                }
                return defaultRelPath;
            }
            
            string destinationEnumPath = EditorUtility.SaveFilePanel(_filePanelDescription,
                Path.GetDirectoryName(csPath),
                Path.GetFileName(csPath), _extension);

            if (!string.IsNullOrEmpty(destinationEnumPath) && AssetDatabase.IsValidFolder(LDtkPathUtility.DirectoryOfAssetPath(destinationEnumPath)))
            {
                string relPath = LDtkPathUtility.GetRelativePath(assetPath, destinationEnumPath);
                relPath = LDtkPathUtility.CleanPathSlashes(relPath);
                _pathProp.stringValue = relPath;
            }
            else
            {
                LDtkDebug.LogWarning("Export: Cannot specify within a folder outside of the Unity project");
            }

            return _pathProp.stringValue;
        }
        
        // todo not contextual enough to a generalized folder field, might apply differently later
        public string DrawFolderField()
        {
            string assetPath = Path.GetFullPath(_originalPath);
            string defaultRelPath = LDtkPathUtility.DirectoryOfAssetPath(assetPath);
            
            if (!DrawFieldAndButton(defaultRelPath))
            {
                string propStringValue = _pathProp.stringValue;
                if (!string.IsNullOrEmpty(propStringValue))
                {
                    return propStringValue;
                }
                return defaultRelPath;
            }
            
            string destinationPath = EditorUtility.OpenFolderPanel(_filePanelDescription,
                defaultRelPath,
                "");

            if (destinationPath.StartsWith(Application.dataPath))
            {
                destinationPath = "Assets" + destinationPath.Substring(Application.dataPath.Length);
            }

            if (!string.IsNullOrEmpty(destinationPath) && AssetDatabase.IsValidFolder(destinationPath))
            {
                _pathProp.stringValue = destinationPath;
            }
            else
            {
                LDtkDebug.LogWarning($"Export: Cannot specify a folder outside of the Unity project\n{destinationPath}");
            }
            
            return _pathProp.stringValue;
        }

        private bool DrawFieldAndButton(string defaultRefPath)
        {
            bool buttonClicked = false;
            
            using (new EditorGUILayout.HorizontalScope())
            {
                Rect rect = LDtkEditorGUI.PropertyFieldWithDefaultText(_pathProp, _labelContent, defaultRefPath);
                buttonClicked = DrawButton(rect);
            }

            return buttonClicked;
        }

        private bool DrawButton(Rect rect)
        {
            Rect buttonRect = new Rect(rect)
            {
                xMin = rect.xMax - BUTTON_WIDTH
            };

            bool button;
            using (new EditorGUIUtility.IconSizeScope(Vector2.one * 16))
            {
                button = GUILayout.Button(_folderButtonContent, EditorStyles.miniButton, GUILayout.Width(BUTTON_WIDTH));
            }
            return button;
        }
    }
}