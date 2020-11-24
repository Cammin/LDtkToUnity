# Install/Update Guide
This guide will show you how to [Install](https://github.com/Cammin/LDtkUnity/blob/master/INSTALL.md#install) and [Update](https://github.com/Cammin/LDtkUnity/blob/master/INSTALL.md#update) the LDtk tool in Unity. There is also a [Sample](https://github.com/Cammin/LDtkUnity/blob/master/INSTALL.md#sample) available to try out.
<br/>

**Note:** 
- *Minimum* Unity version 2018.3.
- This package uses *Newtonsoft.Json for Unity* as the tool to deserialize a LDtk project. It is automatically installed if installing through OpenUPM.

# Install
- [Install the OpenUPM-CLI](https://openupm.com/docs/getting-started.html#installing-openupm-cli) if you haven't yet.
- [Install the LDtkUnity Package](https://openupm.com/docs/getting-started.html#installing-a-upm-package) by typing this into the command line interface:  
```
openupm add com.cammin.ldtkunity
```  
Note: *Newtonsoft.Json for Unity* is automatically installed; no worries.
<br/>

### If Below 2019.1 or would prefer to install with `.unitypackage`
- [Install Newtonsoft.Json for Unity.](https://github.com/jilleJr/Newtonsoft.Json-for-Unity/wiki/Installation-via-UPM)  
- Install the `.unitypackage` at the [OpenUPM page](https://openupm.com/packages/com.cammin.ldtkunity/), or [here](https://package-installer.glitch.me/v1/installer/OpenUPM/com.cammin.ldtkunity?registry=https%3A%2F%2Fpackage.openupm.com).  

# Update
It's as easy as pressing the update button in the Unity Package Manager. (2019.1+)  
![LDtkUnityPackageManagerUpdate](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/LDtkUnityPackageManagerUpdate.png)  

If below Unity 2019.1, you must delete the current installation in your `Assets` folder, and download/install the [latest `.unitypackage`](https://package-installer.glitch.me/v1/installer/OpenUPM/com.cammin.ldtkunity?registry=https%3A%2F%2Fpackage.openupm.com) at the [OpenUPM page](https://openupm.com/packages/com.cammin.ldtkunity/).  
  
**Note:** When updating, some of your current code may be broken due to changes during this package's development. Correct them if necessary.  

# Sample
The sample includes a scene in `Example Setup/Scenes` to test a level being built out of a LDtk project.  

You can import an example at the Package Manager to explore a completed usage setup. The sample will be added to your Assets folder. (2019.1+)  
![Sample](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/SamplePackageManager.png)
![Sample](https://github.com/Cammin/LDtkUnity/blob/master/DocImages~/SampleProjectView.png)

If below Unity 2019.1, then you can try out the sample by copying the folder from `Packages/Samples~` into `Assets` in the file explorer, and then rename the `Samples~` folder to `Samples`.  
