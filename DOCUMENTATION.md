# Usage Guide



## Preparing the Project
Simply store the LDtk project file in the Unity project.  
(WIP project file in project window)

## Preparing Levels
LevelIdentifiers offer a simple means of informing which level we want the Level builder to build. Created under  
`Create > LDtk > LDtkLevelIdentifier`.  
**The Asset's name is the "link" to the LDtk's level identifiers. Ensure they match.**
<br />
<br />


## IntGrid Values

**- Used to define what sort of collider an IntGrid value is. (Block, Slope, etc.)**
**- The IntGrid asset stores a sprite, which is purely used to set the physics shape of the tile.**

To make IntGrid layer assets, create the IntGrid asset and it's Collection asset, created under  
`Create > LDtk > LDtkIntGridValue` 
![IntGrid Value Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetIntGridValue.png)
`Create > LDtk > LDtkIntGridValueCollection` 
![IntGrid Value Collection](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetIntGridValueCollection.png)
<br />

Once some tiles have been made and added to a collection, We will need a tilemap to set these tiles to during runtime when a level is built.  
It's up to your discresion how you want to make the Tilemap prefab, but the bare minimum is a prefab with a `Grid` component, and it's child gameobject containing a Tilemap component.  
Feel free to add what you wish (such as TileMapRenderer, TilemapCollider2D, CompositeCollider2D, etc). Look to the example project for guidance.
<br />
<br />


## Tilemap Assets
To make Tilemap assets, create the Tilemap objects and it's Collection object, created under  
`Create > LDtk > LDtkTileset`   



`Create > LDtk > LDtkTilesetCollection`   


You can assign a sprite into here, which is the same image file that is used as the same tileset in the LDtk editor.
Ensure to name the asset the exact same name as the Tileset's identifier from within the LDtk editor.

<br />


## Entity Assets
To make Entity Instance assets, create the Entity Instance objects and it's Collection object, created under  
`Create > LDtk > LDtkEntityInstance`
![Entity Asset](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/AssetEntity.png) 
`Create > LDtk > LDtkEntityInstanceCollection` in the project view respectively.
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
This is because LDtk's coordinate system is based on a top-left origin point, and Unity's is bottom-left. When  `Point` is converted over to Unity, it adjusts the y vector value to maintain a correct position in world space. Because of this, the `Point` field is not a dependable Vector2Int for conventional means, and only expected to store values for position purposes.

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
Alternatively, you can pass in a string argument to seperate the naming of the LDtk field identifier from the field name itself.
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
Interface that contracts a function to fire after an entity instance's fields are finished being injected. Order of excecution is as follows:<br />
`Awake` -> `OnEnable` -> `OnLDtkFieldsInjected` -> `Start`
- **`LDtkInjectableFieldAttribute`s do not require this interface be implemented to work. `ILDtkInjectedFieldEvent` is optional when needed.**

### `LDtkLevelBuilder.OnLevelBuilt`
Static event that fires as after as a level is finished building and all entities injected.
<br />


## Building a level in runtime
Once all of the preperation is done, you can now begin generating your levels.
The main component to add to a GameObject is the `LDtk Level Builder`, which can be found in the AddComponent Menu.
<br />
<br />

More documentation soon.
