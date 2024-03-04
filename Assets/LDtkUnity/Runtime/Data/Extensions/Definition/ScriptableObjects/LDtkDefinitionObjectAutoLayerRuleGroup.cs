using System;
using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_LayerDefJson)]
    public class LDtkDefinitionObjectAutoLayerRuleGroup : ScriptableObject
    {
        [field: Header("Internal")]
        [field: SerializeField] public bool Active { get; private set; }

        [field: SerializeField] public int BiomeRequirementMode { get; private set; }

        [field: SerializeField] public Color Color { get; private set; }

        [field: SerializeField] public LDtkDefinitionObjectTilesetRectangle Icon { get; private set; }

        [field: SerializeField] public bool IsOptional { get; private set; }

        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public string[] RequiredBiomeValues { get; private set; }

        [field: SerializeField] public LDtkDefinitionObjectAutoLayerRule[] Rules { get; private set; }

        [field: SerializeField] public int Uid { get; private set; }

        [field: SerializeField] public bool UsesWizard { get; private set; }
        
        internal void Populate(LDtkDefinitionObjectsCache cache, AutoLayerRuleGroup def)
        {
            name = $"RuleGroup_{def.Uid}_{def.Name}";
            
            Active = def.Active;
            BiomeRequirementMode = def.BiomeRequirementMode;
            Color = def.UnityColor;
            if (def.Icon != null)
            {
                Icon = ScriptableObject.CreateInstance<LDtkDefinitionObjectTilesetRectangle>();
                Icon.Populate(cache, def.Icon);
            }
            IsOptional = def.IsOptional;
            Name = def.Name;
            RequiredBiomeValues = def.RequiredBiomeValues;
            Rules = def.Rules.Select(p => cache.GetObject(cache.Rules, p.Uid)).ToArray();
            Uid = def.Uid;
            UsesWizard = def.UsesWizard;

            for (int i = 0; i < Rules.Length; i++)
            {
                Rules[i].Populate(cache, def.Rules[i]);
            }
        }
    }
}