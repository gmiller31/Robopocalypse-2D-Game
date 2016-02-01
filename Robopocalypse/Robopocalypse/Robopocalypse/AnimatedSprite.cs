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

namespace Robopocalypse
{
    public class AnimatedSprite
    {
        private Texture2D m_sprite;
        private int m_cellCount;
        public int m_height;
        public int m_width;
        private int m_framerate;
        //private int pass = 0;

        private int m_currentCell;
        private Boolean isPlaying;
        private Boolean isLineable;

        private int m_framecount;
        private int m_count;
        public Vector2 m_position;

        Rectangle source;
        Rectangle dest;


        public AnimatedSprite(String SpriteFileName, int CellCount, int height, int width, int framerate, Vector2 Position)
        {
            m_sprite = GameState.content.Load<Texture2D>(SpriteFileName);

            if (CellCount > 0)
                m_cellCount = CellCount;

            if (height > 0)
                m_height = height;

            if (width > 0)
                m_width = width;

            if (framerate > 0)
                m_framerate = framerate;

            m_position = Position;
            m_currentCell = 0;
            isPlaying = false;
            isLineable = false;

            m_framecount = framerate / CellCount;
            m_count = 0;

            source = new Rectangle(m_currentCell * m_width, 0, m_width, m_height);
            dest = new Rectangle((int)m_position.X, (int)m_position.Y, m_width, m_height);
        }

        public AnimatedSprite(String SpriteFileName, int CellCount, int height, int width, int framerate, Vector2 Position, Boolean Lineable)
        {
            m_sprite = GameState.content.Load<Texture2D>(SpriteFileName);

            if (CellCount > 0)
                m_cellCount = CellCount;

            if (height > 0)
                m_height = height;

            if (width > 0)
                m_width = width;

            if (framerate > 0)
                m_framerate = framerate;

            m_position = Position;
            m_currentCell = 0;
            isPlaying = false;
            isLineable = Lineable ;

            m_framecount = framerate / CellCount;
            m_count = 0;

            source = new Rectangle(m_currentCell * m_width, 0, m_width, m_height);
            dest = new Rectangle((int)m_position.X, (int)m_position.Y, m_width, m_height);
        }

        public int get_height()
        {
            return m_height;
        }
        public int get_width()
        {
            return m_width;
        }
        public Vector2 get_position()
        {
            return m_position;
        }

        public Boolean Lineable
        {
            get { return isLineable; }
            set { isLineable = value; }

        }

        //public int Passing
        //{
        //    get { return pass; }
        //    set { pass = value; }

        //}

        public void Update()
        {
            if (isPlaying)
            {
                if ((m_count % m_framecount) == 0)
                {
                    if (m_currentCell != m_cellCount - 1)       //Sets the Cell each second its playing if not at end
                        m_currentCell++;
                    else
                        m_currentCell = 0;


                }
                m_count++;
                //pass++;
                m_position.X = m_position.X - GameState.GameSpeed;
            }
            //source = new Rectangle(m_currentCell * m_width, 0, m_width, m_height); 
            //dest = new Rectangle((int)m_position.X - pass, (int)m_position.Y, m_width, m_height);
            
            dest.X = (int)m_position.X;
        }

        public void Draw()      //subtracts the pass from destination then draws the objects.
        {
            GameState.spriteBatch.Draw(m_sprite, dest, source, Color.White, MathHelper.ToRadians(0), Vector2.Zero, SpriteEffects.None, 0);
        }

        public void Play()
        {
            isPlaying = true;
        }

        public void Pause()
        {
            isPlaying = false;
        }

        public void Stop()
        {
            isPlaying = false;
            m_currentCell = 0;
        }

    }
}
