using System;
using System.Reflection;
using UnityEngine;

namespace LDtkUnity
{
    public abstract class LDtkSceneDrawerBase
    {
        protected Transform Transform;
        protected Component Source;
        protected EditorDisplayMode Mode;
        protected float GridSize;
        
        private string _fieldName;
        
        public void SupplyReferences(Component source, string fieldName, EditorDisplayMode mode, float gridSize)
        {
            Source = source;
            Transform = source.transform;
            _fieldName = fieldName;
            Mode = mode;
            GridSize = gridSize;
        }
        
        protected FieldInfo GetFieldInfo()
        {
            if (Source == null || string.IsNullOrEmpty(_fieldName))
            {
                Debug.LogError("GetField Error");
                return null;
            }

            Type type = Source.GetType();
                
            FieldInfo fieldInfo = type.GetField(_fieldName);
            return fieldInfo;
        }
        
        public abstract void Draw();


    }
}