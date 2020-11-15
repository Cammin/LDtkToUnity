# Usage Guide

## Premise
A level gets built by supplying three things: The project data, the level identifier to build, and the project's assets used. An entire level gets built during runtime, so it's able to be used well in a relatively empty scene.

## Level Builder Controller
This component is a simple way to get a LDtk level built, which builds the level upon it's `Start` event.
Supplement it with the [LDtk project file](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#the-project), a [Level Identifier](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#level-identifier-asset) asset and the [Project Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#project-assets) asset.  
![Level Builder Controller Component](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/BuilderControllerComponent.png)  
For more control instead of using this component,(WIP)

## The Project
Store the LDtk project file in the Unity project so that it can be referenced as a Text Asset. (Also helps getting tracked by source control)
![LDtk Project](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetProjectJson.png)

## Project Assets
`Create > LDtk > LDtkProjectAssets`.  
![Project Assets](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetProject.png)

## Level Identifier Asset
Level Identifiers offer an asset-based approach to keeping track of levels from LDtk. It's used to inform which level we want the Level builder to build. Created under  
`Create > LDtk > LDtkLevelIdentifier`.  
![Level Identifier Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetLevel.png)

**Ensure the asset's name matches with the corresponding level string identifier from the LDtk editor.**   


## IntGrid Value Asset

**- Used to define what sort of collider an IntGrid value is. (Block, Slope, etc.)**
**- The IntGrid asset stores a sprite, which is purely used to set the physics shape of the tile.**

To make IntGrid layer assets, create the IntGrid asset and it's Collection asset, created under  
`Create > LDtk > LDtkIntGridValue`  
![IntGrid Value Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetIntGridValue.png)
`Create > LDtk > LDtkIntGridValueCollection`  
![IntGrid Value Collection](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetIntGridValueCollection.png)
<br />

Once some tiles have been made and added to a collection, We will need a tilemap to set these tiles to during runtime when a level is built.  
It's up to your discretion how you want to make the Tilemap prefab, but the bare minimum is a prefab with a `Grid` component, and it's child GameObject containing a Tilemap component.  
Feel free to add what you wish (such as TileMapRenderer, TilemapCollider2D, CompositeCollider2D, etc). Look at the example project for guidance.


## Tilemap Asset
To make Tilemap assets, create the Tilemap objects and it's Collection object, created under  
`Create > LDtk > LDtkTileset`  
![Tileset Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetTileset.png)


`Create > LDtk > LDtkTilesetCollection`  
![Tileset Collection](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetTilesetCollection.png)

You can assign a sprite into here, which is the same image file that is used as the same tileset in the LDtk editor.
Ensure to name the asset the same name as the Tileset's identifier from within the LDtk editor.

<br />


## Entity Asset
To make Entity Instance assets, create the Entity Instance objects and it's Collection object, created under  
`Create > LDtk > LDtkEntityInstance`  
![Entity Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetEntity.png)  
<br />
`Create > LDtk > LDtkEntityInstanceCollection`  
![Entity Asset Collection](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetEntityCollection.png)
<br />
<br />

Entity Instances can have fields in the LDtk editor.
![LDtk Editor Entity Fields]()

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

Note: **`Point` to `Vector2Int` will not translate the expected vector values.**
This is because LDtk's coordinate system is based on a top-left origin point, and Unity's is bottom-left. When  `Point` is converted over to Unity, it adjusts the y vector value to maintain a correct position in world space. Because of this, the `Point` field is not a dependable Vector2Int for conventional means and only expected to store values for position purposes.

### `LDtkField` Attribute  
When we utilize an Entity Instance, it might have instance fields set up from the LDtk editor.  
You can apply the values upon instantiation by using this attribute on fields with matching names.
``` 
[LDtkField] public int theInt = default;
[LDtkField] public float theFloat = default;
[LDtkField] public bool theBool = default;
[LDtkField] public string theString = default;
[LDtkField] public ForceMode theEnum = default;
[LDtkField] public Color theColor = default;
[LDtkField] public Vector2Int thePoint = default; 
```
Also for arrays.
``` 
[LDtkField] public int[] intArray = default;
[LDtkField] public float[] floatArray = default;
[LDtkField] public bool[] boolArray = default;
[LDtkField] public string[] stringArray = default;
[LDtkField] public ForceMode[] enumArray = default;
[LDtkField] public Color[] colorArray = default;
[LDtkField] public Vector2Int[] pointArray = default; 
```
Alternatively, you can pass in a string argument to separate the naming of the LDtk field identifier from the field name itself.
``` 
[LDtkField("int")] public int _theInt = default;
[LDtkField("float")] public float _theFloat = default;
[LDtkField("bool")] public bool _theBool = default;
[LDtkField("string")] public string _theString = default;
[LDtkField("enum")] public ForceMode _theEnum = default;
[LDtkField("color")] public Color _theColor = default;
[LDtkField("point")] public Vector2Int thePoint = default; 

[LDtkField("intArray")] public int[] _theInts = default;
[LDtkField("floatArray")] public float[] _theFloats = default;
[LDtkField("boolArray")] public bool[] _theBools = default;
[LDtkField("stringArray")] public string[] _theStrings = default;
[LDtkField("enumArray")] public ForceMode[] _theEnums = default;
[LDtkField("colorArray")] public Color[] _theColors = default;
[LDtkField("pointArray")] public Vector2Int[] _thePoints = default; 
```
**Note:**
- **The fields must be public.**
- **Enums must match the exact naming conventions for both type and value as they are in the LDtk editor.**
<br />


### `ILDtkFieldInjectedEvent`
An interface that contracts a function to fire after an entity instance's fields is finished being injected. The order of execution is as follows:<br />
`Awake` -> `OnEnable` -> `OnLDtkFieldsInjected` -> `Start`
- **` LDtkInjectableFieldAttribute`s do not require this interface to be implemented to work. `ILDtkInjectedFieldEvent` is optional when needed.**

### `LDtkLevelBuilder.OnLevelBuilt`
A static event that fires after a level is finished building and all entities injected.
<br />


## Building a level in runtime
Once all of the preparation is done, you can now begin generating your levels.
The main component to add to a GameObject is the `LDtkLevelBuilderController`, which can be found in the AddComponent Menu.
<br />
