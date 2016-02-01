using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Robopocalypse_Library;

namespace Robopocalypse.Screens
{
    public class SplashScreen : GameScreen
    {
        private Texture2D background;
        private Vector2 background_pos;

        public SplashScreen()
        {
            background = GameState.content.Load<Texture2D>(@"Textures\splash");
            background_pos = Vector2.Zero;
            GameState.cue = GameState.soundBank.GetCue("G4final");
            GameState.cue.Play();
        }

        public override void Update(GameTime gameTime)
        {
            if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
            {
                GameState.screenManager.Pop();
                GameState.screenManager.Push(new MainMenuScreen());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameState.spriteBatch.Draw(background, background_pos, Color.White);
        }
    }
}
