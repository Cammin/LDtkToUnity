using UnityEngine;

namespace LDtkUnity.Editor
{
    public class LDtkLevelBackgroundBuilder
    {
        private readonly LDtkProjectImporter _importer;
        private readonly GameObject _levelTransform;
        private readonly LDtkSortingOrder _layerSortingOrder;
        private readonly Level _level;
        private readonly Vector2 _worldSpaceSize;

        private Texture2D _texture;

        public LDtkLevelBackgroundBuilder(LDtkProjectImporter importer, GameObject levelTransform, LDtkSortingOrder layerSortingOrder, Level level, Vector2 worldSpaceSize)
        {
            _importer = importer;
            _levelTransform = levelTransform;
            _layerSortingOrder = layerSortingOrder;
            _level = level;
            _worldSpaceSize = worldSpaceSize;
        }


        /// <returns>
        /// The sliced sprite result of the backdrop.
        /// </returns>
        public void BuildBackground()
        {
            BuildBackgroundTexture();
            BuildSimpleBgColor();
        }

        private void BuildSimpleBgColor()
        {
            SpriteRenderer renderer = CreateGameObject("_BgColor");
            renderer.sprite = LDtkResourcesLoader.LoadDefaultTileSprite();
            renderer.color = _level.UnityBgColor;
            
            _layerSortingOrder.Next();
            renderer.sortingOrder = _layerSortingOrder.SortingOrderValue;

            ManipulateColorTransform(renderer.transform);
        }

        private void BuildBackgroundTexture()
        {
            //if no path defined, then no background was set.
            if (string.IsNullOrEmpty(_level.BgRelPath))
            {
                return;
            }

            LDtkRelativeGetterLevelBackground getter = new LDtkRelativeGetterLevelBackground();
            _texture = getter.GetRelativeAsset(_level, _importer.assetPath);

            if (_texture == null)
            {
                return;
            }

            Sprite sprite = GetSprite();
            if (sprite == null)
            {
                return;
            }

            SpriteRenderer renderer = CreateGameObject("_BgImage");
            renderer.sprite = sprite;

            _layerSortingOrder.Next();
            renderer.sortingOrder = _layerSortingOrder.SortingOrderValue;

            ManipulateImageTransform(renderer.transform);

            _importer.AddBackgroundArtifact(sprite);
        }

        private void ManipulateColorTransform(Transform trans)
        {
            trans.parent = _levelTransform.transform;
            trans.localPosition = _worldSpaceSize/2;
            trans.localScale = new Vector3(_worldSpaceSize.x, _worldSpaceSize.y, 1);
        }
        private void ManipulateImageTransform(Transform trans)
        {
            trans.parent = _levelTransform.transform;

            Vector2 levelPosition = LDtkCoordConverter.LevelBackgroundImagePosition(_level.BgPos.UnityTopLeftPx, _level.BgPos.UnityCropRect.height, _importer.PixelsPerUnit, _level.BgPos.UnityScale.y);
            
            trans.localPosition = levelPosition;

            Vector2 scale = _level.BgPos.UnityScale;
            trans.localScale = new Vector3(scale.x, scale.y, 1);
        }

        private SpriteRenderer CreateGameObject(string extraName)
        {
            GameObject go = new GameObject(_level.Identifier + extraName);
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