# Install/Update Guide
This guide will show you how to [Install](https://github.com/Cammin/LDtkUnity/blob/master/INSTALL.md#install) and [Update](https://github.com/Cammin/LDtkUnity/blob/master/INSTALL.md#update) the LDtk tool in Unity. There is also a [Sample](https://github.com/Cammin/LDtkUnity/blob/master/INSTALL.md#sample) available to try out.

## Install
This package requires *Newtonsoft.Json for Unity* as the tool to deserialize a LDtk project.
- [Install Newtonsoft.Json for Unity.](https://github.com/jilleJr/Newtonsoft.Json-for-Unity/wiki/Installation-via-UPM)  
![Json.Net](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/JsonNetForUnityPackageManagerWindow.png)

- Navigate to your `manifest.json` file by right-clicking the Packages folder in Unity, and select "Show in Explorer". Then navigate into the Packages folder in Explorer.  
![Packages](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/PackagesShowInExplorer.png)  

- Open the `manifest.json` file in a text editor, like Notepad.  
![Manifest File](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/ManifestExplorer.png)

- Insert this text entry into dependencies, and save the text file.  
 ```"com.cammin.ldtkunity": "https://github.com/Cammin/LDtkUnity.git?path=/Assets/LDtkUnity#master"```  

- After focusing back into Unity, the package will automatically be downloaded and installed.
![Unity Reloading](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/UnityReloading.png)

Note: If you install LDtk package before Newtonsoft.Json, you will get a dependency error. If this happens, install Newtonsoft.Json and you should be good to go.  
![Dependency Error](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/DependencyError.png)

## Update

Unlike normal Unity packages, an update button is not available for custom packages.  
(Plans to research and implement scoped registries will be added soon to enable updating from the Package Manager Window)  
![No Update Button](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/MissingUpdateButtonPackageManager.png)

- To update the package, open the Packages folder in explorer.  
![Packages](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/PackagesShowInExplorer.png)  

- Open the `packages-lock.json` file in a text editor, like Notepad.  
![packages-lock in explorer](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/PackagesLockExplorer.png)

- Then delete this segment of text:  
![Delete this text to update](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/DeletingPackagesLockEntry.png)

- After focusing back into Unity, the previous package will automatically be replaced by a newly downloaded version installation.
![Unity Reloading](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/UnityReloading.png)
  
Note: When updating, some of your current code may be broken due to LDtk class definition changes during this package's development. Change them to the correct classes if nessesary.

## Sample
You can import an example at the Package Manager to explore a completed usage setup. The sample will be added to your Assets folder.  
The sample includes a scene in `Example Setup/Scenes` to test a level being built out of a LDtk project.
![Sample](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/SamplePackageManager.png)
![Sample](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/SampleProjectView.png)
