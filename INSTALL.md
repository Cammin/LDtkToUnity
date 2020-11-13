# Installation Guide

- [Install Newtonsoft.Json for Unity.](https://github.com/jilleJr/Newtonsoft.Json-for-Unity/wiki/Installation-via-UPM)  
![Json.Net](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/JsonNetForUnityPackageManagerWindow.png)

- Navigate to your `manifest.json` file by right-clicking the Packages folder in Unity, and select "Show in Explorer". Then navigate into the Packages folder in Explorer.  
![Packages](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/PackagesShowInExplorer.png)  

- Open the `manifest.json` file in your preferred text editor. Notepad works fine.  
![Manifest File](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/ManifestExplorer.png)

- Then add this text entry into dependencies, and save the file.  
 ```"com.cammin.ldtkunity": "https://github.com/Cammin/LDtkUnity.git?path=/Assets/LDtkUnity#master"```  

- After focusing back into Unity, the package will automatically be downloaded and installed.
![Unity Reloading](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/UnityReloading.png)

Note: If you install LDtk package before Newtonsoft.Json, you will get a dependency error. If this happens, install Newtonsoft.Json and you should be good to go.  
![Dependency Error](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/DependencyError.png)

# Sample

You can download an example here to gain a quicker understanding of a completed usage setup. The sample will be added to your Assets folder.  
![Sample](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/SamplePackageManager.png)
![Sample](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/SampleProjectView.png)

# Updating

Unlike normal Unity packages, an update button is not available for custom packages.  
(Plans to research and implement scoped registries will be added soon to enable updating from the Package Manager Window)  
![No Update Button](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/MissingUpdateButtonPackageManager.png)

- To update the package, open the Packages folder in explorer.  
![Packages](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/PackagesShowInExplorer.png)  

- Open the `packages-lock.json` file in your preferred text editor. Notepad works fine.  
![packages-lock in explorer](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/PackagesLockExplorer.png)

- Then delete this segment of text:  
![Delete this text to update](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/DeletingPackagesLockEntry.png)

- After focusing back into Unity, the previous package will automatically be replaced by a newly downloaded version installation.
![Unity Reloading](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/UnityReloading.png)
  
Note: When updating, some of your current code may be broken due to LDtk class definition changes during this package's development. Change them to the correct classes if nessesary.
