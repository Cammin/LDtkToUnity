# Installation Guide (WIP)

[Install Newtonsoft.Json for Unity.](https://github.com/jilleJr/Newtonsoft.Json-for-Unity/wiki/Installation-via-UPM)
(newtonsoft package manager window image)  

Note: If you install LDtk package before Newtonsoft.Json, you will get a dependency error. If this happens, install Newtonsoft.Json and you should be good to go.  
![Dependency Error](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/DependencyError.png)


Open the manifest.json file in your preferred text editor. Notepad works fine.  
![Packages](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/PackagesShowInExplorer.png)  
![Manifest File](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/ManifestExplorer.png)

Then sandwich this text within the others:  
 ```"com.cammin.ldtkunity": "https://github.com/Cammin/LDtkUnity.git?path=/Assets/LDtkUnity#master"```  


After focusing back into Unity, the package will automatically be downloaded and installed.
![Unity Reloading](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/UnityReloading.png)

# Sample

You can download an example here to gain a quicker understanding of a completed usage setup. The sample will be added to your Assets folder.  
(sample download button image)  
(sample project view window image)  

# Updating

Unlike normal Unity packages, an update button is not available for custom packages. (Plans to research and implement scoped registries will be added soon so you can update from the Unity Package Manager Window)  
(image showing package manager window of LDtkUnity [no update button])  

In order to update the package open the Packages folder in explorer.  
(unity packages open in explorer image)  

- Open the `packages lock` file in your preferred text editor. Notepad works fine.  
(packages lock explorer image)

- Then delete this segment of text:  
![Delete this text to update](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/DeletingPackagesLockEntry.png)

After clicking back into Unity, the previous package will automatically be replaced by a newly downloaded installation.
  
Note: When updating, it might break some of your current code due to class name changes or otherwise during this package's development. Change them to the correct classes if need be.
