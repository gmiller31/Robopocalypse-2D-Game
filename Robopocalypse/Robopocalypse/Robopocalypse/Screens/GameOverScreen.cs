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
    public class GameOverScreen : GameScreen
    {
        private Texture2D background;



        //Adding it in so it shows up with the backdrop
        private Texture2D backdrop;
        private int selection = 0;
        private int blinkcounter = 0;
        private Boolean isWhite = false;

        public GameOverScreen()
        {
            background = GameState.content.Load<Texture2D>(@"Textures\gameover");
            backdrop = GameState.content.Load<Texture2D>(@"Textures\Menu\menu_gameover");
            GameState.cue.Stop(AudioStopOptions.AsAuthored);
        }

        public override void Update(GameTime gameTime)
        {
            blinkcounter += gameTime.ElapsedGameTime.Milliseconds;

            if (blinkcounter > 750)
            {
                if (isWhite)
                    isWhite = false;
                else
                    isWhite = true;
                blinkcounter = 0;
            }

            if ((GameState.inputManager.scrollDown(0) || GameState.inputManager.scrollDown(1)) && selection < 1)
            {
                selection++;
                GameState.soundBank.PlayCue("MenuChangeSelection");
            }
            if ((GameState.inputManager.scrollUp(0) || GameState.inputManager.scrollUp(1)) && selection > 0)
            {
                selection--;
                GameState.soundBank.PlayCue("MenuChangeSelection");
            }

            if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
            {
                if(selection == 0)
                {
                    GameState.gameOver = false;
                    GameState.levelStart = false;
                    GameState.deathtimelag = 0;
                    GameState.cue = GameState.soundBank.GetCue("MusicTrack1");
                    GameState.cue.Play();
                    GameState.objectManager.LevelView.LoadLevel(GameState.currentLevel);
                    GameState.objectManager.players.New_Level();

                    if (GameState.screenManager.Count() == 4)
                    {
                        GameState.screenManager.Pop();
                        GameState.screenManager.Pop();
                    }
                    else
                    {
                        GameState.screenManager.Pop();
                    }
                }
                else if(selection == 1)
                {
                    GameState.cue = GameState.soundBank.GetCue("G4final");
                    GameState.cue.Play();

                    if(GameState.screenManager.Count() == 4)
                    {
                        GameState.screenManager.Pop();
                        GameState.screenManager.Pop();
                        GameState.screenManager.Pop();
                    }
                    else
                    {
                        GameState.screenManager.Pop();
                        GameState.screenManager.Pop();
                    }
                    GameState.objectManager.Reset();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //GameState.spriteBatch.Draw(background, Vector2.Zero, Color.White);

            GameState.objectManager.Draw(gameTime);

            GameState.spriteBatch.Draw(backdrop, new Vector2(GameState.SCREEN_WIDTH / 2 - backdrop.Width / 2, 250), Color.White);



            if (selection == 0)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Continue", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Continue").X / 2, 300), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Continue", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Continue").X / 2, 300), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Continue", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Continue").X / 2, 300), Color.Aqua);

            if (selection == 1)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 340), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 340), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 340), Color.Aqua);
        }
    }
}
