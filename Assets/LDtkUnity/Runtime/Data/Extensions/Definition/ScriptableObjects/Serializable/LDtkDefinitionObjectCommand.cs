using System;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.COMPONENT_PROJECT)]
    [Serializable]
    public sealed class LDtkDefinitionObjectCommand
    {
        [field: SerializeField] public string Command { get; private set; }
        
        [field: Tooltip("Possible values: `Manual`, `AfterLoad`, `BeforeSave`, `AfterSave`")]
        [field: SerializeField] public When When { get; private set; }
        
        internal LDtkDefinitionObjectCommand(LdtkCustomCommand command)
        {
            Command = command.Command;
            When = command.When;
        }
    }
}