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

namespace Robopocalypse.Managers
{
    public class PlayerPair
    {
        public int firstToDrain;
        public Player[] players;
        bool isConnected;
        public Vector2[] positions = {Vector2.Zero, Vector2.Zero};
        Texture2D lineTexture;
        Texture2D lineTexture_med;
        Texture2D lineTexture_short;

        float lineAngle;
        Vector2 origin;
        const int LINE_WIDTH = 8; // CHANGE THIS FOR NEW LINE DIMENSIONS


        //used to determine the line length outside of draw
        float distance;

        public PlayerPair()
        {
            distance = 0f;
            //switched out LineTexture for the new texture
            lineTexture = GameState.content.Load<Texture2D>(@"Textures\Player\TransparentBar_longwavy");
            lineTexture_med = GameState.content.Load<Texture2D>(@"Textures\Player\TransparentBar_wavyshortyellow");
            lineTexture_short = GameState.content.Load<Texture2D>(@"Textures\Player\TransparentBar_wavyshortred");
            origin = Vector2.Zero;
            origin.Y = LINE_WIDTH / 2;
            firstToDrain = -1;

            players = new Player[2];
            players[0] = new Player(0);
            players[1] = new Player(1);

            isConnected = true;

        }

        public void Set_Player_Positions(Vector2 pos1, Vector2 pos2)
        {
            players[0].Set_PlayerPosition(pos1);
            players[1].Set_PlayerPosition(pos2);
        }

        public Vector2[] Get_Player_Positions()
        {
            positions[0] = players[0].myPos;
            positions[1] = players[1].myPos;
            return positions;
        }
        public bool Get_isInvincible(int playerNum)
        {
            return players[playerNum].isInvincible;
        }
        public bool Get_invFromDamage(int playerNum)
        {
            return players[playerNum].invFromDamage;
        }
        public int Get_Player_Level(int playerNum, int selectionNum)
        {
            if (selectionNum == 0)
                return players[playerNum].accelLevel;
            else if (selectionNum == 1)
                return players[playerNum].speedLevel;
            else if (selectionNum == 2)
                return players[playerNum].maxEnergyLevel;
            else if (selectionNum == 3)
                return players[playerNum].energyRechargeLevel;
            else if (selectionNum == 4)
                return players[playerNum].energyDrainLevel;
            else if (selectionNum == 5)
                return players[playerNum].damageLevel;
            else if (selectionNum == 6)
                return players[playerNum].healthPickupLevel;
            else if (selectionNum == 7)
                return players[playerNum].invTimeLevel;

            return 0;
        }
        public string Get_Player_Stat(string title, int playerNum, int selectionNum)
        {
            switch (selectionNum)
            {
                case 0:
                    title += (players[playerNum].data_distanceTraveledTotal / 10).ToString() + " feet";
                    break;
                case 1:
                    title += (players[playerNum].data_timeSpentInFrontTotal / 1000).ToString() + " seconds";
                    break;
                case 2:
                    title += (players[playerNum].data_timeSpentIdleTotal / 1000).ToString() + " seconds";
                    break;
                case 3:
                    title += players[playerNum].data_pickupsGrabbedTotal.ToString() + " pickups";
                    break;
                //case 4:
                //    title += (players[playerNum].data_timeSpentSpeedingScreen / 1000).ToString() + " seconds";
                //    break;
                //case 5:
                //    title += players[playerNum].data_timesSpeedActivated.ToString();
                //    break;
                case 4:
                    title += players[playerNum].data_totalCashEarned.ToString();
                    break;
                case 5:
                    title += players[playerNum].data_enemiesDefeated.ToString();
                    break;
                case 6:
                    title += (players[playerNum].data_crushDeathsTotal + players[playerNum].data_batteryDeathsTotal + players[playerNum].data_damageDeathsTotal).ToString();
                    break;
                case 7:
                    title += players[playerNum].data_crushDeathsTotal.ToString();
                    break;
                case 8:
                    title += players[playerNum].data_batteryDeathsTotal.ToString();
                    break;
                case 9:
                    title += players[playerNum].data_damageDeathsTotal.ToString();
                    break;
                case 10:
                    for (int i = 0; i < players[playerNum].data_deathsPerLevel.Length; i++)
                    {
                        if (players[playerNum].data_deathsPerLevel[i] > 0)
                        {
                            title = "First death occured on level " + i.ToString();
                            return title;
                        }
                        title = "";
                    }
                    break;
                case 11:
                    int totalDeaths = 0;
                    int occuredOnLevel = -1;
                    for (int i = 0; i < players[playerNum].data_deathsPerLevel.Length; i++)
                    {
                        if(players[playerNum].data_deathsPerLevel[i] > totalDeaths)
                        {
                            occuredOnLevel = i;
                            totalDeaths = players[playerNum].data_deathsPerLevel[i];
                        }
                    }
                    if (occuredOnLevel > 0)
                    {
                        title = "Most deaths: ";
                        if(occuredOnLevel <= 3)
                            title += "Tutorial " + occuredOnLevel.ToString() + " with " + totalDeaths.ToString() + " death";
                        else
                            title += "Main Level " + (occuredOnLevel - 3).ToString() + " with " + totalDeaths.ToString() + " death";
                        if (totalDeaths > 1)
                            title += "s";
                    }
                    else
                        title = "";
                    break;
            }

            return title;
        }
        public int Get_Player_Level_Energy(int playerNum)
        {
            return players[playerNum].maxEnergyLevel;
        }
        public int Get_Player_Level_Accel(int playerNum)
        {
            return players[playerNum].accelLevel;
        }
        public int Get_Player_Level_Damage(int playerNum)
        {
            return players[playerNum].damageLevel;
        }
        public int Get_Player_Level_Drain(int playerNum)
        {
            return players[playerNum].energyDrainLevel;
        }
        public int Get_Player_Level_Recharge(int playerNum)
        {
            return players[playerNum].energyRechargeLevel;
        }
        public int Get_Player_Level_Speed(int playerNum)
        {
            return players[playerNum].speedLevel;
        }
        public int Get_Player_Cash(int playerNum)
        {
            return players[playerNum].playerCash;
        }
        public int Get_Player_CashPickedUp(int playerNum)
        {
            return players[playerNum].playerCashPickedUp;
        }
        public float Get_Player_MaxEnergyRatio(int playerNum)
        {
            return players[playerNum].maxEnergy / players[playerNum].baseMaxEnergy;
        }
        public void Award_Player_Cash(int playerNum, int reward)
        {
            players[playerNum].playerCash += reward;
            players[playerNum].data_totalCashEarned += reward;
        }

