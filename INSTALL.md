# Install/Update Guide
This guide will show you how to [Install](https://github.com/Cammin/LDtkUnity/blob/master/INSTALL.md#install) and [Update](https://github.com/Cammin/LDtkUnity/blob/master/INSTALL.md#update) the LDtk tool in Unity. There is also a [Sample](https://github.com/Cammin/LDtkUnity/blob/master/INSTALL.md#sample) available to try out.
<br/>

**Note:** 
- *Minimum* Unity version 2019.2.
- This package uses [*LDtk Unity Parser*](https://github.com/Cammin/LDtkUnityParser), which uses [*Newtonsoft.Json for Unity*](https://github.com/jilleJr/Newtonsoft.Json-for-Unity) as the tool to deserialize a LDtk project. They are automatically installed if installing through OpenUPM.

# Install
- [Install the OpenUPM-CLI](https://openupm.com/docs/getting-started.html#installing-openupm-cli) if you haven't before.
- [Install the LDtkUnity Package](https://openupm.com/docs/getting-started.html#installing-a-upm-package) by typing this into the command line while the directory is scoped on the root of the Unity project:  
```
openupm add com.cammin.ldtkunity
```  
Note: *LDtk Unity Parser* and *Newtonsoft.Json for Unity* is also installed with this single command; no worries.
<br/><br/>

#### If you would prefer to install with a `.unitypackage`
- [Install *Newtonsoft.Json for Unity*'s `.unitypackage`](https://openupm.com/packages/jillejr.newtonsoft.json-for-unity/).  
- [Install *LDtk Unity Parser*'s `.unitypackage`](https://openupm.com/packages/com.cammin.ldtkunityparser/).  
- [Install *LDtk Unity*'s `.unitypackage`](https://openupm.com/packages/com.cammin.ldtkunity/).

# Update
It's as easy as pressing the update button in the Unity Package Manager.
![LDtkUnityPackageManagerUpdate](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkUnityPackageManagerUpdate.png)

#### If installed with a `.unitypackage`
- Delete the current installation from your `Assets` folder.
- Install the latest `.unitypackage` from the [OpenUPM page](https://openupm.com/packages/com.cammin.ldtkunity/).  
- If there are any installation bugs, try deleting and redownloading the [dependencies](https://github.com/Cammin/LDtkUnity/blob/master/INSTALL.md#if-you-would-prefer-to-install-with-a-unitypackage).
  
**Note:** When updating, some of your current code may be broken due to changes during this package's development. Correct them if necessary.  

# Sample
The sample includes a scene in `Example Setup/Scenes` to test a level being built out of a LDtk project.  
You can import an example at the Package Manager to explore a completed usage setup. The sample will be added to your `Assets` folder.  
![Sample](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/SamplePackageManager.png)
![Sample](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/SampleProjectView.png)

#### If installed with a `.unitypackage`
Rename the `Samples~` folder to `Samples`.
