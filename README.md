# LDtk to Unity
[![OpenUPM](https://img.shields.io/npm/v/com.cammin.ldtkunity?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.cammin.ldtkunity/)
[![Compatibility](https://img.shields.io/badge/-2019.2+-11191F?logo=Unity)](https://unity3d.com/get-unity/download/archive)
[![GitHub Repo stars](https://img.shields.io/github/stars/Cammin/LDtkUnity?color=%23dca&label=%E2%AD%90)](https://github.com/Cammin/LDtkUnity)

A Unity importer system for the [Level Designer Toolkit](https://ldtk.io/), created by [deepnight](https://deepnight.net/).

![Banner](DocFX/images/img_logo_GitHub.png)  


[![Documentation](https://img.shields.io/badge/Documentation-FFCE00?style=for-the-badge)](https://cammin.github.io/LDtkUnity/)  
[![Changelog](https://img.shields.io/badge/Changelog-171515?style=for-the-badge&logo=GitHub)](Assets/LDtkUnity/CHANGELOG.md)  
[![Survey](https://img.shields.io/badge/Provide%20Feedback-7520B9?style=for-the-badge&logo)](https://forms.gle/a7iRkuBFxpgZpwRd8)  
[![Trello](https://img.shields.io/badge/Project%20Tracking-blue?style=for-the-badge&logo=Trello)](https://trello.com/b/YPgO5283)  
[![OpenUPM](https://img.shields.io/badge/Open%20UPM%20Page-3068E5?style=for-the-badge)](https://openupm.com/packages/com.cammin.ldtkunity/)  

![Scene](DocFX/images/img_Unity_SceneDrawers.png)  

## Features  
- Uses [ScriptedImporters](https://docs.unity3d.com/Manual/ScriptedImporters.html) to import an LDtk project
  - Automatically re-imports whenever the LDtk project is saved
- TileLayers, AutoLayers, and level backgrounds work out of the box
  - Can pack into a [SpriteAtlas](https://docs.unity3d.com/Manual/class-SpriteAtlas.html)
- IntGridValue assets used for collision
  - Optionally GameObjects too
- Entities from prefabs
- Entities and levels have all field data available
- Automatic enum script generation
- Many properties and functions that extend onto LDtk data for better context in Unity ([API here](https://cammin.github.io/LDtkUnity/api/LDtkUnity.html))
- Works with/without separate level files
- Supports Unity's [Configurable Enter Play Mode](https://docs.unity3d.com/Manual/ConfigurableEnterPlayMode.html)  

**It's a simple drag and drop!**  
![DragNDrop](DocFX/images/gif_DragNDrop.gif)

If you have any questions/problems then post an issue; I'd gladly take any feedback.  
Alternatively, contact me at cameo221@gmail.com, on Discord at Cammin#1689, or Twitter [@CKrebbers](https://twitter.com/CKrebbers).

If you find yourself enjoying this tool, please send me a message about it! I can't track who downloads this package, so sending a kind word would make my day 😄
If you make a game using this, then I'd be happy to check it out! Contact me or give me a shout-out on [Twitter](https://twitter.com/CKrebbers).

### Disclaimer
This project is in development, so there will/may be breaking changes from new updates.  
The changelog documents the breaking changes, but feel free to post issues regarding any problems.

###### Premise & Review
This project started as an attempt to find a level creation solution without worrying about a repetitive workflow in Unity. 
When searching for a solution to mass-produce levels, I discovered LDtk, as it looks/feels amazing and is still relatively new. 
I started working on a personal level importer, but after noticing how convenient and standalone this project has become, I took it upon myself and decided to make it a public repo as a unity package! 
Enjoying it's production, and learning in the process.

![Opacity](DocFX/images/gif_LDtkUnityOpacity.gif)  
