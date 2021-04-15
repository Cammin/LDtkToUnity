# Enums Section

The enums would be automatically generated as scripts.  
![Section](../../images/unity/inspector/Enums.png)

This section does not appear if no enums are defined in LDtk.


An auto-generate button will be available which generates a folder relative to this asset, which contains a single C# script with all the enums of the project.  
If new LDtk enums are added or change over time, hit the button to overwrite the enum file.  

If desired, an optional namespace field is available to specify the namespace that the script lives in (Leave empty for no namespace).  
Also, an optional assembly field is available to specify what assembly definition the script should live in (Leave empty to use Unity's default assembly).


**Note:**
Be aware that overwriting enums may break your current codebase if an assembly/namespace was changed, or if the enum definition name/values are changed.

