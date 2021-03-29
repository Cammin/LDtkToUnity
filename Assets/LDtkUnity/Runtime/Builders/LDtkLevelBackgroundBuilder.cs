using System;
using UnityEngine;

namespace LDtkUnity
{
    public class LDtkLevelBackgroundBuilder
    {
        private readonly Transform _levelTransform;
        private readonly Level _level;
        private readonly Texture2D _texture;
        private readonly int _layerSortingOrder;
        private readonly int _pixelsPerUnit;
        


        public LDtkLevelBackgroundBuilder(Transform levelTransform, Level level, Texture2D imageSprite, int layerSortingOrder, int pixelsPerUnit)
        {
            _levelTransform = levelTransform;
            _level = level;
            _texture = imageSprite;
            _layerSortingOrder = layerSortingOrder;
            _pixelsPerUnit = pixelsPerUnit;
        }

        public void BuildBackground()
        {
            if (_texture == null)
            {
                Debug.LogError("null Sprite");
                return;
            }

            Sprite sprite = GetSprite();
            if (sprite == null)
            {
                Debug.LogError("Sprite null");
                return;
            }

            SpriteRenderer renderer = CreateGameObject();
            renderer.sprite = sprite;
            renderer.sortingOrder = _layerSortingOrder;
            
            ManipulateTransform(renderer.transform);
            
            LDtkEditorUtil.Dirty(renderer);
        }

        private void ManipulateTransform(Transform trans)
        {
            trans.parent = _levelTransform;

            Vector2 levelPosition = LDtkToolOriginCoordConverter.LevelBackgroundPosition(_level.BgPos.UnityTopLeftPx, _level.BgPos.UnityCropRect.height, _pixelsPerUnit, _level.BgPos.UnityScale.y);
            
            trans.localPosition = levelPosition;

            Vector2 scale = _level.BgPos.UnityScale;
            trans.localScale = new Vector3(scale.x, scale.y, 1);
            
            LDtkEditorUtil.Dirty(trans);
        }

        private SpriteRenderer CreateGameObject()
        {
            GameObject go = new GameObject(_level.Identifier + "_Bg");
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            return renderer;
        }

        private Sprite GetSprite()
        {
            Rect r = _level.BgPos.UnityCropRect;
            //RectInt = new RectInt()
            
            r.position = LDtkToolOriginCoordConverter.LevelBackgroundImageSliceCoord(r.position, _texture.height, r.height);
            
            if (!IsLegalSpriteSlice(_texture, r))
            {
                Debug.LogError($"Illegal Sprite slice {r} from texture ({_texture.width}, {_texture.height})");
                return null;
            }

            Sprite sprite = Sprite.Create(_texture, r, Vector2.up, _pixelsPerUnit);
            
            sprite.name = _texture.name;
            return sprite;
        }

        private static bool IsLegalSpriteSlice(Texture2D tex, Rect rect)
        {
            if (rect.x < 0 || rect.x + Mathf.Max(0, rect.width) > tex.width + 0.001f)
            {
                return false;
            }
            
            if (rect.y < 0 || rect.y + Mathf.Max(0, rect.height) > tex.height + 0.001f)
            {
                return false;
            }

            return true;
        }
    }
}