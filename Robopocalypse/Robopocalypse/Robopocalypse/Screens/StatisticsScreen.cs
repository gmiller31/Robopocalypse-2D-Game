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
    class StatisticsScreen : GameScreen
    {
        private string[][] statTitles;

        Texture2D backdrop;
        
        public StatisticsScreen()
        {
            backdrop = GameState.content.Load<Texture2D>(@"Textures/Menu/menu_statistics");

            statTitles = new string[2][];
            statTitles[0] = new string[12];
            statTitles[1] = new string[12];
            for( int i = 0; i < 2; i++)
            {
                statTitles[i][0] = "Distance Traveled: ";
                statTitles[i][1] = "Time Spent Leading: ";
                statTitles[i][2] = "Time Spent Idle: ";
                statTitles[i][3] = "Number of Pickups Grabbed: ";
                //statTitles[i][4] = "Time Spent with Fast Screen: ";
                //statTitles[i][5] = "Times you activated fast screen: ";
                statTitles[i][4] = "Total Cash Earned: $";
                statTitles[i][5] = "Number of Enemies Defeated: ";
                statTitles[i][6] = "Total Deaths: ";
                statTitles[i][7] = "Crushed Deaths: ";
                statTitles[i][8] = "Energy Drained Deaths: ";
                statTitles[i][9] = "Damage Deaths: ";
                statTitles[i][10] = "First Death: ";
                statTitles[i][11] = "Most Deaths Occured on Level: ";
            }

            for(int i = 0; i < statTitles[0].Length; i++)
            {
                for (int j = 0; j < 2; j++)
                    statTitles[j][i] = GameState.objectManager.players.Get_Player_Stat(statTitles[j][i], j, i);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1) || GameState.inputManager.Start(0) || GameState.inputManager.Start(1))
            {
                GameState.screenManager.Pop();                
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameState.spriteBatch.Draw(backdrop, new Vector2(GameState.SCREEN_WIDTH / 2 - backdrop.Width / 2, 100), Color.White);

            for (int i = 0; i < statTitles[0].Length; i++)
            {
                GameState.spriteBatch.DrawString(GameState.font, statTitles[0][i], new Vector2(GameState.SCREEN_WIDTH / 4 + 30 - GameState.font.MeasureString(statTitles[0][i]).X / 2, (i * 30) + 200), Color.White);
            }

            for (int i = 0; i < statTitles[1].Length; i++)
            {
                GameState.spriteBatch.DrawString(GameState.font, statTitles[1][i], new Vector2(((GameState.SCREEN_WIDTH / 4) * 3) - 30 - GameState.font.MeasureString(statTitles[1][i]).X / 2, (i * 30) + 200), Color.White);
            }
        }
    }
}
