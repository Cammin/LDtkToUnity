# Fields

Entity Instances can have fields in the LDtk editor. They can translate to Unity.
![LDtk Editor Entity Fields](../../images/ldtk/EntityFields.png)  
Instantiated Entity/Level prefabs will gain a fields component if they are defined in LDtk.  
![Section](../../images/unity/inspector/Fields.png)  
You can get any field's value on an entity/level with the field's matching identifier through code.

- **During Runtime:** Get a reference to the component.
  ```
  int hp = GetComponent<LDtkFields>().GetInt("HP");
  ```    

- **During Import:** Use the [ILDtkImportedFields](../../api/LDtkUnity.ILDtkImportedFields.yml) interface on any custom scripts.  
  ```
  public class Player : MonoBehaviour, ILDtkImportedFields
  {
        public Item[] items;
        public int hp;

        public void OnLDtkImportFields(LDtkFields fields)
        {
            items = fields.GetEnumArray<Item>("inventory");
            hp = fields.GetColor("HP");
        }
  }
  ```
Refer to the [LDtkFields](../../api/LDtkUnity.LDtkFields.yml) scripting reference for getting field values with this component.

### Note:

- The `MultiLines` type translates to create new lines correctly for Unity's text components. 
  - (ex. Text, TextMesh, etc)

- **`Point` to `Vector2` will not translate to the expected vector values.**  
  - This is because LDtk's coordinate system is based on a top-left origin point, and Unity's is bottom-left. When `Point` is converted over to Unity, it adjusts the Y vector value to maintain a correct position in world space. Because of this, the `Point` field is not a dependable Vector2 for conventional means and is only expected to store values for position use-cases.  

- The enum values are serialized as strings in the inspector. 
  - This is because enum scripts may or may not be generated.
  - See the [Enums Section](../Importer/Enums.md) to learn about generating enum files during import.