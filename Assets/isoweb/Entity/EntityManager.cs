using System.Collections.Generic;

namespace isoweb.Entity
{
    public class EntityManager
    {
        static Dictionary<uint, Entity> _entities;
        static Dictionary<ulong, EntityDef> _entityDefs;

        public static void Init()
        {
            _entities = new Dictionary<uint, Entity>();
            _entityDefs = new Dictionary<ulong, EntityDef>();
        }

        public static Entity Get(uint entityId)
        {
            Entity val;

            if (!_entities.TryGetValue(entityId, out val))
            {
                val = _entities[entityId] = new Entity(entityId);
            }

            return val;
        }

        public static EntityDef GetDef(ulong hash)
        {
            EntityDef val;

            if (!_entityDefs.TryGetValue(hash, out val))
            {
                val = _entityDefs[hash] = new EntityDef(hash);
            }

            return val;
        }
    }
}