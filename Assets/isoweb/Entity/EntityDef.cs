using System.Collections.Generic;
using System.Diagnostics;
using SimpleJSON;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

public class EntityDef
{
    private ulong _hash;
    JSONNode _data; // TODO : lol...

    public EntityDef(ulong hash)
    {
        _hash = hash;
    }

    public void Update(JSONNode parsed)
    {
        _data = parsed;
    }

    public void Populate(Entity entity)
    {
        foreach (KeyValuePair<string, JSONNode> kvp in _data.AsObject)
        {
            EntityComponent c = null;

            switch (kvp.Key)
            {
                case "Sprite":
                {
                    c = new SpriteEntityComponent(entity);
                    break;
                }
                default:
                    Debug.LogError("Unknown Component Type: " + kvp.Key);
                    break;
            }

            if (c != null)
            {
                c.Update(kvp.Value);
            }
        }
    }
}