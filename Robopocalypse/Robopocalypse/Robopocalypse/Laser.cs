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
    public class Laser
    {
        private float distance;
        public Vector2 startPoint;
        public Vector2 endPoint;
        private float timer;
        private float delay;
        private float delayTimer = 0.0f;
        private Texture2D myTexture;
        private int type; //0 = normal, 1 = sweeping

        private Vector2 target; //for the sweep

        public float lineAngle;
        private Vector2 origin;

        private int flipCounter = 0;
        private int currentSprite = 0;
        private int totalSprites = 8;
        private bool endDelay = false;

        private float xIncrement;
        private float yIncrement;

        private const int LINE_WIDTH = 8;

        private int counter;

        public Laser(Vector2 start, Vector2 end, float life)
        {
            startPoint = start;
            endPoint = end;
            timer = life;

            delay = 0;
            distance = 200.0f;
            origin = new Vector2(0, LINE_WIDTH / 2);

            target.X = endPoint.X;
            target.Y = endPoint.Y;

            type = 0;

            myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\TransparentBar_longwavy_boss");

        }

        public Laser(Vector2 start, Vector2 end, float life, float X, float Y)
        {
            startPoint = start;
            endPoint = end;
            timer = life;

            delay = 50;
            delayTimer = 0;
            counter = 0;

            distance = 100.0f;
            origin = new Vector2(0, LINE_WIDTH / 2);

            type = 1;

            myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\TransparentBar_longwavy_boss");

            xIncrement = X / 120.0f;
            yIncrement = Y / 120.0f;

        }

        public void Update(GameTime gameTime)
        {
            lineAngle = (float)(Math.PI * 1.5) - (float)Math.Atan2(startPoint.X - endPoint.X, startPoint.Y - endPoint.Y);
            distance = Vector2.Distance(startPoint, endPoint);

            if (type == 0) //normal
            {
                timer = timer - 1;
                if (timer <= 0)
                {
                    GameState.objectManager.RemoveLaser(this);

                }
            }
            else //sweeping
            {

                if (delayTimer >= delay && counter < 120)
                {
                    endPoint.X = endPoint.X + xIncrement;

                    endPoint.Y = endPoint.Y + yIncrement;

                    counter++;
                }

                if (counter == 120)
                {
                    if (endDelay == false)
                        delayTimer = 0;

                    endDelay = true;
                }

                if (endDelay == true)
                {
                    if (delayTimer >= delay)
                    {
                        GameState.objectManager.RemoveLaser(this);
                        counter = 0;
                    }
                }

            }

            if (startPoint.Y == endPoint.Y && startPoint.X < 0 && endPoint.X - startPoint.X > 800)
            {
                startPoint.X = 0; 
                distance = Vector2.Distance(startPoint, endPoint);
            }

            startPoint.X = startPoint.X - GameState.GameSpeed;
            endPoint.X = endPoint.X - GameState.GameSpeed;
            target.X = target.X - GameState.GameSpeed;

            delayTimer++;

            Update_Sprite(gameTime);

        }
        private void Update_Sprite(GameTime gameTime)
        {
            flipCounter += gameTime.ElapsedGameTime.Milliseconds;

            if (flipCounter > 8 && !GameState.gameOver)
            {

                currentSprite++;
                flipCounter = 0;
                if (currentSprite > totalSprites)
                    currentSprite = 0;
            }


        } //End Update_Sprite

        public void Draw(GameTime gameTime)
        {


            if(startPoint.X > 0 || endPoint.X > 0)
            GameState.spriteBatch.Draw(myTexture, new Rectangle((int)startPoint.X, (int)startPoint.Y, (int)distance, LINE_WIDTH), new Rectangle(80 * currentSprite, 0, (int)distance, LINE_WIDTH), Color.White, lineAngle, origin, SpriteEffects.None, 0);


        }
    }
}

