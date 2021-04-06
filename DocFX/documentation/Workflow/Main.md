A level gets built by supplying two things: The project data, and the level identifier to build. Entire levels are built during runtime, so it's expected to be used in a relatively empty scene.

This tool is used for simple LDtk project deserialization for the entire project and all of it's lower data structures.  
However, it also provides a level building approach to build up 2D levels with the goal to mimic exactly what is created in the LDtk editor.  

For pixel 2D games, Using the Pixel Perfect Camera is strongly advised; available as a package from the Unity Package Manager.  