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
    public class ControlMenuScreen : GameScreen
    {
        private int selection = 0;
        private int blinkcounter = 0;
        private int mode = 0;
        private int change = 0;
        private Boolean isWhite;
        Texture2D backdrop;

        public ControlMenuScreen()
        {
            backdrop = GameState.content.Load<Texture2D>(@"Textures/Menu/menu_controls");
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

            if (change == 0)
            {
                switch (mode)
                {
                    case 0:
#if WINDOWS
                        if ((GameState.inputManager.scrollDown(0) || GameState.inputManager.scrollDown(1)) && selection < 2)
                        {
                            selection++;
                            GameState.soundBank.PlayCue("MenuChangeSelection");
                        }
                        if ((GameState.inputManager.scrollUp(0) || GameState.inputManager.scrollUp(1)) && selection > 0)
                        {
                            selection--;
                            GameState.soundBank.PlayCue("MenuChangeSelection");
                        }
#endif

#if XBOX
                        if ((GameState.inputManager.scrollDown(0) || GameState.inputManager.scrollDown(1)) && selection < 2)
                        {
                            selection += 2;
                            GameState.soundBank.PlayCue("MenuChangeSelection");
                        }
                        if ((GameState.inputManager.scrollUp(0) || GameState.inputManager.scrollUp(1)) && selection > 0)
                        {
                            selection -= 2;
                            GameState.soundBank.PlayCue("MenuChangeSelection");
                        }
#endif

                        if(GameState.inputManager.Cancel(0) || GameState.inputManager.Cancel(1))
                        {
                            GameState.soundBank.PlayCue("Back");
                            GameState.screenManager.Pop();
                        }

                        switch (selection)
                        {
                            case 0:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                {
                                    if (GameState.gamePadInput.isConnected(0) || GameState.gamePadInput.isConnected(1) || GameState.gamePadInput.isConnected(2) || GameState.gamePadInput.isConnected(3))
                                    {
                                        selection = 0;
                                        mode = 1;
                                    }
                                    else
                                    {
                                        GameState.soundBank.PlayCue("Back");
                                    }
                                }
                                break;

                            case 1:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                {
                                    selection = 0;
                                    mode = 2;
                                }
                                break;

                            case 2:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                {
                                    GameState.soundBank.PlayCue("Back");
                                    GameState.screenManager.Pop();
                                }
                                break;
                        }
                        break;

                    case 1://GamePad
                    case 2://Keyboard
                        if ((GameState.inputManager.scrollDown(0) || GameState.inputManager.scrollDown(1)) && selection < 8)
                        {
                            selection++;
                            GameState.soundBank.PlayCue("MenuChangeSelection");
                        }
                        if ((GameState.inputManager.scrollUp(0) || GameState.inputManager.scrollUp(1)) && selection > 0)
                        {
                            selection--;
                            GameState.soundBank.PlayCue("MenuChangeSelection");
                        }
                        if (GameState.inputManager.Cancel(0) || GameState.inputManager.Cancel(1))
                        {
                            GameState.soundBank.PlayCue("Back");
                            selection = 0;
                            mode = 0;
                        }

                        switch (selection)
                        {
                            case 0:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                change = 1;
                                break;

                            case 1:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                change = 2;
                                break;

                            case 2:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                change = 3;
                                break;

                            case 3:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                change = 4;
                                break;

                            case 4:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                change = 5;
                                break;

                            case 5:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                change = 6;
                                break;

                            case 6:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                change = 7;
                                break;

                            case 7:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                change = 8;
                                break;

                            case 8:
                                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                                {
                                    selection = 0;
                                    mode = 0;
                                }
                                break;
                        }
                        break;
                }
            }
            else
            {
                if(mode == 1)
                {
                    Buttons button;
                    button = GameState.gamePadInput.nextButtonPressed();
                    if(button != Buttons.Start)
                    {
                        switch (change)
                        {
                            case 1:
                                GameState.b_Up1 = button;
                                break;

                            case 2:
                                GameState.b_Up2 = button;
                                break;

                            case 3:
                                GameState.b_Down1 = button;
                                break;

                            case 4:
                                GameState.b_Down2 = button;
                                break;

                            case 5:
                                GameState.b_Left1 = button;
                                break;

                            case 6:
                                GameState.b_Left2 = button;
                                break;

                            case 7:
                                GameState.b_Right1 = button;
                                break;

                            case 8:
                                GameState.b_Right2 = button;
                                break;
                        }
                        change = 0;
                    }
                }
                else if (mode == 2)
                {
                    Keys key;
                    key = GameState.keyboardInput.nextKeyPressed();
                    if (key != Keys.Escape)
                    {
                        switch (change)
                        {
                            case 1:
                                GameState.k_Up1 = key;
                                break;

                            case 2:
                                GameState.k_Up2 = key;
                                break;

                            case 3:
                                GameState.k_Down1 = key;
                                break;

                            case 4:
                                GameState.k_Down2 = key;
                                break;

                            case 5:
                                GameState.k_Left1 = key;
                                break;

                            case 6:
                                GameState.k_Left2 = key;
                                break;

                            case 7:
                                GameState.k_Right1 = key;
                                break;

                            case 8:
                                GameState.k_Right2 = key;
                                break;
                        }
                        change = 0;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameState.spriteBatch.Draw(backdrop, new Vector2(GameState.SCREEN_WIDTH / 2 - backdrop.Width / 2, 250), Color.White);
            //GameState.spriteBatch.DrawString(GameState.font, "Control Menu", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Control Menu").X / 2, 100), Color.White);

            if (mode == 0)
            {
                if (selection == 0)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Controller", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Controller").X / 2, 300), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Controller", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Controller").X / 2, 300), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Controller", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Controller").X / 2, 300), Color.Aqua);
