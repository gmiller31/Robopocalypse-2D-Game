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
    public class VictoryScreen : GameScreen
    {
        private int[] levelClearBonus;
        private int[] maxEnergyBonus;
        private int[] cashPickups;
        private int[] prevCash;

        private int viewStage;
        private double timeElapsed;
        private double timeToView;
        Texture2D backdrop;
        public VictoryScreen()
        {
            GameState.cue.Stop(AudioStopOptions.AsAuthored);

            backdrop = GameState.content.Load<Texture2D>(@"Textures/Menu/menu_levelup");
            levelClearBonus = new int[2];
            maxEnergyBonus = new int[2];
            cashPickups = new int[2];
            prevCash = new int[2];

            levelClearBonus[0] = 1000;
            levelClearBonus[1] = 1000;

            maxEnergyBonus[0] = ((int)GameState.objectManager.players.Get_Player_MaxEnergyRatio(0) * 1000);
            maxEnergyBonus[1] = ((int)GameState.objectManager.players.Get_Player_MaxEnergyRatio(1) * 1000);

            cashPickups[0] = GameState.objectManager.players.Get_Player_CashPickedUp(0);
            cashPickups[1] = GameState.objectManager.players.Get_Player_CashPickedUp(1);

            prevCash[0] = GameState.objectManager.players.Get_Player_Cash(0);
            prevCash[1] = GameState.objectManager.players.Get_Player_Cash(1);

            viewStage = 0;
            timeElapsed = 0;
            timeToView = 1000;
        }

        public override void Update(GameTime gameTime)
        {

            if(viewStage < 5)
            {
                timeElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                if(viewStage == 0 || viewStage == 4)
                {
                    if(timeElapsed >= timeToView * 2)
                    {
                        viewStage++;
                        timeElapsed = 0;
                    }
                }
                else
                {
                    if(timeElapsed >= timeToView)
                    {
                        viewStage++;
                        timeElapsed = 0;
                    }
                }
            }

            if ((GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1)) && viewStage >= 5)
            {
                GameState.objectManager.players.Award_Player_Cash(0, levelClearBonus[0] + maxEnergyBonus[0] + cashPickups[0]);
                GameState.objectManager.players.Award_Player_Cash(1, levelClearBonus[1] + maxEnergyBonus[1] + cashPickups[1]);
                GameState.screenManager.Pop();

                if(GameState.currentLevel == 15)
                {
                    GameState.screenManager.Pop();

                    //TODO: Create a Win on final level
                    GameState.screenManager.Push(new FinalVictoryScreen());
                }
                else
                {
                    GameState.screenManager.Push(new LevelUpScreen());
                }                
            }
            if ((GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1)) && viewStage < 5)
            {
                viewStage++;
                timeElapsed = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameState.spriteBatch.Draw(backdrop, new Vector2(GameState.SCREEN_WIDTH / 2 - backdrop.Width / 2, 100), Color.White);
            //GameState.spriteBatch.DrawString(GameState.font, "Robopocalypse", new Vector2(512 - GameState.font.MeasureString("Robopocalypse").X / 2, 100), Color.White);

            GameState.spriteBatch.DrawString(GameState.font, "Level Clear Bonus", new Vector2(512 - GameState.font.MeasureString("Level Clear Bonus").X / 2, 280), Color.White);

            GameState.spriteBatch.DrawString(GameState.font, "Cash Pickups", new Vector2(512 - GameState.font.MeasureString("Cash Pickups").X / 2, 360), Color.White);

            GameState.spriteBatch.DrawString(GameState.font, "Max Energy Bonus", new Vector2(512 - GameState.font.MeasureString("Max Energy Bonus").X / 2, 440), Color.White);

            GameState.spriteBatch.DrawString(GameState.font, "Total Cash Bonus", new Vector2(512 - GameState.font.MeasureString("Total Cash Bonus").X / 2, 520), Color.White);

            GameState.spriteBatch.DrawString(GameState.font, "New Cash Total", new Vector2(512 - GameState.font.MeasureString("New Cash Total").X / 2, 600), Color.White);

            if (viewStage > 0)
            {
                GameState.spriteBatch.DrawString(GameState.font, "+$" + levelClearBonus[0].ToString(), new Vector2(180, 280), Color.White);
                GameState.spriteBatch.DrawString(GameState.font, "+$" + levelClearBonus[1].ToString(), new Vector2(780, 280), Color.White);
            }

            if (viewStage > 1)
            {
                GameState.spriteBatch.DrawString(GameState.font, "+$" + cashPickups[0].ToString(), new Vector2(180, 360), Color.White);
                GameState.spriteBatch.DrawString(GameState.font, "+$" + cashPickups[1].ToString(), new Vector2(780, 360), Color.White);
            }

            if (viewStage > 2)
            {
                GameState.spriteBatch.DrawString(GameState.font, "+$" + maxEnergyBonus[0].ToString(), new Vector2(180, 440), Color.White);
                GameState.spriteBatch.DrawString(GameState.font, "+$" + maxEnergyBonus[1].ToString(), new Vector2(780, 440), Color.White);
            }

            if (viewStage > 3)
            {
                GameState.spriteBatch.DrawString(GameState.font, "$" + (maxEnergyBonus[0] + cashPickups[0] + levelClearBonus[0]).ToString(), new Vector2(180, 520), Color.White);
                GameState.spriteBatch.DrawString(GameState.font, "$" + (maxEnergyBonus[1] + cashPickups[1] + levelClearBonus[1]).ToString(), new Vector2(780, 520), Color.White);
            }

            if (viewStage > 4)
            {
                GameState.spriteBatch.DrawString(GameState.font, "$" + (maxEnergyBonus[0] + cashPickups[0] + levelClearBonus[0] + prevCash[0]).ToString(), new Vector2(180, 600), Color.White);
                GameState.spriteBatch.DrawString(GameState.font, "$" + (maxEnergyBonus[1] + cashPickups[1] + levelClearBonus[1] + prevCash[1]).ToString(), new Vector2(780, 600), Color.White);
            }
        }
    }
}
