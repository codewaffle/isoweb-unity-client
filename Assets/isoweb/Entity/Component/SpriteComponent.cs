using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class SpriteComponent : ComponentBase
{
    private Sprite _unitySprite;

    public override void Update(JSONNode value)
    {
        foreach (KeyValuePair<string, JSONNode> kvp in value.AsObject)
        {
            switch (kvp.Key)
            {
                case "sprite":
                    Debug.Log(Config.AssetBase + kvp.Value);
                    break;
            }
        }
    }
}
