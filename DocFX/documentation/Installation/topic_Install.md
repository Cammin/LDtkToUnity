# Install

There are a few install methods.  
If you have issues, look towards [Troubleshooting](../Topics/topic_Troubleshooting.md).

## Easy Install 
###### (2019.4+ only)
- In Unity, Go to `Edit > Project Settings > Package Manager`
- Create a new scoped registry:
  - Name: `OpenUPM`
  - URL: `https://package.openupm.com`
  - Scope(s): `com.cammin.ldtkunity`
- Click Save. Now the importer is listed in the package manager.

![PrefsPackage](../../images/img_Unity_Package_Prefs.png)

- Go to the Unity Package Manager window and select `My Registries` then select install.

![MyRegistries](../../images/img_Unity_Package_MyRegistries.png)


## Via .unitypackage
- Download a `.unitypackage` from the [Releases](https://github.com/Cammin/LDtkToUnity/releases) section of the Github repo.
- Drag it into Unity to install. It will be installed into your Assets folder, which is particularly useful for ease of code modification
- Use these instead if it's a better option for you. But otherwise, the package manager is still the recommended installation method.
- When installing from a `.unitypackage`, ensure that the importer is uninstalled from the package manager (if applicable)
- When updating from an old `.unitypackage`, ensure that the previous installation is deleted before installing a new one to ensure stability.

## Via OpenUPM CLI
- [Install the OpenUPM-CLI](https://openupm.com/docs/getting-started.html#installing-openupm-cli) if not installed already.
- [Install the LDtkUnity Package](https://openupm.com/docs/getting-started.html#installing-a-upm-package) by entering this into the command line while the directory is scoped on the root of your Unity project:  
```
openupm add com.cammin.ldtkunity
```
This video showcases the OpenUPM CLI installation method.
[![Video](../../images/img_Video_Install.png)](https://youtu.be/ah5MLaU5m8s)

## Note
- *Minimum* supported Unity version 2019.3.  
- This importer includes the [*Utf8Json*](https://github.com/neuecc/Utf8Json) library to deserialize LDtk json files.