        public void LevelUp( int selection, int playerNum, int cost)
        {
            /* 0 = Acceleration
             * 1 = Max Speed
             * 2 = Max Energy
             * 3 = Recharge Speed
             * 4 = Energy Drain Speed
             */

            players[playerNum].LevelUp(selection, cost);
        }

        public void Victory()
        {
            players[0].playerCashPickedUp += players[1].cashForPartner;
            players[0].data_totalCashEarned += players[1].cashForPartner;
            players[1].playerCashPickedUp += players[0].cashForPartner;
            players[1].data_totalCashEarned += players[0].cashForPartner;
            players[0].Reset_For_New_Level();
            players[1].Reset_For_New_Level(); //Anything that needs to be reset after a level is finished should go in there
            GameState.screenManager.Push(new Screens.VictoryScreen());
        }

        public void Update(GameTime gameTime)
        {

            distance = Vector2.Distance(players[0].myPos, players[1].myPos);
            lineAngle = (float)(Math.PI * 1.5) - (float)Math.Atan2(players[0].myPos.X - players[1].myPos.X, players[0].myPos.Y - players[1].myPos.Y);

            players[0].Move_Line();
            players[1].Move_Line();

            Player.Update_Line_Color();
            players[0].Update(gameTime, isConnected);
            players[1].Update(gameTime, isConnected);
            isConnected = Player.Update_Line_Intersect();

            //added this in here to move the spritesheet
            Update_Sprite(gameTime);
            if (isConnected)
                Player.Check_Line_Weapon();


            if ((players[0].currentEnergy <= 0 && players[1].currentEnergy <= 0) || players[0].maxEnergy <= 0 || players[1].maxEnergy <= 0)
            {
                if(!GameState.gameOver)
                {
                    if(players[0].maxEnergy <= 0)
                    {
                        players[0].data_damageDeathsTotal++;
                        players[0].data_deathsPerLevel[GameState.currentLevel]++;
                    }
                    else if(players[0].currentEnergy <= 0)
                    {
                        players[0].data_batteryDeathsTotal++;
                        players[0].data_deathsPerLevel[GameState.currentLevel]++;
                    }
                    if(players[1].maxEnergy <= 0)
                    {
                        players[1].data_damageDeathsTotal++;
                        players[1].data_deathsPerLevel[GameState.currentLevel]++;
                    }
                    else if(players[1].currentEnergy <= 0)
                    {
                        players[1].data_batteryDeathsTotal++;
                        players[1].data_deathsPerLevel[GameState.currentLevel]++;
                    }

                }
                GameState.GameSpeed = 0;
                Player.Game_Over();
            }
            else if (players[0].currentEnergy <= 0 || players[1].currentEnergy <= 0)
            {
                if (players[0].currentEnergy <= 0)
                    firstToDrain = 0;
                else
                    firstToDrain = 1;
            }
            else
            {
                firstToDrain = -1;
            }

            
            if (players[0].onVictoryTile && players[1].onVictoryTile) 
            {
                Victory();
            }
            
        }

