using UnityEngine;

namespace isoweb.Entity
{
    public class SpriteBehaviour : ComponentBehaviour<SpriteEntityComponent>
    {
        private SpriteRenderer _renderer;
        private Sprite _sprite;
        private string _url;
    
        public void SetUrl(string url)
        {
            _url = url;

            AssetManager.Fetch(url, www =>
            {
                var tex = new Texture2D(1,1, TextureFormat.RGBA32, true);
                www.LoadImageIntoTexture(tex);
                SetTexture(tex);
            });
        }

        void OnEnable()
        {
            if (_url != null)
                SetUrl(_url);
        }

        private void SetTexture(Texture2D texture)
        {
            texture.Apply(true);
            if (_renderer == null)
            {
                _renderer = gameObject.AddComponent<SpriteRenderer>();
                _renderer.material = Config.DefaultMaterial;
            }

            _renderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one*0.5f, 128f);
            //_renderer.sprite.texture.Apply(true);
        }
    }
}
