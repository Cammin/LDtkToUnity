# Tilemap Seams
  
Initially, imported LDtk projects may have tearing on the tilemaps.  

![Project Inspector](../../images/img_Unity_TearingTilemap.png)

### Explanation
This problem is present in every game engine, but there are several ways to fix this problem.

This is an issue related to how sprites are sliced.   
The edges may have open space on the tile's edges in the texture which is what may create this appearance.  
For a deeper explanation, You can read [this](https://tiled2unity.readthedocs.io/en/latest/fixing-seams/).


## Solutions

#### Sprite Atlas
Typically, seams are solved by adding padding to sprites. 
- For Unity, the [**Sprite Atlas**](https://docs.unity3d.com/Manual/sprite-atlas.html) is the solution.  
The sprite atlas generates a texture of the sprites but with extra padding on the edges, which fixes seams.
- Adding sprites to an atlas is also generally more optimized for the GPU.
- Ensure to configure the desired [**Sprite Packer Mode**](https://docs.unity3d.com/Documentation/Manual/SpritePackerModes.html) in Unity's project settings to confirm that the tearing is eliminated. If the sprite packer mode is set to `Always Enable`, then the tearing is fixed, but only in play mode.

#### Anti Aliasing
By default, anti aliasing is on and can be a cause for seams. You can try disabling this setting in the quality settings at `Edit > Project Settings > Quality`

#### Pixel Perfect
You can try using Unity's [**2D Pixel Perfect**](https://docs.unity3d.com/Packages/com.unity.2d.pixel-perfect@latest/index.html) package or the Cinemachine Pixel Perfect module, which can hide the issue from the camera's perspective.