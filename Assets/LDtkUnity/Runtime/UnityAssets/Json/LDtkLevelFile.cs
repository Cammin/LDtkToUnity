using UnityEngine;

namespace LDtkUnity.UnityAssets
{
    public class LDtkLevelFile : LDtkJsonFile<Level>, ILDtkAsset
    {
        [SerializeField] private string _identifier = null;

        public string Identifier => _identifier;
        public string AssetTypeName => nameof(LDtkLevelFile);
        public bool AssetExists => true;
        public Object Object => null;

        public override void SetJson(string json)
        {
            base.SetJson(json);

            Level lvl = FromJson;
            _identifier = lvl.Identifier;
        }

        public override Level FromJson => Level.FromJson(_json);
    }
}