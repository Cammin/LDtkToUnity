This MonoBehaviour component is a simple way to get a LDtk level built, which builds a specified level upon it's `Start` event.  
Supply it with the 
[LDtk project file](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#the-project), a 
[Level Identifier](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#level-identifier-asset) asset and the 
[Project Assets](https://github.com/Cammin/LDtkUnity/blob/master/DOCUMENTATION.md#project-assets) asset.  
![Level Builder Controller Component](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/BuilderControllerComponent.png)  

This component is only meant for simple level creation, and only makes one level.  
For more control with custom scripting instead of using this component, Call the static method  
`LDtkLevelBuilder.BuildLevel(LDtkDataProject, LDtkLevelIdentifier, LDtkProjectAssets)`.  
`LDtkDataProject` can be created by calling the static method  
`LDtkToolProjectLoader.DeserializeProject(string)`, where the string is the LDtk project's JSON text.  