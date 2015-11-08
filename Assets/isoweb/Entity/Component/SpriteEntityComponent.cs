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
            _behaviour.enabled = IsActive;
        }
        _behaviour.SetUrl(url);
    }

    protected override void Activate()
    {
        if (_behaviour != null)
            _behaviour.enabled = true;
    }

    protected override void Deactivate()
    {
        if (_behaviour != null)
            _behaviour.enabled = false;
    }
}
