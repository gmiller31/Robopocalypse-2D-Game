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
    //Basically this is a class that generates explosions across the screen, didn't want to throw random code into the GameObjectManager
    public class GlobalSpriteEffect
    {
        private string spriteEffect;
        private Random random;
        private int generationTimer;
        private int effectFrequency;


        public GlobalSpriteEffect(string effect,int frequency)
        {
            spriteEffect = effect;
            generationTimer = 0;
            effectFrequency = frequency;
            random = new Random();
        }

        
        public void Update(GameTime gameTime)
        {
            generationTimer += gameTime.ElapsedGameTime.Milliseconds;
            if(generationTimer>effectFrequency)
            {
                generationTimer = 0;
                GameState.objectManager.AddExplosion(new Explosion(spriteEffect, new Vector2(random.Next(0,GameState.SCREEN_WIDTH),random.Next(0,GameState.SCREEN_HEIGHT))));
                
            }
        }

    }
}
