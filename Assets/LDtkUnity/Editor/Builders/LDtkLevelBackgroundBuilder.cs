using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkLevelBackgroundBuilder
    {
        private readonly LDtkProjectImporter _importer;
        private readonly GameObject _levelTransform;
        private readonly LDtkSortingOrder _layerSortingOrder;
        private readonly Level _level;
        
        private Texture2D _texture;

        public LDtkLevelBackgroundBuilder(LDtkProjectImporter importer, GameObject levelTransform, LDtkSortingOrder layerSortingOrder, Level level)
        {
            _importer = importer;
            _levelTransform = levelTransform;
            _layerSortingOrder = layerSortingOrder;
            _level = level;
        }


        /// <returns>
        /// The sliced sprite result of the backdrop.
        /// </returns>
        public void BuildBackground()
        {
            //if no path defined, then no background was set
            if (string.IsNullOrEmpty(_level.BgRelPath))
            {
                return;
            }
            
            LDtkRelativeGetterLevelBackground getter = new LDtkRelativeGetterLevelBackground();
            _texture = getter.GetRelativeAsset(_level, _importer.assetPath);
            
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
            
            _layerSortingOrder.Next();
            renderer.sortingOrder = _layerSortingOrder.SortingOrderValue;
            
            ManipulateTransform(renderer.transform);
            
            _importer.AddBackgroundArtifact(sprite);
        }

        private void ManipulateTransform(Transform trans)
        {
            trans.parent = _levelTransform.transform;

            Vector2 levelPosition = LDtkCoordConverter.LevelBackgroundPosition(_level.BgPos.UnityTopLeftPx, _level.BgPos.UnityCropRect.height, _importer.PixelsPerUnit, _level.BgPos.UnityScale.y);
            
            trans.localPosition = levelPosition;

            Vector2 scale = _level.BgPos.UnityScale;
            trans.localScale = new Vector3(scale.x, scale.y, 1);
        }

        private SpriteRenderer CreateGameObject()
        {
            GameObject go = new GameObject(_level.Identifier + "_Bg");
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            return renderer;
        }

        private Sprite GetSprite()
        {
            Rect rect = _level.BgPos.UnityCropRect;

            rect.position = LDtkCoordConverter.LevelBackgroundImageSliceCoord(rect.position, _texture.height, rect.height);
            
            if (!LDtkTextureSpriteSlicer.IsLegalSpriteSlice(_texture, rect))
            {
                Debug.LogError($"Illegal Sprite slice {rect} from texture ({_texture.width}, {_texture.height})");
                return null;
            }

            Sprite sprite = Sprite.Create(_texture, rect, Vector2.up, _importer.PixelsPerUnit);
            sprite.name = _texture.name;
            return sprite;
        }
    }
}