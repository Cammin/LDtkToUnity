# Dependencies Section

Dependencies ensure to keep your LDtk asset updated when these dependencies change.    
This section lists all of the dependencies for the inspected project/level.  
When an LDtk project or level depends on an asset, then the project/level will reimport when a particular dependency is changed.

## Project Dependencies
The project can depend on numerous assets.
![Section](../../images/img_Unity_Section_Dependencies.png)  
If separate level files are **not** used, projects will add dependencies for:
- Tileset files
- Level background textures 
- Custom level prefab 
- IntGrid Tiles 
- Entities  

If separate level files are used, projects will add dependencies for:
- Tileset files
- Level background textures  

All other assets will be depended on by the levels instead.  

## Level Dependencies

This section is also listed in the importer inspector for separate level files.  
![Section](../../images/img_Unity_Section_Dependencies_Level.png)  
Separate level files will depend on:
- The source project
- Level background textures
- Custom level prefab
- IntGrid tiles 
- Entities

Any dependencies in levels are smartly depended on; only the IntGrid tiles and entities that are involved inside a level are depended on.  
For example, the image above for the level will only depend on `Item` and `Ladder` because they are the only type of entities existing in this particular level. Whereas a project without separate levels files would depend on all used assets.  

In the future, dependencies will be handled a bit more smartly. For one, only the levels that use a given tileset should depend on the appropriate tileset, and projects should never depend on tileset files.  

## Tileset Dependencies
Tileset files only depend on their texture.  
![Section](../../images/img_Unity_Section_Dependencies_Tileset.png)

## Depend On Dependencies
Dependency sections have a toggle field that can change if the associated file should reimport if dependencies are changed. (ex. saved changes to a prefab)  
If turned off, then this LDtk project/level will not update the import result when the dependencies are changed (which includes updating prefabs in the import result), in which case you will need to manually reimport this project/level.  
Only consider turning off if changes to assets are causing too many unwanted reimports.