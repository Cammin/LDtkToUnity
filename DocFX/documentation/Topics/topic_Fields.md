# Fields

[_**Scripting Reference**_](../../api/LDtkUnity.LDtkFields.yml)

Entity Instances can have fields in the LDtk editor. They can translate to Unity.  
![LDtk Editor Entity Fields](../../images/img_LDtk_EntityFields.png)  

Instantiated Entity/Level prefabs will gain a fields component if they are defined in LDtk.  
![Section](../../images/img_Unity_Importer_Fields.png)  

You can get any field's value on an entity/level with the field's matching identifier through code.

- **During Runtime:** Get a reference to the component.
  ```csharp
  int hp = GetComponent<LDtkFields>().GetInt("HP");
  ```    

- **During Import:** Use the [**ILDtkImportedFields**](../../api/LDtkUnity.ILDtkImportedFields.yml) interface on any custom scripts.  
  ```csharp
  public class Player : MonoBehaviour, ILDtkImportedFields
  {
        public Item[] items;
        public int health;

        public void OnLDtkImportFields(LDtkFields fields)
        {
            items = fields.GetEnumArray<Item>("inventory");
            health = fields.GetInt("HP");
        }
  }
  ```
  The above example uses an `Item` type, which was generated as a C# file by the importer.

## Nullables
Fields are nullable from within LDtk. They are also reflected in the fields component and can be checked if they are null in code.   
![Section](../../images/img_Unity_Importer_Fields_Nullable.png)  
See: [LDtkFields.IsNull](../../api/LDtkUnity.LDtkFields.yml)



### Note

- The `MultiLines` type translates to create new lines correctly for Unity's text components.  
(ex. Text, TextMesh, etc)

- **`Int` or `Float` may not translate to the expected value if they were set to display as a radius in LDtk.**  
This is to match the physical relative radius of entities in the LDtk editor, in case the pixels per unit setting in the importer inspector are different then expected.

- **`Point` to `Vector2` will not match the expected numerical values.**   
Points are stored as child transforms, so they will move with the entity/level. As such, they do not return the exact numerical value as indicated in LDtk.
  
- Enum values are serialized as strings in the inspector.  
This is because enum scripts may or may not be generated.  
See the [**Enums Section**](../Importer/topic_Section_Enums.md) to learn about generating enum files during import.

- Entity References are a [LDtkReferenceToAnEntityInstance](../../api/LDtkUnity.LDtkReferenceToAnEntityInstance.yml) which contains a reference to an entity and it's layer, level, and world.
- The references are internally their `iid` string. When getting the entity reference from code, it will get their GameObject of their specific `iid`. If the object was not found, then it will return null.  