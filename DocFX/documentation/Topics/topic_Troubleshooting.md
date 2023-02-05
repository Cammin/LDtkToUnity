# Troubleshooting
Over the project's development, there have been some common reports/questions about issues with the importer to provide an explanation.  
If any issues are found, [**post an issue on GitHub**](https://github.com/Cammin/LDtkUnity/issues) or report it in the **[Discord server](https://discord.gg/7RPGAW9dJx)**.

## Outdated Version
If your LDtk project is saved in an outdated version of LDtk, then there may be import issues.   
Save your project in the latest version of LDtk first to ensure that the import process is successful.

## Compile Errors On Install
If you install the package and you're getting compile errors, there could be multiple reasons and solutions to them:

### Unsupported unity version
If you're below the minimum supported version by the package, then there may or may not be compile errors. The minimum supported version is on the front page of the github repo is [here](https://github.com/Cammin/LDtkToUnity).

### Unconventional Installation
If you're not installing though the instructions described in [Install](), then you're treading unintentional install territory.

## Reimport Project
If there are any problems in the import process, try reimporting, which can often fix the issue.  
But if there are still problems or if consistent manual reimporting is required to function properly, then report the issue.  
![Reimport](../../images/img_Unity_Reimport.png)  
For separate level files, the imported level files depend on the project file to be imported properly firstly.  
Consider reimporting the project file before the levels if there are also import issues with the level.

## Editor Slowdown
If the scene view becomes slow, there are some possible reasons why, which can be fixed.  
- If an LDtk project has lots of levels, entities and entity references, then lots of elements will be drawn in the scene window. They can be turned off in the [LDtk preferences](topic_Preferences.md).
- If an [IntGrid Tile](topic_IntGridTile.md) has a GameObject Prefab that is costly on performance in high quantities like a [SpriteMask](https://docs.unity3d.com/Manual/class-SpriteMask.html), then consider another technique to work around the issue.

## Loading Relative Assets
An issue that can happen is that tileset textures or separate levels fail to load. But to explain this, first understand how LDtk manages files:  
*LDtk projects reference external assets like textures and separate levels from a relative path (ex.`"Art/Tileset.png"`).  
LDtk separate levels also reference their source project (ex.`"../GameProject.ldtk"`).  
This means that if any assets are moved, then the path reference is lost in LDtk unless both are moved to maintain a proper relative path.*

This is the same case for the Unity importer. The importer also uses the same relative paths to load assets.  
A rule of thumb to consider is that **as long as LDtk can load the asset, then so can the importer**.  
Also, it is important for Unity to have all assets contained in the Unity project in order to be loadable. If they are not, then the importer will encounter a loading error.

## Failed Import (white file)
Ensure that your LDtk project is saved in the newest version of LDtk to potentially fix the problem.  
This is never expected to happen even if an import issue is inevitable. 
If a LDtk project/level file fails to import, then this is an objective problem with the importer that the developer should always fix.  
Report the issue to help fix the problem.


## Custom Prefab Identifier Uniqueness
When using custom prefabs for levels or entities, it is discouraged to use two of the same component type in any given prefab GameObject. (ex. Two CircleCollider2Ds in the same GameObject)  
This is because an imported hierarchy needs to maintain identities for each component to remember any possible prefab overrides between reimports. Using multiple of the same component confuses Unity on which component is being tracked for prefab overrides.    
Unity will log a warning when this happens.


## Tilemap Seams
The solution is by using a sprite atlas to add padding between sprite slices.  
See **[Tilemap Tearing](../Topics/topic_TilemapTearing.md)** and **[Main/SpriteAtlas](../Importer/topic_Section_Main.md)**


## Unexpected Composite Collider
Sometimes it's possible that the composite colliders retain changes between the previous import which can negatively impact the expected collision of levels.
To fix this, select the root LDtk project in the scene, and then revert the prefab overrides, which should revert any unexpected changes applied to the composite collider components.  
A fix was investigated, but not found yet. This may still be fixed in the future.

## Missing Levels Array
When digging through the root json data (via LDtkPostprocessor.OnPostprocessLevel for instance), you may find that the `Levels` array is empty when it shouldn't be. 
This is actually a deprecated array that will be removed in the future.  
Instead, use the newer `Levels` array inside of the `Worlds` array.
