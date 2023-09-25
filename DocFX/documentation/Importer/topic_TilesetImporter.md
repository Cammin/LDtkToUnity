# Tileset Importer

The importer creates tileset files, which generate the sprites and tiles.
 - The file format for tileset files is `.ldtkt`.  
 - The files are required to use the importer, and are not generated initially.
 - Only modified tileset files will reimport, resulting in less unnecessary imports when applicable. Generating sprites and tiles are a marginal portion of the heavy work.
 - Tileset files can be added to a SpriteAtlas asset, and will pack all the sub-sprites generated.

## Sprite Editor
This importer interacts with the sprite editor window much like a Texture Importer. You can define physics shapes through this file!

## Inspector
The importer inspector displays a reference to the level's project which can be clicked to locate the project in the hierarchy window.  
The project can also be re-imported from here with a quick button.  
![Tileset Importer](../../images/img_Unity_TilesetImporter.png)

### Pixels Per Unit
Tile assets pixels per unit. This should always match with the project's Pixels per unit.

### Depend On Dependencies
Used by all LDtk assets. [See here.](topic_Section_Dependencies.md#depend-on-dependencies)
