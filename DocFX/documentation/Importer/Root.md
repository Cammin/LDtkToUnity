# Root Section

The root area has some general settings.  
![Section](../../images/unity/inspector/Root.png)

### Main Pixels Per Unit
This pixels per unit number dictates what all of the instantiated Tileset scales will change their scale to, in case several LDtk layer's GridSize's are different from one another. Set this as the base scale to work from in regards to scale.

### Sprite Atlas
Create your own [Sprite Atlas](https://docs.unity3d.com/Manual/class-SpriteAtlas.html) and assign it if desired.  
All tileset art that is used in levels will be packed to the Sprite Atlas.


- This primarily solves the "tearing" in the sprites of the tilemaps with padding.
- The sprite atlas is reserved for auto-generated sprites only; Any foreign sprites assigned to the atlas will be removed.
- Only the tiles that are actually used are packed, resulting in optimal size.

### Custom Level Prefab
Optional. This prefab is instantiated as the root GameObject for all levels in the build process.  
Whether the field is assigned or not, the instantiated GameObject will have a [`LDtkFields`]() component added for getting the level's fields.  
Use this prefab field as a primary means of executing custom events upon import with the [import interfaces]().  
View more about field values at [Fields](../Topics/Fields.md)

### De-parent In Runtime
If this is set on, then the project, levels, and entity-layer GameObjects will have components that act to de-parent all of their children on start.  
This results in increased runtime performance by minimizing the hierarchy depth.  
Keep this on if the exact level/layer hierarchy structure is not a concern in runtime.  
[Article about this particular optimization](https://blogs.unity3d.com/2017/06/29/best-practices-from-the-spotlight-team-optimizing-the-hierarchy/)  



### Log Build Times
Use this to log the count of levels built, and how long it took to generate them.

### Render IntGrid Values
When this is enabled, all [IntGrid Tile Assets]() will have their tile's sprite revealed.  
If the tile's collision style is none/grid, then a square will be rendered. If using a custom physics sprite, then the sprite will be rendered.  
The sprite will have it's color affected by the IntGrid Value's color definition in LDtk.




