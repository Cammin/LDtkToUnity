# LDtk for Unity
A package for easy Unity-integration with the [Level Designer Toolkit, created by deepnight](https://github.com/deepnight/ldtk).  
It's still in very early stages and prone to bugs, but I hope to share this with anyone else who can find this useful.  
Available for download through the Unity Package Manager.  

## Features  
- Fully parsed Json data into C# structs
- IntGrid tiles via ScriptableObject into Unity's Tilemap component for collision,
- Tileset sprites via ScriptableObject into Unity Tilemap component for the level art; referencing the same image that the LDtk project references(not implemented yet, WIP),
- Entities spawned from GameObject prefabs via ScriptableObject, with injectable fields into their scripts if they have some. (all of LDtk's field types supported).

- Supports Unity's [Enter Play Mode options](https://docs.unity3d.com/Manual/ConfigurableEnterPlayMode.html)  

## [Documentation](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md)  

## Required
This package uses Newtonsoft.Json for Unity to deserialize a LDtk project.  
[Newtonsoft.Json for Unity](https://github.com/jilleJr/Newtonsoft.Json-for-Unity)

## [Install/Update](https://github.com/Cammin/LDtkUnity/blob/master/INSTALL.md)
 
 ### Features Currently Missing and Planned
 
 - Tileset layer creation not implemented yet
 - Add Images into Documentation and Readme, finish documentation with strong explainations
