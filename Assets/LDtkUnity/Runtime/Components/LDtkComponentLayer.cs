using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    /// <summary>
    /// This component can be used to get certain LDtk information of a layer instance. Accessible from layer GameObjects.
    /// </summary>
    [HelpURL(LDtkHelpURL.COMPONENT_LAYER)]
    [AddComponentMenu("")]
    public sealed class LDtkComponentLayer : MonoBehaviour
    {
        [ExcludeFromDocs] public const string PROPERTY_IDENTIFIER = nameof(_identifier);
        [ExcludeFromDocs] public const string PROPERTY_DOC = nameof(_doc);
        [ExcludeFromDocs] public const string PROPERTY_TYPE = nameof(_type);
        [ExcludeFromDocs] public const string PROPERTY_LAYER_SCALE = nameof(_scale);
        
        [SerializeField] private string _identifier;
        [SerializeField, Multiline] private string _doc;
        [SerializeField] private TypeEnum _type;
        [SerializeField] private float _scale;
        
        //todo add a potential mapping of all intgrid values for coordinates to make this easy. would input tilemap coords. also tileset info.
        
        /// <value>
        /// The LDtk identifier of this layer.
        /// </value>
        [PublicAPI] public string Identifier => _identifier;        
        
        /// <value>
        /// User defined documentation for this element to provide help/tips to level designers.
        /// </value>
        [PublicAPI] public string Doc => _doc;
            
        /// <value>
        /// The type of this layer component. Can be: IntGrid, Entities, Tiles or AutoLayer.
        /// </value>
        [PublicAPI] public TypeEnum LayerType => _type;
        
        /// <value>
        /// The scale of this layer, which is the layer's GridSize divided by the importer's pixels per unit.<br/>
        /// For example, a layer of 8 GridSize and a importer pixels per unit of 16 means that this layer's scale is 0.5.<br/>
        /// In most situations, this will be 1.
        /// </value>
        [PublicAPI] public float LayerScale => _scale;

        internal void SetIdentifier(string identifier, string doc)
        {
            _identifier = identifier;
            _doc = doc;
        }

        internal void SetType(TypeEnum type)
        {
            _type = type;
        }

        internal void SetScale(float scale)
        {
            _scale = scale;
        }
    }
}