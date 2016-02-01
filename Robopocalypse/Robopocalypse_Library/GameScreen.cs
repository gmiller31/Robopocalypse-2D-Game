using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Robopocalypse_Library
{
    public abstract class GameScreen
    {
        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    }
}
