# `LDtkField`
#### [API Reference](../../api/LDtkUnity.LDtkFieldAttribute.yml)

Instantiated GameObjects with scripts that use a `LDtkField` attribute in their fields will apply the field data from LDtk.

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
| LocalEnum.(type)| (type) |
| Color      | Color       |
| Point      | Vector2Int  |
| File Path  | string      |
   
### Note:
- **The C# fields must be public.** 
  - However, they can be hidden from the inspector by using `[HideInInspector]`.  

- Only the scripts in the root of the instantiated GameObject will be injected with LDtk fields. Child GameObjects will not be checked.

- The `MultiLines` type translates to create new lines correctly for Unity's text components. 
  - (ex. Text, TextMesh, etc)

- **`Point` to `Vector2Int` will not translate to the expected vector values.**  
  - This is because LDtk's coordinate system is based on a top-left origin point, and Unity's is bottom-left. When `Point` is converted over to Unity, it adjusts the Y vector value to maintain a correct position in world space. Because of this, the `Point` field is not a dependable Vector2Int for conventional means and is only expected to store values for position use-cases.  

- See the [Enums Section](../ProjectAsset/Enums.md) to learn about automatically generating enum files.

![LDtk Enum Definition](../../images/ldtk/EnumDefinition.png)
  
![Unity Enum Definition](../../images/code/EnumDefinition.png)