        public void New_Level()
        {
            players[0].New_Level();
            players[1].New_Level();

            distance = Vector2.Distance(players[0].myPos, players[1].myPos);
            lineAngle = (float)(Math.PI * 1.5) - (float)Math.Atan2(players[0].myPos.X - players[1].myPos.X, players[0].myPos.Y - players[1].myPos.Y);

            players[0].Move_Line();
            players[1].Move_Line();

            Player.Update_Line_Color();
        }


        //Just throwing these here for testing
        private int flipCounter = 0;
        private int currentSprite = 0;
        private int totalSprites = 8;

        private void Update_Sprite(GameTime gameTime)
        {
            flipCounter += gameTime.ElapsedGameTime.Milliseconds;

            //Switched this over so it will play idle animation instead of no animation for not moving
           if (flipCounter > 20 && !GameState.gameOver)
            {
               
                currentSprite++;
                flipCounter = 0;
                if (currentSprite > totalSprites)
                    currentSprite = 0;
            }
            

        } //End Update_Sprite



        public void Draw_Line(GameTime gameTime)
        {
           
            if (isConnected)
            {
                //This will cut from the right //LUKE added in 100*currentsprite to also move on the sprite sheet to that area // replaced Vector2.Distance(players[0].myPos, players[1].myPos with distance
                if (distance<100)
                    GameState.spriteBatch.Draw(lineTexture_short, new Rectangle((int)players[0].myPos.X + 2, (int)players[0].myPos.Y, (int)distance, LINE_WIDTH), new Rectangle(80*currentSprite, 0, (int)distance, LINE_WIDTH), Color.White, lineAngle, origin, SpriteEffects.None, 0);
                else if(distance<150)
                    GameState.spriteBatch.Draw(lineTexture_med, new Rectangle((int)players[0].myPos.X + 2, (int)players[0].myPos.Y, (int)distance, LINE_WIDTH), new Rectangle(100 * currentSprite, 0, (int)distance, LINE_WIDTH), Color.White, lineAngle, origin, SpriteEffects.None, 0);
                else
                    GameState.spriteBatch.Draw(lineTexture, new Rectangle((int)players[0].myPos.X + 2, (int)players[0].myPos.Y, (int)distance, LINE_WIDTH), new Rectangle(150 * currentSprite, 0, (int)distance, LINE_WIDTH), Color.White, lineAngle, origin, SpriteEffects.None, 0);
                //This will stretch the line
                //GameState.spriteBatch.Draw(lineTexture, new Rectangle((int)players[0].myPos.X + 2, (int)players[0].myPos.Y, (int)Vector2.Distance(players[0].myPos, players[1].myPos), LINE_WIDTH), null, Color.White, lineAngle, origin, SpriteEffects.None, 0);
            }
        }

        public void Draw(GameTime gameTime)
        {
            players[0].Draw(gameTime);
            players[1].Draw(gameTime);
        }
    }
}
