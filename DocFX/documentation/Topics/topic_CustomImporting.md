# Custom Importing
When importing a LDtk project, there may be some specific customization needed on a case by case basis for a project.

- [Postprocessor](#ldtkpostprocessor)  
- [Interfaces](#import-event-interfaces)  

## LDtkPostprocessor

[_**Scripting Reference**_](../../api/LDtkUnity.Editor.LDtkPostprocessor.yml)

Inspired by [Unity's own workflow](https://docs.unity3d.com/ScriptReference/AssetPostprocessor.html), this lets you hook into the import pipeline and run scripts after importing an LDtk project.


Various parts of the project can be modified over:
- Projects
- Levels
- AutoLayer Layers
- IntGrid Layers
- Backgrounds
- Entities

For example, this could be useful if a different material was needed for a tilemap renderer.

To use this, create a script and make it inherit from [`LDtkPostprocessor`](../../api/LDtkUnity.Editor.LDtkPostprocessor.yml). Then hook into it's various virtual voids.
Keep in mind that this



## Import Event Interfaces
During the import process, any level/entity prefabs with custom scripts inheriting these interfaces can trigger functions during the project import process. 

For example, These could be useful to immediately set serialized fields in a component instead of getting them in runtime, or to properly set the sorting order of entity prefabs between certain tile layers. 

- Level/Entities:
  - [**ILDtkImportedFields**](../../api/LDtkUnity.ILDtkImportedFields.yml)

  
- Level:
  - [**ILDtkImportedLevel**](../../api/LDtkUnity.ILDtkImportedLevel.yml)
  

- Entities:
  - [**ILDtkImportedEntity**](../../api/LDtkUnity.ILDtkImportedEntity.yml)
  - [**ILDtkImportedSortingOrder**](../../api/LDtkUnity.ILDtkImportedSortingOrder.yml)
  - [**ILDtkImportedLayer**](../../api/LDtkUnity.ILDtkImportedLayer.yml)