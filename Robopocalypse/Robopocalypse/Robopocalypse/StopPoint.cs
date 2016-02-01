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
    public class StopPoint
    {
        int StopDistance;
        public StopPoint( int distance )
        {
            StopDistance = distance;
        }

        public void Update(GameTime gameTime)
        {
            if (StopDistance <= GameState.DistanceTraversed)
            {
                GameState.GameSpeed = 0;
                StopDistance = 1000000;
            }
        }
    }
}
