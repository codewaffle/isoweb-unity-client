using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AssetCallback(WWW www);

public class AssetManager : MonoBehaviour
{
    private Dictionary<string, WWW>  _cache = new Dictionary<string, WWW>();

    private static AssetManager _instance;
    private static AssetManager GetInstance()
    {
        if (_instance == null)
        {
            var go = new GameObject("AssetManager");
            _instance = go.AddComponent<AssetManager>();
        }
        return _instance;
    }

    protected void InternalFetch(string url, AssetCallback callback)
    {
        WWW www;

        if (!_cache.TryGetValue(url, out www))
            www = _cache[url] = new WWW(url);

        if (!www.isDone)
        {
            StartCoroutine(WaitForFinish(www, callback));
        }
        else
        {
            callback(www);
        }

    }

    public static void Fetch(string url, AssetCallback callback)
    {
        GetInstance().InternalFetch(url, callback);
    }

    private IEnumerator WaitForFinish(WWW www, AssetCallback callback)
    {
        while (!www.isDone)
        {
            yield return null;
        }

        callback(www);
    }
}

