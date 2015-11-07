using SimpleJSON;
using UnityEngine;

public class EntityComponent
{
    public Entity AttachedEntity { get; private set; }

    public EntityComponent(Entity ent)
    {
        AttachedEntity = ent;
    }

    public virtual void Update(JSONNode value)
    {
        Debug.LogError("Unhandled Component Data: " + this + " : " + value.ToString());
    }
}
