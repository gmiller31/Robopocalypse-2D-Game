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
    public class Terrain
    {
        private List<Line2D> lines;
        private float speed;
        private float pass;

        public Terrain()
        {
            lines = new List<Line2D>();
            speed = 1;

            //Temporary Terrain
            for ( int i = 1; i < 100; i++ )
            {
                if(i % 2 == 0)
                {
                    lines.Add(new Line2D(new Vector2(100 * i, 0), new Vector2(100 * i, 350), Color.Red));
                    lines.Add(new Line2D(new Vector2(100 * i, 350), new Vector2(100 * (i + 1), 350), Color.Red));
                    lines.Add(new Line2D(new Vector2(100 * (i + 1), 350), new Vector2(100 * (i + 1), 0), Color.Red));
                }
                else
                {
                    lines.Add(new Line2D(new Vector2(100 * i, 768), new Vector2(100 * i, 380), Color.Red));
                    lines.Add(new Line2D(new Vector2(100 * i, 380), new Vector2(100 * (i + 1), 380), Color.Red));
                    lines.Add(new Line2D(new Vector2(100 * (i + 1), 380), new Vector2(100 * (i + 1), 768), Color.Red));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            pass += speed;
        }//End Update

        public void Draw(GameTime gameTime)
        {
            foreach(Line2D line in lines)
            {
                GameState.primitiveBatch.AddVertex(line.StartPosition.X - pass, line.StartPosition.Y, line.Color);
                GameState.primitiveBatch.AddVertex(line.EndPosition.X - pass, line.EndPosition.Y, line.Color);
            }
        }//End Draw
    }
}
