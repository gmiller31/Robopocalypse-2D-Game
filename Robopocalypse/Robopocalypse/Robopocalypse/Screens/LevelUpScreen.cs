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
    public class LevelUpScreen : GameScreen
    {
        private int[] selections;

        private int[] accelCosts;
        private int[] speedCosts;
        private int[] energyCosts;
        private int[] rechargeCosts;
        private int[] drainCosts;
        private int[] damageCosts;
        private int[][] costs;
        private int playerToChange = 0;
        private string[] upgrades;

        private Color drawColor;

        private bool ready = false;
        private bool ready2 = false;
        Texture2D backdrop;
        public LevelUpScreen()
        {
            selections = new int[2];
            selections[0] = 0;
            selections[1] = 0;
            if (GameState.PlayerCount == 1)
                selections[1] = -2;
            set_costs();
            backdrop = GameState.content.Load<Texture2D>(@"Textures/Menu/menu_levelup");
        }

        private void set_costs()
        {
            speedCosts = new int[6];
            energyCosts = new int[6];
            drainCosts = new int[6];
            accelCosts = new int[6];
            rechargeCosts = new int[6];
            damageCosts = new int[6];

            upgrades = new string[8];
            upgrades[0] = "Acceleration";
            upgrades[1] = "Max Speed";
            upgrades[2] = "Max Energy";
            upgrades[3] = "Energy Recharge Speed";
            upgrades[4] = "Energy Drain Speed";
            upgrades[5] = "Beam Damage";
            upgrades[6] = "Health Pickup Bonus";
            upgrades[7] = "Invincibility Time";

            //FOR COSTS:::::
            //0 = accel
            //1 = speed
            //2 = energy
            //3 = recharge
            //4 = drain
            //5 = damage
            costs = new int[8][];
            for (int i = 0; i < 8; i++)
            {
                costs[i] = new int[6];
            }

            //8200
            costs[0][0] = 300;
            costs[0][1] = 800;
            costs[0][2] = 1400;
            costs[0][3] = 2200;
            costs[0][4] = 3500;
            costs[0][5] = 0;

            //12600
            costs[1][0] = 500;
            costs[1][1] = 1100;
            costs[1][2] = 2000;
            costs[1][3] = 3500;
            costs[1][4] = 5500;
            costs[1][5] = 0;

            //12600
            costs[2][0] = 500;
            costs[2][1] = 1100;
            costs[2][2] = 2000;
            costs[2][3] = 3500;
            costs[2][4] = 5500;
            costs[2][5] = 0;

            //5000
            costs[3][0] = 200;
            costs[3][1] = 500;
            costs[3][2] = 900;
            costs[3][3] = 1400;
            costs[3][4] = 2000;
            costs[3][5] = 0;

            //5000
            costs[4][0] = 200;
            costs[4][1] = 500;
            costs[4][2] = 900;
            costs[4][3] = 1400;
            costs[4][4] = 2000;
            costs[4][5] = 0;

            //13300
            costs[5][0] = 400;
            costs[5][1] = 1100;
            costs[5][2] = 2100;
            costs[5][3] = 3700;
            costs[5][4] = 6000;
            costs[5][5] = 0;

            //11600
            costs[6][0] = 500;
            costs[6][1] = 1100;
            costs[6][2] = 2000;
            costs[6][3] = 3200;
            costs[6][4] = 4800;
            costs[6][5] = 0;

            //13300
            costs[7][0] = 400;
            costs[7][1] = 1100;
            costs[7][2] = 2100;
            costs[7][3] = 3700;
            costs[7][4] = 6000;
            costs[7][5] = 0;

            //1-2700
            //2-6200
            //3-11500
            //4-19600
            //5-30500

            //t-70500
        }

        private bool check_if_maxed(int select, int playerNum)
        {
            bool returnVal = false;
            if (GameState.objectManager.players.Get_Player_Level(playerNum, select) == 5)
                returnVal = true;
            return returnVal;
        }

        private void scroll_to_next_down(int playerNum)
        {
            while(check_if_maxed(selections[playerNum], playerNum))
            {
                selections[playerNum]++;
            }
        }

        private void Check_Input()
        {
            if (GameState.inputManager.scrollDown(0))
            {
                selections[playerToChange]++;
                if (selections[playerToChange] == 9)
                    selections[playerToChange] = 0;

                bool isMaxed = false;

                do
                {
                    isMaxed = check_if_maxed(selections[playerToChange], playerToChange);
                    if (isMaxed)
                        selections[playerToChange]++;
                } while (isMaxed == true);

                if (!ready && playerToChange == 0 || !ready2 && playerToChange == 1)
                    GameState.soundBank.PlayCue("MenuChangeSelection");
            }

            if (GameState.inputManager.scrollUp(0))
            {
                selections[playerToChange]--;
                if (selections[playerToChange] == -1)
                    selections[playerToChange] = 8;

                bool isMaxed = false;

                do
                {
                    isMaxed = check_if_maxed(selections[playerToChange], playerToChange);
                    if (isMaxed)
                        selections[playerToChange]--;
                    if (selections[playerToChange] == -1)
                    {
                        selections[playerToChange] = 8;
                        isMaxed = false;
                    }
                } while (isMaxed == true);

                if(!ready && playerToChange == 0 || !ready2 && playerToChange == 1)
                    GameState.soundBank.PlayCue("MenuChangeSelection");
            }
            

            if(GameState.PlayerCount == 1)//Single Player
            {
                if (GameState.inputManager.scrollLeft(0))
                {
                    if (selections[1] != -2)
                        selections[0] = selections[1];
                    selections[1] = -2;
                    scroll_to_next_down(0);
                    playerToChange = 0;

                    if(!ready && !ready2)
                        GameState.soundBank.PlayCue("MenuChangeSelection");
                }
                
                if (GameState.inputManager.scrollRight(0))
                {
                    if (selections[0] != -2)
                        selections[1] = selections[0];
                    selections[0] = -2;
                    scroll_to_next_down(1);
                    playerToChange = 1;

                    if(!ready && !ready2)
                        GameState.soundBank.PlayCue("MenuChangeSelection");
                }
            }
            else //Multiplayer
            {
                if (!ready2)
                {
                    if (GameState.inputManager.scrollDown(1))
                    {
                        selections[1]++;
                        if (selections[1] == 9)
                            selections[1] = 0;

                        bool isMaxed = false;

                        do
                        {
                            isMaxed = check_if_maxed(selections[1], 1);
                            if (isMaxed)
                                selections[1]++;
                        } while (isMaxed == true);

                        GameState.soundBank.PlayCue("MenuChangeSelection");
                    }
                    if (GameState.inputManager.scrollUp(1))
                    {
                        selections[1]--;
                        if (selections[1] == -1)
                            selections[1] = 8;

                        bool isMaxed = false;

                        do
                        {
                            isMaxed = check_if_maxed(selections[1], 1);
                            if (isMaxed)
                                selections[1]--;
                            if (selections[1] <= -1)
                            {
                                selections[1] = 8;
                                isMaxed = false;
                            }
                        } while (isMaxed == true);

                        GameState.soundBank.PlayCue("MenuChangeSelection");
                    }
                }
            }

            if (GameState.inputManager.Accept(0))
            {
                if (selections[playerToChange] != 8)
                {
                    if (costs[selections[playerToChange]][GameState.objectManager.players.Get_Player_Level(playerToChange, selections[playerToChange])] <= GameState.objectManager.players.Get_Player_Cash(playerToChange) && costs[selections[playerToChange]][GameState.objectManager.players.Get_Player_Level(playerToChange, selections[playerToChange])] != 0)
                    {
                        GameState.objectManager.players.LevelUp(selections[playerToChange], playerToChange, costs[selections[playerToChange]][GameState.objectManager.players.Get_Player_Level(playerToChange, selections[playerToChange])]);
                        if(GameState.objectManager.players.Get_Player_Level(playerToChange, selections[playerToChange]) >= 7)
                        {
                            scroll_to_next_down(playerToChange);
                        }
                    }
                }
                else
                {
                    if (playerToChange == 0)
                    {
                        ready = true;
                        if (GameState.PlayerCount == 1 && !ready2)
                        {
                            selections[0] = -2;
                            selections[1] = 8;
                            playerToChange = 1;

                            GameState.soundBank.PlayCue("MenuChangeSelection");
                        }
                    }
                    else
                    {
                        ready2 = true;
                        if(GameState.PlayerCount == 1 && !ready)
                        {
                            selections[1] = -2;
                            selections[0] = 8;
                            playerToChange = 0;

                            GameState.soundBank.PlayCue("MenuChangeSelection");
                        }
                    }
                }
            }

            if(GameState.inputManager.Accept(1) && GameState.PlayerCount == 2 && !ready2)
            {
                if (selections[1] != 8)
                {
                    if (costs[selections[1]][GameState.objectManager.players.Get_Player_Level(1, selections[1])] <= GameState.objectManager.players.Get_Player_Cash(1))
                    {
                        GameState.objectManager.players.LevelUp(selections[1], 1, costs[selections[1]][GameState.objectManager.players.Get_Player_Level(1, selections[1])]);
                        if (GameState.objectManager.players.Get_Player_Level(1, selections[1]) >= 7)
                        {
                            scroll_to_next_down(1);
                        }
                    }
                }
                else
                {
                    ready2 = true;
                    selections[1] = -2;
                }
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            Check_Input();
            if(ready && ready2)
            {
                GameState.levelStart = false;
                GameState.currentLevel++;
                GameState.screenManager.Pop();
                if (GameState.screenManager.Top().ToString() == "Robopocalypse.Screens.MainMenuScreen")
                {
                    //GameState.screenManager.Push(new PlayScreen());
                }
                else
                {
                    GameState.cue = GameState.soundBank.GetCue("MusicTrack1");
                    GameState.cue.Play();
                }
                GameState.objectManager.LevelView.LoadLevel(GameState.currentLevel);
                GameState.objectManager.players.New_Level();
            }
        }

        public override void Draw(GameTime gameTime)
        {

            GameState.spriteBatch.Draw(backdrop, new Vector2(GameState.SCREEN_WIDTH / 2 - backdrop.Width / 2, 100), Color.White);
            //GameState.spriteBatch.DrawString(GameState.font, "Robopocalypse", new Vector2(512 - GameState.font.MeasureString("Robopocalypse").X / 2, 100), Color.White);

            //This first half is all for player 1 ------- Player 2 to follow
            if (!ready)
            {
                GameState.spriteBatch.DrawString(GameState.font, "Your Cash -- $" + GameState.objectManager.players.Get_Player_Cash(0), new Vector2(200, 180), Color.White);
                GameState.spriteBatch.DrawString(GameState.font, "Stat Boost ----------- Cost", new Vector2(180, 280), Color.White);

                for (int i = 0; i <= 8; i++)
                {
                    drawColor = Color.White;
                    if (selections[0] == i)
                        drawColor = Color.Red;
                    if (i < 8)
                    {
                        if (GameState.objectManager.players.Get_Player_Level(0, i) < 5)
                        {
                            GameState.spriteBatch.DrawString(GameState.font, upgrades[i], new Vector2(180, 300 + (20 * i)), drawColor);
                            GameState.spriteBatch.DrawString(GameState.font, "$" + costs[i][GameState.objectManager.players.Get_Player_Level(0, i)].ToString(), new Vector2(420, 300 + (20 * i)), drawColor);
                        }
                        else
                        {
                            GameState.spriteBatch.DrawString(GameState.font, "MAXED!!!", new Vector2(180, 300 + (20 * i)), drawColor);
                        }
                    }
                    else
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Done", new Vector2(180, 300 + (20 * i)), drawColor);
                    }
                }
            }

            //This half is for player 2
            if (!ready2)
            {
                GameState.spriteBatch.DrawString(GameState.font, "Your Cash -- $" + GameState.objectManager.players.Get_Player_Cash(1), new Vector2(600, 180), Color.White);
                GameState.spriteBatch.DrawString(GameState.font, "Stat Boost ----------- Cost", new Vector2(580, 280), Color.White);


                for (int i = 0; i <= 8; i++)
                {
                    drawColor = Color.White;
                    if (selections[1] == i)
                        drawColor = Color.Red;
                    if (i < 8)
                    {
                        if (GameState.objectManager.players.Get_Player_Level(1, i) < 5)
                        {
                            GameState.spriteBatch.DrawString(GameState.font, upgrades[i], new Vector2(580, 300 + (20 * i)), drawColor);
                            GameState.spriteBatch.DrawString(GameState.font, "$" + costs[i][GameState.objectManager.players.Get_Player_Level(1, i)].ToString(), new Vector2(820, 300 + (20 * i)), drawColor);
                        }
                        else
                        {
                            GameState.spriteBatch.DrawString(GameState.font, "MAXED!!!", new Vector2(580, 300 + (20 * i)), drawColor);
                        }
                    }
                    else
                    {
                        GameState.spriteBatch.DrawString(GameState.font, "Done", new Vector2(580, 300 + (20 * i)), drawColor);
                    }
                }
            }
        }
    }
}
