# 1.2.8
- LDtk to Unity now utilises the C# Quicktype Json Schema! Allows for easy, clean data additions in the future.
- All extension methods to extend on the LDtk's data are now partial classes to coencide with the Json Schema addition
- Levels and entities and their data are now built in their correct position based on LDtk's world position
- Updated the Level Builder Controller to be simpler, and can choose to build single, partial, or all levels
- Improved the hierarchy of instantiated LDtk layers to help with organization

# 1.2.7
- Hotfix containing fix to incompatibibility created by Unity 2020.2

# 1.2.6
- All samples provided by LDtk are set up in Unity, explorable from the sample in the Unity Package Manager
- Created some new components, all available in the Add Component Menu:
  - Settable Renderer, for setting render-related data from LDtk to an entity
  - Post-Field Injection Reciever as a UnityEvent
  - Level Built Reciever as a UnityEvent
  - Reciever for the built level's background color as a UnityEvent
- Added feature to load a default grid tilemap if one wasn't assigned; similar concept to how a Physics Material resolves itself in collider fields
- LDtk Project Asset has a pixels per unit field in order to unite scales of layers with different pixels per unit
- Entities' points and radius fields are drawn in the scene view, fainthfully like how the LDtk editor presents their own

- LDtk Project Asset has error checking for if an image is not set as read/write enabled
- Fixed bug where an IntGrid tile sprite's empty physics shape had collision anyways
- General Polish to the LDtk Project Asset's inspector menu
- Fixed visual bug where a thrown wanring was oversized on higher-scale Unity editor UI
- Fixed bug where parsing a single empty Point field resulted in a parse error
- Created a resolve to a bug where it was impossible to set up an intgridvalue with an unwritten, empty value identifier
- Fixed bug related to LDtk enums with spaces in their identifier
- Many other minor bug fixes and changes to improve the overall package

# 1.2.5
- Added some visual warning and error handling in the Project Assets inspector if anything is not set up correctly, allowing an easier time setting up the project assets

# 1.2.4
- Changed versioning convention to solve internal issues.

# 1.2.03
- Seperated the parser system from this tool into it's own repo, to offer more freedom if one would prefer to just simply parse data
- Greatly modified namespaces; almost all of them. This is to make an effort towards simplicity. Make the appropriate corrections in your custom code
- If using custom assembly definitions, then the new assembly definition `LDtkParser.Runtime` must be referenced if any of your custom code references LDtk data or the LDtkLoader
- All data that had fields or methods are now extension methods.

**Note:** Due to this dependency change, a reinstall of the package will be required

# 1.2.02
- Due the new changes in requirements, the new minimum required Unity version is 2019.2. My apologies if you worked in a lower version
- Compiler warning fix, and problematic assembly definition reference fixed

# 1.2.01
- Hotfix that addresses bugs related to loading editor resources like icons or text templates

# 1.2.0
- Brand new concept of how the assets are stored; all of the assets are condensed to a single asset that stores the json project, which automatically displays the proper fields for int grid values, entities, and tilemaps based off the definitions in the LDtk project
- Automatic enum script generation based off of the enum definitions in the LDtk project
- Added a new "simple project" in the sample
- Bunch of bug fixes

**Note:** The concept of asset collections has been removed in favor of the reworked projects asset. Revise your LDtk asset references in response to this change.

# 1.1.42
- Updated Json struct data for LDtk 0.6.0
- Added some more properties for the data/definition classes
- Updated enum icon to match LDtk's updated enum icon

# 1.1.41
- Hotfix for tile textures having incorrect offset
- New error check for if a tileset sprite is not read/write enabled

# 1.1.40
- Fixed tilemap art tearing at the seams

# 1.1.39
- Updated displayed package name in Unity

# 1.1.38
- Updated Json.Net version to 12.0.301 (from 12.0.201)

# 1.1.37
- Initial version tag