#if WINDOWS
                if (selection == 1)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Keyboard", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Keyboard").X / 2, 320), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Keyboard", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Keyboard").X / 2, 320), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Keyboard", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Keyboard").X / 2, 320), Color.Aqua);
#endif
                if (selection == 2)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 400), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 400), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 400), Color.Aqua);
            }
            else if (mode == 1 || mode == 2)
            {
                if (selection == 0)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Up 1", new Vector2(410, 300 + selection * 20), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Up 1", new Vector2(410, 300 + selection * 20), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Up 1", new Vector2(410, 300 + 0 * 20), Color.Aqua);

                if (selection == 1)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Up 2", new Vector2(410, 300 + selection * 20), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Up 2", new Vector2(410, 300 + selection * 20), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Up 2", new Vector2(410, 300 + 1 * 20), Color.Aqua);

                if (selection == 2)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Down 1", new Vector2(410, 300 + selection * 20), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Down 1", new Vector2(410, 300 + selection * 20), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Down 1", new Vector2(410, 300 + 2 * 20), Color.Aqua);

                if (selection == 3)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Down 2", new Vector2(410, 300 + selection * 20), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Down 2", new Vector2(410, 300 + selection * 20), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Down 2", new Vector2(410, 300 + 3 * 20), Color.Aqua);

                if (selection == 4)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Left 1", new Vector2(410, 300 + selection * 20), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Left 1", new Vector2(410, 300 + selection * 20), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Left 1", new Vector2(410, 300 + 4 * 20), Color.Aqua);

                if (selection == 5)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Left 2", new Vector2(410, 300 + selection * 20), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Left 2", new Vector2(410, 300 + selection * 20), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Left 2", new Vector2(410, 300 + 5 * 20), Color.Aqua);

                if (selection == 6)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Right 1", new Vector2(410, 300 + selection * 20), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Right 1", new Vector2(410, 300 + selection * 20), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Right 1", new Vector2(410, 300 + 6 * 20), Color.Aqua);

                if (selection == 7)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Right 2", new Vector2(410, 300 + selection * 20), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Right 2", new Vector2(410, 300 + selection * 20), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Right 2", new Vector2(410, 300 + 7 * 20), Color.Aqua);

                if (selection == 8)
                    if (isWhite)
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 300 + selection * 20), Color.LightYellow);
                    }
                    else
                        GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 300 + selection * 20), Color.Yellow);
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 300 + 8 * 20), Color.Aqua);

                if(mode == 1)
                {
                    //Display Current buttons
                    GameState.spriteBatch.DrawString(GameState.font, GameState.b_Up1.ToString(), new Vector2(560, 300 + 0 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.b_Up2.ToString(), new Vector2(560, 300 + 1 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.b_Down1.ToString(), new Vector2(560, 300 + 2 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.b_Down2.ToString(), new Vector2(560, 300 + 3 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.b_Left1.ToString(), new Vector2(560, 300 + 4 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.b_Left2.ToString(), new Vector2(560, 300 + 5 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.b_Right1.ToString(), new Vector2(560, 300 + 6 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.b_Right2.ToString(), new Vector2(560, 300 + 7 * 20), Color.Aqua);
                }
                else if(mode == 2)
                {
                    //Display Current Keys
                    GameState.spriteBatch.DrawString(GameState.font, GameState.k_Up1.ToString(), new Vector2(560, 300 + 0 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.k_Up2.ToString(), new Vector2(560, 300 + 1 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.k_Down1.ToString(), new Vector2(560, 300 + 2 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.k_Down2.ToString(), new Vector2(560, 300 + 3 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.k_Left1.ToString(), new Vector2(560, 300 + 4 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.k_Left2.ToString(), new Vector2(560, 300 + 5 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.k_Right1.ToString(), new Vector2(560, 300 + 6 * 20), Color.Aqua);
                    GameState.spriteBatch.DrawString(GameState.font, GameState.k_Right2.ToString(), new Vector2(560, 300 + 7 * 20), Color.Aqua);
                }
            }
        }
    }
}
