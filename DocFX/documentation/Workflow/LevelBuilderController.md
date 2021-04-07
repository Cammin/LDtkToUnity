This MonoBehaviour component is a simple way to get a LDtk level built, which builds a specified level.

This component is for makes levels in the scene.
For more control with custom scripting instead of using this component, Call the static method  
`LDtkLevelBuilder.BuildLevel`.  
`LDtkDataProject` can be created by calling the static method  
`LDtkToolProjectLoader.DeserializeProject(string)`, where the string is the LDtk project's JSON text.  