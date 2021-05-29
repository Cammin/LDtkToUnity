# Grid Prefabs Section

The Grid prefabs used when instantiating the level layers.  
![Section](../../images/unity/inspector/GridPrefabs.png)

This section is completely optional, but you can create custom grid prefabs if you wish to customize them.
If a field is not assigned, then the default will be used.
Check the rules for how a custom tilemap prefab is created in the documentation, or use the default grid prefab as a guide.

The tilemap prefab is the object involved in the creation of both the IntGridValue layers and Tile layers.

This field is only optional to set up. If the Grid prefab in the Project asset is not assigned, then a basic default prefab will be used instead.

But perhaps you may want more customization over any component, like physics interaction, renderer material, etc. Then you would want to use a custom tilemap prefab.
The bare minimum requirement is a GameObject with a Grid component, and it's child GameObject containing a:
- Tilemap 
- Tilemap Renderer
- Tilemap Collider 2D

Unity provides a quick and easy way to get started by creating `GameObject > 2D Object > Tilemap`.

