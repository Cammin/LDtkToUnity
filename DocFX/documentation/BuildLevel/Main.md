A level gets built by supplying the project asset. Once set up, once or more levels can be built. (WIP)


# Entity Scale Determination
When an entity's prefab is instantiated, that object's scale adjusts accordingly if the entity instance was resized in LDtk. 
 
The scale value is determined by the difference between the original entity size and the resized entity in LDtk.
So when making the prefab for an entity instance, make the prefab match the exact same scale size as the entity's definition in LDtk, and not the resized entity instance.

Example: In LDtk, an entity's definition size is 16x16 pixels. If an entity instance resized to 32x48 pixels in size in the level, then the scale value in unity would be: (2, 3, 1)
