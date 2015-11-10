using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

internal class StaticPolygonEntityComponent : EntityComponent
{
    public StaticPolygonEntityComponent(Entity ent) : base(ent)
    {
    }

    StaticPolygonBehaviour _behaviour;
    StaticPolygonBehaviour Behaviour
    {
        get { return _behaviour ?? (_behaviour = AttachedEntity.GameObject.AddComponent<StaticPolygonBehaviour>()); }
    }

    public override void Update(JSONNode value)
    {
        foreach (KeyValuePair<string, JSONNode> kvp in value.AsObject)
        {
            switch (kvp.Key)
            {
                case "texture":
                    Behaviour.SetTextureUrl(kvp.Value);
                    break;
                case "points":
                    HandlePoints(kvp.Value);
                    break;

                default:
                    Debug.LogWarning("Unknown: " + kvp.Key);
                    break;
            }
        }
    }

    private void HandlePoints(JSONNode pointsArr)
    {
        Vector2[] points = new Vector2[pointsArr.Count];
        var i = 0;

        foreach (JSONNode pointArr in pointsArr.AsArray)
        {
            var point = pointArr.AsArray;
            points[i++].Set(point[0].AsFloat, point[1].AsFloat);
        }

        Behaviour.SetPoints(points);
    }
}