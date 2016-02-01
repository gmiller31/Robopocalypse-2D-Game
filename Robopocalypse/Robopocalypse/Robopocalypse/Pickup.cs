using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Robopocalypse_Library;
using Microsoft.Xna.Framework.Graphics;

namespace Robopocalypse
{
    public class Pickup
    {
        public int amount;
        public char effect;
        public Texture2D texture;
        public Rectangle myPos;


        //adding in the ability to animate
        private int currentSprite;
        private int totalSprites;
        private double flipCounter;
        private int flippat;


        public Pickup( int amt, char fx, Vector2 pos )
        {

            flipCounter = 0;

            amount = amt;
            effect = fx;
            myPos.X = (int)pos.X;
            myPos.Y = (int)pos.Y;
            myPos.Width = 32;
            myPos.Height = 32;

            if (effect == 'm' || effect == 'M')
            {
                effect = 'm';
                texture = GameState.content.Load<Texture2D>(@"Textures\Powerup\powerup_coin");
                totalSprites = 9;
                flippat = 120;
            }
            else if (effect == 'e' || effect == 'E')
            {
                effect = 'e';
                texture = GameState.content.Load<Texture2D>(@"Textures\Powerup\batterypickup");
                totalSprites = 5;
                flippat = 40;
            }
            else if (effect == 'i' || effect == 'I')
            {
                effect = 'i';
                texture = GameState.content.Load<Texture2D>(@"Textures\Powerup\powerup_invincible");
                totalSprites = 7;
                flippat = 40;
            }
            else if (effect == 'f' || effect == 'F')    //CHANGE THIS WHEN IMPLEMENTING FREEZE ITEMS.
            {
                effect = 'f';
                texture = GameState.content.Load<Texture2D>(@"Textures\Powerup\powerup_freeze");
                totalSprites = 7;
                flippat = 80;
            }
        }

        public bool Check_Collision(Rectangle playerRect)
        {
            return !(playerRect.Left > myPos.Right
                    || playerRect.Right < myPos.Left
                    || playerRect.Top > myPos.Bottom
                    || playerRect.Bottom < myPos.Top);
        }


        private void Update_Sprite(GameTime gameTime)
        {
            flipCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (flipCounter > flippat)
            {
                currentSprite++;
                flipCounter = 0;
                if (currentSprite > totalSprites)
                    currentSprite = 0;
            }
        }


        public void Update(GameTime gameTime)
        {
            myPos.X -= GameState.GameSpeed;
            Update_Sprite(gameTime);
        }
        public void Draw(GameTime gameTime)
        {
            GameState.spriteBatch.Draw(texture, myPos, new Rectangle(currentSprite * 32, 0, 32, 32), Color.White);
        }
    }
}
