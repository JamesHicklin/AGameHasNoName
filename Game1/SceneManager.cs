using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game1
{
    public class SceneManager
    {
        private const int OFFSET_X = 0;
        private const int OFFSET_Y = 0;

        private EntityManager _entityManager;
        private TextureManager _textureManager;
        private SpriteBatch _spriteBatch;
        private Dictionary<int, Entity> _entities;

        public SceneManager(TextureManager textureManager, SpriteBatch spriteBatch, EntityManager entityManager)
        {
            _textureManager = textureManager;
            _spriteBatch = spriteBatch;
            _entityManager = entityManager;
        }

        public void Initialize()
        {
            BuildScene();
        }
        
        public void BuildScene()
        {
            
        }

        public void RenderScene()
        {
            _entities = _entityManager.GetAllEntities();
            foreach (KeyValuePair<int, Entity> entity in _entities)
            {
                entity.Value.Draw();
            }

            Entity player = _entityManager.Get(0);
            player.Draw();
        }



        private void BuildMap()
        {

        }
    }
}
