# Hierarchy, Definitions and Components
The imported project generates a hierarchy of GameObjects with accompanying scripts for the json hierarchy.
- Project Root
    - Worlds
        - Levels
            - Layers
                - Entity/Tilemap GameObjects

![GameObject Hierarchy](../../images/img_unity_HierarchyWindow.png)

In addition, ScriptableObjects are generated for every definition in the project, and referenced by the appropriate components.  
![DefinitionObjects](../../images/img_unity_DefinitionObjects.png)

The whole hierarchy of components and definitions (almost) match the structure of the [json](https://ldtk.io/json/), whether instance or definition

Some alterations were made from the original json data for better ease of use in Unity:
  - X/Y values into `Vector2` or `Vector2Int`
  - Color string/int into the `Color` struct
  - Definition uid references into its definition ScriptableObject references
  - Tileset rectangles into it's sliced Sprite
  - Some component's fields are changed to reference other components instead, like the level's layers array.
  - `LDtkFields` have definition object references and can be accessed from `LDtkFields.GetDefinition`

## Components

### LDtkComponentProject
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkComponentProject.yml)

### LDtkComponentWorld
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkComponentWorld.yml)

### LDtkComponentLevel
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkComponentLevel.yml)

### LDtkComponentLayer
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkComponentLayer.yml)

### LDtkComponentEntity
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkComponentEntity.yml)

### LDtkComponentLayerIntGridValues
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkComponentLayerIntGridValues.yml)  
This component contains data to infer tilemap positions with IntGrid values.

### LDtkComponentLayerTilesetTiles
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkComponentLayerTilesetTiles.yml)  
This component contains some helper functions to work with the potential of several tilemaps. 

### LDtkComponentLayerParallax
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkComponentLayerParallax.yml)  
This component aims to match exactly what is presented in the LDtk editor if layer parallax options are utilized.

## Definition Objects

### LDtkDefinitionObjectLayer
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkDefinitionObjectLayer.yml)

### LDtkDefinitionObjectEntity
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkDefinitionObjectEntity.yml)

### LDtkDefinitionObjectTileset
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkDefinitionObjectTileset.yml)

### LDtkDefinitionObjectField
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkDefinitionObjectField.yml)

### LDtkDefinitionObjectEnum
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkDefinitionObjectEnum.yml)

### LDtkDefinitionObjectAutoLayerRule
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkDefinitionObjectAutoLayerRule.yml)

### LDtkDefinitionObjectAutoLayerRuleGroup
[_**Scripting Reference**_](../../api/LDtkUnity.LDtkDefinitionObjectAutoLayerRuleGroup.yml)