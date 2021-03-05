namespace LDtkUnity
{
    /*public class LDtkTilemapTileFactory : MonoBehaviour
    {
        //[SerializeField] private List<LDtkUnderlyingTileData> _cachedTilesetSprites = new List<LDtkUnderlyingTileData>();
/*
        //todo gain a reference to this instead of statically trying to get it, its hacky
        public static Tile Get(Texture2D grandTileset, Vector2Int srcPos, int pixelsPerUnit)
        {
            return FindObjectOfType<LDtkTilemapTileFactory>()?.GetGeneratedTile(grandTileset, srcPos, pixelsPerUnit);
        }
        
        public Tile GetGeneratedTile(Texture2D grandTileset, Vector2Int srcPos, int pixelsPerUnit)
        {
            
            
            string dataName = LDtkUnderlyingTileData.GetName(grandTileset, srcPos, pixelsPerUnit);

            //if we don't have it yet from a previous operation
            if (!ContainsKey(dataName))
            {
                LDtkUnderlyingTileData data = new LDtkUnderlyingTileData(grandTileset, srcPos, pixelsPerUnit);
                _cachedTilesetSprites.Add(data);
            }
            
            return this[dataName];
        }

        private bool ContainsKey(string key)
        {
            return _cachedTilesetSprites.Any(p => key == p.Key);
        }

        public Tile this[string key] => _cachedTilesetSprites.FirstOrDefault(p => key == p.Key)?.Tile;
        
        public void Dispose()
        {
            _cachedTilesetSprites.Clear();
            _cachedTilesetSprites = null;
        }#1#
    }*/
}