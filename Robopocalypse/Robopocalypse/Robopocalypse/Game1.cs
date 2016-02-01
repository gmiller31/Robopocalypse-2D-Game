using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Robopocalypse_Library;

namespace Robopocalypse
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public Game1()
        {
            GameState.graphicsManager = new GraphicsDeviceManager(this);
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
            GameState.graphicsManager.PreferredBackBufferWidth = GameState.SCREEN_WIDTH;
            GameState.graphicsManager.PreferredBackBufferHeight = GameState.SCREEN_HEIGHT;
            GameState.graphicsManager.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GameState.game = this;
            GameState.graphicsDevice = GraphicsDevice;
            GameState.content = Content;
            GameState.spriteBatch = new SpriteBatch(GameState.graphicsDevice);
            GameState.primitiveBatch = new PrimitiveBatch(GraphicsDevice);
            GameState.screenManager = new ScreenManager();
            GameState.keyboardInput = new KeyboardInput(this);
            Components.Add(GameState.keyboardInput);
            GameState.gamePadInput = new GamePadInput(this);
            Components.Add(GameState.gamePadInput);
            GameState.inputManager = new Managers.InputManager();
            GameState.fps = new FPS(this);
            GameState.font = GameState.content.Load<SpriteFont>(@"Font\ConsoleFont12");
            Components.Add(GameState.fps);
            GameState.Structure = new Structure();
            GameState.objectManager = new Managers.GameObjectsManager();
            GameState.objectManager.LoadManagers();
            GameState.sbHasEnded = false;
            //GameState.objectManager.SpriteList = GameState.Structure.LoadLevel(@"Content\Levels\level1.txt", GameState.BuildingSprite);

            //initializing frost screen
            GameState.frostsprite = GameState.content.Load<Texture2D>(@"Textures\frostscreen");
            GameState.frostexplosions = new GlobalSpriteEffect("freeze", 200);

            GameState.audioEngine = new AudioEngine("Content/Audio/Robopocalypse.xgs");                  //
            GameState.waveBank = new WaveBank(GameState.audioEngine, "Content/Audio/Wave Bank.xwb");     //  AUDIO
            GameState.soundBank = new SoundBank(GameState.audioEngine, "Content/Audio/Sound Bank.xsb");  //
            GameState.musicCategory = GameState.audioEngine.GetCategory("Music");                        //
            GameState.soundCategory = GameState.audioEngine.GetCategory("Effects");                      //
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void BeginRun()
        {
            GameState.screenManager.Push(new Screens.SplashScreen());
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            GameState.audioEngine.Update();

            GameState.screenManager.Top().Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); 
            GameState.spriteBatch.Begin();
            GameState.sbHasEnded = false;
            GameState.screenManager.Top().Draw(gameTime);

            //spriteBatch needs to end early for line and some particles to render properly.
            if (!GameState.sbHasEnded)
            {
                GameState.spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
