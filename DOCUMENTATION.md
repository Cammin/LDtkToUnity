# Documentation
The documentation will guide you with the use of this tool to build LDtk levels just as they are in the LDtk editor.  
This guide assumes you have an understanding of C# concepts like Events, Attributes, and Interfaces.  

This tool attempts to be as flexible as possible, but feedback is always appreciated.  
Post issues if you find any bugs or want to suggest features or ideas that offer more flexibility for programming.  
 
If you get lost, all MonoBehaviours and ScriptableObjects in this package have help references available to quickly refer back to topics in this guide.  
![Asset Reference](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/HelpUrl.png)
 
## Table of Contents
 - [Premise](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#premise)
 - [Level Builder Controller](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#level-builder-controller)
   - [The Project](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#the-project)
   - [Level Identifier Asset](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#level-identifier-asset)
   - [Project Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#project-assets)
      - [IntGridValue Asset](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#intgridvalue-asset)
      - [Entity Asset](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#entity-asset)
      - [Tileset Asset](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#tileset-asset)
      - [Tilemap Grid Prefab](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#tilemap-grid-prefab)
 - [Entity Field Injection](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#entity-field-injection)
   - [`LDtkField` Attribute](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#ldtkfield-attribute)
   - [`ILDtkFieldInjectedEvent` Interface](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#ildtkfieldinjectedevent-interface)
   - [`ILDtkSettableSortingOrder` Interface](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#ildtksettablesortingorder-interface)
   - [`ILDtkSettableOpacity` Interface](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#ildtksettableopacity-interface)
 - [`LDtkLevelBuilder.OnLevelBuilt` Event](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#ldtklevelbuilderonlevelbuilt-event)
 - [LDtk Data Properties](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#ldtk-data-properties)
 


# Premise
A level gets built by supplying two things: The project data, and the level identifier to build. An entire level gets built during runtime, so it's expected to be used in a relatively empty scene.

This tool is used for simple LDtk project deserialization for the entire project and all of it's lower data structures.  
However, it also provides an asset-based approach to easily set up 2D levels with the goal to mimic exactly what is created in the LDtk editor.  

The level builder's coordinate space only builds from (0,0,0) in a Unity scene.  This will be changed when LDtk 0.6 comes out with the world editor.  
For pixel 2D games, Using the Pixel Perfect Camera is strongly advised; available as a package from the Unity Package Manager.  


# Level Builder Controller
This MonoBehaviour component is a simple way to get a LDtk level built, which builds a specified level upon it's `Start` event.  
Supply it with the 
[LDtk project file](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#the-project), a 
[Level Identifier](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#level-identifier-asset) asset and the 
[Project Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#project-assets) asset.  
![Level Builder Controller Component](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/BuilderControllerComponent.png)  

This component is only meant for simple level creation, and only makes one level.  
For more control with custom scripting instead of using this component, Call the static method  
`LDtkLevelBuilder.BuildLevel(LDtkDataProject, LDtkLevelIdentifier, LDtkProjectAssets)`.  
`LDtkDataProject` can be created by calling the static method  
`LDtkToolProjectLoader.DeserializeProject(string)`, where the string is the LDtk project's JSON text.  



# The Project
Store the LDtk project file in the Unity project so that it can be referenced as a Text Asset.   
![LDtk Project](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetProjectJson.png)  
The `.ldtk` file format is able to be recognised as a TextAsset just like `.json` or `.txt`. 



# Level Identifier Asset
Level Identifier assets contain no fields, but its asset name represents a level identifier. It's used to inform the level builder which particular level to load.  

Create from the Asset Menu:  
`Create > LDtk > Level Identifier`.  
![Level Identifier Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetLevel.png)  
**Ensure the asset's name matches with the corresponding level's identifier from the LDtk editor.**   
![LDtk Level Identifier](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorLevel.png)  



# Project Asset
The Project Asset is the main asset for containing all the elements of a level. 
- [IntGridValue Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#intgridvalue-asset)
- [Entity Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#entity-asset)
- [Tileset Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#tileset-asset)
- [Tilemap Grid Prefab](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#tilemap-grid-prefab)

Create from the Asset Menu:  
`Create > LDtk > LDtk Project`.  

After assigning the LDtk project file, all of it's definitions will be displayed for assignment.  
If the project contains enum definitions, an auto-generate button will be available which generates a sibling folder containing a single C# script with all the enums of the project, under the namespace `LDtkUnity.Enums`. If new LDtk enums are added or change over time, hit the button to update the enums.  
![Project Assets](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetProject.png)  



## IntGridValue Asset
The IntGridValue asset is used to define what sort of collider an IntGrid value is. (Block, Slope, etc.)  
It stores a sprite, which is purely used for getting the sprite's physics shape to apply for this tile.  

Create from the Asset Menu:  
`Create > LDtk > Int Grid Value`  
![IntGrid Value Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetIntGridValue.png)

**Ensure the asset's name matches with the corresponding identifiers of any IntGrid layer's values from the LDtk editor.**   
![LDtk IntGridValues](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorGridValues.png)



## Entity Asset
Entity assets store a GameObject prefab. The entities are instantiated in a position based on a pivot point. Their fields are also [injected](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#entity-field-injection) to the instantiated object's scripts.

Create from the Asset Menu:  
`Create > LDtk > Entity`  
![Entity Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetEntity.png)  

**Ensure the asset's name matches with the corresponding entity's identifier from the LDtk editor.**   
![LDtk Entity](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorEntityIdentifier.png)  



## Tileset Asset
You can assign a sprite into this asset, which is the same image file also referenced in the LDtk editor.
Ensure to name the asset the same name as the Tileset's identifier from within the LDtk editor.  
The system will automatically know which tiles to slice and use. No manual slicing is required.

Create from the Asset Menu: 
`Create > LDtk > Tileset`  
![Tileset Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetTileset.png)  

**Ensure the asset's name matches with the corresponding tileset's identifier from the LDtk editor.**   
![LDtk Tileset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorTilesets.png)

**Important:** Any tileset sprite you use must have `Read/Write Enabled` on to work correctly.  
![Read Write Enabled](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/UnityReadWriteEnabled.png)  

## Tilemap Grid Prefab
The tilemap prefab is the object involved in the creation of both the IntGridValue layers and Tile layers.
It's up to your discretion how you want to customize the Grid prefab, but the bare minimum is a GameObject with a Grid component, and it's child GameObject containing a:
- Tilemap 
- Tilemap Renderer
- Tilemap Collider 2D

It's left open-ended like this to allow adjustment of any component, like physics interaction, renderer material, etc.  
Unity provides a quick and easy way to get started by creating `GameObject > 2D Object > Tilemap`.

(This concept may change for an easier/flexible design approach. Feedback is appreciated.)


# Entity Field Injection

Entity Instances can have fields in the LDtk editor.  
![LDtk Editor Entity Fields](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorMobFields.png)  

These fields can be applied to the fields in scripts of instantiated GameObjects that use a [`LDtkField`](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#ldtkfield-attribute) attribute.
![Unity Entity Fields](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/UnityMobFields.png)  



## `LDtkField` Attribute  
You can apply the values upon instantiation by adding this attribute to fields with matching names.  
`[LDtkField] public int theInt;`  

Also for arrays.  
`[LDtkField] public int[] theInts;`  

Alternatively, you can pass a string argument into the attribute to individualize the naming of the LDtk field from the C# field name.  
`[LDtkField("theInt")] public int customInt;`  
`[LDtkField("theInts")] public int[] customInts;`  
   
#### Type translation table
| LDtk       | C# (Unity)  |
| ---------- | ----------- |
| Int        | int         |
| Float      | float       |
| Bool       | bool        |
| String     | string      |
| MultiLines | string      |
| Enum.(type)| (type)      |
| Color      | Color       |
| Point      | Vector2Int  |
| File Path  | string      |
   
### Note:
- **The C# fields must be public.** However, they can be hidden from the inspector by using `[HideInInspector]` or `[NonSerialized]`.  

- C# fields with the `[LDtkField]` attribute are non-editable in the inspector while not in play mode. This is to communicate that the values would be injected.

- Only the scripts in the root of the instantiated GameObject will be injected with LDtk fields. Child GameObjects will not be checked.

- The `MultiLines` type translates to create new lines correctly for Unity's text components. (ex. Text, TextMesh, etc)

- **`Point` to `Vector2Int` will not translate to the expected vector values.**  
This is because LDtk's coordinate system is based on a top-left origin point, and Unity's is bottom-left. When `Point` is converted over to Unity, it adjusts the Y vector value to maintain a correct position in world space. Because of this, the `Point` field is not a dependable Vector2Int for conventional means and is only expected to store values for position use-cases.  

- C# enum definition names do not require the same name as the enum identifier in the LDtk editor (Though, it's encouraged to match enum definition names).  
**However, the enum values must match names.**  
![LDtk Enum Definition](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorEnumDefinition.png)
![Unity Enum Definition](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/UnityEnumDefinition.png)
<br/>



### `ILDtkFieldInjectedEvent` Interface
An interface that contracts a function to fire after an entity's fields is finished injection. 
Use this for immediately reacting to the values being provided to an instantiated entity.
``` 
public void OnLDtkFieldsInjected()
{
    //Initialization code for LDtk fields...
}
```
The order of execution is as follows:  
`Awake`, `OnEnable`, `OnLDtkFieldsInjected`, `Start`  
**` LDtkInjectableFieldAttribute` does not require an implementation of this interface to work. `ILDtkInjectedFieldEvent` is optional when needed.**  
<br/>



### `ILDtkSettableSortingOrder` Interface
An interface that contracts a function intended to set an entity's sorting order.  
This passes in an `int` that represents the sorting order to set.  
The sorting order value is automatically determined by the layer generation; use the interface for simply setting a renderer's sorting order if applicable. ex. Renderer, SpriteRenderer, SortingGroup, etc.
```
SpriteRenderer renderer;
public void OnLDtkSetSortingOrder(int sortingOrder)
{
    renderer.sortingOrder = sortingOrder;
}
```
<br/>



### `ILDtkSettableOpacity` Interface
An interface that contracts a function intended to set an entity's alpha colour.  
This passes in a `float` ranging from 1 to 0, using the alpha value from the LDtk layer's opacity value that the entity was stored in.  
```
SpriteRenderer renderer;
public void OnLDtkSetOpacity(float alpha)
{
    Color newColor = renderer.color;
    newColor.a = newAlpha;
    renderer.color = newColor;
}
```
<br/>



### `LDtkLevelBuilder.OnLevelBuilt` Event
A static event that fires after a level is finished building and all entities are injected their fields. Provides `LDtkDataLevel` as an argument, which can be used to access further data like layers.  
<br />

## LDtk Data Properties
All of the data of an LDtk Project is deserialized from `JSON` format into many structs. In addition to the data fields, there are also some extra utility properties to provide a smoother experience.  

The fields in the data and definitions are named exactly as they are in the LDtk [JSON_DOC](https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md) (despite the standard C# field naming conventions) to retain consistency with the original value names.

Utilize `LDtkLevelBuilder.OnLevelBuilt` static event to get the built level's data, which allows access to all of the other data associated:

| `LDtkDataLevel` Properties   | Return Type           | Summary 
|------------------------------|-----------------------|-|
| `BgColor`                    | `Color`               | The color of the `__bgColor`.
| `PxSize`                     | `Vector2Int`          | Size of the level in pixels.
| `WorldCoord`                 | `Vector2Int`          | World coordinate in pixels.
| `GetLevelBounds(int)`        | `Bounds`              | Input pixels per unit. Return world-space bounds of the level.

| `LDtkDataProject` Properties | Return Type           | Summary 
|------------------------------|-----------------------|-|
| `BgColor`                    | `Color`               | The color of the `bgColor`.
| `DefaultLevelBgColor`        | `Color`               | The color of the `defaultLevelBgColor`.
| `DefaultPivot`               | `Vector2`             | A Vector2 of `defaultPivotX` and `defaultPivotY`.
| `WorldGridSize`              | `Vector2Int`          | A Vector2Int of the `worldGridWidth` and `worldGridHeight`.

| `LDtkDataLayer` Properties | Return Type            | Summary 
|----------------------------|------------------------|-|
| `Definition`               | `LDtkDefinitionLayer`  | Reference to the Layer definition.
| `TilesetDefinition`        | `LDtkDefinitionTileset`| Reference to the Tileset definition, if exists.
| `LevelReference`           | `LDtkDataLevel`        | Reference to the level containing this layer instance.
| `IsIntGridLayer`           | `bool`                 | Returns true if the layer is an IntGrid Layer.
| `IsAutoTilesLayer`         | `bool`                 | Returns true if the layer is an AutoTiles Layer.
| `IsGridTilesLayer`         | `bool`                 | Returns true if the layer is a GridTiles Layer.
| `IsEntityInstancesLayer`   | `bool`                 | Returns true if the layer is an EntityInstance Layer.
| `CellSize`                 | `Vector2Int`           | Returns the world-space size of the layer.
| `PxTotalOffset`            | `Vector2Int`           | Returns the total layer pixel offset, including both instance and definition offsets.
| `PxOffset`                 | `Vector2Int`           | Returns the offset in pixels to render this layer.
| `LayerUnitBounds`          | `Bounds`               | Returns the world-space bounds of the layer.

| `LDtkDataTile` Properties | Return Type  | Summary 
|-------------------------- |--------------|-|
| `FlipX`                   | `bool`       | Returns if the tile is flipped horizontally.
| `FlipY`                   | `bool`       | Returns if the tile is flipped vertically.
| `LayerPixelPosition`      | `Vector2Int` | Pixel coordinates of the tile in the layer.
| `SourcePixelPosition`     | `Vector2Int` | Pixel coordinates of the tile in the tileset.
| `AutoLayerRuleID`         | `int`        | AutoLayer tiles only. The Rule ID.
| `AutoLayerCoordID`        | `int`        | AutoLayer tiles only. The Coord ID.
| `TileLayerCoordId`        | `int`        | TileLayer tiles only. The Tile Coord ID.

| `LDtkDataEntity` Properties | Return Type            | Summary 
|-----------------------------|------------------------|-|
| `Definition`                | `LDtkDefinitionEntity` | Reference of the Entity definition.

| `LDtkDataEntityTile` Properties | Return Type             | Summary 
|---------------------------------|-------------------------|-|
| `Definition`                    | `LDtkDefinitionTileset` | Reference to the Tileset definition being used by this entity tile.
| `SourceRect`                    | `Rect`                  | The rectangle that refers to the tile in the tileset image.

| `LDtkDataField` Properties | Return Type           | Summary 
|----------------------------|-----------------------|-|
| `Definition`               | `LDtkDefinitionField` | Reference of the Field definition.

| `LDtkDefinitionLayer` Properties | Return Type             | Summary 
|----------------------------------|-------------------------|-|
| `AutoSourceLayerDefinition`      | `LDtkDefinitionLayer`   | Reference of the IntGrid Layer defintion using this auto tile layer(?, TBD)
| `AutoTilesetDefinition`          | `DtkDefinitionTileset`  | Reference of the Tileset definition being used by this auto-layer rules.
| `TileLayerDefinition`            | `LDtkDefinitionTileset` | Reference of the Tileset definition being used by this tile layer.

| `LDtkDefinitionIntGridValue` Properties | Return Type | Summary 
|-----------------------------------------|-------------|-|
| `Color`                                 | `Color`     | The color of the IntGridValue.

| `LDtkDefinitionEnum` Properties | Return Type             | Summary 
|---------------------------------|-------------------------|-|
| `IconTileset`                   | `LDtkDefinitionTileset` | Reference of the tileset definition that this enum definition uses.

| `LDtkDefinitionEnumValue` Properties | Return Type | Summary 
|--------------------------------------|-------------|-|
| `SourceRect`                         | `Rect`      | The rectangle of the tile for the enum definition's tileset.


