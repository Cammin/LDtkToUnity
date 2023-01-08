# Tilemap Tearing
  
Initially, imported LDtk projects may have tearing on the tilemaps.  

![Project Inspector](../../images/img_Unity_TearingTilemap.png)

## Solution
- This tearing is solved by using a [**Sprite Atlas**](../Importer/topic_Section_Main.md) with the importer. It introduces padding to the texture.

- Ensure to configure the desired [**Sprite Packer Mode**](https://docs.unity3d.com/Documentation/Manual/SpritePackerModes.html) in Unity's project settings to confirm that the tearing is eliminated.   
If the sprite packer mode is set to `Always Enable`, then the tearing is fixed, but only in play mode.

- Another setting to ensure is that anti-aliasing is disabled in the quality settings. Configurable at `Edit > Project Settings > Quality`

- An alternative solution is by using Unity's [**2D Pixel Perfect**](https://docs.unity3d.com/Packages/com.unity.2d.pixel-perfect@latest/index.html) package.

### Explanation
This is an issue related to how sprites are sliced.   
The edges may have open space on the tile's edges in the texture which is what may create this appearance.  
The sprite atlas adds padding to sprites assigned to it so that the slices no longer have the gap issue, and is also more optimized.

For a deeper explanation, You can read [this](https://tiled2unity.readthedocs.io/en/latest/fixing-seams/).