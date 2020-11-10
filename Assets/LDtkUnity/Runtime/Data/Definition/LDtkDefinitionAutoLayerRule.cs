// ReSharper disable InconsistentNaming

namespace LDtkUnity.Runtime.Data.Definition
{
    //https://github.com/deepnight/ldtk/blob/master/JSON_DOC.md#211-auto-layer-rule-definition
    public struct LDtkDefinitionAutoLayerRule
    {
        /// <summary>
        /// If FALSE, the rule effect isn't applied, and no tiles are generated.
        /// </summary>
        public bool active;
        
        /// <summary>
        /// When TRUE, the rule will prevent other rules to be applied in the same cell if it matches (TRUE by default).
        /// </summary>
        public bool breakOnMatch;
        
        /// <summary>
        /// Chances for this rule to be applied (0 to 1)
        /// </summary>
        public float chance;
        
        /// <summary>
        /// If TRUE, enable checker mode
        /// </summary>
        //public bool checker; //todo is listed as bool in docs, but is a string
        
        /// <summary>
        /// If TRUE, allow rule to be matched by flipping its pattern horizontally
        /// </summary>
        public bool flipX;
        
        /// <summary>
        /// If TRUE, allow rule to be matched by flipping its pattern vertically
        /// </summary>
        public bool flipY;
        
        /// <summary>
        /// Rule pattern (size x size)
        /// </summary>
        public int[] pattern;
        
        /// <summary>
        /// If TRUE, enable Perlin filtering to only apply rule on specific random area
        /// </summary>
        public bool perlinActive;
        
        /// <summary>
        /// 
        /// </summary>
        public float perlinOctaves;
        
        /// <summary>
        /// 
        /// </summary>
        public float perlinScale;
        
        /// <summary>
        /// 
        /// </summary>
        public float perlinSeed;
        
        /// <summary>
        /// Only 'Stamp' tile mode.
        /// X pivot of a tile stamp (0-1)
        /// </summary>
        public float pivotX;
        
        /// <summary>
        /// Only 'Stamp' tile mode
        /// Y pivot of a tile stamp (0-1)
        /// </summary>
        public float pivotY;
        
        /// <summary>
        /// 
        /// </summary>
        public int size;
        
        /// <summary>
        /// Array of all the tile IDs. They are used randomly or as stamps, based on tileMode value.
        /// </summary>
        public int[] tileIds;
        
        /// <summary>
        /// Defines how tileIds array is used
        /// </summary>
        //public Enum tileMode;//todo is an enum, not sure about definition yet
        
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        public int uid;
        
        /// <summary>
        /// X cell coord modulo
        /// </summary>
        public int xModulo;
        
        /// <summary>
        /// Y cell coord modulo
        /// </summary>
        public int yModulo;
    }
}