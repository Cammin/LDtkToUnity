# IntGrid Tile
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkIntGridTile.yml)  

In the importer's [**IntGrid section**](../Importer/topic_Section_IntGrids.md), you can assign Int Grid Tiles, which has options for custom collision, rendering colors, and GameObjects.

Make some at `Assets > Create > LDtkIntGridTile`

![Level Inspector](../../images/img_Unity_IntGridTile.png)

### Collider Type
- None: No collision. 
  - Renders a square if rendering IntGridValues is enabled.
- Sprite: Use a sprite's physics shape(s) for collision.   
  - Renders the sprite if rendering IntGridValues is enabled.
- Grid: Square collision. 
  - Renders a square if rendering IntGridValues is enabled.

### Custom Physics Sprite
The collision shape is based on the physics shape(s) of the sprite which is previewed here for convenience.  
Commonly useful for slopes, etc.

### Game Object Prefab 
Spawns a prefab at this tile.  
Optional; Use this when a GameObject is more fit for a situation.


## Sprite Collider
When the collider type is set to Sprite, you can use a sprite's physics shape(s) for collision.  
To make a custom physics shape on a sprite, go to the **Physics Shape** section of a sprite editor window.  
![PhysicsShape](../../images/img_Unity_SpritePhysicsShape.png)