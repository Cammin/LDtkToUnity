# Installation Guide

- Install Newtonsoft.Json for Unity. They have an install guide Here: (link)
(newtonsoft package manager window image)


- Open the file in your preferred text editor. Notepad works fine.
(unity packages open in explorer image)
(manifest file)

- Then sandwich this text within the others:  
 ```"com.cammin.ldtkunity": "https://github.com/Cammin/LDtkUnity.git?path=/Assets/LDtkUnity#master"```
(manifest text image)

After clicking back into unity, the package will automatically be downloaded and installed.

# Sample

You can download an example here to gain a quicker understanding of a completed usage setup. The sample will be added to your Assets folder.
(sample download button image)
(sample project view window image)

# Updating

Unlike normal Unity packages, an update button is not available for custom packages.
(image showing package manager window of LDtkUnity [no update button])

In order to update the package open the Packages folder in explorer.
(unity packages open in explorer image)

- Open the `packages lock` file in your preferred text editor. Notepad works fine.
(packages lock explorer image)

-Then delete this segment of text:
(packages lock image)

After clicking back into Unity, the previous package will automatically be replaced by a newly downloaded installation.

Note: When updating, it might break some of your current code due to class name changes or otherwise during this package's development. Change them to the correct classes if need be.