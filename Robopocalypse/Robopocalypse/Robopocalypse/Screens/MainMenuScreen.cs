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
    public class MainMenuScreen : GameScreen
    {
        private int selection = 0;
        private int blinkcounter;
        private Boolean isWhite;
        //Texture to hold the backdrop
        Texture2D backdrop;
        public MainMenuScreen()
        {
            backdrop = GameState.content.Load<Texture2D>(@"Textures/Menu/menu");
            blinkcounter = 0;
        }

        public override void Update(GameTime gameTime)
        {
            blinkcounter += gameTime.ElapsedGameTime.Milliseconds;

            if(blinkcounter>750)
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
                        GameState.PlayerCount = 1;
                        GameState.screenManager.Push(new LevelSelectScreen());
                    }
                    break;

                case 1:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.PlayerCount = 2;
                        GameState.screenManager.Push(new LevelSelectScreen());
                    }
                    break;

                case 2:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.screenManager.Push(new OptionMenuScreen());
                    }
                    break;

                case 3:
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.screenManager.Pop();
                        GameState.game.Exit();
                    }
                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {

            GameState.spriteBatch.Draw(backdrop, new Vector2(GameState.SCREEN_WIDTH / 2 - backdrop.Width / 2, 250), Color.White);


           // GameState.spriteBatch.DrawString(GameState.font, "Robopocalypse", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Main Menu").X / 2, 250), Color.White);
            
            if (selection == 0)
                if(isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Single Player", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Single Player").X / 2, 300), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Single Player", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Single Player").X / 2, 300), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Single Player", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Single Player").X / 2, 300), Color.Aqua);

            if (selection == 1)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Multiplayer", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Multiplayer").X / 2, 320), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Multiplayer", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Multiplayer").X / 2, 320), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Multiplayer", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Multiplayer").X / 2, 320), Color.Aqua);

            if (selection == 2)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Option Menu", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Option Menu").X / 2, 340), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Option Menu", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Option Menu").X / 2, 340), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Option Menu", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Option Menu").X / 2, 340), Color.Aqua);

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
