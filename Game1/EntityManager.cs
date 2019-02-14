using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Game1
{
    public class EntityManager
    {
        Dictionary<int, Entity> _entities;

        public EntityManager()
        {
            _entities = new Dictionary<int, Entity>();
        }

        public void Add(int key, Entity entity)
        {
            if (_entities.ContainsKey(key))
            {
                return;
            }

            entity.ID = key;
            _entities.Add(key, entity);
        }

        public Entity Get(int key)
        {
            if (_entities.ContainsKey(key))
            {
                return _entities[key];
            }

            return null;
        }

        public Entity Get(float x, float y)
        {
            foreach (KeyValuePair<int, Entity> entity in _entities)
            {
                if (entity.Value.GetType() == Entity.EntityType.PLAYER) continue;

                Vector2 position = entity.Value.GetPosition();
                Vector2 bounds = entity.Value.GetDimensions();
                if (x >= position.X && x < position.X + bounds.X && y >= position.Y && y < position.Y + bounds.Y)
                {
                    return entity.Value;
                }
            }

            return null;
        }

        public Dictionary<int, Entity> GetAllEntities()
        {
            return _entities;
        }
    }
}
