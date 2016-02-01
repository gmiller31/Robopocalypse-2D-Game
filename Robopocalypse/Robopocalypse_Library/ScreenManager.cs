using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Robopocalypse_Library;

namespace Robopocalypse_Library
{
    public class ScreenManager
    {
        private Stack<GameScreen> m_screens;

        public ScreenManager()
        {
            m_screens = new Stack<GameScreen>();
        }

        public int Count()
        {
            return m_screens.Count;
        }

        public void Push(GameScreen screen)
        {
            m_screens.Push(screen);
        }

        public void Pop()
        {
            m_screens.Pop();
        }

        public GameScreen Top()
        {
            
            return m_screens.Peek();
        }
    }
}
