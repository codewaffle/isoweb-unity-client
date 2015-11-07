using SimpleJSON;
using UnityEngine;

public class ComponentBase {
    public virtual void Update(JSONNode value)
    {
        Debug.LogError("Unhandled Component Data: " + this + " : " + value.ToString());
    }
}
