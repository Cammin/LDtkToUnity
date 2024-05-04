using System.Linq;
using UnityEngine;

namespace LDtkUnity
{
    [HelpURL(LDtkHelpURL.LDTK_JSON_LAYER_DEF_JSON)]
    public class LDtkDefinitionObjectAutoLayerRuleGroup : LDtkDefinitionObject<AutoLayerRuleGroup>, ILDtkUid
    {
        [field: Header("Internal")]
        [field: SerializeField] public bool Active { get; private set; }

        [field: SerializeField] public int BiomeRequirementMode { get; private set; }

        [field: SerializeField] public Color Color { get; private set; }

        [field: SerializeField] public Sprite Icon { get; private set; }

        [field: SerializeField] public bool IsOptional { get; private set; }

        [field: SerializeField] public string Name { get; private set; }

        [field: SerializeField] public string[] RequiredBiomeValues { get; private set; }

        [field: SerializeField] public LDtkDefinitionObjectAutoLayerRule[] Rules { get; private set; }

        [field: SerializeField] public int Uid { get; private set; }

        [field: SerializeField] public bool UsesWizard { get; private set; }
        
        internal override void SetAssetName()
        {
            name = $"RuleGroup_{Uid}_{Name}";
        }
        
        internal override void Populate(LDtkDefinitionObjectsCache cache, AutoLayerRuleGroup def)
        {
            Active = def.Active;
            BiomeRequirementMode = def.BiomeRequirementMode;
            Color = def.UnityColor;
            Icon = cache.GetSpriteForTilesetRectangle(def.Icon);
            IsOptional = def.IsOptional;
            Name = def.Name;
            RequiredBiomeValues = def.RequiredBiomeValues;
            Rules = def.Rules.Select(p => cache.GetObject<LDtkDefinitionObjectAutoLayerRule>(p.Uid)).ToArray();
            Uid = def.Uid;
            UsesWizard = def.UsesWizard;

            for (int i = 0; i < Rules.Length; i++)
            {
                Rules[i].Populate(cache, def.Rules[i]);
            }
        }
    }
}