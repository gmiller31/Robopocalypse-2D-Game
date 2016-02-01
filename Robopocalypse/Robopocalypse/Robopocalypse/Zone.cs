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

namespace Robopocalypse
{
    public class Zone
    {
        Vector2 pos;
        public String type;
        Texture2D texture;
        private Rectangle myRect;
        private Rectangle effectRect;

        private int currentSprite;
        private int totalSprites;
        private double flipCounter;

        public Zone()
        {


        }

        public Zone(Vector2 position, String zoneType)
        {
            //height = 32;
            //width = 32;
            type = zoneType;
            pos = position;
            myRect.X = (int)pos.X;
            myRect.Y = (int)pos.Y;
            myRect.Height = 32;
            myRect.Width = 32;
            effectRect.X = myRect.X + 4;
            effectRect.Y = myRect.Y + 4;
            effectRect.Height = myRect.Height - 8;
            effectRect.Width = myRect.Width - 8;

            switch (type)
            {
                case "SpeedUp":
                    texture = GameState.content.Load<Texture2D>(@"Textures\Environment\speedtile"); //might want to change the filepath, but it's a placeholder texture for now
                    totalSprites = 3; //one less than total? is this right?
                    break;
                case "Spikes":
                    texture = GameState.content.Load<Texture2D>(@"Textures\Environment\spike_hooked");
                    totalSprites = 0;//should be different if it's animated
                    break;
                case "SpeedDown":
                    texture = GameState.content.Load<Texture2D>(@"Textures\Environment\slowTile");
                    totalSprites = 19;
                    break;
                case "Victory":
                    texture = GameState.content.Load<Texture2D>(@"Textures\Environment\FloorTile_WinTile");
                    totalSprites = 0;//should be different if it's animated
                    break;
                default:
                    break;
            }
       
        }

        public bool Check_Collision(Rectangle playerRect)
        {
            return !(playerRect.Left >= effectRect.Right
                    || playerRect.Right <= effectRect.Left
                    || playerRect.Top >= effectRect.Bottom
                    || playerRect.Bottom <= effectRect.Top);
        }

        private void Update_Sprite(GameTime gameTime)
        {
            flipCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (flipCounter > 40)
            {
                currentSprite++;
                flipCounter = 0;
                if (currentSprite > totalSprites)
                    currentSprite = 0;
            }
        }



        public void Update(GameTime gameTime)
        {

            pos.X = pos.X - GameState.GameSpeed; //autoscrolling
            myRect.X = myRect.X - GameState.GameSpeed;
            effectRect.X -= GameState.GameSpeed;
            Update_Sprite(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            GameState.spriteBatch.Draw(texture, pos, new Rectangle(currentSprite * 32, 0, 32, 32), Color.White);
        }




    }
}
