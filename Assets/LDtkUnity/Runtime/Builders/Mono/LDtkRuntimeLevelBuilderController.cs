using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LDtkUnity.BuildEvents;
using LDtkUnity.Tools;
using LDtkUnity.UnityAssets;
using UnityEngine;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

namespace LDtkUnity.Builders
{
    //todo consider bringing this back at some point.
    
    /*[AddComponentMenu(LDtkAddComponentMenu.ROOT + COMPONENT_NAME)]
    [HelpURL(LDtkHelpURL.COMPONENT_LEVEL_BUILD_CONTROLLER)]
    public class LDtkRuntimeLevelBuilderController : LDtkLevelBuilderController
    {
        private const string COMPONENT_NAME = "Runtime Level Builder"; //todo this const alongside the others can belong in their own class instead, LDtkAddComponentMenu
        protected override bool DisposeDefinitionMemoryAfterBuilt => false;
        
        private void Start()
        {
            BuildProject();
        }

    }*/
}