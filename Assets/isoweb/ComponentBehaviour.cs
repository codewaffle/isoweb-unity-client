using isoweb.Entity;
using UnityEngine;

namespace isoweb
{
    public class ComponentBehaviour<T> : MonoBehaviour where T : EntityComponent
    {
        public Entity.Entity AttachedEntity
        {
            get { return _component.AttachedEntity; }
        }

        private T _component;

        public T Component
        {
            get { return _component; }
            set { SetComponent(value); }
        }

        private void SetComponent(T c)
        {
            _component = c;
        }
    }
}