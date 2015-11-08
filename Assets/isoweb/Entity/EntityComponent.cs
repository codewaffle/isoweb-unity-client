using System.Runtime.Remoting;
using SimpleJSON;
using UnityEngine;

public class EntityComponent
{
    public Entity AttachedEntity { get; private set; }
    public bool IsActive { get; private set; }

    public EntityComponent(Entity ent)
    {
        AttachedEntity = ent;
    }

    public virtual void Update(JSONNode value)
    {
        Debug.LogError("Unhandled Component Data: " + this + " : " + value.ToString());
    }

    public void SetActive(bool active)
    {
        if (active == IsActive)
            return;

        IsActive = active;
        if (active)
            Activate();
        else
            Deactivate();
    }

    protected virtual void Activate()
    {
    }

    protected virtual void Deactivate()
    {
    }
}
