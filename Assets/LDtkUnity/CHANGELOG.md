# 2.1.7
###### October 31, 2021 🎃

- Added a "Native" Prefab export option, accessible from the importer inspector
  - Saves a prefab (and other assets) to a specified folder, stripped from all associations to LDtk data/components.
  - Useful if there is a desire to uninstall the importer package, but still maintain (most of) the creation in Unity.

- Added a new field to `LDtkIntGridTile` to optionally set a custom `PhysicsMaterial2D` for tilemaps
- Updated API incompatibilities for Unity 2021.2
- Updated the used Newtonsoft Json package to 2.0.2 (12.0.301)

- Changed how the project hierarchy is built
  - Layer GameObjects that don't ultimately serve a purpose for a given level will not be built 
    - (ex. IntGrid layer with no values, entity layer with no entities)
    
- Improved information in the inspector if there was a breaking import issue
- Fixed a bug where importing/saving a level/entity prefab would break the corresponding reference in the imported LDtk project
- Fixed an import error related to using level fields that were ints, floats, or points
- Fixed a substring error that may have logged while moving assets
- Added a safeguard when level/entity field data is being built
- Added a safeguard from potential problems with any chosen file paths that are outside the Unity project
  - Implemented for selecting enum path and prefab export path
- Fixed a minor GUI error that would appear when assigning a GameObject for a `LDtkIntGridTile`

