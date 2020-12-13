# 1.2.5
- Added some visual warning and error handling in the Project Assets inspector if anything is not set up correctly, allowing an easier time setting up the project assets

# 1.2.4
- Changed versioning convention to solve internal issues.

# 1.2.03
- Seperated the parser system from this tool into it's own repo, to offer more freedom if one would prefer to just simply parse data
- Greatly modified namespaces; almost all of them. This is to make an effort towards simplicity. Make the appropriate corrections in your custom code
- If using custom assembly definitions, then the new assembly definition `LDtkParser.Runtime` must be referenced if any of your custom code references LDtk data or the LDtkLoader
- All data that had fields or methods are now extension methods.

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
