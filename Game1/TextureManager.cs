using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Game1
{
    public class TextureManager
    {
        Dictionary<string, Texture2D> _textures;
        ContentManager _contentManager;

        public TextureManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _textures = new Dictionary<string, Texture2D>();
        }

        public void Add(string textureName)
        {
            if (_textures.ContainsKey(textureName))
                return;

            Texture2D texture = _contentManager.Load<Texture2D>(textureName);
            _textures.Add(textureName, texture);
        }

        public Texture2D Get(string textureName)
        {
            if (_textures.ContainsKey(textureName))
            {
                return _textures[textureName];
            }

            return null;
        }
    }
}
