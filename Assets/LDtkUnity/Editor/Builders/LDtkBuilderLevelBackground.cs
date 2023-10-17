using UnityEngine;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkBuilderLevelBackground
    {
        private readonly LDtkProjectImporter _importer;
        private readonly GameObject _levelTransform;
        private readonly LDtkSortingOrder _layerSortingOrder;
        private readonly Level _level;
        private readonly Vector2 _worldSpaceSize;

        private Texture2D _texture;

        public LDtkBuilderLevelBackground(LDtkProjectImporter importer, GameObject levelTransform, LDtkSortingOrder layerSortingOrder, Level level, Vector2 worldSpaceSize)
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
            TryBuildSimpleBgColor();
        }

        private void TryBuildSimpleBgColor()
        {
            if (!_importer.CreateBackgroundColor)
            {
                return;
            }
            
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
            if (_level.BgPos == null)
            {
                return;
            }

            //todo this texture is only here because we need the texture's height only. once ldtk adds the image's width+height to bgpos, it will not need to be loaded anymore for performance
            LDtkRelativeGetterLevelBackground getter = new LDtkRelativeGetterLevelBackground(); 
            _texture = getter.GetRelativeAsset(_level, _importer.assetPath);

            if (_texture == null)
            {
                return;
            }

            Sprite sprite = _importer.GetBackgroundArtifact(_level);
            if (sprite == null)
            {
                return;
            }
            
            _layerSortingOrder.Next();
            
            SpriteRenderer renderer = CreateGameObject("_BgImage");
            renderer.sprite = sprite;
            renderer.sortingOrder = _layerSortingOrder.SortingOrderValue;
            if (_level.LevelBgPos is BgPos.Repeat)
            {
                renderer.drawMode = SpriteDrawMode.Tiled;
                renderer.size = _worldSpaceSize;
            }

            ManipulateImageTransform(renderer.transform);
        }
        
        private void ManipulateColorTransform(Transform trans)
        {
            trans.parent = _levelTransform.transform;
            trans.localPosition = _worldSpaceSize/2;
            trans.localScale = new Vector3(_worldSpaceSize.x, _worldSpaceSize.y, 1);
        }
        private void ManipulateImageTransform(Transform trans)
        {
            LevelBackgroundPosition bgPos = _level.BgPos;
            Vector2 scale = bgPos.UnityScale;
            
            Vector2 levelPosition = new Vector2(0, 0)
            {
                x = _worldSpaceSize.x * _level.BgPivotX,
                y = _worldSpaceSize.y * (1 - _level.BgPivotY)
            };
            
            trans.parent = _levelTransform.transform;
            trans.localPosition = levelPosition;
            trans.localScale = new Vector3(scale.x, scale.y, 1);
        }

        private SpriteRenderer CreateGameObject(string extraName)
        {
            GameObject go = new GameObject(_level.Identifier + extraName);
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            return renderer;
        }
    }
}