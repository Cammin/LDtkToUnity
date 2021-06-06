# Entities Section

Assign GameObjects that would be spawned as entities.  
![Section](../../images/img_unity_Entities.png)

### Position
The prefab instance's transform position is set as the exact position in LDtk.  
Ensure the root prefab's pivot point is similar to the entity's pivot defined in LDtk.

### Scale
The prefab instance's transform scale adjusts accordingly if the entity instance was resized in LDtk.  
The scale value is determined by the difference between the original entity size and the resized entity in LDtk.  
So when making the prefab for an entity instance, make the prefab match the exact same scale size as the entity's definition in LDtk, and not the resized entity instance.  

Example: In LDtk, an entity's definition size is 16x16 pixels and the prefab's root scale is (1, 2, 3). 
If an entity instance is resized to 32x48 pixels in the level, then the scale multiplier would be (2, 3, 1), and thus, the resultant scale of the entity instance in Unity would be (2, 6, 3)

### Scene Drawing
If an entity has fields that `Display in Editor`, they will draw here in the scene.

If the entity's editor visual uses a tileset tile, then it will be displayed in the scene view as a gizmo.

![Section](../../images/img_Unity_SceneDrawers.png)

## Fields
The instantiated GameObject will have a [`LDtkFields`]() component added for getting the level's fields.  
Use this prefab field as a primary means of executing custom events upon import with the [import interfaces]().  
View more about field values at [Fields](../Topics/Fields.md)

