The Project Asset is the main asset for containing all the elements of a level.

Create from the Asset Menu:  
`Create > LDtk > LDtk Project`.  

After assigning the LDtk project file, all of it's definitions will be displayed for assignment.  

If the project contains enum definitions, an auto-generate button will be available which generates a sibling folder containing a single C# script with all the enums of the project. 
If new LDtk enums are added or change over time, hit the button to update the enums. Be aware however, that updating may break your current codebase if an assembly/namespace was changed, or if the enum definition name/values are changed.

![Project Assets](~/images/unity/inspector/ProjectAsset.png)  