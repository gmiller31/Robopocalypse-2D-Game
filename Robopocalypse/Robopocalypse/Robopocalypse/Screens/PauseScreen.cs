using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Robopocalypse_Library;

namespace Robopocalypse.Screens
{
    public class PauseScreen : GameScreen
    {
        SpriteFont font;
        String message = "P A U S E D";
        Vector2 center;
        private int selection = 0;


        Texture2D backdrop;
        private int blinkcounter;
        private Boolean isWhite;

        public PauseScreen()
        {

            backdrop = GameState.content.Load<Texture2D>(@"Textures/Menu/menu_options");
            blinkcounter = 0;
            font = GameState.content.Load<SpriteFont>(@"Font\UIFont");
            center = new Vector2(GameState.graphicsDevice.Viewport.Bounds.Center.X, GameState.graphicsDevice.Viewport.Bounds.Center.Y);
            center.X -= font.MeasureString(message).X / 2;
            center.Y -= font.MeasureString(message).Y / 2;
        }

        public override void Update(GameTime gameTime)
        {

            if (blinkcounter > 750)
            {
                if (isWhite)
                    isWhite = false;
                else
                    isWhite = true;
                blinkcounter = 0;
            }

            if ((GameState.inputManager.scrollDown(0) || GameState.inputManager.scrollDown(1)) && selection < 3)
            {
                selection++;
                GameState.soundBank.PlayCue("MenuChangeSelection");
            }
            if ((GameState.inputManager.scrollUp(0) || GameState.inputManager.scrollUp(1)) && selection > 0)
            {
                selection--;
                GameState.soundBank.PlayCue("MenuChangeSelection");
            }

            switch (selection)
            {
                case 0:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.screenManager.Pop();
                    }
                    break;

                case 1:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.screenManager.Push(new OptionMenuScreen());
                    }
                    break;

                case 2:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.screenManager.Push(new StatisticsScreen());
                    }
                    break;

                case 3:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);

                        GameState.cue = GameState.soundBank.GetCue("G4final");
                        GameState.cue.Play();
                        
                        GameState.screenManager.Pop();
                        GameState.screenManager.Pop();
                        GameState.objectManager.Reset();
                    }
                    break;
            }
            if (GameState.inputManager.Start(0) || GameState.inputManager.Start(1))
            {
                GameState.screenManager.Pop();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //GameState.spriteBatch.DrawString(font, message, center, Color.White);


            GameState.objectManager.Draw(gameTime);

            GameState.primitiveBatch.Begin(PrimitiveType.LineList);//If you need to use primitiveBatch, please use it in this block.
            GameState.objectManager.players.Draw_Line(gameTime);
            GameState.primitiveBatch.End();

            GameState.spriteBatch.Draw(backdrop, new Vector2(GameState.SCREEN_WIDTH / 2 - backdrop.Width / 2, 250), Color.White);



            if (selection == 0)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Resume", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Resume").X / 2, 300), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Resume", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Resume").X / 2, 300), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Resume", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Resume").X / 2, 300), Color.Aqua);

            if (selection == 1)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Option Menu", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Option Menu").X / 2, 320), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Option Menu", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Option Menu").X / 2, 320), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Option Menu", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Option Menu").X / 2, 320), Color.Aqua);

            if (selection == 2)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Statistics", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Statistics").X / 2, 340), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Statistics", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Statistics").X / 2, 340), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Statistics", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Statistics").X / 2, 340), Color.Aqua);

            if (selection == 3)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 360), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 360), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 360), Color.Aqua);
        }
    }
}
