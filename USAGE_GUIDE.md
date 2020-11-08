# Usage Guide  
-stub


## Preperation
-stub
To make a level, it will need some collision blocks, and Entity Instances.


## Building a level  
-stub

### `LDtkInjectableField`

When we utilize an Entity Instance, it might have instance fields set up from the LDtk editor. 

You can apply the values upon instantiation by using this attribute on fields with matching names.  
**Note: The fields must be public.**

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
*LDtk's Point Type translates to a `Vector2Int`.

### `ILDtkInjectedFieldEvent`
Interface that contracts a function to fire after an entity instance's fields are finished being injected. Is invoked during instantiation, so before the `MonoBehaviour.Start` function.

### `LDtkLevelBuilder.OnLevelBuilt`
Static event that fires as soon as a level is finished building.

More documentation soon.
