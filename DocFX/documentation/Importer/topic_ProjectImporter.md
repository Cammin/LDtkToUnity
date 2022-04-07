# Project Importer

Store the LDtk project file in the Unity project so that it can be imported.

The file format must be `.ldtk`.  
![Use Extension](../../images/img_ldtk_UseLDtkExtension.png)  

**Simply drag-and-drop this main asset into the scene.**  
![Drag-N-Drop](../../images/gif_DragNDrop.gif)  

## Inspector
The importer inspector is composed of several sections:   
[**Main**](topic_Section_Main.md), 
[**IntGrids**](topic_Section_IntGrids.md), 
[**Entities**](topic_Section_Entities.md), 
and [**Enums**](topic_Section_Enums.md).
![Inspector](../../images/img_Unity_ProjectAsset.png)
- After making any changes, click the apply button at the bottom to reimport.
- If any section is hidden, it's because there were no associated definitions in the LDtk project.


## Hierarchy
The imported project generates a hierarchy of GameObjects:  
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
- Sprites
  - The sprites are used in Auto-Layers and Tile-Layers, but can also can be referenced for any needs.
    
![Project Window](../../images/img_unity_ProjectWindow.png)   


## Reimport
If there are any problems in the import process, try reimporting.  
  If there are still problems, [**post an issue on GitHub**](https://github.com/Cammin/LDtkUnity/issues).  
  ![Reimport](../../images/img_Unity_Reimport.png)

## Tilesets
All tileset art assets will be automatically generated without any extra work.  
However, Aseprite files are currently not supported.


## Important
*This issue may be fixed in the future, but important to follow for now.*  
Due to technical issues related to how prefabs are imported, it is **not** recommended for LDtk projects/levels to be nested inside prefab and prefab variants:  
![Nested Levels](../../images/img_Unity_NestedProject.png)
![Nested Levels Variant](../../images/img_Unity_NestedProjectVariant.png)