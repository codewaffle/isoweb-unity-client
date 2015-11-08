using SimpleJSON;
using UnityEngine;

public class Entity
{
    private uint _id;
    private string _name;
    private Entity _parent;
    private EntityDef _def;
    private Vector2 _pos;
    private Vector2 _vel;
    private float _rot;
    private GameObject _gameObject;

    public GameObject GameObject
    {
        get
        {
            if (_gameObject == null)
            {
                _gameObject = new GameObject(_name);
                if (_parent != null)
                {
                    _gameObject.transform.parent = _parent.GameObject.transform;
                }
            }

            return _gameObject;
        }
    }

    public Entity(uint entityId)
    {
        _id = entityId;
    }

    public void TakeControl()
    {
        Debug.Log("Assuming Control");
    }

    public void SetAttribute(string attr, string val)
    {
        switch (attr)
        {
            case "name":
                _name = val;
                break;
            default:
                Debug.Log("Unknown String Attr: " + attr);
                break;
        }
    }

    public void SetAttribute(string attr, float val)
    {
        throw new System.NotImplementedException();
    }

    public void SetParent(Entity parent)
    {
        _parent = parent;
        if (_gameObject != null)
        {
            _gameObject.transform.parent = _parent.GameObject.transform;
        }
    }

    public void SetDefinition(EntityDef def)
    {
        _def = def;
    }

    public void UpdatePosition(float x, float y, float r, float velX, float velY)
    {
        _pos.Set(x, y);
        _rot = r;
        _vel.Set(velX, velY);
        
        GameObject.transform.position = new Vector3(x,y,0);
        GameObject.transform.localEulerAngles = new Vector3(0, 0, _rot * Mathf.Rad2Deg);
    }

    public void UpdateAttributes(JSONNode attrs)
    {
        Debug.Log(attrs);
    }

    public void Enable()
    {
        _def.Populate(this);
    }

    public T AddComponent<T>(T comp) where T : EntityComponent
    {
        return comp;
    }
}