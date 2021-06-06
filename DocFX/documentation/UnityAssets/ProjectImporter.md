# Project Importer

Store the LDtk project file in the Unity project so that it can be imported.

The file format must be `.ldtk`.  
![Use Extension](../../images/img_ldtk_UseLDtkExtension.png)  

**Simply drag-and-drop this main asset into the scene.**  
![Drag-N-Drop](../../images/gif_DragNDrop.gif)  

## Hierarchy
The imported project generates a hierarchy of GameObjects:  
- Project Root
    - Levels
        - Layers
            - Entity/Tilemap GameObjects
    
![GameObject Hierarchy](../../images/img_unity_HierarchyWindow.png)


## Sub-Assets
In addition to the generated GameObjects, some other sub-assets are also generated:
- [Artifact Asset](ArtifactAssets.md)
- [Json Project](ProjectFile.md)
- Sprites
  - The sprites are used in Auto-Layers and Tile-Layers, also can be referenced for any needs.
    
![Project Window](../../images/img_unity_ProjectWindow.png)    





