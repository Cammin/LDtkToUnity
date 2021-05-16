using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LDtkUnity
{
    /// <summary>
    /// This stores Fields from field instances in LDtk. Access them to use them.
    /// </summary>
    /// <code>
    /// GetComponentLDtkFields.GetInt("FieldThing")
    /// </code>
    public class LDtkFields : MonoBehaviour
    {
        public LDtkFieldBranch[] fields;

        public void AddFieldData(FieldInstance[] instances)
        {
            foreach (FieldInstance fieldInstance in instances)
            {
                AddFieldInstance(fieldInstance);
            }
        }

        private void AddFieldInstance(FieldInstance field)
        {
            string fieldIdentifier = field.Identifier;
        }

        public int GetInt(string identifier)
        {
            LDtkFieldBranch fieldBranch = fields.FirstOrDefault(field => field.identifier == identifier);
            return fieldBranch.array[0]._int;
        }







    }
}