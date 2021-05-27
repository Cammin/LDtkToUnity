using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public abstract class LDtkSceneDrawerBase
    {
        protected Transform Transform;
        protected string Identifier;
        protected LDtkFields Fields;
        protected EditorDisplayMode Mode;
        protected float GridSize;
        
        
        public void SupplyReferences(LDtkFields fields, string identifier, EditorDisplayMode mode, float gridSize)
        {
            Fields = fields;
            Transform = fields.transform;
            Identifier = identifier;
            Mode = mode;
            GridSize = gridSize;
        }

        public abstract void Draw();
    }
}