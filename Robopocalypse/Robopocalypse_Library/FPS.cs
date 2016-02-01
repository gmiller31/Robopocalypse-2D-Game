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


namespace Robopocalypse_Library
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FPS : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private float UpdateFPS, currentUpdateFPS;
        private float DrawFPS, currentDrawFPS;
        private float elapsedUpdateTime;
        private float elapsedDrawTime;
        public String fps;

        public FPS(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            UpdateFPS = 0.0f;
            currentUpdateFPS = 0.0f;
            DrawFPS = 0.0f;
            currentDrawFPS = 0.0f;
            elapsedUpdateTime = 0.0f;
            elapsedDrawTime = 0.0f;
            fps = String.Empty;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            elapsedUpdateTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedUpdateTime >= 1000)
            {
                elapsedUpdateTime = 0;
                UpdateFPS = currentUpdateFPS;
                currentUpdateFPS = 0;
            }
            else
            {
                currentUpdateFPS++;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            elapsedDrawTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedDrawTime >= 1000)
            {
                fps = String.Format("Update: {0}; Draw: {1}", UpdateFPS.ToString(), DrawFPS.ToString());
                this.Game.Window.Title = fps;
                elapsedDrawTime = 0;
                DrawFPS = currentDrawFPS;
                currentDrawFPS = 0;
            }
            else
            {
                currentDrawFPS++;
            }

            base.Draw(gameTime);
        }
    }
}
