using System.Collections.Generic;
using UnityEngine;

namespace isoweb.Entity { 
    public class InteractiveComponent : EntityComponent
    {
        private bool _include_position;

        private GameObject _interactiveGo;
        Collider2D _collider;
        private string _shape;
        private JSONArray _args;
        InteractiveBehaviour _behaviour;

        GameObject InteractiveGameObject
        {
            get { return _interactiveGo ?? (_interactiveGo = InitGameObject()); }
        }

        private GameObject InitGameObject()
        {
            var go = new GameObject("__INTERACTIVE");
            go.transform.parent = AttachedEntity.GameObject.transform;
            _behaviour = go.AddComponent<InteractiveBehaviour>();
            _behaviour.Component = this;
            return go;
        }

        public InteractiveComponent(Entity ent) : base(ent)
        {
        }

        public override void Update(JSONNode value)
        {
            foreach (KeyValuePair<string, JSONNode> kvp in value.AsObject)
            {
                switch (kvp.Key)
                {
                    case "include_position":
                        _include_position = kvp.Value.AsBool;
                        break;
                    case "shape":
                        SetShape(kvp.Value);
                        break;
                    case "shape_args":
                        SetArgs(kvp.Value.AsArray);
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetArgs(JSONArray args)
        {
            _args = args;

            if (_shape == null)
                return;

            switch (_shape)
            {
                case "circle":
                    SetCircleArgs();
                    break;
                default:
                    Debug.LogWarning("Unhandled Args for Shape");
                    break;
            }
        }

        private void SetCircleArgs()
        {
            var cc = (CircleCollider2D) _collider;
            cc.offset = new Vector2(_args[0].AsFloat, _args[1].AsFloat);
            cc.radius = _args[2].AsFloat;
        }

        private void SetShape(string shape)
        {
            _shape = shape;
            switch (shape)
            {
                case "circle":
                    _collider = InteractiveGameObject.AddComponent<CircleCollider2D>();
                    SetupCollider();
                    break;
                default:
                    Debug.Log("unknown shape : " + shape);
                    break;
            }

            if (_args != null)
                SetArgs(_args);
        }

        private void SetupCollider()
        {
            InteractiveGameObject.layer = 8;
        }
    }
}