# Usage Guide  
The short overview process to setup using LDtk in Unity is as follows:  

- Adding/Creating a LDtk project into the Unity project (Both .ldtk and .json work, .ldtk is encouraged)
- Creating some scriptable object assets that store: 
  - IntGrid Tiles
  - Tilesets (in-progress)
  - Entity Instances
  - Level Identifiers  
- Adding some LevelBuilder scripts to a GameObject in the scene
- (more documentation soon)



## Preperation
-stub
To make a level, it will need some collision blocks, and Entity Instances.

## Preparing for IntGrid Layers
To make IntGrid layers, you will make the IntGrid Scriptable Objects, created under Create > LDtk > .



## Preparing for Tilemap Layers
- Feature not implemented yet

## Preparing for IntGrid Layers






## Preparing for Entity Instance Layers

### `LDtkInjectableField` Attribute  
When we utilize an Entity Instance, it might have instance fields set up from the LDtk editor.  
You can apply the values upon instantiation by using this attribute on fields with matching names.


``` 
[LDtkInjectableField] public int theInt = default;
[LDtkInjectableField] public float theFloat = default;
[LDtkInjectableField] public bool theBool = default;
[LDtkInjectableField] public string theString = default;
[LDtkInjectableField] public ForceMode theEnum = default;
[LDtkInjectableField] public Color theColor = default;
[LDtkInjectableField] public Vector2Int thePoint = default; 
```
Also for arrays.
``` 
[LDtkInjectableField] public int[] intArray = default;
[LDtkInjectableField] public float[] floatArray = default;
[LDtkInjectableField] public bool[] boolArray = default;
[LDtkInjectableField] public string[] stringArray = default;
[LDtkInjectableField] public ForceMode[] enumArray = default;
[LDtkInjectableField] public Color[] colorArray = default;
[LDtkInjectableField] public Vector2Int[] pointArray = default; 
```
Alternatively, you can pass in a string argument to seperate the naming of the LDtk field identifier from the field name itself.
``` 
[LDtkInjectableField("int")] public int _theInt = default;
[LDtkInjectableField("float")] public float _theFloat = default;
[LDtkInjectableField("bool")] public bool _theBool = default;
[LDtkInjectableField("string")] public string _theString = default;
[LDtkInjectableField("enum")] public ForceMode _theEnum = default;
[LDtkInjectableField("color")] public Color _theColor = default;
[LDtkInjectableField("point")] public Vector2Int thePoint = default; 

[LDtkInjectableField("intArray")] public int[] _theInts = default;
[LDtkInjectableField("floatArray")] public float[] _theFloats = default;
[LDtkInjectableField("boolArray")] public bool[] _theBools = default;
[LDtkInjectableField("stringArray")] public string[] _theStrings = default;
[LDtkInjectableField("enumArray")] public ForceMode[] _theEnums = default;
[LDtkInjectableField("colorArray")] public Color[] _theColors = default;
[LDtkInjectableField("pointArray")] public Vector2Int[] _thePoints = default; 
```

**Note:**
- **The fields must be public.**
- **LDtk's Point Type translates to a `Vector2Int`.**
- **Enum types must match the exact naming conventions for both type and value as they are in the LDtk editor.**




### `ILDtkInjectedFieldEvent`
Interface that contracts a function to fire after an entity instance's fields are finished being injected. Is invoked during instantiation, so before the `MonoBehaviour.Start` function.
- **`LDtkInjectableFieldAttribute`s do not require this interface be implemented to work. `ILDtkInjectedFieldEvent` is optional when needed.**

### `LDtkLevelBuilder.OnLevelBuilt`
Static event that fires as after as a level is finished building and all entities injected.


### Building a level in runtime
Once all of the preperation is done, you can now begin generating your levels.

More documentation soon.
