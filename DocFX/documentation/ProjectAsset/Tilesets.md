# Tilesets Section
The textures are used to generate Tile Collections in the section below this one.  
![Section](../../images/unity/inspector/Tilesets.png)

Hit the button at the bottom of this section to automatically assign the textures.

This section is not involved in the level building process. However, it is necessary to generate/update the Tile Collections for Tile/AutoLayer tiles.


## Sprite Button
Hit the Sprite button to automatically slice the texture into sprites Exactly as they are sliced in LDtk.
Slicing the texture is necessary to generate the Tile Collection for this texture. (See next)

_**Warning**_: This removes all previously sliced sprites before generating new slices in this texture.


## Tile Collection Button

Hit the Tile Collection button to generate a Tile Collection, containing a tile for every sliced sprite.  
Make sure you create your sliced sprites first, or else you may not generate all of the expected tiles.  
These generated Tile Collections would be assigned to their appropriate field in the [Tile Collections Section](TileCollections.md).  


