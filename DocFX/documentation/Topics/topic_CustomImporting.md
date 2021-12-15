# Custom Importing
When importing a LDtk project, there may be a specific customization needed on an imported project.

- [**Postprocessor**](#ldtkpostprocessor)  
- [**Interfaces**](#import-event-interfaces)  

## LDtkPostprocessor

Inspired by [**Unity's own workflow**](https://docs.unity3d.com/ScriptReference/AssetPostprocessor.html), this lets you hook into the import pipeline and run scripts after importing an LDtk project.  
This will allow alterations to the import result depending on what is needed for your game.

For example, this could be useful to change a material for some tilemap renderers, or to give entities a tag for a specific level.

To use this, create a class that inherits from [**LDtkPostprocessor**](../../api/LDtkUnity.Editor.LDtkPostprocessor.yml), and then override any of the two methods:  
- [**OnPostProcessProject**](../../api/LDtkUnity.Editor.LDtkPostprocessor.yml)  
- [**OnPostProcessLevel**](../../api/LDtkUnity.Editor.LDtkPostprocessor.yml)  

Note:   
LDtkPostprocessor is in the `LDtkUnity.Editor` namespace, so remember to keep any files inheriting from LDtkPostprocessor to be contained in an [**editor folder**](https://docs.unity3d.com/Manual/SpecialFolders.html), or have the script contained within an editor-only [**assembly definition**](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html).

```
using LDtkUnity;
using LDtkUnity.Editor;
using UnityEngine;

public class ExamplePostprocessor : LDtkPostprocessor
{
    protected override void OnPostProcessProject(GameObject root)
    {
        Debug.Log($"Post process LDtk project: {root.name}");
    }

    protected override void OnPostProcessLevel(GameObject root, LdtkJson projectJson)
    {
        Debug.Log($"Post process LDtk level: {root.name}");
    }
}
```


## Import Event Interfaces
During the import process, any level/entity prefabs with custom MonoBehaviours inheriting these interfaces can trigger functions during the project import process. 

For example, These could be useful to immediately set serialized fields in a component instead of getting them in runtime, or to utilise the sorting order of entity prefabs to render between layers. 

- Level/Entities:
  - [**ILDtkImportedFields**](../../api/LDtkUnity.ILDtkImportedFields.yml)

  
- Level:
  - [**ILDtkImportedLevel**](../../api/LDtkUnity.ILDtkImportedLevel.yml)
  

- Entities:
  - [**ILDtkImportedEntity**](../../api/LDtkUnity.ILDtkImportedEntity.yml)
  - [**ILDtkImportedSortingOrder**](../../api/LDtkUnity.ILDtkImportedSortingOrder.yml)
  - [**ILDtkImportedLayer**](../../api/LDtkUnity.ILDtkImportedLayer.yml)

```
using LDtkUnity;
using UnityEngine;

public class ExampleLabel : MonoBehaviour, ILDtkImportedFields
{
    [SerializeField] private TextMesh _textMesh;
    
    //This class inherits from ILDtkImportedFields, which implements OnLDtkImportFields.
    //The LDtk entity for this prefab has a string field named "text" and a color field named "color". 
    public void OnLDtkImportFields(LDtkFields fields)
    {
        _textMesh.text = fields.GetString("text");
        _textMesh.color = fields.GetColor("color");
    }
}
```