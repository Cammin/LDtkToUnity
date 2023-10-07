﻿namespace LDtkUnity
{
    internal static class LDtkHelpURL
    {
        //main: https://cammin.github.io/LDtkToUnity/
        private const string ROOT = "https://cammin.github.io/LDtkToUnity/";
        
        //Imported Assets
        public const string IMPORTER_LDTK_PROJECT = 
            ROOT + "documentation/Importer/topic_ProjectImporter.html";
        public const string IMPORTER_LDTK_LEVEL = 
            ROOT + "documentation/Importer/topic_LevelImporter.html";        
        public const string IMPORTER_LDTK_TILESET = 
            ROOT + "documentation/Importer/topic_TilesetImporter.html";

        public const string JSON_PROJECT = 
            ROOT + "documentation/Topics/topic_ProjectFile.html";
        public const string JSON_LEVEL = 
            ROOT + "documentation/Topics/topic_LevelFile.html";
        public const string JSON_TILESET = 
            ROOT + "documentation/Topics/topic_TilesetFile.html";
        
        //section contents
        public const string SECTION_MAIN = 
            ROOT + "documentation/Importer/topic_Section_Main.html";
        
        public const string SECTION_INTGRID = 
            ROOT + "documentation/Importer/topic_Section_IntGrids.html";
        
        public const string SECTION_ENTITIES = 
            ROOT + "documentation/Importer/topic_Section_Entities.html";
        
        public const string SECTION_ENUMS = 
            ROOT + "documentation/Importer/topic_Section_Enums.html";

        public const string SECTION_DEPENDENCIES =
            ROOT + "documentation/Importer/topic_Section_Dependencies.html";

        //components that would automatically be added in the import process
        public const string COMPONENT_SCENE_DRAWER = 
            ROOT + "documentation/Importer/topic_Section_Entities.html#scene-drawing";
        public const string COMPONENT_FIELDS = 
            ROOT + "documentation/Topics/topic_Fields.html";

        public const string COMPONENT_PROJECT =
            ROOT + "api/LDtkUnity." + nameof(LDtkComponentProject) + ".html";
        public const string COMPONENT_WORLD =
            ROOT + "api/LDtkUnity." + nameof(LDtkComponentWorld) + ".html";
        public const string COMPONENT_LEVEL =
            ROOT + "api/LDtkUnity." + nameof(LDtkComponentLevel) + ".html";
        public const string COMPONENT_LAYER =
            ROOT + "api/LDtkUnity." + nameof(LDtkComponentLayer) + ".html";
        public const string COMPONENT_IID = 
            ROOT + "api/LDtkUnity." + nameof(LDtkIid) + ".html";

        public const string SO_ARTIFACT_ASSETS = 
            ROOT + "documentation/Topics/topic_ArtifactAssets.html";
        public const string SO_INT_GRID_TILE = 
            ROOT + "documentation/Topics/topic_IntGridTile.html";
        public const string SO_TILESET_TILE = 
            ROOT + "documentation/Importer/topic_TilesetImporter.html";
        public const string SO_TOC = 
            ROOT + "documentation/Topics/topic_TableOfContents.html";
        
        
        public const string SO_ART_TILE_OVERRIDE = 
            ROOT + "documentation/Topics/topic_ArtifactAssets.html"; //todo change this to better page setup when animated tiles are implemented
        
        public const string EXPORT_NATIVE_PREFAB =
            ROOT + "documentation/Topics/topic_ExportNativePrefab.html";

    }
}