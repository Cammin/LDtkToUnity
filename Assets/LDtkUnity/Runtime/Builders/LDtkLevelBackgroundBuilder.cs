using UnityEngine;

namespace LDtkUnity
{
    public class LDtkLevelBackgroundBuilder
    {
        private readonly Level _level;
        private readonly Sprite _imageSprite;


        public LDtkLevelBackgroundBuilder(Level level, Sprite imageSprite)
        {
            _level = level;
            _imageSprite = imageSprite;
        }

        public void BuildBackground()
        {
            if (_imageSprite == null)
            {
                Debug.LogError("null Sprite");
                return;
            }

            GameObject go = new GameObject(_level.Identifier + "_Bg");
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();

            renderer.sprite = _imageSprite;

            LevelBackgroundPosition pos = _level.BgPos;

            Vector2 scale = _level.UnityBgPivot;

            BgPos? bgPos = _level.LevelBgPos;
        }
    }
}