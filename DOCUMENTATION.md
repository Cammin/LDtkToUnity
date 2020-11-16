# Documentation
The documentation will guide you with the use of this tool.

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
   - [`LDtkLevelBuilder.OnLevelBuilt` Event](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#ldtklevelbuilderonlevelbuilt-event)


# Premise
A level gets built by supplying three things: The project data, the level identifier to build, and the project's assets used. An entire level gets built during runtime, so it's expected to be used in a relatively empty scene.

# Level Builder Controller
This Monobehaviour component is a simple way to get a LDtk level built, which builds a specified level upon it's `Start` event.  
Supply it with the 
[LDtk project file](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#the-project), a 
[Level Identifier](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#level-identifier-asset) asset and the 
[Project Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#project-assets) asset.  
![Level Builder Controller Component](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/BuilderControllerComponent.png)  
For more control instead of using this component,(WIP)

# The Project
Store the LDtk project file in the Unity project so that it can be referenced as a Text Asset.   
![LDtk Project](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetProjectJson.png)  
The `.ldtk` file format is able to be recognised as a TextAsset just like `.json` or `.txt`. 



# Level Identifier Asset
Level Identifier assets contain no fields, but it's own asset name represents a level identifier. It's used to inform the level builder which particular level to load.  

Create from the Asset Menu:  
`Create > LDtk > LDtkLevelIdentifier`.  
![Level Identifier Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetLevel.png)  
**Ensure the asset's name matches with the corresponding level's identifier from the LDtk editor.**   
![LDtk Level Identifier](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorLevel.png)  



# Project Assets
The Project Assets is the main asset for containing all the elements of a level:
- [IntGridValue Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#intgridvalue-asset)
- [Entity Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#entity-asset)
- [Tileset Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#tileset-asset)
- [Tilemap Grid Prefab](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#tilemap-grid-prefab)

Create from the Asset Menu:  
`Create > LDtk > LDtkProjectAssets`.  
![Project Assets](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetProject.png)



## IntGridValue Asset
The IntGridValue asset is used to define what sort of collider an IntGrid value is. (Block, Slope, etc.)  
It stores a sprite, which is purely used to use the physics shape of the tile and nothing else.  

Create from the Asset Menu:  
`Create > LDtk > LDtkIntGridValue`  
![IntGrid Value Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetIntGridValue.png)

**Ensure the asset's name matches with the corresponding identifiers of any IntGrid layer's values from the LDtk editor.**   
![LDtk IntGridValues](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorGridValues.png)

This asset is stored within a collection referenced by the Project Assets.  
Create from the Asset Menu:  
`Create > LDtk > LDtkIntGridValueCollection`  
![IntGrid Value Collection](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetIntGridValueCollection.png)



## Entity Asset
Entity assets store a GameObject prefab. The entities are instantiated in a position based on pivot point. Their fields are also [injected](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#entity-field-injection) to the instantiated object's scripts.

Create from the Asset Menu:  
`Create > LDtk > LDtkEntityAsset`  
![Entity Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetEntity.png)  

**Ensure the asset's name matches with the corresponding entity's identifier from the LDtk editor.**   
![LDtk Entity](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorEntityIdentifier.png)  

This asset is stored within a collection referenced by the Project Assets.  
Create from the Asset Menu:  
`Create > LDtk > LDtkEntityAssetCollection`  
![Entity Asset Collection](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetEntityCollection.png)



## Tileset Asset
You can assign a sprite into here, which is the same image file also referenced in the LDtk editor.
Ensure to name the asset the same name as the Tileset's identifier from within the LDtk editor.

Create from the Asset Menu: 
`Create > LDtk > LDtkTilesetAsset`  
![Tileset Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetTileset.png)  

**Ensure the asset's name matches with the corresponding tileset's identifier from the LDtk editor.**   
![LDtk Tileset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorTilesets.png)

This asset is stored within a collection referenced by the Project Assets.  
Create from the Asset Menu: 
`Create > LDtk > LDtkTilesetAssetCollection`  
![Tileset Collection](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetTilesetCollection.png)



## Tilemap Grid Prefab
The tilemap prefab is the object ivolved in making both the IntGridValue layers and Tile layers.
It's up to your discretion how you want to customize the Grid prefab, but the bare minimum is a GameObject with a Grid component, and it's child GameObject containing a:
- Tilemap 
- Tilemap Renderer
- Tilemap Collider 2D
- CompositeCollider2D (optional)  

Unity provides a quick and easy way to get started with `GameObject > 2D Object > Tilemap`.



# Entity Field Injection

Entity Instances can have fields in the LDtk editor.  
![LDtk Editor Entity Fields](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorMobFields.png)  

These fields can be applied to the fields in scripts of instantiated GameObjects that use a [`LDtkField`](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#ldtkfield-attribute) attribute.
![Unity Entity Fields](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/UnityMobFields.png)  



## `LDtkField` Attribute  
You can apply the values upon instantiation by add this attribute on fields with matching names.  
`[LDtkField] public int theInt;`  

Also for arrays.  
`[LDtkField] public int[] theInts;`  

Alternatively, you can pass a string argument into the attribute to individualize the naming of the LDtk field from the C# field name.  
`[LDtkField("int")] public int theInt;`  
`[LDtkField("intArray")] public int[] theInts;`  
   
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
   
### Note:
- **The fields must be public.**

- The `MultiLines` type translates to create new lines lines correctly for Unity's text components. (ex. Text, TextMesh, etc)

- **Enums must match naming conventions for both type and value as they are in the LDtk editor.**

- **`Point` to `Vector2Int` will not translate to the expected vector values.**  
This is because LDtk's coordinate system is based on a top-left origin point, and Unity's is bottom-left. When `Point` is converted over to Unity, it adjusts the Y vector value to maintain a correct position in world space. Because of this, the `Point` field is not a dependable Vector2Int for conventional means and is only expected to store values for position use-cases.



## `ILDtkFieldInjectedEvent` Interface
An interface that contracts a function to fire after an entity instance's fields is finished injection. The order of execution is as follows:<br />
`Awake`,
`OnEnable`, 
`OnLDtkFieldsInjected`,
`Start`
- **` LDtkInjectableFieldAttribute` does not require an implementation of this interface to work. `ILDtkInjectedFieldEvent` is optional when needed.**

## `LDtkLevelBuilder.OnLevelBuilt` Event
A static event that fires after a level is finished building and all entities injected.
<br />
