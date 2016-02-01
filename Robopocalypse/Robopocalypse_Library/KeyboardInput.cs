using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Robopocalypse_Library
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class KeyboardInput : Microsoft.Xna.Framework.GameComponent
    {
        KeyboardState kbPrev;
        KeyboardState kbCurr;

        public KeyboardInput(Game game) : base(game)
        {
            kbPrev = kbCurr = Keyboard.GetState();
        }   

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            kbPrev = kbCurr;
            kbCurr = Keyboard.GetState();

            base.Update(gameTime);
        }

        public Boolean isKeyPressed(Keys key)
        {
            return !kbPrev.IsKeyDown(key) && kbCurr.IsKeyDown(key);
        }

        public Boolean isKeyRealeased(Keys key)
        {
            return kbPrev.IsKeyDown(key) && !kbCurr.IsKeyDown(key);
        }

        public Boolean isKeyHeld(Keys key)
        {
            return kbPrev.IsKeyDown(key) && kbCurr.IsKeyDown(key);
        }

        public Keys nextKeyPressed()
        {
            foreach (Keys k in kbCurr.GetPressedKeys())
                if (isKeyPressed(k))
                    return k;

            return Keys.Escape;
        }
    }
}
