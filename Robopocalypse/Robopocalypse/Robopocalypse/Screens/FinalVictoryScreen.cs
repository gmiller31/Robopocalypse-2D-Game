using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Robopocalypse_Library;
using Microsoft.Xna.Framework.Audio;

namespace Robopocalypse.Screens
{
    public class FinalVictoryScreen : GameScreen
    {
        Texture2D backdrop;

        public FinalVictoryScreen()
        {
            backdrop = GameState.content.Load<Texture2D>(@"Textures/victory");
        }

        public override void Update(GameTime gameTime)
        {
            if ((GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1)))
            {
                GameState.cue = GameState.soundBank.GetCue("G4final");
                GameState.cue.Play();
                GameState.objectManager.Reset();
                GameState.screenManager.Pop();
                GameState.screenManager.Push(new StatisticsScreen());
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameState.spriteBatch.Draw(backdrop, new Vector2(0, 0), Color.White);
        }
    }
}
