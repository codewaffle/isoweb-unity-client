using System.Collections.Generic;

namespace isoweb.Entity
{
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
                entity.GetComponent(kvp.Key).Update(kvp.Value);
            }
        }
    }
}