using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    public class PathDrawer
    {
        private readonly SerializedProperty _pathProp;
        private readonly string _originalPath;
        private readonly GUIContent _labelContent;
        private readonly GUIContent _folderButtonContent;
        private readonly string _extension;
        private readonly string _filePanelDescription;


        public PathDrawer(GUIContent labelContent, SerializedProperty pathProp, string originalPath, string folderButtonTooltip, string extension = "", string filePanelDescription = "Select location")
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

        public string GetRelativePath(string fromPath, string destinationPath)
        {
            Uri startUri = new Uri(fromPath);
            Uri endUri = new Uri(destinationPath);
            Uri relUri = startUri.MakeRelativeUri(endUri);
            string rel = Uri.UnescapeDataString(relUri.ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            /*if (rel.Contains(Path.DirectorySeparatorChar.ToString()) == false)
            {
                rel = $".{ Path.DirectorySeparatorChar }{ rel }";
            }*/
            return rel;
        }

        public void DrawPathField()
        {
            string assetPath = Path.GetFullPath(_originalPath);
            string csPath = Path.ChangeExtension(assetPath, $".{_extension}");

            string defaultRefPath = GetRelativePath(assetPath, csPath);

            const float buttonWidth = 26;
            Rect rect = LDtkEditorGUI.PropertyFieldWithDefaultText(_pathProp, _labelContent, defaultRefPath, buttonWidth + 2);
            Rect buttonRect = new Rect(rect)
            {
                xMin = rect.xMax - buttonWidth
            };

            bool button;
            using (new LDtkIconSizeScope(Vector2.one * 16))
            {
                button = GUI.Button(buttonRect, _folderButtonContent, EditorStyles.miniButton);
            }

            if (!button)
            {
                return;
            }
            
            string destinationEnumPath = EditorUtility.SaveFilePanel(_filePanelDescription,
                Path.GetDirectoryName(csPath),
                Path.GetFileName(csPath), _extension);

            /*if (destinationEnumPath.StartsWith(Application.dataPath))
            {
                destinationEnumPath = "Assets/" + destinationEnumPath.Substring(Application.dataPath.Length + 1);
            }*/
            
            if (!string.IsNullOrEmpty(destinationEnumPath))
            {
                string relPath = GetRelativePath(assetPath, destinationEnumPath);
                relPath = LDtkPathUtility.CleanPathSlashes(relPath);
                _pathProp.stringValue = relPath; 
            }
        }
        
        /// <summary>
        /// bad organization, combine these 2 functions
        /// </summary>
        public void DrawFolderField()
        {
            string assetPath = Path.GetFullPath(_originalPath);
            string csPath = assetPath;

            string defaultRefPath = GetRelativePath(assetPath, csPath);

            const float buttonWidth = 26;
            Rect rect = LDtkEditorGUI.PropertyFieldWithDefaultText(_pathProp, _labelContent, defaultRefPath, buttonWidth + 2);
            Rect buttonRect = new Rect(rect)
            {
                xMin = rect.xMax - buttonWidth
            };

            bool button;
            using (new LDtkIconSizeScope(Vector2.one * 16))
            {
                button = GUI.Button(buttonRect, _folderButtonContent, EditorStyles.miniButton);
            }

            if (!button)
            {
                return;
            }
            
            string destinationEnumPath = EditorUtility.OpenFolderPanel(_filePanelDescription,
                Path.GetDirectoryName(csPath),
                Path.GetFileName(csPath));

            /*if (destinationEnumPath.StartsWith(Application.dataPath))
            {
                destinationEnumPath = "Assets/" + destinationEnumPath.Substring(Application.dataPath.Length + 1);
            }*/
            
            if (!string.IsNullOrEmpty(destinationEnumPath))
            {
                string relPath = GetRelativePath(assetPath, destinationEnumPath);
                relPath = LDtkPathUtility.CleanPathSlashes(relPath);
                _pathProp.stringValue = relPath; 
            }
        }
    }
}