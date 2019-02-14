using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class Entity
    {
        public enum EntityType
        {
            PLAYER,
            ENEMY,
            STATIC,
            FLOOR
        }

        public enum AliveState
        {
            ALIVE,
            DEAD
        }

        public enum MoveState
        {
            IDLE,
            MOVING_UP,
            MOVING_RIGHT,
            MOVING_LEFT,
            MOVING_DOWN
        }

        public enum PowerUpState
        {
            NONE,
            SUPER
        }

        private TextureManager _textureManager;
        private SpriteBatch _spriteBatch;
        private EntityType _entityType;
        private Vector2 _position;
        private Vector2 _dimensions;
        private AliveState _currentAliveState;
        private MoveState _currentMovingState;
        private MoveState _nextMovingState;
        private BoundingBox _boundingBox;
        private float _velocity;
        private bool _isVisible;
        Texture2D _currentTexture;

        public int ID { get; set; }

        public Entity(TextureManager textureManager, SpriteBatch spriteBatch)
        {
            _textureManager = textureManager;
            _spriteBatch = spriteBatch;
            _currentAliveState = AliveState.ALIVE;
            _currentMovingState = MoveState.IDLE;
            _dimensions = new Vector2(32, 32);
            _position = new Vector2(0, 0);
            //_boundingBox = new BoundingBox()
            _isVisible = true;
        }

        public void Initialize(string textureName, EntityType entityType)
        {
            _textureManager.Add(textureName);
            _currentTexture = _textureManager.Get(textureName);
            _entityType = entityType;
        }

        public Vector2 GetDimensions()
        {
            return _dimensions;
        }

        public EntityType GetType()
        {
            return _entityType;
        }

        public void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public void SetVelocity(float velocity)
        {
            _velocity = velocity;
        }

        public float GetVelocity()
        {
            return _velocity;
        }

        public void SetAliveState(AliveState state)
        {
            _currentAliveState = state;
            _velocity = 0;
        }

        public void SetMoveState(MoveState state)
        {
            _currentMovingState = state;
        }

        public MoveState GetMoveState()
        {
            return _currentMovingState;
        }

        public void Update()
        {
            if (_currentAliveState != AliveState.ALIVE)
                return;

            switch (_currentMovingState)
            {
                case MoveState.MOVING_DOWN:
                    _position.Y += _velocity;
                    break;
                case MoveState.MOVING_LEFT:
                    _position.X -= _velocity;
                    break;
                case MoveState.MOVING_RIGHT:
                    _position.X += _velocity;
                    break;
                case MoveState.MOVING_UP:
                    _position.Y -= _velocity;
                    break;
                case MoveState.IDLE:

                    break;
            }
        }   

        public void Draw()
        {
            _spriteBatch.Draw(_currentTexture, _position, Color.White);
        }
    }
}
