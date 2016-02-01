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
    public class Explosion
    {

        int playTimes;
        Texture2D expTexture;
        int spriteCount;
        int currentSprite;
        int currentSpriteColumn;
        int columnCount;
        double flipCount;
        Vector2 myPos;
        bool finished;
        bool small;
        bool big64 = false;
        //Did this for money "eplosion" to give the option of movement to a effect
        bool movement;
        Vector2 movementrate;

        //just added this for now to draw the 96x96 tile for the explosion
        bool boss;
        public Explosion(Vector2 position)
        {

            currentSpriteColumn = 0;
            columnCount = 0;
            movementrate = Vector2.Zero;
            playTimes = 1;
            movement = false;
            small = false;
            currentSprite = 0;
            spriteCount = 7;
            finished = false;
            myPos = position;
            expTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\explosion");
            boss = false;
        }

        public Explosion(String enemy, Vector2 position)
        {
            currentSpriteColumn = 0;
            columnCount = 0;
            movementrate = Vector2.Zero;
            movement = false;
            playTimes = 1;
            small = false;
            currentSprite = 0;
            boss = false;
            finished = false;
            myPos =position;

            //checks boss type to set the explosion
            if(enemy.Equals("BossHoming"))
            {
                boss = true;
                spriteCount = 20;

                expTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\homing_boss_death_21"); 

            }
            else if (enemy.Equals("spark"))
            {
                spriteCount = 3;
                columnCount = 2;
                playTimes = 3;
                small = true;
                expTexture = GameState.content.Load<Texture2D>(@"Textures\Player\spark"); 
            }
            else if (enemy.Equals("coin"))
            {
                movement = true;
                movementrate = new Vector2(-.5f,-.5f);
                spriteCount = 5;
                playTimes = 2;
                expTexture = GameState.content.Load<Texture2D>(@"Textures\Powerup\moneypoof_big"); 
            }
            else if (enemy.Equals("health"))
            {
                playTimes = 4;
                spriteCount = 5;

                expTexture = GameState.content.Load<Texture2D>(@"Textures\Powerup\healthpoof");
            }
            else if (enemy.Equals("Rubble"))
            {
                playTimes = 2;
                spriteCount = 5;
               

                expTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\rockexplosion");
            }
            else if(enemy.Equals("invuln"))
            {
                playTimes = 6;
                spriteCount = 3;

                expTexture = GameState.content.Load<Texture2D>(@"Textures\Powerup\invinciblepoof");
            }
            else if (enemy.Equals("freeze"))
            {
                playTimes = 1;
                spriteCount = 5;
                movementrate = new Vector2(-1f, 1f);

                expTexture = GameState.content.Load<Texture2D>(@"Textures\Powerup\icestop_poof");
            }
            else if(enemy.Equals("Explosion2"))
            {
                spriteCount = 15;
                expTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\explosion2"); 
            }
            else if (enemy.Equals("Explosion3"))
            {
                spriteCount = 15;
                big64 = true;
                expTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\explosion3");
            }
            else if(enemy.Equals("purple"))
            {
                spriteCount = 7;
                expTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\explosion_purple"); 
            }
            else if(enemy.Equals("green"))
            {
                spriteCount = 7;
                expTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\explosion_green"); 
            }
            else if(enemy.Equals("Homing"))
            {
                spriteCount = 7;
                expTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\explosion_green"); 
            }
            else{
            spriteCount = 7;
            expTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\explosion");   
            }
        }

        public void Update(GameTime gameTime)
        {
            myPos.X -= GameState.GameSpeed;
            flipCount += gameTime.ElapsedGameTime.Milliseconds;

            if (flipCount > 28)
            {
                flipCount = 0;
                if (currentSprite < spriteCount)
                    currentSprite++;
                else
                {
                    //added this in just incase we want effects to go longer
                    if (playTimes == 1)
                    {
                        finished = true;
                        GameState.objectManager.AddFizzledExplosion(this);
                    }
                    else
                    {
                        if(currentSpriteColumn<columnCount)
                        {
                            currentSpriteColumn++;
                        }
                        else
                        {
                            currentSpriteColumn = 0;
                        }
                        currentSprite = 0;
                        playTimes--;

                    }
                }
            }
            if(movement)
            {
                myPos += movementrate;
            }
                
        }

        public void Draw(GameTime gameTime)
        {
            if(!finished)
                if (boss) //if it's a boss draw abigger explosion
                {
                    GameState.spriteBatch.Draw(expTexture, myPos, new Rectangle(currentSprite * 96, 0, 96, 96), Color.White);
                }
                else if(small) //for 16x16 textures to animate
                {
                    GameState.spriteBatch.Draw(expTexture, myPos, new Rectangle(currentSprite * 16, currentSpriteColumn*16, 16, 16), Color.White);
                }
                else if(big64)
                {
                    GameState.spriteBatch.Draw(expTexture, myPos, new Rectangle(currentSprite * 64, currentSpriteColumn * 64, 64, 64), Color.White);
                }
                else
                    GameState.spriteBatch.Draw(expTexture, myPos, new Rectangle(currentSprite * 32, currentSpriteColumn*32, 32, 32), Color.White);
        }
    }
}
