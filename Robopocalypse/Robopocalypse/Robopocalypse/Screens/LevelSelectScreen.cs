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
    public class LevelSelectScreen : GameScreen
    {
        private int selection = 0;
        private int blinkcounter;
        private Boolean isWhite;
        //Texture to hold the backdrop
        Texture2D backdrop;
        public LevelSelectScreen()
        {
            backdrop = GameState.content.Load<Texture2D>(@"Textures/Menu/menu_levelselect");
            blinkcounter = 0;
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

            if (GameState.inputManager.Cancel(0) || GameState.inputManager.Cancel(1))
            {
                GameState.soundBank.PlayCue("Back");
                GameState.screenManager.Pop();
            }
            if ((GameState.inputManager.scrollDown(0) || GameState.inputManager.scrollDown(1)) && selection < 15)
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
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.currentLevel = selection + 1;
                        GameState.screenManager.Push(new PlayScreen());
                    }
                    break;

                case 1:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 2:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 3:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 4:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 5:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 6:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 7:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 8:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 9:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 10:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 11:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 12:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 13:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 14:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.cue.Stop(AudioStopOptions.AsAuthored);
                        GameState.screenManager.Pop();
                        GameState.screenManager.Push(new PlayScreen());
                        GameState.currentLevel = selection;
                        GameState.objectManager.players.Award_Player_Cash(0, 3000 * selection);
                        GameState.objectManager.players.Award_Player_Cash(1, 3000 * selection);
                        GameState.screenManager.Push(new LevelUpScreen());
                    }
                    break;

                case 15:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.screenManager.Pop();
                    }
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {

            GameState.spriteBatch.Draw(backdrop, new Vector2(GameState.SCREEN_WIDTH / 2 - backdrop.Width / 2, 250), Color.White);


            // GameState.spriteBatch.DrawString(GameState.font, "Robopocalypse", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Main Menu").X / 2, 250), Color.White);

            //Tutorial
            GameState.spriteBatch.DrawString(GameState.font, "Tutorial", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Tutorial").X / 2, 300), Color.Aqua);

            if (selection == 0)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 1", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 1").X / 2, 320), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 1", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 1").X / 2, 320), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 1", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 1").X / 2, 320), Color.Aqua);

            if (selection == 1)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 2", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 2").X / 2, 340), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 2", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 2").X / 2, 340), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 2", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 2").X / 2, 340), Color.Aqua);

            if (selection == 2)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 3", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 3").X / 2, 360), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 3", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 3").X / 2, 360), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 3", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 3").X / 2, 360), Color.Aqua);

            //Main Game
            GameState.spriteBatch.DrawString(GameState.font, "Main Game", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Main Game").X / 2, 380), Color.Aqua);

            if (selection == 3)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 1", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 1").X / 2, 400), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 1", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 1").X / 2, 400), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 1", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 1").X / 2, 400), Color.Aqua);

            if (selection == 4)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 2", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 2").X / 2, 420), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 2", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 2").X / 2, 420), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 2", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 2").X / 2, 420), Color.Aqua);

            if (selection == 5)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 3", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 3").X / 2, 440), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 3", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 3").X / 2, 440), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 3", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 3").X / 2, 440), Color.Aqua);

            if (selection == 6)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 4", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 4").X / 2, 460), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 4", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 4").X / 2, 460), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 4", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 4").X / 2, 460), Color.Aqua);

            if (selection == 7)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 5", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 5").X / 2, 480), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 5", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 5").X / 2, 480), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 5", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 5").X / 2, 480), Color.Aqua);

            if (selection == 8)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 6", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 6").X / 2, 500), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 6", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 6").X / 2, 500), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 6", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 6").X / 2, 500), Color.Aqua);

            if (selection == 9)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 7", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 7").X / 2, 520), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 7", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 7").X / 2, 520), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 7", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 7").X / 2, 520), Color.Aqua);

            if (selection == 10)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 8", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 8").X / 2, 540), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 8", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 8").X / 2, 540), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 8", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 8").X / 2, 540), Color.Aqua);

            if (selection == 11)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 9", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 9").X / 2, 560), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 9", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 9").X / 2, 560), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 9", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 9").X / 2, 560), Color.Aqua);

            if (selection == 12)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 10", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 10").X / 2, 580), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 10", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 10").X / 2, 580), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 10", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 10").X / 2, 580), Color.Aqua);

            if (selection == 13)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 11", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 11").X / 2, 600), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 11", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 11").X / 2, 600), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 11", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 11").X / 2, 600), Color.Aqua);

            if (selection == 14)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Level 12", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 12").X / 2, 620), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Level 12", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 12").X / 2, 620), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Level 12", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Level 12").X / 2, 620), Color.Aqua);

            if (selection == 15)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 640), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 640), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 640), Color.Aqua);
        }
    }
}
