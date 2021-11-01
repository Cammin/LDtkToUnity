# Export Native Prefab

It is possible to export a prefab, independent of LDtk and LDtkToUnity.  
![Hierarchy](../../images/img_Unity_ExportNativePrefabHierarchy.png)  

This prefab uses assets and components that are entirely native to unity, so it could function properly even if LDtkToUnity was uninstalled.

This can be particularly useful if a specific workflow benefits from this option, or if there was a need to uninstall the LDtkToUnity package, but still want to maintain the level designs in Unity.

When exporting the prefab with native unity components/assets, changes are made:  

- Assets are created:
  - All `LDtkArtTile`s will be exported to Unity's `Tile` asset. It will have it's associated exported sprite.
  - All `LDtkIntGridTile`s will be exported to Unity's `Tile` asset. It will maintain it's proper collision
  - All sprites are cloned, and associated to their corresponding new tiles.

- Components will be removed throughout the hierarchy:
  - `LDtkComponentProject`
  - `LDtkComponentLevel`
  - `LDtkFields`
  - `LDtkDetachComponent`
  - `LDtkEntityDrawerComponent`

- All tilemaps in the hierarchy will have their tiles replaced by their new native tile assets.

## Export
To begin exporting a native prefab, click the `Export` button in the importer inspector of an LDtk Project to open a new window.  
![ButtonImage](../../images/img_Unity_ExportNativePrefabButton.png)  

Inside the window, is a prefab field to assign the imported LDtk Project.   
This is automatically filled with the project used to open this window, however other projects can be assigned.  
Once a project is assigned, a path starting with the `Assets` folder can be specified for where the prefab and it's assets should be exported.   
Leave the path field empty to create the export folder relative to the LDtk project's directory.
Then select the export button.  
![EditorWindow](../../images/img_Unity_ExportNativePrefabWindow.png)

Once the export process is done, the folder will be built with this hierarchy, and the prefab is available to use.  
![Hierarchy](../../images/img_Unity_ExportNativePrefabHierarchy.png)


### Note:
- The exported prefab will be missing the `LDtkFields` component.
- The same textures for tilesets and backgrounds is still depended on.
- The same sprites for custom shapes of `LDtkIntGridValue` tiles is still depended on.



