# Install

- Add this auto-installer [**Unity Package**](https://package-installer.glitch.me/v1/installer/OpenUPM/com.cammin.ldtkunity?registry=https%3A%2F%2Fpackage.openupm.com) to your unity project.

### Alternate Option
- [Install the OpenUPM-CLI](https://openupm.com/docs/getting-started.html#installing-openupm-cli) if not installed already.
- [Install the LDtkUnity Package](https://openupm.com/docs/getting-started.html#installing-a-upm-package) by entering this into the command line while the directory is scoped on the root of your Unity project:  
```
openupm add com.cammin.ldtkunity
```  

### Note
- *Minimum* supported Unity version 2019.3.
- This package uses [*Newtonsoft Json Unity Package*](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@2.0/manual/index.html) as the tool to deserialize an LDtk json project.   
  It is automatically installed alongside; no worries.
- The first install option is simpler but potentially unstable. If the first one doesn't work, then try the safer, second option.
