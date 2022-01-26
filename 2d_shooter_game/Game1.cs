using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace _2d_shooter_game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //control the start of the game
        Texture2D startButton;
        bool gameStart = false;

        //Texture uploads 
        Texture2D target_Sprite;
        Texture2D crosshair_Sprite;
        Texture2D background_Sprite;
        //needed for mouse logic
        MouseState mState;
        bool mReleased = true;
        int score = 0;
        float mouseTargetDist;
        const int TARGET_RADIUS = 45;
        const int CROSSHAIR_RADIUS = 20;

        //for target randomization
        Random rand = new Random();
        Vector2 targetPosition = new Vector2(300, 300);
        Vector2 crosshairLocation;
        //timer for game
        float timer = 10f; //count down for 10 seconds
        //font style for game
        SpriteFont gameFont;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            background_Sprite = Content.Load<Texture2D>("forest");
            crosshair_Sprite = Content.Load<Texture2D>("crosshair_blue_small");
            target_Sprite = Content.Load<Texture2D>("duck_target_white");
            startButton = Content.Load<Texture2D>("start_button");

            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
           
            mState = Mouse.GetState();
            crosshairLocation = new Vector2(mState.X - CROSSHAIR_RADIUS, mState.Y - CROSSHAIR_RADIUS);
            //is the mouse being clicked and the game has not started?
            if(mState.LeftButton == ButtonState.Pressed && mReleased == true && gameStart == false)
            {
                //start button is 405 by 195 
                if (mState.X > 200 && mState.X < 605 && mState.Y > 100 && mState.Y < 295)
                    //this will make start button disappear and draw the target in Draw() function
                    gameStart = true;

            }

            mouseTargetDist = Vector2.Distance(targetPosition, new Vector2(mState.X, mState.Y));
            if (timer > 0 && gameStart == true)
            {
                timer = timer - (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if(timer < 0 && gameStart == true)
                //get a nice even 0
                timer = 0;
            if (mState.LeftButton == ButtonState.Pressed && mReleased == true && timer > 0)
            {
                //its a hit
                if (mouseTargetDist < TARGET_RADIUS)
                {
                    score++;
                    //randomize target location after hit
                    targetPosition.X = rand.Next(TARGET_RADIUS, _graphics.PreferredBackBufferWidth - TARGET_RADIUS);
                    targetPosition.Y = rand.Next(TARGET_RADIUS, _graphics.PreferredBackBufferHeight - TARGET_RADIUS);
                }
                mReleased = false;
            }
            if (mState.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }

            base.Update(gameTime);
            
            
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(background_Sprite, new Vector2(0, 0), Color.White);
            //only draw the mouse and start button until we start the game
            if (gameStart == false)
            { 
                _spriteBatch.Draw(startButton, new Vector2(200, 100), Color.White);
                _spriteBatch.Draw(crosshair_Sprite, crosshairLocation, Color.White);
            }
            //game has started, add in the target and redraw crosshair so it is on top of target
            if (gameStart == true)
            {
                _spriteBatch.Draw(target_Sprite, new Vector2(targetPosition.X - TARGET_RADIUS, targetPosition.Y - TARGET_RADIUS), Color.White);
                _spriteBatch.Draw(crosshair_Sprite, crosshairLocation, Color.White);

            }
            //only want to see these while game is going
            if (timer > 0 && gameStart == true)
            {
                _spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(500, 0), Color.Black);
                _spriteBatch.DrawString(gameFont, "Time: " + timer.ToString(), new Vector2(100, 0), Color.White);
            }
            //time is up, display message in center of screen
            if(timer == 0 && gameStart == true)
            {
                _spriteBatch.Draw(background_Sprite, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(crosshair_Sprite, crosshairLocation, Color.White);
                _spriteBatch.DrawString(gameFont, "Time's Up! Score: " + score.ToString(), new Vector2(250, 200), Color.Black);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
