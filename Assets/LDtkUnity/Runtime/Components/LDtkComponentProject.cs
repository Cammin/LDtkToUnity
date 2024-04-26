#pragma warning disable 0414
using System;
using System.Linq;
using UnityEngine;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LDtkUnity
{
    /// <summary>
    /// A component available in the import result's root GameObject. Reference this to access the json data.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_PROJECT)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentProject : MonoBehaviour
    {
        #region  Custom
        
        [field: Tooltip("The project file, in case you want to deserialize the json yourself.")]
        [field: SerializeField] public LDtkProjectFile Json { get; private set; }
        
        #endregion
        
        [field: Space]
        [field: Tooltip("Project background color")]
        [field: SerializeField] public Color BgColor { get; private set; }
        
        [field: Tooltip("If TRUE, one file will be saved for the project (incl. all its definitions) and one file in a sub-folder for each level.")]
        [field: SerializeField] public bool ExternalLevels { get; private set; }
        
        [field: Tooltip("Unique project identifier")]
        [field: SerializeField] public LDtkIid Iid { get; private set; }
        
        [field: Tooltip("File format version")]
        [field: SerializeField] public string JsonVersion { get; private set; }
        
        [field: Tooltip("All instances of entities that have their `exportToToc` flag enabled are listed in this array.")]
        [field: SerializeField] public LDtkTableOfContents Toc { get; private set; }
        
        [field: Tooltip("This array will be empty, unless you enable the Multi-Worlds in the project advanced settings.\n\n In current version, a LDtk project file can only contain a single world with multiple levels in it. In this case, levels and world layout related settings are stored in the root of the JSON.\n With \"Multi-worlds\" enabled, there will be a `worlds` array in root, each world containing levels and layout settings. Basically, it's pretty much only about moving the `levels` array to the `worlds` array, along with world layout related values (eg. `worldGridWidth` etc).\n\nIf you want to start supporting this future update easily, please refer to this documentation: https://github.com/deepnight/ldtk/issues/231")]
        [field: SerializeField] public LDtkComponentWorld[] Worlds { get; private set; }

        #region Internal
        
        [field: Header("Internal")]
        [field: Tooltip("LDtk application build identifier.\nThis is only used to identify the LDtk version that generated this particular project file, which can be useful for specific bug fixing. Note that the build identifier is just the date of the release, so it's not unique to each user (one single global ID per LDtk public release), and as a result, completely anonymous.")]
        [field: SerializeField] public float AppBuildId { get; private set; }
        
        [field: Tooltip("Number of backup files to keep, if the `backupOnSave` is TRUE")]
        [field: SerializeField] public int BackupLimit { get; private set; }
        
        [field: Tooltip("If TRUE, an extra copy of the project will be created in a sub folder, when saving.")]
        [field: SerializeField] public bool BackupOnSave { get; private set; }
        
        [field: Tooltip("Target relative path to store backup files")]
        [field: SerializeField] public string BackupRelPath { get; private set; }
        
        [field: Tooltip("An array of command lines that can be ran manually by the user")]
        [field: SerializeField] public LDtkDefinitionObjectCommand[] CustomCommands { get; private set; }
        
        [field: Tooltip("Default size for new entities")]
        [field: SerializeField] public Vector2Int DefaultEntitySize { get; private set; }
        
        [field: Tooltip("Default grid size for new layers")]
        [field: SerializeField] public int DefaultGridSize { get; private set; }
        
        [field: Tooltip("Default background color of levels")]
        [field: SerializeField] public Color DefaultLevelBgColor { get; private set; }
        
        [field: Tooltip("Default pivot for new entities")]
        [field: SerializeField] public Vector2 DefaultPivot { get; private set; }
        
        [field: Tooltip("If the project isn't in MultiWorlds mode, this is the IID of the internal \"dummy\" World.")]
        [field: SerializeField] public string DummyWorldIid { get; private set; }
        
        [field: Tooltip("If TRUE, the exported PNGs will include the level background (color or image).")]
        [field: SerializeField] public bool ExportLevelBg { get; private set; }
        
        [field: Tooltip("If TRUE, a Tiled compatible file will also be generated along with the LDtk JSON file (default is FALSE)")]
        [field: SerializeField] public bool ExportTiled { get; private set; }
        
        [field: Tooltip("An array containing various advanced flags (ie. options or other states). Possible values: `DiscardPreCsvIntGrid`, `ExportOldTableOfContentData`, `ExportPreCsvIntGridFormat`, `IgnoreBackupSuggest`, `PrependIndexToLevelFileNames`, `MultiWorlds`, `UseMultilinesType`")]
        [field: SerializeField] public Flag[] Flags { get; private set; }
        
        [field: Tooltip("Naming convention for Identifiers (first-letter uppercase, full uppercase etc.) Possible values: `Capitalize`, `Uppercase`, `Lowercase`, `Free`")]
        [field: SerializeField] public IdentifierStyle IdentifierStyle { get; private set; }
        
        [field: Tooltip("\"Image export\" option when saving project. Possible values: `None`, `OneImagePerLayer`, `OneImagePerLevel`, `LayersAndLevels`")]
        [field: SerializeField] public ImageExportMode ImageExportMode { get; private set; }
        
        [field: Tooltip("The default naming convention for level identifiers.")]
        [field: SerializeField] public string LevelNamePattern { get; private set; }
        
        [field: Tooltip("If TRUE, the Json is partially minified (no indentation, nor line breaks, default is FALSE)")]
        [field: SerializeField] public bool MinifyJson { get; private set; }
        
        [field: Tooltip("Next Unique integer ID available")]
        [field: SerializeField] public int NextUid { get; private set; }
        
        [field: Tooltip("File naming pattern for exported PNGs")]
        [field: SerializeField] public string PngFilePattern { get; private set; }
        
        [field: Tooltip("If TRUE, a very simplified will be generated on saving, for quicker and easier engine integration.")]
        [field: SerializeField] public bool SimplifiedExport { get; private set; }
        
        [field: Tooltip("This optional description is used by LDtk Samples to show up some information and instructions.")]
        [field: SerializeField] public string TutorialDesc { get; private set; }
        
        #endregion

        internal void OnImport(LdtkJson json, LDtkProjectFile file, LDtkTableOfContents toc, LDtkComponentWorld[] worlds, LDtkIid iid)
        {
            BgColor = json.UnityBgColor;
            ExternalLevels = json.ExternalLevels;
            Iid = iid;
            JsonVersion = json.JsonVersion;
            Toc = toc;
            Worlds = worlds;
            
            //internal
            AppBuildId = json.AppBuildId;
            BackupLimit = json.BackupLimit;
            BackupOnSave = json.BackupOnSave;
            BackupRelPath = json.BackupRelPath;
            CustomCommands = json.CustomCommands.Select(p => new LDtkDefinitionObjectCommand(p)).ToArray();
            DefaultEntitySize = json.UnityDefaultEntitySize;
            DefaultGridSize = json.DefaultGridSize;
            DefaultLevelBgColor = json.UnityDefaultLevelBgColor;
            DefaultPivot = json.UnityDefaultPivot;
            DummyWorldIid = json.DummyWorldIid;
            ExportLevelBg = json.ExportLevelBg;
            ExportTiled = json.ExportTiled;
            Flags = json.Flags;
            IdentifierStyle = json.IdentifierStyle;
            ImageExportMode = json.ImageExportMode;
            LevelNamePattern = json.LevelNamePattern;
            MinifyJson = json.MinifyJson;
            NextUid = json.NextUid;
            PngFilePattern = json.PngFilePattern;
            SimplifiedExport = json.SimplifiedExport;
            TutorialDesc = json.TutorialDesc;
            
            //custom
            Json = file;
        }

        [Obsolete("Deprecated. Reference the artifact manually")]
        public LdtkJson FromJson()
        {
            LDtkDebug.LogError("Deprecated. Reference the artifact manually");
            return null;
        }
    }
}
