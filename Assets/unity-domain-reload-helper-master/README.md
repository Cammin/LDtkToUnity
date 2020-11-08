# unity-domain-reload-helper
A couple of attributes that help disabling [Domain Reloading](https://docs.unity3d.com/2019.3/Documentation/Manual/DomainReloading.html) in Unity easier. By default, there are a few attributes to aid in resetting static fields. These are however quite clunky. Here's a few helpful replacements!

## ClearOnReloadAttribute
Use on static fields, properties or events that you wish to reset on playmode. You can either "clear" the field (set the value to default), set it to a specified value, or make it assign itself a new instance of its type.

## ExecuteOnReloadAttribute
Use on static methods that you want to execute during a domain reload. 

### Examples
```csharp
public class CharacterManager : MonoBehaviour
{
  //Will set value to default (null)
  [ClearOnReload]
  static CharacterManager instance;
  
  //Works on events!
  [ClearOnReload]
  static event Action onDoSomething;
  
  //Will set value to defined value (10)
  [ClearOnReload(valueToAssign=10)]
  static int startsAsTen;
  
  //Will reset value, but make it not null (creates a new instance)
  [ClearOnReload(assignNewTypeInstance=true)]
  static CharacterManager myNeverNullManager;
  
  [ExecuteOnReloadAttribute]
  static void RunThis() 
  {
      Debug.Log("Clean up here or something")
  }
}
```
