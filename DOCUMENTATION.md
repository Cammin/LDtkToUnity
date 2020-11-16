# Documentation
The documentation will guide you with the use of this tool.

## Premise
A level gets built by supplying three things: The project data, the level identifier to build, and the project's assets used. An entire level gets built during runtime, so it's expected to be used in a relatively empty scene.

## Level Builder Controller
This Monobehaviour component is a simple way to get a LDtk level built, which builds a specified level upon it's `Start` event.  
Supply it with the 
[LDtk project file](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#the-project), a 
[Level Identifier](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#level-identifier-asset) asset and the 
[Project Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#project-assets) asset.  
![Level Builder Controller Component](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/BuilderControllerComponent.png)  
For more control instead of using this component,(WIP)

## The Project
Store the LDtk project file in the Unity project so that it can be referenced as a Text Asset. (Also helps with being tracked by source control in addition to the Unity project) ![LDtk Project](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetProjectJson.png)  
The `.ldtk` file format is able to be recognised as a Text Asset just like `.json`. 

## Level Identifier Asset
Level Identifiers offer an asset-based approach to keeping track of levels from LDtk. It's used to inform which level we want the Level builder to build. Created under

Create from the Asset Menu:
`Create > LDtk > LDtkLevelIdentifier`.  
![Level Identifier Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetLevel.png)  
**Ensure the asset's name matches with the corresponding level's identifier from the LDtk editor.**   

## Project Assets
The Project Assets is the main asset for containing all the elements of a level:
- [IntGrid Value Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#intgrid-value-asset)
- [Entity Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#the-project)
- [Tileset Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#the-project)

Create the Project Assets asset from the Asset Menu:
`Create > LDtk > LDtkProjectAssets`.  
![Project Assets](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetProject.png)


## IntGrid Value Asset
The IntGrid Value asset is used to define what sort of collider an IntGrid value is. (Block, Slope, etc.)  
It stores a sprite, which is purely used to use the physics shape of the tile and nothing else.  

To make IntGrid layer assets, create the IntGrid asset and it's Collection asset, created under  
`Create > LDtk > LDtkIntGridValue`  
![IntGrid Value Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetIntGridValue.png)
`Create > LDtk > LDtkIntGridValueCollection`  
![IntGrid Value Collection](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetIntGridValueCollection.png)
<br />

Once some tiles have been made and added to a collection, We will need a tilemap to set these tiles to during runtime when a level is built.  
It's up to your discretion how you want to make the Tilemap prefab, but the bare minimum is a prefab with a `Grid` component, and it's child GameObject containing a Tilemap component.  
Feel free to add what you wish (such as TileMapRenderer, TilemapCollider2D, CompositeCollider2D, etc). Look at the example project for guidance.

## Entity Asset
To make Entity Instance assets, create the Entity Instance objects and it's Collection object, created under  
`Create > LDtk > LDtkEntityInstance`  
![Entity Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetEntity.png)  
<br />
`Create > LDtk > LDtkEntityInstanceCollection`  
![Entity Asset Collection](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetEntityCollection.png)

## Tilemap Asset
You can assign a sprite into here, which is the same image file that is used as the same tileset in the LDtk editor.
Ensure to name the asset the same name as the Tileset's identifier from within the LDtk editor.
To make Tilemap assets, create the Tilemap objects and it's Collection object, created under  
`Create > LDtk > LDtkTileset`  
![Tileset Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetTileset.png)

`Create > LDtk > LDtkTilesetCollection`  
![Tileset Collection](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetTilesetCollection.png)


## Entity Field Injection

Entity Instances can have fields in the LDtk editor.  
![LDtk Editor Entity Fields](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkEditorMobFields.png)  
These fields can be applied to the scripts of instantiated GameObjects.  
![Unity Entity Fields](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/UnityMobFields.png)  

Type translation table:
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

Note: **`Point` to `Vector2Int` will not translate to the expected vector values.**
This is because LDtk's coordinate system is based on a top-left origin point, and Unity's is bottom-left. When  `Point` is converted over to Unity, it adjusts the Y vector value to maintain a correct position in world space. Because of this, the `Point` field is not a dependable Vector2Int for conventional means and is only expected to store values for position purposes.

The `MultiLines` type translates to create new lines lines correctly for Unity's text components. (ex. Text, TextMesh, etc)

### `LDtkField` Attribute  
When we utilize an Entity Instance, it might have instance fields set up from the LDtk editor.  
You can apply the values upon instantiation by add this attribute on fields with matching names.  
`[LDtkField] public int theInt = default;`  
Also for arrays.  
`[LDtkField] public int[] intArray = default;`  

Alternatively, you can pass a string argument into the attribute to separate the naming of the LDtk field identifier from the field name itself.  
`[LDtkField("int")] public int _theInt = default;`  
`[LDtkField("intArray")] public int[] _theInts = default;`  
   
**Note:**
- **The fields must be public.**
- **Enums must match naming conventions for both type and value as they are in the LDtk editor.**



### `ILDtkFieldInjectedEvent`
An interface that contracts a function to fire after an entity instance's fields is finished being injected. The order of execution is as follows:<br />
`Awake` -> `OnEnable` -> `OnLDtkFieldsInjected` -> `Start`
- **` LDtkInjectableFieldAttribute`s do not require this interface to be implemented to work. `ILDtkInjectedFieldEvent` is optional when needed.**

### `LDtkLevelBuilder.OnLevelBuilt`
A static event that fires after a level is finished building and all entities injected.
<br />
