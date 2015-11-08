using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class SpriteEntityComponent : EntityComponent
{
    SpriteBehaviour _behaviour;
    public SpriteEntityComponent(Entity ent) : base(ent)
    {
    }

    public override void Update(JSONNode value)
    {
        foreach (KeyValuePair<string, JSONNode> kvp in value.AsObject)
        {
            switch (kvp.Key)
            {
                case "sprite":
                    UpdateSpriteUrl(Config.AssetBase + kvp.Value);
                    break;
            }
        }
    }

    private void UpdateSpriteUrl(string url)
    {
        if (_behaviour == null)
        {
            _behaviour = AttachedEntity.GameObject.AddComponent<SpriteBehaviour>();
            _behaviour.AttachedEntity = AttachedEntity;
        }
        _behaviour.SetUrl(url);
    }
}
