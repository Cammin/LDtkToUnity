![LDtkToUnity](DocFX/images/img_logo_GitHub.png)  
[![OpenUPM](https://img.shields.io/npm/v/com.cammin.ldtkunity?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.cammin.ldtkunity/)
[![Compatibility](https://img.shields.io/badge/-2019.3+-11191F?logo=Unity)](https://unity3d.com/get-unity/download/archive)
[![GitHub Repo stars](https://img.shields.io/github/stars/Cammin/LDtkUnity?color=%23dca&label=%E2%AD%90)](https://github.com/Cammin/LDtkToUnity)
[![Hits](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2FCammin%2FLDtkToUnity&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=views&edge_flat=false)](https://hits.seeyoufarm.com)
[![Downloads](https://img.shields.io/badge/dynamic/json?color=brightgreen&label=downloads&query=%24.downloads&suffix=%2Fmonth&url=https%3A%2F%2Fpackage.openupm.com%2Fdownloads%2Fpoint%2Flast-month%2Fcom.cammin.ldtkunity)](https://openupm.com/packages/com.cammin.ldtkunity/)
[![Discord](https://img.shields.io/badge/Discord-7789FF?logo=Discord)](https://discord.gg/7RPGAW9dJx)  

A Unity importer system for [deepnight](https://deepnight.net/)'s [Level Designer Toolkit](https://ldtk.io/).

[![Discord](https://img.shields.io/badge/Discord-7789FF?style=for-the-badge&logo=Discord)](https://discord.gg/7RPGAW9dJx)  
[![Install](https://img.shields.io/badge/📁%20Install-7393B3?style=for-the-badge)](https://cammin.github.io/LDtkToUnity/documentation/Installation/topic_Install.html)  
[![Documentation](https://img.shields.io/badge/📚%20Documentation-FFCE00?style=for-the-badge)](https://cammin.github.io/LDtkToUnity/)  
[![Changelog](https://img.shields.io/badge/✨%20Changelog-3a3b3c?style=for-the-badge)](Assets/LDtkUnity/CHANGELOG.md)  
[![Survey](https://img.shields.io/badge/📝%20Provide%20Feedback-7520B9?style=for-the-badge&logo)](https://forms.gle/a7iRkuBFxpgZpwRd8)  
[![Trello](https://img.shields.io/badge/Project%20Tracking-blue?style=for-the-badge&logo=Trello)](https://trello.com/b/YPgO5283)  
[![OpenUPM](https://img.shields.io/badge/📦%20OpenUPM%20Page-3068E5?style=for-the-badge)](https://openupm.com/packages/com.cammin.ldtkunity/)  
<!-- [![KoFi](https://img.shields.io/badge/Donate-a73b38?style=for-the-badge&logo=Ko-fi)](https://ko-fi.com/cammin41357) -->

**It's a simple drag and drop!**  
![DragNDrop](DocFX/images/gif_DragNDrop.gif)


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
- Many properties and functions that extend onto LDtk data for better context in Unity ([API](https://cammin.github.io/LDtkToUnity/api/LDtkUnity.html))
- Works with/without separate level files
  - Can also add individual levels for modularity (ex. dungeon rooms)
- Custom scripted postprocessor to customize the import result ([API](https://cammin.github.io/LDtkToUnity/documentation/Topics/topic_CustomImporting.html))
- Supports Unity's [Configurable Enter Play Mode](https://docs.unity3d.com/Manual/ConfigurableEnterPlayMode.html)  
- [Fully documented](https://cammin.github.io/LDtkToUnity/)

**Can add separate level files too!**  
![Drag-N-Drop](DocFX/images/gif_DragNDropLevel.gif)

If you have any questions/problems then post an issue; I'd gladly take any feedback.  
Alternatively, contact me at cameo221@gmail.com, the [Discord server](https://discord.gg/7RPGAW9dJx), or Twitter [@CKrebbers](https://twitter.com/CKrebbers).  

If you like this importer, consider signing a [survey](https://forms.gle/a7iRkuBFxpgZpwRd8) for feedback.  
If you make a game using this, then I'd be happy to check it out! Showcase your work on the [Discord server](https://discord.gg/7RPGAW9dJx) or give me a shout-out on [Twitter](https://twitter.com/CKrebbers).

### Disclaimer
This project has stabilized development! :tada:  
There may still be some new features, but development is calming down.  
The changelog documents any breaking changes, but feel free to post issues regarding any problems.

###### Premise & Review
This importer started as an attempt to produce levels in an easy and streamlined fashion.
When searching for a solution to mass-produce levels, I discovered LDtk, as its really fun to use and is relatively young. 
I started working on a personal level importer, but after noticing how convenient and standalone the importer has become, I took it upon myself to make this a public repo as a package!
I enjoy it's production, and learning a lot in the process.

<!-- 
**Entities have scene drawers like in LDtk!**  
![Scene](DocFX/images/img_Unity_SceneDrawers.png)
-->

![Opacity](DocFX/images/gif_LDtkUnityOpacity.gif)  
