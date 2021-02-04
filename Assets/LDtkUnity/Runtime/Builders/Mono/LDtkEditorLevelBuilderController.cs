using LDtkUnity.BuildEvents;
using LDtkUnity.UnityAssets;
using UnityEngine;

namespace LDtkUnity.Builders
{
    [AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    public class LDtkEditorLevelBuilderController : LDtkLevelBuilderController
    {
        private const string COMPONENT_NAME = "Editor Level Builder";

        public void BuildLevels()
        {
            BuildProject();
        }
        
    }
}