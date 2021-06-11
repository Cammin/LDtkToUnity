using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    public abstract class LDtkSceneDrawerField : LDtkSceneDrawerBase
    {
        protected readonly LDtkFields Fields;
        protected readonly EditorDisplayMode Mode;
        
        [SerializeField] private string _fieldName;


        protected LDtkSceneDrawerField(Component fields, string identifier, Color gizmoColor) : base(fields, identifier, gizmoColor)
        {
            Fields = fields.GetComponent<LDtkFields>();
            Mode = mode;
        }
    }
}