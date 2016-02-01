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
    public class CreditScreen : GameScreen
    {

        Texture2D backdrop;

        public CreditScreen()
        {
            backdrop = GameState.content.Load<Texture2D>(@"Textures/Menu/menu_credits");
        }

        public override void Update(GameTime gameTime)
        {
            if (GameState.inputManager.Cancel(0) || GameState.inputManager.Cancel(1) || GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
            {
                GameState.soundBank.PlayCue("Back");
                GameState.screenManager.Pop();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameState.spriteBatch.Draw(backdrop, new Vector2(GameState.SCREEN_WIDTH / 2 - backdrop.Width / 2, 250), Color.White);
            //GameState.spriteBatch.DrawString(GameState.font, "Credits", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Credits").X / 2, 100), Color.White);

            //Credits
            //GameState.spriteBatch.DrawString(GameState.font, "Project Lead", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Project Lead").X / 2, 280), Color.Aqua);
            GameState.spriteBatch.DrawString(GameState.font, "Brian Kowalczk", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Brian Kowalczk").X / 2, 300), Color.Aqua);

            //GameState.spriteBatch.DrawString(GameState.font, "Project Team", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Project Team").X / 2, 300), Color.Aqua);
            GameState.spriteBatch.DrawString(GameState.font, "Luke Dobben", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Luke Dobben").X / 2, 320), Color.Aqua);
            GameState.spriteBatch.DrawString(GameState.font, "Cody Garvey", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Cody Garvey").X / 2, 340), Color.Aqua);
            GameState.spriteBatch.DrawString(GameState.font, "Nicholas Gunthorp", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Nicholas Gunthorp").X / 2, 360), Color.Aqua);
            GameState.spriteBatch.DrawString(GameState.font, "Geoffrey Miller", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Geoffrey Miller").X / 2, 380), Color.Aqua);
            GameState.spriteBatch.DrawString(GameState.font, "Michael Snyder", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Michael Snyder").X / 2, 400), Color.Aqua);

        }
    }
}
