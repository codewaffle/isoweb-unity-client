using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteBehaviour : EntityBehaviour
{
    private SpriteRenderer _renderer;
    private Sprite _sprite;
    private string _url;
    
    public void SetUrl(string url)
    {
        _url = url;

        AssetManager.Fetch(url, www => SetTexture(www.texture));
    }

    void OnEnable()
    {
        if(_url != null)
            AssetManager.Fetch(_url, www => SetTexture(www.texture));
    }

    private void SetTexture(Texture2D texture)
    {
        if (_renderer == null)
            _renderer = gameObject.AddComponent<SpriteRenderer>();

        _renderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one*0.5f, 128f);
    }
}
