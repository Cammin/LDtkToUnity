# 1.3.1
###### Mar 16, 2020
- Hotfix containing fix to the inability to draw the package manager due to a too-long file path in the samples directory.

# 1.3.0
###### Mar 15, 2020
A massive update with massive changes, with the star of the show being the ability to create levels in edit mode instead of runtime!

###Features
- Levels are now built in the scene inside of edit mode.

- Introduced a new asset called a **Tile Collection**, which is used as a storage for many auto-generated `Tile`s used for generating levels.

- Improved the LDtk Project asset inspector UI/UX:
  - All object fields are now directly assignable, instead of using an extra required pass through a scriptable object asset.
  - Sections of each asset type are now collapsable/expandable dropdowns and spaced apart by lines.
  - Smarter detection of potential problems in the LDtk Project asset.
  - Added tooltips in various places

  - Can auto-slice sprite assets of the textures used in the LDtk project Json with a Sprite Button.
  - Can generate a **Tile Collection** for collision, generated with a button, based on the sprites assigned for each int grid value.
  - Can generate a **Tile Collection** for the visual tiles, based on a texture's meta sprites.

  - Hover over enum definitions to view their values in the tooltip
  - Can automatically assign Level files and textures into the LDtk Project via the Json's `relPath` with some easy buttons.
  - Auto-generated enums now have the option to be in a custom namespace and/or assembly definition.
  - IntGrids display their index number like in LDtk.


- All nullable LDtk field data is now supported
  - Enums now generate an extra "Null" value in case the enum in LDtk is nullable.
- Made most static classes in the codebase into normal instantiable classes.
- Converted over to using the IntGridCsv format that came with the new version of LDtk
- Updated Json schema data for LDtk 0.8.0 (Will be updated further later)
- Updated all examples to reflect the changes with this update

- Fixed the "Inconsistent line endings" warning when generating the enum file
- Fixed incorrect world position if no tiles were involved in the level's creation
- Many other bug fixes

### Breaking Changes:
- The API has changed; adjust your current project accordingly.
- Changed the level building workflow from building in runtime into building in edit mode (Runtime option may return in the future)
- Removed all current custom LDtk scriptable object assets in favour of simplicity. Previously many different assets represented entities, IntGrid values, etc. The only scriptable objects that are created are now only the LDtk Project and the new **Tile Collections**. Because of the asset removals, feel free to delete the old unused scriptable objects.

####Shortcomings that will/may be fixed/added:
- The built level(s) have no special association to its relative LDtk Json data.
- Layer's opacity does no effect.
- Resizable entity not implemented.
- Auto-slicing a texture into sprites from the LDtk project does not account for any offsets or padding.
- Level Backgrounds not implemented.

Note: The built level's tiles now may look like they are tearing by the seams. This can be alleviated by adding padding to the sprites, via packing into a [Sprite Atlas.](https://docs.unity3d.com/Manual/SpriteAtlasWorkflow.html)

# 1.2.15
###### Feb 3, 2020
- Fixed all null coalescing assignments to support older unity version

# 1.2.14
###### Feb 3, 2020
- Hotfix containing interface fix to an error on older unity versions

# 1.2.13
###### Feb 3, 2020
- Removed Level identifier assets, replaced by the newly separate `.ldtkl` files
  - This is a new requirement
  - Make the necessary adjustments to this change
- Both project files and level files display some extra details
- Updated file icon graphics