# 2.1.6
###### September 25, 2021
- A new Discord server for [LDtkToUnity](https://discord.gg/7RPGAW9dJx) has launched!
- Produced a new installation tutorial video in the documentation
- Improved the pixels-per-unit field in the importer inspector
  - Can now properly set what the pixels per unit should be (such as 1 pixel per unity unit) and scales accordingly
  - Does not affect entities
  - Scales float/int fields if they were set as a radius in LDtk
  - May be a breaking change for imported projects, as it modifies the size of the import result to match the requested pixels per unit, but in most cases will not break projects
- New common detections added to point out some fixable import problems
- Improved existing logging to be more helpful
- Fixed inability to click documentation icons in the inspector UI in older unity versions
- Texture assets used by LDtk are now dependencies, meaning that LDtk projects will be re-imported upon a texture reimporting
- Fixed level background sprites unintentionally being packed into the sprite atlas
- `LDtkArtifactAssets` now lists background sprites individually in the inspector
- Fixed an editor crash in Unity 2019.3 when packing a sprite atlas containing lost references

# 2.1.5
###### July 21, 2021
- Added numerous features to the LDtkIntGridTile inspector
  - Added a new Tag and Layer field to the IntGridTile asset
    - Allows splitting up tilemaps with customized tags/layers
  - Added a `Save Project` button
  - Added a safeguard to prevent lag/crashes when assigning imported LDtk projects to the GameObject Prefab field
- Fixed all HelpURL broken links (was broken since 2.1.0)

# 2.1.4
###### July 15, 2021
- Improved whether black/white text should be chosen for scene text or the importer's IntGrid values
- Fixed a scene drawer error if a particular scene was unloaded
- Fixed an import bug if an LDtk project was a dependency in a prefab
- Fixed a 'recursive' error that occasionally appears when importing a project or moving assets

# 2.1.3
###### July 4, 2021
- Added functionality for using a Tileset override for Tile layers
  - This also fixes a major bug related to using this
- Changed the importer's label "Main Pixels Per Unit" to "Main Grid Size" to avoid confusion

# 2.1.2
###### June 27, 2021
- Fixed a documentation repo link to the newly named repository from the unity package manager
- Updated some minor package details

# 2.1.1
###### June 27, 2021
- Changed the minimum compatible unity version to 2019.3
  - This fixes some old compatibility bugs, like problems generating enum scripts in 2019.2

# 2.1.0
###### June 27, 2021
- Changed the Json package dependency
  - Changed from [Newtonsoft Json for Unity](https://github.com/jilleJr/Newtonsoft.Json-for-Unity) to [Newtonsoft Json Unity Package](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@2.0/manual/index.html) to avoid assembly clashes
  - Feel free to uninstall the previous package if it still exists after updating (expect a GUID warning in the console)
  
- Created a new media banner for the package
- Changed the name of the repository, and corrected all relevant links
- Added a simpler install option to the documentation
- Added a topic to the documentation to explain tilemap tearing
- Level/Entity scene labels are no longer red if the color's saturation was 0

# 2.0.6
###### June 21, 2021
A complete rework to the scene drawing for levels/entities!

- Improved scene drawers
  - Everything is now drawn with anti-aliasing
  - Added identifier text for entities/levels
    - Label is colored for entity color or level background color respectively
  - Added shapes for entities (Rectangle, Ellipse, Cross)
    - Drawn as set from LDtk, whether hollow or not 
  - Entity icons and text are now clickable to select it's corresponding GameObject
  - Tweaked how the entity icon is drawn
  - Fixed a radius-size error
  - Fixed radius drawer to now involve a color field's first color


- Added a new preferences menu, located in Unity's preferences window
  - Adjust editor-based options here
  - Added various scene drawing options to preferences
  - Also moved _**Log Build Times**_ from the Project importer to preferences


- Changed the import process
  - Changed the enum generation path from absolute to relative 
  - Empty GameObjects will now be created if an entity's prefab field is unassigned in the importer (instead of creating nothing)
  - Levels now have their `bgColor` created as a full background (ordered behind background image)
  - Fixed level backgrounds being parented into the last layer gameObject


- Updated examples and documentation
- Many other minor additions/changes/fixes

# 2.0.5
###### June 13, 2021
- Added a warning dialog if an LDtk project/level tries to get moved
- Added a safeguard for using Aseprite files, which are unsupported at the moment
  - Added a new Aseprite sample scene
- Adjusted the main section's Pixels Per Unit field
  - Value will never go below 1
  - Value will default to the `DefaultGridSize` in the LDtk json upon first import or reset
- Fixed small icon size for the enum namespace field if the namespace is invalid

# 2.0.4
###### June 6, 2021
- Added a toggle in the importer to create a CompositeCollider2D component on all IntGrid tilemaps
- Added support for LDtk layer pixel offsets
- Added a help reference button for each importer section 
- Added extra height to `MultiLine` fields in the LDtkFields component
- Changed how the level borders are drawn in scene view
- Fixed a potential recursive crash related to assigning imported GameObjects to the Level/Entity fields  
- Improved the tooltips for the level/entity fields in the importer inspector
- Other minor tweaks and fixes

# 2.0.3
###### June 6, 2021
- Updated/cleaned the Wiki with update 2.0.0's content
  - Completely cleaned up the API section to be simpler to read
  - Given new XML documentation to all exposed classes in the wiki's API section
  - Updated links that point to the documentation
- Added a new extended property to get a TilesetDefinition's tag EnumDefinition
- Made a collapsable section for the main content of the Importer inspector
- Fixed sprite atlas's meta data sometimes being cleared by now automatically packing on import
- Other minor tweaks and fixes

# 2.0.2
###### May 24, 2021
- Added scene icons for entities if their editor visual is a tileset tile in LDtk
- Added new API functionality in `LDtkFields`
  - Can get first occuring color
  - Can check if a field is an array
  - Can check if the field is a certain type
- Added the scene line drawers back
- Reworked the entity interface events (Changed their API)
  - Simpler interface options
  - Updated sample scripts that use this
  - Added back the level prefab field for custom scripting, with it's own new interface events
- Fixed broken prefabs in the import result if they were changed or externally modified
- Fixed a bug when trying to get a Multiline string field from `LDtkFields`
- Fixed a visual bug where tiles that are built with off-grid pixel positions are now correctly built (ie. The shelves in the Typical2DPlatformer sample)
- Various sample edits

# 2.0.1
###### May 20, 2021
- Fixed a bug related to importing a LDtk project for the first time

# 2.0.0
###### May 18, 2021
Massive update with major changes.  
- Migrated to using ScriptedImporters instead of building a level into the scene with a component
  - It's now a simple drag and drop prefab process to add LDtk projects into the scene
  - It's automatically re-imported whenever the `.ldtk` or `.ldtkl` files are saved
  - The project is an imported GameObject prefab, so it's open to any prefab overrides/reverts
  - The import result is stored in Unity's Library folder, resulting in small asset sizes

- Added support for multi-selecting LDtk Projects
- Updated the examples, resulting in a smaller size and simpler folder navigation
- Added a component for the level root that draws the bounds of the level with the level's `bgColor`
  - The component stores these two values, so they are retrievable with `GetComponent`
- Added `Level` functionality to get next/previous levels of projects in a linear world layout
- Updated Json.Net version to 13.0.102 (from 12.0.301)
- Fixed the ordering of AutoLayers unexpectedly under the IntGrid values (if rendering the IntGrid values was enabled)

##### Added many features/changes to the main project inspector, further simplifying the UI:
- Main Section
  - Added a Sprite Atlas field
    - Optional
    - AutoLayer/TileLayer tiles are automatically packed into the atlas which eliminates tilemap tearing and improves GPU performance
    - Only packs what's necessary
  - Added a toggle to log the build times
  - Removed the Level Fields Prefab field
    - May add this back for more customization potential

- Levels
  - Removed this section
    - Getting the levels is now automated
    - Added support for non-separate level files. Either option is now acceptable

- Level Backgrounds
  - Removed this section
    - Getting the level background is now automated

- IntGrids
  - IntGrid value fields now use a new special tile (LDtkIntGridTile) instead of sprites, creatable from the create asset menu
    - Choose from three collision options: None, Grid, and Sprite
      - If an IntGrid tile's sprite is assigned, then its collision shape will be previewed in the inspector for easy reference
    - There's a GameObject field if necessary for a prefab to be included in a tile's place (Deep potential)
    - The tile asset is open for inheritance in case any extra functionality is desired

- Entities
  - Completely reworked the entity field injection system for entities/levels
    - It's now placed onto the GameObject as a standalone component, and its data is accessible in code by its identifier
    - The former `LDtkField` is deprecated and this new system would be used instead

- Enums
  - Improved the enum script generation; Continuous script generation or overwrites into a preferred path upon every import
    - Added a toggle to only generate enums
    - Added a path field to designate a path for the enum script
      - Has a button to pick them easily
    - Removed the assembly definition field

- Tilesets
  - Getting the relevant textures is now automated
    - Native sprites are generated instead of overwriting a texture's sprite sheet
  - All textures are automatically sliced and placed into a sprite atlas (if assigned)

- Tile Collection
  - Removed the Tile Collection concept
    - The relevant assets are automatically stored in the imported object

- Grid Prefabs
  - Removed the Grid Prefabs system
    - To fulfill similar needs, you can edit the imported project prefab for whichever overrides are needed

This is a major update that changes many aspects of the entire level building pipeline. So expect backwards compatibility to be broken.  
There will also likely be some old assets lying around that can be safely deleted (like the Tile Collections).  
This project is nearing completion. However the project is still in rapid development, so be wary of future API/feature changes as this project continues improving.


# 1.3.9
###### May 14, 2021
- Fixed a bug related to LDtkField drawer with Odin inspector

# 1.3.8
###### Apr 16, 2021
- LDtk 0.9.0 JSON compatibility.
- Lots of [Wiki](https://cammin.github.io/LDtkUnity/) progress was done. The wiki is now in a state with relative completeness
- Changed all HelpURLs to point towards the new wiki for all Scriptable Objects and MonoBehaviour components

# 1.3.7
###### Apr 9, 2021
- A brand new website has been made for the wiki! Check it out [here](https://cammin.github.io/LDtkUnity/)
- The documentation, changelog, and licence are now selectable from the Unity package manager
- Can now select Grid prefabs from the asset picker
  - BREAKING CHANGE: The grid prefab section has gotten their type changed from `Grid` to `GameObject`, and their previous references will be lost. Reassign your prefabs to fix this.

# 1.3.6
###### Mar 31, 2021

- Added support for LDtk's resizable entities
- Added support for LDtk's level field instances
- Added support for LDtk's level backgrounds
- Added some more LDtk data properties
- Auto-sliced Tilesets now account for padding and spacing (Previously did not)
- Added scene gizmos for entities that make their fields draw them in LDtk (PointPath, PointStar, RadiusGrid, RadiusPx)


- Added some new functionality to the Level Builder:
  - Added the option to spawn a level at a specific position instead of what LDtk sets if desired
  - Added the option to not log the build times when building a level
  - Added a notification to use the EditorOnly tag for the component's GameObject if not set


- Added some new functionality to the LDtk Project asset:
  - Added a new section for Level backgrounds, complete with an auto-assignment button (only appears if any level backgrounds are defined in LDtk)
  - Added a prefab field for LDtk's level field instances, complete with its warning handling
  - Added the option to detach GameObjects from their parent at the start of runtime for hierarchy performance


- Fixed broken Tilemap GUID connections when overwriting a Tile Collection by changing how the tile collections get auto-generated; If an asset or sub-asset already exists by name, its properties will be overwritten instead of being replaced, resulting in maintained GUID references


- IntGridValue Tile Collections now generate a tile for every value (instead of for every sprite)
  - Empty IntGridValue sprite fields will have tiles generated with a small white texture
    - Reason is that it strangely lags heavily in the scene if a sprite field is empty for Tile assets
  - This change helps them render their proper LDtk colour (instead of white) if they were set as visible from the Project Asset
  - However, there is a new breaking change to enable this behaviour, see below


- Improved Tile Collection inspector UI
- Updated Example scenes with respective new scripts/prefabs for the new features

- Fixed tilemap opacity from always being opaque when set from LDtk transparent layers
- Fixed LDtkField injectable fields not drawing correctly for some types like MultiLine and Point
- Added a check for if a directory contained a period at the start of a folder to prohibit its use
- Added an error check if an auto-located texture is outside the Unity project

### Breaking Change
Because of the change to how Tile Collections' tiles are accessed for IntGrid Values, any IntGrid Tile Collections will need to be regenerated.

# 1.3.5
###### Mar 24, 2021

- Added a Grid prefab section into the LDtk Project inspector
  - This is an expansion on the custom grid prefab field from before, except it's extended to all layers that use a tilemap. 
    Used for customizable options like specifying Tags, LayerMask, Material/Shader, PhysicsMaterial2D, etc.
  
- Changes to the Project Inspector
  - Added accompanying images to the buttons
  - Removed the Grid Prefab field (restructured into the Grid Prefabs section)
  - Moved the "Int Grid Value Colors Visible" field into the IntGrid section 
  - Fixed Int Grid value drawer's text not being white for dark backgrounds in light mode
  - Fixed potential harmless error appearing when initially loading the project inspector

- Changed the appearance of a Tile Collection into Unity's Tilemap icon (was LDtk's AutoLayer icon)
- Added the ability to get the field definition of a Level's field instance (Previously couldn't)
- Added summaries for all extension properties and methods to LDtk schema data
- Created a sprite atlas for the sample textures
- Other minor cleanup/fixes

# 1.3.4
###### Mar 19, 2021
- Removed the LDtkUnity.EntityEvents namespace (everything should now be under the namespace LDtkUnity)
- Made dark theme images named more uniformly to Unity's EditorIcons
  - This also fixes errors where the respective images were not loadable sometimes
- Improved some HelpUrls to point to the wiki's articles
- Fixed a compiler problem with trying to use the package via a repository download

# 1.3.3
###### Mar 18, 2021
- Updated LDtk json schema files to 0.8.1 (from 0.8.0)

# 1.3.2
###### Mar 18, 2021
- The LDtk project's inspector icons/dividers are now configured for Unity's light theme
- Fixed a compiler error related to namespaces

# 1.3.1
###### Mar 16, 2021
- Hotfix containing fix to the inability to draw the package manager due to a too-long file path in the samples directory

# 1.3.0
###### Mar 15, 2021
A massive update with massive changes, with the star of the show being the ability to create levels in edit mode instead of runtime!

### Features
- Levels are now built in the scene inside of edit mode

- Introduced a new asset called a **Tile Collection**, which is used as a storage for many auto-generated `Tile`s used for generating levels

- Improved the LDtk Project asset inspector UI/UX:
  - All object fields are now directly assignable, instead of using an extra required pass through a scriptable object asset
  - Sections of each asset type are now collapsable/expandable dropdowns and spaced apart by lines
  - Smarter detection of potential problems in the LDtk Project asset
  - Added tooltips in various places

  - Can auto-slice sprite assets of the textures used in the LDtk project Json with a Sprite Button
  - Can generate a **Tile Collection** for collision, generated with a button, based on the sprites assigned for each int grid value
  - Can generate a **Tile Collection** for the visual tiles, based on a texture's meta sprites

  - Hover over enum definitions to view their values in the tooltip
  - Can automatically assign Level files and textures into the LDtk Project via the Json's `relPath` with some easy buttons
  - Auto-generated enums now have the option to be in a custom namespace and/or assembly definition
  - IntGrids display their index number like in LDtk


- All nullable LDtk field data is now supported
  - Enums now generate an extra "Null" value in case the enum in LDtk is nullable
- Made most static classes in the codebase into normal instantiable classes
- Converted over to using the IntGridCsv format that came with the new version of LDtk
- Updated Json schema data for LDtk 0.8.0 (Will be updated further later)
- Updated all examples to reflect the changes with this update

- Fixed the "Inconsistent line endings" warning when generating the enum file
- Fixed incorrect world position if no tiles were involved in the level's creation
- Many other bug fixes

### Breaking Changes:
- The API has changed; adjust your current project accordingly
- Changed the level building workflow from building in runtime into building in edit mode (Runtime option may return in the future)
- Removed all current custom LDtk scriptable object assets in favour of simplicity. Previously many different assets represented entities, IntGrid values, etc. The only scriptable objects that are created are now only the LDtk Project and the new **Tile Collections**. Because of the asset removals, feel free to delete the old unused scriptable objects

#### Shortcomings that will/may be fixed/added:
- The built level(s) have no special association to its relative LDtk Json data
- Layer's opacity does no effect
- Resizable entity not implemented
- Auto-slicing a texture into sprites from the LDtk project does not account for any offsets or padding
- Level Backgrounds not implemented

Note: The built level's tiles now may look like they are tearing by the seams. This can be alleviated by adding padding to the sprites, via packing into a [Sprite Atlas.](https://docs.unity3d.com/Manual/SpriteAtlasWorkflow.html)

# 1.2.15
###### Feb 3, 2021
- Fixed all null coalescing assignments to support older unity version

# 1.2.14
###### Feb 3, 2021
- Hotfix containing interface fix to an error on older unity versions

# 1.2.13
###### Feb 3, 2021
- Removed Level identifier assets, replaced by the newly separate `.ldtkl` files
  - This is a new requirement
  - Make the necessary adjustments to this change
- Both project files and level files display some extra details
- Updated file icon graphics

# 1.2.12
###### Jan 24, 2021
- The Github Wiki has been created since this update!
- Added some new properties and methods to the extended class functionality. Changed naming of a few
  - The Wiki's LDtk data classes are up to date with the extended class functionality in the tool
- Fixed incorrect unity level bounds calculation
- Minor compilation warning fixes

# 1.2.11
###### Jan 21, 2021
- Hotfix to impossible division operator on older unity versions

# 1.2.10
###### Jan 21, 2021
- Hotfix containing interface definition fix to an error on older unity versions

# 1.2.9
###### Jan 21, 2021
- Hotfix containing a fix to an error on older unity versions when selecting the package in the package manager window

# 1.2.8
###### Jan 21, 2021
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
  - Truly started development during the "LEd" days since October 1st, 2020.
  - Fun fact, was experimenting with this in a personal project before making it a full-fledged Unity package tool :)
