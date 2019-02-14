using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private int[,] _map = new int[,]
        {
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 }
        };

        private const int OFFSET_X = 128;
        private const int OFFSET_Y = 0;
        private Entity.MoveState _nextMoveState;
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;
        private TextureManager _textureManager;
        private SceneManager _sceneManager;
        private EntityManager _entityManager;
        Vector2 _targetPosition = new Vector2(0, 0);
        private int _selectedTile = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1024;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 768;   // set this value to the desired height of your window

            this.IsMouseVisible = true;

            graphics.ApplyChanges();
            _textureManager = new TextureManager(this.Content);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _entityManager = new EntityManager();

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sceneManager = new SceneManager(_textureManager, _spriteBatch, _entityManager);
            _sceneManager.Initialize();

            // TODO: use this.Content to load your game content here
            Entity player = new Entity(_textureManager, _spriteBatch);
            player.Initialize("test", Entity.EntityType.PLAYER);
            player.SetPosition(160, 32);

            int staticEntityId = 0;
            _entityManager.Add(staticEntityId++, player);
            
            for (int idx1 = 0; idx1 < _map.GetLength(0); idx1++)
            {
                for (int idx2 = 0; idx2 < _map.GetLength(1); idx2++)
                {
                    int indexValue = _map[idx1, idx2];
                    Entity entity = new Entity(_textureManager, _spriteBatch);
                    entity.SetPosition((idx2 * 32) + OFFSET_X, (idx1 * 32) + OFFSET_Y);

                    switch (indexValue)
                    {
                        case 0:
                            entity.Initialize("floor01", Entity.EntityType.FLOOR);
                            break;
                        case 1:
                            entity.Initialize("wall01", Entity.EntityType.STATIC);
                            break;
                    }

                    _entityManager.Add(staticEntityId++, entity);
                }
            }

            font = Content.Load<SpriteFont>("arial");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            Entity player = _entityManager.Get(0);

            player.SetVelocity(200 * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (kstate.IsKeyDown(Keys.Escape))
                this.Exit();

            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (HasFocus())
                {
                    int mouseX = mouseState.X;
                    int mouseY = mouseState.Y;
                    System.Drawing.Rectangle bounds = GetWindowBounds();

                    if (mouseX >= 0 && mouseX < 1024 && mouseY > 0 && mouseY < 768)
                    {
                        int rowLength = _map.GetLength(0);
                        int columnLength = _map.GetLength(1);
                        int width = rowLength * 32 + OFFSET_X;
                        int height = columnLength * 32 + OFFSET_Y;

                        if (mouseState.X > OFFSET_X &&
                            mouseState.X < width &&
                            mouseState.Y > OFFSET_Y &&
                            mouseState.Y < height)
                        {
                            int row = (mouseState.X - OFFSET_X) / 32;
                            int column = (mouseState.Y - OFFSET_Y) / 32;

                            Entity pickedEntity = _entityManager.Get(mouseState.X, mouseState.Y);

                            if (pickedEntity != null)
                            {
                                pickedEntity.Initialize("wall01", Entity.EntityType.STATIC);
                                _map[row, column] = 1;
                            }
                            else
                            {
                                _entityManager.Get(mouseState.X, mouseState.Y);
                                System.Console.Write("test");
                            }
                        }
                    }
                }
            }

            Entity.MoveState currentMoveState = player.GetMoveState();
            bool directionChanged = false;
            bool turnAround = false;
            if (kstate.IsKeyDown(Keys.Up))
            {
                if (currentMoveState == Entity.MoveState.MOVING_DOWN)
                {
                    turnAround = true;
                    _nextMoveState = Entity.MoveState.MOVING_UP;
                }
                else if (currentMoveState != Entity.MoveState.MOVING_UP)
                {
                    directionChanged = true;
                    _nextMoveState = Entity.MoveState.MOVING_UP;
                }
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                if (currentMoveState == Entity.MoveState.MOVING_UP)
                {
                    turnAround = true;
                    _nextMoveState = Entity.MoveState.MOVING_DOWN;
                }
                else if (currentMoveState != Entity.MoveState.MOVING_DOWN)
                {
                    directionChanged = true;
                    _nextMoveState = Entity.MoveState.MOVING_DOWN;
                }
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                if (currentMoveState == Entity.MoveState.MOVING_RIGHT)
                {
                    turnAround = true;
                    _nextMoveState = Entity.MoveState.MOVING_LEFT;
                }
                else if (currentMoveState != Entity.MoveState.MOVING_LEFT)
                {
                    directionChanged = true;
                    _nextMoveState = Entity.MoveState.MOVING_LEFT;
                }
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                if (currentMoveState == Entity.MoveState.MOVING_LEFT)
                {
                    turnAround = true;
                    _nextMoveState = Entity.MoveState.MOVING_RIGHT;
                }
                else if (currentMoveState != Entity.MoveState.MOVING_RIGHT)
                {
                    directionChanged = true;
                    _nextMoveState = Entity.MoveState.MOVING_RIGHT;
                }
            }

            if (_nextMoveState != Entity.MoveState.IDLE)
            {
                if (player.GetMoveState() == Entity.MoveState.IDLE || turnAround)
                {
                    player.SetMoveState(_nextMoveState);
                    _targetPosition.X = 0;
                    _targetPosition.Y = 0;
                    //_nextMoveState = Entity.MoveState.IDLE;
                }
                //else if (CheckNextMove(player, _targetPosition))
                //{
                //    _targetPosition.X = 0;
                //    _targetPosition.Y = 0;
                //    CorrectPosition(player, player.GetMoveState());
                //    player.SetMoveState(_nextMoveState);
                //    _nextMoveState = Entity.MoveState.IDLE;
                //}
                //else
                //{
                else if (directionChanged)
                    {
                        //directionChanged = false;

                        Vector2 nextFreeSpace = DetermineNextMove(player, _nextMoveState);
                        if (nextFreeSpace.X != 0 && nextFreeSpace.Y != 0)
                        {
                            _targetPosition = nextFreeSpace;
                        }
                        else
                        {
                            _nextMoveState = Entity.MoveState.IDLE;
                            _targetPosition.X = 0;
                            _targetPosition.Y = 0;
                        }
                    }

                    player.Update();

                    if (_targetPosition.X != 0 && CheckNextMove(player, _targetPosition))
                    {
                        _targetPosition.X = 0;
                        _targetPosition.Y = 0;
                        CorrectPosition(player, player.GetMoveState());
                        player.SetMoveState(_nextMoveState);
                        _nextMoveState = Entity.MoveState.IDLE;
                    }

                    Entity entity = DetermineCollision(player, player.GetMoveState());
                    if (entity != null)
                    {
                        CorrectPosition(player, player.GetMoveState());
                        player.SetMoveState(Entity.MoveState.IDLE);
                    }
                //}
            }
            else if (player.GetMoveState() != Entity.MoveState.IDLE)
            {
                player.Update();
                Entity entity = DetermineCollision(player, player.GetMoveState());
                if (entity != null)
                {
                    CorrectPosition(player, player.GetMoveState());
                    player.SetMoveState(Entity.MoveState.IDLE);
                }
            }

            base.Update(gameTime);
        }

        private bool CheckNextMove(Entity entity, Vector2 target)
        {
            if (target == null || (target.X == 0 && target.Y == 0))
            {
                return false;
            }

            Vector2 currentPosition = entity.GetPosition();

            switch (entity.GetMoveState())
            {
                case Entity.MoveState.MOVING_LEFT:
                    if (currentPosition.X <= target.X)
                        return true;
                    break;
                case Entity.MoveState.MOVING_RIGHT:
                    if (currentPosition.X >= target.X)
                        return true;
                    break;
                case Entity.MoveState.MOVING_UP:
                    if (currentPosition.Y <= target.Y)
                        return true;
                    break;
                case Entity.MoveState.MOVING_DOWN:
                    if (currentPosition.Y >= target.Y)
                        return true;
                    break;
            }
            return false;
        }

        private Vector2 DetermineNextMove(Entity entity, Entity.MoveState nextMoveState)
        {
            Entity.MoveState currentDirection = entity.GetMoveState();
            Vector2 position = entity.GetPosition();

            int entityPosX = (int)(position.X / 32) * 32;
            int entityPosY = (int)(position.Y / 32) * 32;
            Entity targetEntity = null;

            switch (nextMoveState)
            {
                case Entity.MoveState.MOVING_DOWN:
                    entityPosY += 32;
                    if (currentDirection == Entity.MoveState.MOVING_RIGHT)
                    {
                        entityPosX += 32;
                    }
                    break;
                case Entity.MoveState.MOVING_LEFT:
                    entityPosX -= 32;
                    if (currentDirection == Entity.MoveState.MOVING_DOWN)
                    {
                        entityPosY += 32;
                    }
                    break;
                case Entity.MoveState.MOVING_RIGHT:
                    entityPosX += 32;
                    if (currentDirection == Entity.MoveState.MOVING_DOWN)
                    {
                        entityPosY += 32;
                    }
                    break;
                case Entity.MoveState.MOVING_UP:
                    entityPosY -= 32;
                    if (currentDirection == Entity.MoveState.MOVING_RIGHT)
                    {
                        entityPosX += 32;
                    }
                    break;
            }

            targetEntity = _entityManager.Get(entityPosX, entityPosY);
            if (targetEntity != null)
            {
                if (targetEntity.GetType() != Entity.EntityType.FLOOR)
                {
                    targetEntity.SetMoveState(currentDirection);
                    return DetermineNextMove(targetEntity, currentDirection);
                }
                return targetEntity.GetPosition();
            }

            return new Vector2(0, 0);
        }

        private Entity DetermineCollision(Entity entity, Entity.MoveState nextMoveState)
        {
            Vector2 position = entity.GetPosition();
            Entity targetEntity = null;

            switch (nextMoveState)
            {
                case Entity.MoveState.MOVING_DOWN:
                    targetEntity = _entityManager.Get(position.X, position.Y + 31);
                    if (targetEntity != null && targetEntity.GetType() != Entity.EntityType.FLOOR)
                    {
                        return targetEntity;
                    }
                    targetEntity = _entityManager.Get(position.X + 31, position.Y + 31);
                    if (targetEntity != null && targetEntity.GetType() != Entity.EntityType.FLOOR)
                    {
                        return targetEntity;
                    }
                    break;
                case Entity.MoveState.MOVING_LEFT:
                    targetEntity = _entityManager.Get(position.X, position.Y);
                    if (targetEntity != null && targetEntity.GetType() != Entity.EntityType.FLOOR)
                    {
                        return targetEntity;
                    }
                    targetEntity = _entityManager.Get(position.X, position.Y + 31);
                    if (targetEntity != null && targetEntity.GetType() != Entity.EntityType.FLOOR)
                    {
                        return targetEntity;
                    }
                    break;
                case Entity.MoveState.MOVING_RIGHT:
                    targetEntity = _entityManager.Get(position.X + 31, position.Y);
                    if (targetEntity != null && targetEntity.GetType() != Entity.EntityType.FLOOR)
                    {
                        return targetEntity;
                    }
                    targetEntity = _entityManager.Get(position.X + 31, position.Y + 31);
                    if (targetEntity != null && targetEntity.GetType() != Entity.EntityType.FLOOR)
                    {
                        return targetEntity;
                    }
                    break;
                case Entity.MoveState.MOVING_UP:
                    targetEntity = _entityManager.Get(position.X, position.Y);
                    if (targetEntity != null && targetEntity.GetType() != Entity.EntityType.FLOOR)
                    {
                        return targetEntity;
                    }
                    targetEntity = _entityManager.Get(position.X + 31, position.Y);
                    if (targetEntity != null && targetEntity.GetType() != Entity.EntityType.FLOOR)
                    {
                        return targetEntity;
                    }
                    break;
            }

            return null;
        }

        private void CorrectPosition(Entity entity1, Entity.MoveState moveState)
        {
            Vector2 position = entity1.GetPosition();
            int coordinateX = (int)position.X;
            int coordinateY = (int)position.Y;
            switch (moveState)
            {
                case Entity.MoveState.MOVING_RIGHT:
                    position.X = (coordinateX / 32) * 32;
                    break;
                case Entity.MoveState.MOVING_LEFT:
                    position.X = (coordinateX / 32) * 32 + 32;
                    break;
                case Entity.MoveState.MOVING_UP:
                    position.Y = (coordinateY / 32) * 32 + 32;
                    break;
                case Entity.MoveState.MOVING_DOWN:
                    position.Y = (coordinateY / 32) * 32;
                    break;
            }

            entity1.SetPosition(position.X, position.Y);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _sceneManager.RenderScene();
            // Finds the center of the string in coordinates inside the text rectangle
            //Vector2 textMiddlePoint = font.MeasureString("MonoGame Font Test") / 2;
            // Places text in center of the screen
            //Vector2 position = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            //_spriteBatch.DrawString(font, "MonoGame Font Test", position, Color.Black, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private System.Drawing.Rectangle GetWindowBounds()
        {
            System.Windows.Forms.Form MyGameForm = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
            return MyGameForm.Bounds;
        }

        private bool HasFocus()
        {
            System.Windows.Forms.Form MyGameForm = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(Window.Handle);
            return MyGameForm.Focused;
        }
    }
}
