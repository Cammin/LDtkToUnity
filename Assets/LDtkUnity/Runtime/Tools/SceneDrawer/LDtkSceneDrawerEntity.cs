using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public abstract class LDtkSceneDrawerEntity : LDtkSceneDrawerBase
    {
        protected RenderMode Mode;
        
        protected LDtkSceneDrawerEntity(Component fields, string identifier, RenderMode mode) : base(fields, identifier)
        {
            Mode = mode;
        }
    }
}