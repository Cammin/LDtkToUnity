using System;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LDtkUnity.Editor
{
    internal sealed class LDtkTextureImporterBuilder : ISpriteEditorDataProvider
    {
        private SpriteDataProviderFactories fo = new SpriteDataProviderFactories();

        
        private void OnPreprocessTexture()
        {
            var factory = new SpriteDataProviderFactories();
            factory.Init();
            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(null);
            dataProvider.InitSpriteEditorDataProvider();
            SetPivot(dataProvider, new Vector2(0.5f, 0.5f));
            dataProvider.Apply();
        }

        
        static void SetPivot(ISpriteEditorDataProvider dataProvider, Vector2 pivot)
        {
            var spriteRects = dataProvider.GetSpriteRects();
             foreach (var rect in spriteRects)
             {
                 rect.pivot = pivot;
                 rect.alignment = SpriteAlignment.Custom;
             }
             dataProvider.SetSpriteRects(spriteRects); 
        }
        
        
        public SpriteImportMode spriteImportMode => SpriteImportMode.Multiple;
        public float pixelsPerUnit => 16;
        public Object targetObject => null;
        
        public SpriteRect[] GetSpriteRects()
        {
            throw new NotImplementedException();
        }

        public void SetSpriteRects(SpriteRect[] spriteRects)
        {
            throw new NotImplementedException();
        }

        public void Apply()
        {
            throw new NotImplementedException();
        }

        public void InitSpriteEditorDataProvider()
        {
            throw new NotImplementedException();
        }

        public T GetDataProvider<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public bool HasDataProvider(Type type)
        {
            throw new NotImplementedException();
        }


    }
}