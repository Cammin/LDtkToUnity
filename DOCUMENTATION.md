# Usage Guide (WIP)

*Note: An example is available from the repo's root by opening with Unity.*

The short overview process to setup using LDtk in Unity is as follows:  

- Adding/Creating a LDtk project into the Unity project (Both .ldtk and .json work, .ldtk is encouraged)
- Creating some scriptable object assets that store: 
  - IntGrid Tiles
  - Tilesets (in-progress)
  - Entity Instances
  - Level Identifiers  
- Adding some LevelBuilder scripts to a GameObject in the scene
- (more documentation soon)
<br />


## Preparing the Project
Simply store the LDtk project file in the Unity project.  
(WIP project file in project window)

## Preparing Levels
LevelIdentifiers offer a simple means of informing which level we want the Level builder to build. Created under  
`Create > LDtk > LDtkLevelIdentifier`.  
**The Asset's name is the "link" to the LDtk's level identifiers. Ensure they match.**
<br />
<br />


## IntGrid Layer

**- Used to define what sort of collider an IntGrid value is. (Block, Slope, etc.)**
**- The IntGrid asset stores a sprite, which is purely used to set the physics shape of the tile.**

To make IntGrid layer assets, create the IntGrid asset and it's Collection asset, created under  
`Create > LDtk > LDtkIntGridTile`  and  
`Create > LDtk > LDtkIntGridTileCollection` in the project view respectively.

<br />

Once some tiles have been made and added to a collection, We will need a tilemap to set these tiles to during runtime when a level is built.  
It's up to your discresion how you want to make the Tilemap prefab, but the bare minimum is a prefab with a `Grid` component, and it's child gameobject containing a Tilemap component.  
Feel free to add what you wish (such as TileMapRenderer, TilemapCollider2D, CompositeCollider2D, etc). Look to the example project for guidance.
<br />
<br />


## Preparing Tilemap Assets
To make Tilemap assets, create the Tilemap objects and it's Collection object, created under  
`Create > LDtk > LDtkTileset`  and  
`Create > LDtk > LDtkTilesetCollection` in the project view respectively.  

You can assign a sprite into here, which is the same image file that is used as the same tileset in the LDtk editor.
Ensure to name the asset the exact same name as the Tileset's identifier from within the LDtk editor.



Tilemap feature is not implemented yet.
<br />


## Preparing Entity Instance Layers
To make Entity Instance assets, create the Entity Instance objects and it's Collection object, created under  
`Create > LDtk > LDtkEntityInstance` and  
`Create > LDtk > LDtkEntityInstanceCollection` in the project view respectively.
<br />
<br />


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
- **LDtk's Point Type translates to a `Vector2Int`.**
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
