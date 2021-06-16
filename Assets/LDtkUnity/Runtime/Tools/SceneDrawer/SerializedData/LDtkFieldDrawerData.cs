using System;
using UnityEngine;
using UnityEngine.Internal;

namespace LDtkUnity
{
    [ExcludeFromDocs]
    [Serializable]
    public class LDtkFieldDrawerData : LDtkSceneDrawerBase
    {
        [SerializeField] private string _identifier;
        [SerializeField] private LDtkFields _fields;
        [SerializeField] private EditorDisplayMode _fieldMode;
        [SerializeField] private  int _gridSize;
        
        [SerializeField] private Texture _iconTex;
        [SerializeField] private Rect _iconRect;

        public LDtkFieldDrawerData(LDtkFields fields, Color color, EditorDisplayMode fieldMode, string identifier, int gridSize, Texture iconTex, Rect iconRect) : base(color)
        {
            _identifier = identifier;
            _fields = fields;
            _fieldMode = fieldMode;
            _gridSize = gridSize;
            _iconTex = iconTex;
            _iconRect = iconRect;
        }

        protected override ILDtkGizmoDrawer GetDrawer()
        {
            if (_fields == null)
            {
                Debug.LogError("LDtk: Source is null, not drawing");
                return null;
            }
            
            switch (_fieldMode)
            {
                case EditorDisplayMode.Hidden: 
                    //show nothing
                    break;
                    
                case EditorDisplayMode.ValueOnly:
                case EditorDisplayMode.NameAndValue:
                    //todo choose to show more later? like an icon in a smaller size maybe?
                    return new LDtkFieldDrawerValue(_fields.transform.position + Vector3.up, _identifier);
                    
                case EditorDisplayMode.EntityTile: //REPLACE ENTITY TILE WITH ENUM DEFINITION TILE, so it's special todo target this eventually
                    return new LDtkEntityDrawerIcon(_fields.transform, _iconTex, _iconRect);

                case EditorDisplayMode.PointPath:
                case EditorDisplayMode.PointStar:
                case EditorDisplayMode.PointPathLoop:
                case EditorDisplayMode.Points:
                    return new LDtkFieldDrawerPoints(_fields, _identifier, _fieldMode);
                    
                case EditorDisplayMode.RadiusPx:
                case EditorDisplayMode.RadiusGrid:
                    return new LDtkFieldDrawerRadius(_fields, _identifier, _fieldMode, _gridSize);

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }
    }
}