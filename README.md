# LDtk for Unity
A package for easy Unity-integration with the [Level Designer Toolkit, created by DeepKnight](https://github.com/deepnight/ldtk).  
It's still in super early stages and prone to bugs, but I hope to share this with anyone else who can find this useful.  
Available for download through the Unity Package Manager.  

## [Documentation](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md)  

## Features  
- Supports most features of LDtk (work in progress)  
- Supports Unity's [Enter Play Mode options](https://docs.unity3d.com/Manual/ConfigurableEnterPlayMode.html)  
- Easy and convenient entity instance field injection

## Required

[Json.NET for Unity](https://github.com/jilleJr/Newtonsoft.Json-for-Unity)

## Install  
- Install Json.NET for Unity.
- Then put this in your manifest:  
 ```"com.cammin.ldtkunity": "https://github.com/Cammin/LDtkUnity.git?path=/Assets/LDtkUnity#master"```
 
 ### Features Currently Missing and Planned
 - Only supports x/y axis currently. x/z axis planned
 - Tilesets are not implemented yet
 - Plans to eliminate the requirement to create asset collections
 - Plans to implement a central LDtk settings ScriptableObject
 - Offer example download from the Unity Package Manager
 - Add utility to get `Bounds` of level
