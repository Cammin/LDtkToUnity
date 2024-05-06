# 5.0.0
###### May 5, 2024

### Additions
- Added support for animated tiles in the import pipeline! Learn the setup [**here**](https://cammin.github.io/LDtkToUnity/documentation/Topics/topic_AnimatedTiles.html)
- Added new component `LDtkComponentLayerParallax`: For all layers to mimic the parallax motions seen in LDtk
- Added new component `LDtkComponentLayerIntGridValues`: A component with various functions to help with getting IntGrid value data
- Added new component `LDtkComponentTilesetTiles`: A component with various functions to help with tileset tiles

- Added various new components and ScriptableObjects, eliminating any requirement to deserialize json data. Check the docs [**here**](https://cammin.github.io/LDtkToUnity/documentation/Topics/topic_HierarchyAndComponents.html) for more info
  - Total new ScriptableObjects
    - `LDtkDefinitionObjectAutoLayerRule`
    - `LDtkDefinitionObjectAutoLayerRuleGroup`
    - `LDtkDefinitionObjectEntity`
    - `LDtkDefinitionObjectEnum`
    - `LDtkDefinitionObjectField`
    - `LDtkDefinitionObjectLayer`
    - `LDtkDefinitionObjectTileset`
  - Updated existing components with new json data:
    - `LDtkComponentProject`
    - `LDtkComponentWorld`
    - `LDtkComponentLevel`
    - `LDtkComponentLayer`
    - `LDtkComponentEntity` (new) 

- `LDtkComponentWorld`: Added a new convenience button to the world component inspector to spawn all the world's separate levels
  - Supports multi-selection & undo
  - Project hierarchies with separate levels enabled will now build the world GameObjects in the project hierarchy to allow this feature

- Added three new "reimport all" buttons into project settings to optionally only reimport all `.ldtk`, `.ldtkl`, or `.ldtkt` files

- `LDtkPostprocessor`: Added new AssetImportContext field to work with the ScriptedImporter, like to access the asset path  
- `LDtkTilesetTile`: Added new helper function `GetEnumTagValues` to parse the tile's string fields into enum data
- `LDtkTilesetTile`: Added new `TileId` int field to `LDtkTilesetTile`
- `LDtkReferenceToAnEntityInstance`: Added public fields for getting IID
- `LDtkIid`: Added implicit operator to string

### Changes
- Optimization: Built tilemaps will now [CompressBounds](https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.CompressBounds.html) to potentially improve on memory/processing
- Optimization: Greatly improved the operation speed of clicking the "Reimport All" button in Project Settings
- Updated: New icon for AutoLayer-related content
- Updated: Samples

### Fixes
- Compile Error: Fixed a compile error encountered in Unity 6
- Import Fail: Fixed a tileset file import failure if a tileset texture is not set in LDtk
- Import Fail: Fixed a problem where the export app would not work on macOS easily
  - This issue is not retroactively fixed, so for any Mac users currently facing this issue, add a `$1` to the end of the file's contents in `Library/LDtkTilesetExporter/ExportTilesetDefinitionMac.sh`
- Expectancy: Fixed the project importer's composite collider option not showing in the inspector if there's no IntGrid layer but if there is a Tile/Auto layer
- Expectancy: Fixed issue when reimporting all in project settings, where some assets that failed to import would not reimport whatsoever
- Expectancy: Added new error log if a tileset file's texture is not a TextureType of Sprite
- Expectancy: Removed an unnecessary error log if a layer's tileset definition was not set
- Expectancy: All checks for a backup file will now happen before checking json version to avoid unnecessary errors

### Breaking Changes
- Deprecated some values in the imported components. They are instead accessed in their definition object.
  - All deprecated values will still work the same, except for `LDtkComponentProject.FromJson` which now return null
- Planning to remove `LDtkEntityDrawerComponent` in the future; the new `LDtkComponentEntity` will be responsible instead

# 4.2.2
###### January 16, 2024
- Fixed 4.2.1 compile errors encountered in Unity versions 2020 and below
- Fixed a problem with the tileset export command where it would fail if a project name has spaces. Fixed by including quotations in the second argument
- Improved the message in the importer inspector to display the incorrect command if required

### Note
You will need to supply a new command to your project to include quotations in the project name 

# 4.2.1
###### January 15, 2024
- Updated json support & samples for [LDtk 1.5.3](https://ldtk.io/json/#changes;1.5.3)
- Added new properties to LDtkNeighbour to accomodate the new neighbour directions

### Note
Changed the `LDtkNeighbour._dir` from `char` to `string`. It may affect the `Dir` API access

# 4.2.0
###### January 12, 2024

### Additions
- Updated json support & samples for [LDtk 1.5.2](https://ldtk.io/json/#changes;1.5.2)
- Added LDtkPreprocessor: Use to read/modify any json data before GameObject hierarchies are created
- Added support for the additional Table-of-content data from LDtk 1.5
  - Except the `Fields`; will need to come later
- Updated samples to 1.5.2

### Fixes
- Improved the experience with the tileset export app
  - Instead of the export app looking for an active windows process to get the project name, it's a new parameter in the command, being the project name.
  - The export app will now pause if errors are encountered to enable easier reading

### Breaking Changes
- This importer requires LDtk projects of LDtk version 1.5 at a minimum. Update your LDtk app and save your project.
- Deprecated `LDtkTableOfContents.GetEntities`, use `GetEntry` instead
- This version has an update to the tileset export app, and the command to run is changed. So follow these two steps:
  - Select your LDtk project file, and you will be able to install a newer version with the click of a button.
  - Copy the new command from the importer inspector, and paste it into your LDtk project settings and Re-save.

# 4.1.0
###### October 23, 2023

### Additions
- Added support for [Aseprite](https://www.aseprite.org/) files!
  - To allow the LDtk importer to load Aseprite files, install the [Unity Aseprite Importer](https://docs.unity3d.com/Packages/com.unity.2d.aseprite@1.0/manual/index.html)  
    The Aseprite importer requires Unity 2021.3.15 or above  
- Added support for looping level backgrounds
  - Make the level's looping background pivot to the bottom left for perfect accuracy 
- Added support for individual tile alpha from LDtk [1.3.1](https://ldtk.io/json/#changes;1.3.1)
- Added a LDtkIid for the LDtkJson root object from LDtk [1.2.0](https://ldtk.io/json/#changes;1.2.0)
- Updated samples to 1.4.1

### Quality of Life
- Changed the icons for the imported Project/Level/Tileset to match with the icons from LDtk 1.5
- Updated docs to instruct that a texture should be RGBA32 compression
- Made the LDtkComponentLayer ordered before the LDtkIid component

### Fixes
- Fixed the slow load time to reset tilemap colliders in the scene after reimporting a tileset definition file
  - Added a notification in the scene view indicating how many tilemap colliders were reset, and how long it took
- Fixed a bug where reordering IntGrid value definitions would use the wrong tile references, and in some cases, cause an exception
- Fixed a deserialize exception when making a blank LDtk project whereby `defaultEntityWidth` and `defaultEntityHeight` are null when they shouldn't be
  - This is a temporary measure and LDtk will fix this problem for LDtk 1.5
- Fixed a possible exception during initialization of LDtkFields
- Added safety-check if a tileset definition's `relPath` was null or empty

### Breaking Changes
- Removed the "De-parent in Runtime" feature from the project importer
  - After much thought, the conclusion was that it's not the importer's job to handle this optimization, and has been removed in favor of less bloat.
  - To continue having this optimization in your game, set an important object's parent as `null` to reduce on hierarchy depth.

# 4.0.1
###### September 28, 2023
- Updated json support & samples for [LDtk 1.4](https://ldtk.io/json/#changes;1.4.0)
- Fixed a compile error if Unity's Aseprite importer is also installed
- Fixed an issue where the tileset file export app wasn't able to run on macOS
- Swapped the order of the TilemapCollider2D and CompositeCollider2D in layers to resolve a [physics issue](https://forum.unity.com/threads/tilemap-collider-with-composite-doesnt-work-with-particle-system-collision-trigger.833737/#post-9173561) for particles
- Updated the docs with content for the 4.0.0 update, among other tweaks

# 4.0.0
###### September 22, 2023
Major update that introduces the separate tileset file!

### Additions
- Added a new separate Tileset Definition File (`.ldtkt`)
  - LDtk will run a custom command to write these files to the same location as separate levels. 
  - This tileset file now generates the sprites and tiles instead of the project file
  - Will only reimport when the tileset was changed in LDtk, saving on iteration time
  - Interacts with the sprite editor window:
    - Can change pivot point / border
    - Can define collision shapes
    - Can now access the `customData` and `enumTags` that are associated with tiles through the tilemap components
- Added a new `LDtkComponentWorld` to the project's hierarchy
- Added `Identifier` to `LDtkNeighbour`
- Added a new field into the project importer inspector to specify the geometry type for composite colliders
- Added support for blocking backups at a custom backup path

### Quality of Life
- Optimized the loading speed when separate levels lookup their dependencies
- Failed imports will now display an error icon in the Project window
- Updated codebase to support Unity 2023.1's various API changes
- Largely improved the error/warning handling when importing
  - Less spam, more clear indications when something's wrong, displayed in the importer's UI.
  - Recreated a feature from Unity 2022.2+ for older unity versions to show errors/warnings in the importer inspectors to help point out issues
- IntGrid tile fields in the inspector can now accept TileBase instead.
  - This allows more versatility. However, continue using the IntGridTile type to still utilize the Tag/Layer/PhysicsMaterial.
  
### Fixes
- Fixed a bug causing a failure to draw scene entity references and logging error spam
- Fixed backups not blocking importing properly
- The single `World` GameObject will now have it's `dummyWorldIid` set when using MultiWorlds

### Breaking Changes
- Generating Tileset definitions will be a hard requirement in order to import LDtk projects properly.
  - They are not automatically generated and need a quick one-time setup (It's relatively quick and painless) 
  - To generate these, follow along with the guide displayed in the project import inspector
- Changed the default tile for an empty tile field back to `None` collision. Make any fixes to reflect this change.
  - Because there are now two options for defining collision, turning off grid collision by default became more sensible.
- Removed the SpriteAtlas field from the project importer
  - Instead, you can directly add a tileset file into a sprite atlas to pack its sprites, much like the normal Unity workflow
  - Because sprites and tiles are now generated from each respective tileset file, the references to these assets will be lost if they were referenced prior
- Level background sprites are now keyed by the level's identifier instead of the `{name}{x}{y}{w}{h}` format, so any prior references to these sprites may be lost
- Removed the `Export` Button in the importer inspector until the feature is supported again

### Note
- At the moment, there isn't an easy way to define Tag, Layer, or Physics Material for Tile/Auto layers.
- The documentation is not updated to reflect this update, but should roll out eventually.

# 3.3.3
###### May 3, 2023
- Added json support for the latest LDtk 1.3.2 update
- Increased the minimum supported LDtk version to 1.3.0 due to a json restructure noted in the previous update below
- Updated samples

# 3.3.2
###### April 28, 2023
- Added json support for the latest LDtk 1.3.0 update
- Fixed a major bug related to LDtk 1.3.0's update
- Fixed an entity reference scene drawer bug if a referenced object didn't exist
- Fixed an entity scene drawer bug where the size of an entity was different from the expected size from LDtk
- Updated Samples

### Note
- Due to an unforeseen json restructure, all previous versions of the importer will fail to import LDtk 1.3.0 files if there are any field instances within entity instances or levels.

# 3.3.1
###### March 26, 2023
- Fixed an issue that was failing WebGL builds

# 3.3.0
###### March 14, 2023
- Added support for the Table of contents `toc` data from the json root
- Added support for the `EntityDefinition.doc` as tooltips in the entities section of the importer inspector
- Added support for the `LayerDefinition.doc` as a Doc field in the LDtkComponentLayer
- Point fields will now be represented by a child transform
  - This fixes the issue where points would always be positioned in world space instead of relative to the entity/level
  - The API is unchanged; It will still return a Vector2 from the fields component
- Entity reference fields now have an additional option to access its layer, level and world; Breaking change mentioned below
- Fixed an issue where custom collider shapes for IntGrid value tiles would be scaled improperly if the sprite aspect was not the same as the textures
- Minor fix to samples

### Breaking Changes
- The Json schema classes are changing their field types from `double` to `float`, and `long` to `int`. Re-correct your code if necessary
  - This change is to help streamline using the fields so that casts are not required, but also to match the LDtk Json docs
- Because Point fields are being changed from Vector2 to Transform, the serialized values are reset. This only impacts you if you set prefab overrides onto the point fields.
- The API for getting the neighbours from the LDtkComponentLevel has changed; Re-correct your code if necessary
- The API for getting entity references has changed; Re-correct your code if necessary

# 3.2.0
###### February 5, 2023
- Replaced the Newtonsoft Json library with [Utf8Json](https://github.com/neuecc/Utf8Json), which deserializes json assets significantly faster
  - The importer is now standalone and has no more dependency on an external library
  - The new library should fully support Unity in any of the supported versions and with IL2CPP. However, if there are any direct compatibility issues, feel free to post an issue
- Fixed a load failure related to using the internal icons for any tile layer or field instance tiles
- Fixed an inconsequential error from appearing after clicking away from an importer with dirty changes and selecting save in the popup

# 3.1.6
###### January 12, 2023
- Added json support for the latest LDtk 1.2.5 update
- Updated Samples

# 3.1.5
###### January 8, 2023
- Added support for the `FieldDefinition.doc` from 1.2.0 as tooltips in LDtkFields
- Fixed an important bug related to tile scaling for IntGrid collisions

# 3.1.4
###### January 2, 2023
- Added json support for the latest LDtk 1.2.0 update
  - Updated support for the new embedded tileset dimensions and where it's sourced from
  - Updated samples
- Performed some optimizations:
  - Setting tiles to tilemaps in batches instead of individually for both IntGrid and tile layers
  - Started caching in an extreme performance critical location related to getting assets via a name and rect
- Fixed an issue with scaling tiles in a certain circumstance

As of this update, it's now possible to install via the GitHub releases as a `.unitypackage`.  
Use them instead if it's a better option for you. But otherwise, the package manager is still the recommended installation method.

# 3.1.3
###### October 31, 2022 🎃
- Fixed a longstanding issue where IntGrid tile colliders would scale incorrectly if the physics sprite's pixels per unit was different from the importer's pixels per unit
- Fixed an occasional exception when exiting from an importer inspector
- Fixed an exception when trying to draw gizmos when the scene view is null
- Minor edit in the project/level artifact's TreeView to not show '0' values
- Improved the color of importer-related logs and for light theme
- Improved the renaming experience when generating IntGrid tile assets
- Improved samples to be smoother in play mode

# 3.1.2
###### October 19, 2022
- Added support for LDtk's internal icons
  - Requires self-providing the asset
  - Can assign the texture in the new Project Settings tab
    - The project settings section also includes a button to reimport all LDtk assets at once
- Added a new `LDtkComponentLayer` in the imported asset with a few new fields
- Added easy access to level neighbours in the `LDtkComponentLevel`
- Added an option into the main section of the importer to create a trigger collider for each level's area
- Added a field to get a `Bounds` value in `LDtkComponentLevel`
- Added new order options to the import interfaces, and are also ordered alongside the LDtkPostprocessors
- Added a new option in projects/levels to disable auto-reimporting due to dependency changes such as prefabs
- Added some blocking when trying to rename a LDtk project or level, which should only be renamed from the LDtk editor
- An int/float will now be drawn in the LDtkFields component as a slider if the definition's min and max was defined
- The world depth GUI is now a scene view overlay so that it can be disabled, moved or docked. (For Unity 2021.2 and onward)
- Optimised some LDtk-related Gizmos:
  - Significant optimization when drawing entity references and point fields
    - These drawers in particular will now respect scaling with pixels per unit better
  - Gizmos will now smoothly stop drawing at a certain zoom level to draw less at any given time
    - Added a new preference option to adjust this draw distance threshold
  - Added the option to quickly disable all of these through the Gizmos button in the scene view
  - Removed the ability to click a label to select it's GameObject
- Fixed writing profiler results even if the option was unchecked in the preferences
- Fixed exporting native prefabs to properly remove some components that previously weren't
- Fixed inconsistent drawn dependencies section while multi-selecting
- Fixed an exception drawing in the scene when an entity references itself
- Made a micro optimization by giving many internal classes the sealed keyword
- Minor fix to colliders in samples
- Updates to docs
- Many other small internal fixes and tweaks

#### Breaking Change
This will only affect users who use Unity 2021.1 or below, and also use the import interfaces, which will face a fixable compiler error.  
All the custom import interface events have gained a new method, and will need to be implemented to fix. 
All other users will have a default interface implementation of 0, so no fixes are required.

# 3.1.1
###### July 25, 2022
- Incremented importer version to force a reimport for every LDtk asset (Forgot about this from last update)
- Added a search bar to the artifact assets inspector

# 3.1.0
###### July 24, 2022
This update focuses on significant performance optimization and a revamp to separate level files.
## Features

- Projects/levels can now safely be nested in prefab and prefab variants
- Optimised many internal systems to make importing much faster
- Added a new "Dependencies" section to both the project and level importer inspector to display what will reimport the LDtk Project/Level when a dependency is modified

- Significantly improved support for separate level files
  - Only modified level files will reimport instead of the project and all levels, resulting in quicker import speeds when applicable
  - Only the dependencies involved in a particular level will reimport the level instead of involving all possible dependencies (Entities, IntGrid tiles)
  - Level files are now imported simultaneously in parallel, resulting in faster import times when reimporting multiple levels.
    - Available in Unity 2021.2 or higher. Enable parallel importing at `Project Settings > Editor > Asset Pipeline > Parallel Import`
  - For speed reasons, using separate level files is now highly encouraged moving forward for any moderately sized game project

- Added safe type checking and support for external enums
- All debug logs will now only show a maximum of 50 duplicates, and have coloured text in the console
- Sample scripts now support the new Input System package
- Added a new static collection of all enabled level components to simplify accessing levels in runtime

## Changes
- Changed how the project importer manages assets
  - All artifact assets generated by the project importer are now indexed, resulting in significantly quicker speeds when accessing sprites/tiles
    - This was the largest performance consumer and was exponentially slower on larger scale projects

- Updated the Newtonsoft Json package to 3.0.2
- Changed max displayed artifacts in the artifact assets inspector to 50 for performance reasons

- From the LDtk Preferences, removed the option from logging import times and replaced it with exporting profiler details
  - Alternatively, import times can be viewed from Unity's [Import Activity Window](https://docs.unity3d.com/Manual/ImportActivityWindow.html)
- Removed a MenuItem command that was accidentally included in the package
- Outdated JSON versions of LDtk projects will no longer be imported to enforce safety
- Many other small internal fixes and tweaks

## Fixes
- Fixed some tiles not scaling properly if their gridSize was different from the pixels per unit
- Fixed an important bug where tiles would not scale or space apart correctly when multiple tile layers are involved
- Importing backup files for separate level files is now ignored in addition to project files
- Fixed the ILDtkImportedLevel not invoking when no level fields were defined
- Fixed the level background position being incorrect when set to "Not scaled" or "Fit inside"
- Updated samples
- Assets like entities and IntGrid tiles assigned to the importer where their definitions were deleted, are now properly cleared out from the importer to not result in unnecessary dependencies
- Added texture size check if the imported texture size was too small to prevent a misleading tile slice error
- Many other small fixes and optimizations

### Breaking Changes
- When separate level files are enabled, the project hierarchy does not build levels anymore and will be an empty hierarchy. Use the imported separate level GameObjects
- The JSON asset artifact will no longer have its level's layer instances available if using separate level files for performance reasons. Instead, deserialize the specific separate levels to get a level's data
- Removed the initial `Null` value for every autogenerated enum. This may shift indexes
  - This was originally intended for the 3.0.0 update but was not applied until now

# 3.0.3
###### April 13, 2022
- Significantly updated the documentation page with new details, and also new content relevant to LDtk 1.0
- Added a new sprite atlas button to quickly make and assign a sprite atlas asset in the main importer inspector
- Added a new option to the LDtk Preferences to only show an entity shape's border
- Backup files will no longer import, which was previously causing unnecessary issues
- Added a check to notify if a LDtk project version is old (current minimum requirement LDtk 1.1.0)
- Fixed entities not drawing their shape in the scene when their editor visual is set as `Tile`
- Updated all samples
  - Fixed the scales of some entity prefabs to fit their area properly

# 3.0.2
###### April 3, 2022
- Updated LDtk json schema classes and samples to 1.1.0 (from 1.0.0)
- Fixed null point fields drawing in the scene when they shouldn't
- Fixed a compilation issue if the NUnit Unity package was not installed
- Fixed some samples not having any collision
- Fixed an exception when trying to import separate level files and added some additional handling

#### Important Change
- Null IntGridTile assets in the inspector will now have grid collision enabled by default (instead of none)
  - This change is to make the beginner experience more friendly for collision
  - You can instead set up no collision for IntGrid values by creating an IntGrid tile asset with collision set to "None", and assign to the importer inspector
  - Updated the samples to reflect this change

# 3.0.1
###### March 30, 2022
- Fixed a major bug identified in 3.0.0 where all levels outside the first one would not build art tiles

# 3.0.0 
###### March 29, 2022
This update contains many new compatibilities, fixes, and features to match with LDtk's new 1.0 major update: Gone Gold!

### Features
- LDtk 1.0 JSON compatibility
- Added support for the two new fields: Entity Reference and Tile!
  - Entity references will appear as GameObject fields, but are internally a string `iid`
    - Entity fields are drawn in the scene! (also new toggle option in the preferences to turn on/off)
  - Tile reference is a sprite field. Tiles are also visually readable in the LDtkFields inspector
- Added support for the new Multi-worlds
  - A new World GameObject is inserted into the import hierarchy. See breaking changes below  
  - LDtk only allows one world currently, but multiple worlds will come in the next updates
- Added a scene view window to change the visibility and pick-ability for levels with differing `worldDepth`
- Added many new API functionality
  - New extended json functions for 1.0 
- Added new functionality to `LDtkFields`
  - Nullable fields can be toggled null in the inspector 
  - All nullable fields can be checked if null from code
  - Added a `TryGet` function for every existing field type
  - Added functions to get any field as a string type
  - Added various other helpful functions
- Added two new buttons into the preferences GUI to enable or disable all scene drawers
- Added a new header to the start of autogenerated enum scripts
- Updated the Newtonsoft Json package to 3.0.1
- Many new edits and content to documentation

### Changes
- Point fields drawn in the scene now have their new zigzag style like in LDtk
- Multilines fields in the `LDtkFields` inspector are now drawn as a TextArea to show more lines
- [Import interface events](https://cammin.github.io/LDtkToUnity/documentation/Topics/topic_CustomImporting.html#import-event-interfaces) will now execute at the end of the entire import process instead of after the individual entity/level's creation, resulting in more consistent access to objects outside the respective component.
- Improved performance when executing events for classes inheriting from `LDtkPostProcessor`
- Tweaked the coloring for the level's identifier label

### Fixes
- Fixed a bug where the importer inspector would still display error UI even after the import issue was already resolved
- Fixed an import bug when an enum definition would be named like other types, such as Color, Point, etc.
- Fixed an issue in the project importer where assigned intGrid values or entities would shift indexes when deleting them in LDtk
- Fixed an issue in the project importer where newly introduced IntGrid values or entities would copy the same object as the last used one
- Fixed an issue where override tilesets were not being used for tile layers
- Fixed all previous potential crashes related to packing sprite atlases in older unity versions, and also resulted in quicker sprite packing speed
- Fixed a new issue where LDtk 1.0 levels with a world layout of LinearHorizontal or LinearVertical would have world positions of (-1, -1). Levels will position properly like in 0.9.3
- Many other minor tweaks and fixes

### Breaking Changes 
- LDtk 0.9.3 projects are now incompatible with the importer. Save your project in LDtk to upgrade it to 1.0, so it can be imported properly.  
- A new "World" GameObject is inserted into the import hierarchy, even if multiple-worlds aren't used.  
  This may affect your current GameObject hierarchy traversal, so refactor accordingly.
- Worlds, Levels, Layers and Entities will have their GameObject names include their `iid`. 
  This is to maintain hierarchy uniqueness, as required by Unity's import pipeline.
- Various public APIs were added, changed, or deleted. 
  Understand that there are many major changes in this version, and be prepared to correct them.
- Null Point fields have changed their value from Vector2.negativeInfinity to Vector2.zero, since they can now be checked if the field is null.
- Removed the initial `Null` value for every autogenerated enum. This may shift indexes.

# 2.2.0
###### January 25, 2022
- Individual level hierarchies are now available!
  - Can drag and drop levels into the scene, which enables a more modular workflow
  - Available by saving as separate level files (`.ldtkl`) in LDtk
  - Because of this new feature, the root of the imported separate level file is now a GameObject instead of `LDtkProjectFile`; any previous references to separate level files may be lost
  - Check it out in the [documentation](https://cammin.github.io/LDtkToUnity/documentation/Importer/topic_LevelImporter.html)
- Added a new tree-view in the inspector for the sub assets `LDtkProjectFile` and `LDtkLevelFile` to see the Json data in a hierarchy
- Added a new button in the IntGrid section of the importer inspector to create an IntGrid value asset
  - This is the new way to create IntGrid tiles. The previous method of creating from the asset menu is removed
- Improved the entities section in the importer inspector
  - Entities can be visually grouped by tags
  - Given icons to the fields like in LDtk
  - Fixed issue where entity order was incorrectly ordered
- Improved subsequent loading times for the LDtk project importer inspector (noticeable with large LDtk projects)
- Fixed unnecessary dirty changes to sprite atlases appearing in source control
- Many editor-only classes/fields are now internal to protect against unintended accessibility, and helps de-clutter the intellisense experience
  - Warning: May affect your code if you were using it. Contact me if you believe that something internal should be publicly accessible
- Fixed a bug where entity prefabs would not be instantiated correctly when both the LDtk project importer and prefabs import at the same time (ex. Multi-selection or Reimport All)
  - This specific fix however, prohibits the use of nesting LDtk Projects or levels inside of prefabs, or else import errors will occur
    - Contact me if this is a very important issue. Explanation [here](https://github.com/Seanba/SuperTiled2Unity/issues/144#issuecomment-1011981650)
- LDtk-related content drawn in the scene (lines, labels) will no longer draw if the associated GameObject is inactive
- Some assets will now use an icon designated for light/dark mode instead of previously always being dark in their inspector window
  - For `LDtkLevelFile`, `LDtkProjectFile`, `LDtkArtifactAssets`, and `LDtkIntGridTile`
- Changed/Added to the API. Mostly inconsequential, but correct any issues if they show in your code
- Many other various UI tweaks

# 2.1.8
###### December 16, 2021
- Added a `LDtkPostProcessor` system for custom control over the LDtk import process via scripting
  - Check it out in the [documentation](https://cammin.github.io/LDtkToUnity/documentation/Topics/topic_CustomImporting.html#ldtkpostprocessor)
- Added a new toggle to the importer inspector to choose whether to build a color background for all levels
- Added a new property for `LDtkComponentLevel` to get the level's rectangle area
- Fixed a breaking import error related to importing an entity/level that had a null integer field from LDtk
- Fixed an issue where the importer inspector's section dropdowns would not remember if they were toggled
- Updated the documentation accordingly with the new features

# 2.1.7
###### October 31, 2021 🎃
- Added a "Native" Prefab export option, accessible from the importer inspector  
  - Saves a prefab (and other assets) to a specified folder, stripped from all associations to LDtk data/components
  - Useful if there is a desire to uninstall the importer package, but still maintain (most of) the creation in Unity
- Added a new field to `LDtkIntGridTile` to optionally set a custom `PhysicsMaterial2D` for tilemaps  
- Updated API incompatibilities for Unity 2021.2  
- Updated the used Newtonsoft Json package to 2.0.2 (12.0.301)
- Changed how the project hierarchy is built
  - Layer GameObjects that don't ultimately serve a purpose for a given level will not be built 
    - (ex. IntGrid layer with no values, entity layer with no entities)
- Improved information in the inspector if there was a breaking import issue
- Fixed a bug where importing/saving a level/entity prefab would break the corresponding reference in the imported LDtk project
- Fixed an import error related to using level fields that were int, float, or point
- Fixed a substring error that may have logged while moving assets
- Added a safeguard when level/entity field data is being built
- Added a safeguard against potential problems with any chosen file paths that are outside the Unity project
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
  - Might be a breaking change for imported projects, as it modifies the size of the import result to match the requested pixels per unit, but in most cases will not break projects
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
  - Everything is now drawn with antialiasing
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
- Fixed sprite atlas's metadata sometimes being cleared by now automatically packing on import
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
  - Added back the level prefab field for custom scripting, with its own new interface events
- Fixed broken prefabs in the import result if they were changed or externally modified
- Fixed a bug when trying to get a Multiline string field from `LDtkFields`
- Fixed a visual bug where tiles that are built with off-grid pixel positions are now correctly built (i.e. The shelves in the Typical2DPlatformer sample)
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
This project is nearing completion. However, the project is still in rapid development, so be wary of future API/feature changes as this project continues improving.


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
- A brand-new website has been made for the wiki! Check it out [here](https://cammin.github.io/LDtkUnity/)
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
- The GitHub Wiki has been created since this update!
- Added some new properties and methods to the extended class functionality. Changed naming of a few
  - The Wiki's LDtk data classes are up-to-date with the extended class functionality in the tool
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
- Fixed bug where an IntGrid tile sprite's empty physics shape had collision anyway
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
**Note:** Due to this dependency change, an installation of the package will be required

# 1.2.02
###### Dec 4, 2020
- Due to the new changes in requirements, the new minimum required Unity version is 2019.2. My apologies if you worked in a lower version
- Compiler warning fix, and problematic assembly definition reference fixed

# 1.2.01
###### Dec 4, 2020
- Hotfix that addresses bugs related to loading editor resources like icons or text templates

# 1.2.0
###### Dec 4, 2020
- Brand-new concept of how the assets are stored; all the assets are condensed to a single asset that stores the Json project, which automatically displays the proper fields for int grid values, entities, and Tilemaps based on the definitions in the LDtk project
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
