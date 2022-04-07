# Frequently Asked Questions

### "Is there a way that I could help contribute to the project?"
You can download the repo and load it into unity. The samples initially aren't accessible from unity because of a missing symbolic link. You can fix this by running the .bat file that's in the Assets folder to create a symbolic link.

### "The importer failed to load a separate level?"
The LDtk project file could have possibly lost reference to it's referred assets when moving the LDtk file into the Unity project. Open the file in LDtk and ensure that the files can also be loaded in there. As long as LDtk can load it, then the importer can also locate the file.

### "Why does my collider not go away after reimporting?"
It's possible that the composite colliders retained changes from the previous import. 
You can try fixing it by going to the root LDtk project in the scene, and then revert the prefab overrides. 

# Troubleshooting