# 1.2.12
###### Jan 24, 2020
- The Wiki has been created since this update! Check it out [here](https://github.com/Cammin/LDtkUnity/wiki)
- Added some new properties and methods to the extended class functionality. Changed naming of a few
  - The Wiki's LDtk data classes are up to date with the extended class functionality in the tool
- Fixed incorrect unity level bounds calculation
- Minor compilation warning fixes

# 1.2.11
###### Jan 21, 2020
- Hotfix to impossible division operator on older unity versions

# 1.2.10
###### Jan 21, 2020
- Hotfix containing interface definition fix to an error on older unity versions

# 1.2.9
###### Jan 21, 2020
- Hotfix containing a fix to an error on older unity versions when selecting the package in the package manager window

# 1.2.8
###### Jan 21, 2020
- LDtk to Unity now utilizes the C# Quicktype Json Schema! Allows for easy, clean data additions in the future.
- All extension methods to extend on the LDtk's data are now partial classes to coincide with the Json Schema addition
- Levels and entities and their data are now built-in their correct position based on LDtk's world position
- Updated the Level Builder Controller to be simpler, and can choose to build single, partial, or all levels
- Improved the hierarchy of instantiated LDtk layers to help with organization

# 1.2.7
###### Dec 29, 2020
- Hotfix containing a fix to incompatibility created by Unity 2020.2

# 1.2.6
###### Dec 29, 2020
- All samples provided by LDtk are set up in Unity, explorable from the sample in the Unity Package Manager
- Created some new components, all available in the Add Component Menu:
  - Settable Renderer, for setting render-related data from LDtk to an entity
  - Post-Field Injection Receiver as a UnityEvent
  - Level Built Receiver as a UnityEvent
  - Receiver for the built level's background colour as a UnityEvent
- Added feature to load a default grid Tilemap if one wasn't assigned; similar concept to how a Physics Material resolves itself in collider fields
- LDtk Project Asset has a pixels-per-unit field to unite scales of layers with different pixels per unit
- Entities' points and radius fields are drawn in the scene view, faithfully like how the LDtk editor presents their own

- LDtk Project Asset has error checking for if an image is not set as read/write enabled
- Fixed bug where an IntGrid tile sprite's empty physics shape had collision anyways
- General Polish to the LDtk Project Asset's inspector menu
- Fixed visual bug where a thrown warning was too large on higher-scale Unity editor UI
- Fixed bug where parsing a single empty Point field resulted in a parse error
- Created a resolve to a bug where it was impossible to set up an int grid value with an unwritten, empty value identifier
- Fixed bug related to LDtk enums with spaces in their identifier
- Many other minor bug fixes and changes to improve the overall package

# 1.2.5
###### Dec 13, 2020
- Added some visual warning and error handling in the Project Assets inspector if anything is not set up correctly, allowing an easier time setting up the project assets

# 1.2.4
###### Dec 9, 2020
- Changed versioning convention to solve internal issues.

# 1.2.03
###### Dec 9, 2020
- Separated the parser system from this tool into its own repo, to offer more freedom if one would prefer to just simply parse data
- Greatly modified namespaces; almost all of them. This is to make an effort towards simplicity. Make the appropriate corrections in your custom code
- If using custom assembly definitions, then the new assembly definition `LDtkParser.Runtime` must be referenced in any of your custom code references LDtk data or the LDtkLoader
- All data that had fields or methods are now extension methods.

**Note:** Due to this dependency change, a reinstall of the package will be required

# 1.2.02
###### Dec 4, 2020
- Due to the new changes in requirements, the new minimum required Unity version is 2019.2. My apologies if you worked in a lower version
- Compiler warning fix, and problematic assembly definition reference fixed

# 1.2.01
###### Dec 4, 2020
- Hotfix that addresses bugs related to loading editor resources like icons or text templates

# 1.2.0
###### Dec 4, 2020
- Brand new concept of how the assets are stored; all of the assets are condensed to a single asset that stores the Json project, which automatically displays the proper fields for int grid values, entities, and Tilemaps based on the definitions in the LDtk project
- Automatic enum script generation based on the enum definitions in the LDtk project
- Added a new "simple project" in the sample
- Bunch of bug fixes

**Note:** The concept of asset collections has been removed in favour of the reworked project's asset. Revise your LDtk asset references in response to this change.

# 1.1.42
###### Dec 2, 2020
- Updated Json struct data for LDtk 0.6.0
- Added some more properties for the data/definition classes
- Updated enum icon to match LDtk's updated enum icon

# 1.1.41
###### Nov 25, 2020
- Hotfix for tile textures having the incorrect offset
- New error check for if a tileset sprite is not read/write enabled

# 1.1.40
###### Nov 25, 2020
- Fixed Tilemap art tearing at the seams

# 1.1.39
###### Nov 24, 2020
- Updated displayed package name in Unity

# 1.1.38
###### Nov 24, 2020
- Updated Json.Net version to 12.0.301 (from 12.0.201)

# 1.1.37
###### Nov 23, 2020
- Initial version tag


# 1.0.0
###### Nov 8, 2020
- Project repo initially started.
  - Truly started development during the "LEd" days since Oct. 1.
  - Fun fact, was experimenting with this in a personal project before making it a full-fledged Unity package tool :)