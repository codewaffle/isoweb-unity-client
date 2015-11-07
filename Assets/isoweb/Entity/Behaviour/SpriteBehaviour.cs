using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteBehaviour : EntityBehaviour
{
    private SpriteRenderer _renderer;
    private Sprite _sprite;
    private string _url;

    private static Dictionary<string, WWW> _cache
        = new Dictionary<string, WWW>(); 

    public void SetUrl(string url)
    {
        _url = url;
        StartCoroutine(UpdateRenderer());
    }

    IEnumerator UpdateRenderer()
    {
        WWW www;
        if (!_cache.TryGetValue(_url, out www))
            www = _cache[_url] = new WWW(_url);

        if (!www.isDone)
            yield return www;

        SetTexture(www.texture);
    }

    private void SetTexture(Texture2D texture)
    {
        //_sprite = Sprite.Create(texture, new Rect(0, 0, texture.width/128, texture.height/128), Vector2.zero);

        if (_renderer == null)
        {
            _renderer = gameObject.AddComponent<SpriteRenderer>();
        }
        // _renderer.sprite = _sprite;
        // _renderer.material.mainTexture = texture;
        _renderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 128f);
    }
}
