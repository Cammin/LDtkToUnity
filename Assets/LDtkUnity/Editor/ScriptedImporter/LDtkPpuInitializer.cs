using UnityEditor;
using UnityEngine;

namespace LDtkUnity.Editor
{
    internal struct LDtkPpuInitializer
    {
        public string ProjectPath;
        public int PixelsPerUnit;

        public LDtkPpuInitializer(int ppu, string projectPath)
        {
            PixelsPerUnit = ppu;
            ProjectPath = projectPath;
        }
        
        public bool OnResetImporter()
        {
            //Debug.Assert(PixelsPerUnit == -1, "Initial ppu not -1?");
            if (PixelsPerUnit > 0)
            {
                return false;
            }
            
            int defaultGridSize = -1;
            if (!LDtkJsonDigger.GetDefaultGridSize(ProjectPath, ref defaultGridSize))
            {
                //if problem, then default to what LDtk also defaults to upon a new project
                defaultGridSize = LDtkImporterConsts.DEFAULT_PPU;
            }
            
            return TryInitializePixelsPerUnit(defaultGridSize);
        }
        
        //call this during the regular import process. We need to know what our initial one should be if we're a fresh new asset with a ppu of -1.
        //return true if we did make a change to our ppu
        public bool TryInitializePixelsPerUnit(int initialValue)
        {
            //if it's already configured and fine.
            if (PixelsPerUnit > 0)
            {
                return false;
            }

            PixelsPerUnit = initialValue;
            return true;
        }
    }
}