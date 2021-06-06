# IntGrid Tile
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkIntGridTile.yml)  

In the importer's [IntGrid section](../Importer/topic_IntGrids.md), you can assign Int Grid tiles, which has options for custom collision, rendering colors, and GameObjects.

Make some at `Assets > Create > LDtkIntGridTile`

![Level Inspector](../../images/img_Unity_IntGridTile.png)

### Collider Type
- None: No collision. Renders a square if rendering IntGridValues is enabled.
- Sprite: Use a sprite's physics shape(s) for collision. Renders the sprite if rendering IntGridValues is enabled.
- Grid: Square collision. Renders a square if rendering IntGridValues is enabled.

### Custom Physics Sprite
The collision shape is based on the physics shape(s) of the sprite which is previewed here for convenience. Commonly useful for slopes, etc.

### Game Object Prefab
Optional.  
The GameObject spawned at this tile.

