# Install

There are a few install methods.

## Easy Install 
###### (2019.4+ only)
- In Unity, Go to `Edit > Project Settings > Package Manager`
- Create a new scoped registry:
  - Name: `OpenUPM`
  - URL: `https://package.openupm.com`
  - Scope(s): `com.cammin.ldtkunity`
- Click Save. Now the package is downloadable from the package manager.

![PrefsPackage](../../images/img_Unity_Package_Prefs.png)

- Go to the Unity Package Manager window and select `My Registries` then select install.

![MyRegistries](../../images/img_Unity_Package_MyRegistries.png)


### Video
This video showcases the following two install options below.
[![Video](../../images/img_Video_Install.png)](https://youtu.be/ah5MLaU5m8s)

## Auto-Installer
- Add this auto-installer [**Unity Package**](https://package-installer.glitch.me/v1/installer/OpenUPM/com.cammin.ldtkunity?registry=https%3A%2F%2Fpackage.openupm.com) to your unity project. 
  - It will attempt to automatically perform the first install option shows above, but may fail.

## OpenUPM CLI Option
- [Install the OpenUPM-CLI](https://openupm.com/docs/getting-started.html#installing-openupm-cli) if not installed already.
- [Install the LDtkUnity Package](https://openupm.com/docs/getting-started.html#installing-a-upm-package) by entering this into the command line while the directory is scoped on the root of your Unity project:  
```
openupm add com.cammin.ldtkunity
```

**Note**
- *Minimum* supported Unity version 2019.3.
- This package uses [*Newtonsoft Json Unity Package*](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.0/manual/index.html) as the tool to deserialize a LDtk json files.   
  It is automatically installed alongside; no worries.