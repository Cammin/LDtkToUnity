# Project Importer

This will be a step-by-step guide for how to use the Unity importer for LDtk.  
Lower down in the page contains information regarding the project's importer inspector.

## Setup Guide

The importer is structured where it separates the handling of tileset definitions into separate **[Tileset Files](topic_TilesetImporter.md)**.  
For that reason, this initial setup process is required so that tileset files are automatically generated after saving in LDtk.

This guide entails three major steps:
- Adding a LDtk project to the unity project
- Installing an exporter app into the Library folder
- Configuring the LDtk project to run a custom command on the exporter app

### 1. Add to the Project
   Store the LDtk project file in the Unity project so that it can be imported.
   - The file format must be `.ldtk`.  
     ![Use Extension](../../images/img_ldtk_UseLDtkExtension.png)
   - If the LDtk project was outside the unity project originally,
     make sure that all images are also transferred, and maintain their relativity with the LDtk project.
     LDtk files require a direct relative connection to the image files.
   - You can test if the images were successfully moved by opening the LDtk project and seeing if the images were loaded successfully in LDtk.

### 2. Install the Export App
   The export app will generate tileset files.
   - Select the LDtk project in Unity to show the importer inspector. 
   - You will notice an error displayed at the top instructing to install the export app.  
     ![Install Button](../../images/img_Unity_InstallTilesetExporter.png)
   - Click the Install button.  
   - After clicking, a new app is added to the Unity project's Library folder.  
     - The code of the app available here:  
     **[LDtkTilesetExporter on GitHub](https://github.com/Cammin/LDtkTilesetExporter)**

### 3. Add a Custom Command
  Now that the app is installed, LDtk can execute a command to run this app. But we still need to add the command to LDtk.
  - Again, select the LDtk project in Unity to show the importer inspector.
  - You will notice an error displayed at the top instructing to configure the project with a custom command.  
    ![Fix Button](../../images/img_Unity_FixTilesetExporter.png)
  - From here, there are two ways to add the command to the project: automatically, or manually.
    - **Automatic:**   
        Click the "Fix" button.  
        A new dialogue will appear.  
        Ensure all LDtk apps are closed, and click the "Auto-add command" button.  
        A new dialogue will appear, asking to open the project and save.  
        Open & save your LDtk project, and you're ready!  
    - **Manual:**   
        Click the "Copy" button. This copies the command to the clipboard.  
        Open the LDtk project, and open the project settings. From there, find the "Custom Commands" area.  
        Once there, create a new command, paste the command in the field, and change the timing to "Run after saving".  
        Save your LDtk project, and you're ready!  
        ![Fix Button](../../images/img_ldtk_customCommands.png)
    - When saving with a custom command for the first time, LDtk will display a disclaimer. Select "I understand the risks, allow commands".
      - If you're in doubt, the code of the app is available **[here](https://github.com/Cammin/LDtkTilesetExporter)**.

After you're ready drag the project from the project window into the scene or the hierarchy.  
![Drag-N-Drop](../../images/gif_DragNDrop.gif)  
Tip: Double clicking the LDtk file in the project window will conveniently open the project in LDtk.

## Inspector
The importer inspector is composed of several sections:   
[**Main**](topic_Section_Main.md), 
[**IntGrids**](topic_Section_IntGrids.md), 
[**Entities**](topic_Section_Entities.md), 
[**Enums**](topic_Section_Enums.md),
and [**Dependencies**](topic_Section_Dependencies.md).  
![Inspector](../../images/img_Unity_ProjectAsset.png)
- After making any changes, click the apply button at the bottom to reimport.
- If any section is hidden, it's because there were no associated definitions in the LDtk project.


## Hierarchy
The imported project generates a hierarchy of GameObjects. 
All of the objects have accompanying scripts that contain useful data.
- Project Root
  - Worlds
      - Levels
          - Layers
              - Entity/Tilemap GameObjects
    
![GameObject Hierarchy](../../images/img_unity_HierarchyWindow.png)

## Sub-Assets
In addition to the generated GameObjects, some other sub-assets are also generated:
- [**Artifact Asset**](../Topics/topic_ArtifactAssets.md)
- [**Json Project**](../Topics/topic_ProjectFile.md)
- [**Table of Contents**](../Topics/topic_TableOfContents.md)
- Sprites
  - The sprites are used in Auto-Layers, Tile-Layers, and Tile fields.  
  Referencing these sprite assets for any purpose outside of what's managed by the importer can be risky, as they could go missing if the tile is no longer used in the LDtk project. 
    
![Project Window](../../images/img_unity_ProjectWindow.png)

## Tilesets
All tileset sprites will be automatically generated.  
However, Aseprite files are currently not supported and will arrive in a future update.


## Nested Prefabs
Imported LDtk projects can be nested in prefabs.  
![Nested Project](../../images/img_Unity_NestedProject.png)
![Nested Project Variant](../../images/img_Unity_NestedProjectVariant.png)