using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using UnityEngine;

public class Entity
{
    private uint _id;
    private string _name;
    private Entity _parent;
    private EntityDef _def;
    private Vector2 _pos;
    private float _rot;
    private GameObject _gameObject;
    private CommonBehaviour _behaviour;
    private bool _active = false;

    private Dictionary<string, EntityComponent> _componentMap = new Dictionary<string, EntityComponent>();

    public GameObject GameObject
    {
        get
        {
            if (_gameObject == null)
            {
                _gameObject = new GameObject(_name);
                _behaviour = _gameObject.AddComponent<CommonBehaviour>();
                _behaviour.AttachedEntity = this;
                //_behaviour.enabled = _active;
                //_gameObject.SetActive(_active);

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
        if (Config.CurrentEntity != null)
        {
            Debug.LogError("Don't Know How To Stop Controlling Current Entity");
        }

        Config.CurrentEntity = this;

        Camera.main.gameObject.GetComponent<TrackingCamera>().SetTarget(GameObject);
    }

    public void SetAttribute(string attr, string val)
    {
        switch (attr)
        {
            case "name":
                _name = val;

                if (_gameObject != null)
                    _gameObject.name = val;

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
        if(_def != null)
            Debug.LogError("Definition already set.. whatever");

        _def = def;
        def.Populate(this);
    }

    public void SetPosition(float x, float y, float r)
    {
        _pos.Set(x, y);
        _rot = r;

        GameObject.transform.position = new Vector3(x, y, 0);
        GameObject.transform.localEulerAngles = new Vector3(0, 0, _rot * Mathf.Rad2Deg);
    }

    public void UpdateAttributes(JSONNode attrs)
    {
        foreach (KeyValuePair<string, JSONNode> kvp in attrs.AsObject)
        {
            GetComponent(kvp.Key).Update(kvp.Value);
        }
    }

    public void Enable()
    {
        _active = true;

        if(_gameObject != null)
            _gameObject.SetActive(true);

        foreach (var c in Components)
            c.SetActive(true);
    }

    public void Disable()
    {
        foreach (var c in Components)
            c.SetActive(false);

        if (_gameObject != null)
            _gameObject.SetActive(false);

        _active = false;
    }

    public EntityComponent GetComponent(string cName)
    {
        EntityComponent comp;

        if (!_componentMap.TryGetValue(cName, out comp))
        {
            Debug.Log("Add Component " + cName + " to " + _name);
            comp = _componentMap[cName] = AddComponent(cName);
        }

        return comp;
    }

    public EntityComponent AddComponent(string cName)
    {
        switch (cName)
        {
            case "Sprite":
                return new SpriteEntityComponent(this);
            default:
                Debug.LogError("Unknown AddComponent: " + cName);
                return new EntityComponent(this);
        }
    }

    public IEnumerable<EntityComponent> Components
    {
        get {
            return _componentMap.Select(k => k.Value);
        }
    }

    public void PushPositionUpdate(float stamp, float x, float y, float r)
    {
        _behaviour.PushPositionUpdate(stamp, x, y, r);
    }
}
