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
    public class LevelViewer
    {
        Vector2 CenterTextAdjust = Vector2.Zero;        //Currently not implemented
        int newlev = 1, prevlev = 1, LevelCount;
        String text;
        LinkedListNode<AnimatedSprite> Node;

        public LevelViewer()
        {
            
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(@"Content\Levels\");
            
            LevelCount = dir.GetFiles().Length;
            text = "Hit Spacebar or Start to view Level";

#if WINDOWS
            text = "Hit 1 & 2 to Change Levels";
#endif
#if XBOX
            text = "Hit Left or Right Shoulders to Change Levels";
#endif

            LoadLevel(newlev);

            //CenterTextAdjust.X = Center.X - (text.Count() * 5); //Adjusts the text to the center of the screen based on text size
            //CenterTextAdjust.Y = 0;
        }

        public void Reset()
        {
            newlev = 1;
            prevlev = 1;
            GameState.currentLevel = 1;
        }

        public void LoadLevel(int level)               //change level mid game runs LoadLevel
        {
            GameState.Structure.LoadLevel(@"Content\Levels\level" + level + ".txt", GameState.BuildingSprite);
            Node = GameState.objectManager.SpriteList.First;
            while (Node != GameState.objectManager.SpriteList.Last)
            {
                Node.Value.Play();
                Node = Node.Next;
            }
            Node.Value.Play();


            Node = GameState.objectManager.BackgroundList.First;
            while (Node != GameState.objectManager.BackgroundList.Last)
            {
                Node.Value.Play();
                Node = Node.Next;
            }
            if(Node != null)
                Node.Value.Play();

            //return GameState.objectManager.SpriteList;
        }

        public void Update(GameTime gameTime)
        {
            //Michael's Stuff
            Node = GameState.objectManager.SpriteList.First;
            while (Node != GameState.objectManager.SpriteList.Last)  //Sets nodes to play
            {
                Node.Value.Play();
                Node = Node.Next;
            }
            if(Node != null)
                Node.Value.Play();

            //Nodes for background
            Node = GameState.objectManager.BackgroundList.First;
            while (Node != GameState.objectManager.BackgroundList.Last)  //Sets nodes to play
            {
                Node.Value.Play();
                Node = Node.Next;
            }
            if(Node != null)
                Node.Value.Play();

            prevlev = newlev;

            Node = GameState.objectManager.SpriteList.First;
            while (Node != GameState.objectManager.SpriteList.Last)   //Updates the nodes if necessary.
            {
                Node.Value.Update();
                Node = Node.Next;
            }
            if(Node != null)
                Node.Value.Update();
            //background update
            Node = GameState.objectManager.BackgroundList.First;
            while (Node != GameState.objectManager.BackgroundList.Last)   //Updates the nodes if necessary.
            {
                Node.Value.Update();
                Node = Node.Next;
            }
            if(Node != null)
                Node.Value.Update();

            if (GameState.debug)
            {
                //Controls for Level Viewer
                if (GameState.inputManager.LeftShoulder(0) && prevlev != 1)
                {
                    newlev = GameState.currentLevel - 1;
                    GameState.currentLevel--;
                    GameState.levelStart = false;
                    GameState.objectManager.players.New_Level();
                }

                if (GameState.inputManager.RightShoulder(0) && prevlev != LevelCount)
                {
                    newlev = GameState.currentLevel + 1;
                    GameState.currentLevel++;
                    GameState.levelStart = false;
                    GameState.objectManager.players.New_Level();
                }

#if XBOX
            if (GameState.inputManager.LeftShoulder(1) && prevlev != 1)
            {
                newlev -= 1;
                GameState.currentLevel--;
                GameState.levelStart = false;
                GameState.objectManager.players.New_Level();
            }

            if (GameState.inputManager.RightShoulder(1) && prevlev != LevelCount)
            {
                newlev += 1;
                GameState.currentLevel++;
                GameState.levelStart = false;
                GameState.objectManager.players.New_Level();
            }
#endif
            }

            if (newlev != prevlev)          //runs loadlevel if previous level not equal to newlevel
            {
                LoadLevel(newlev);
            }
        }

        public void Draw(GameTime gameTime)
        {
            //Michael's Node
            

            Node = GameState.objectManager.BackgroundList.First;
            while (Node != GameState.objectManager.BackgroundList.Last)
            {
                Node.Value.Draw();
                Node = Node.Next;
            }
            if(Node != null)
                Node.Value.Draw();

            //switched order since for me drawing background first made more sense -- luke
            Node = GameState.objectManager.SpriteList.First;
            while (Node != GameState.objectManager.SpriteList.Last)
            {
                if (Node.Value.m_position.X > -352 && Node.Value.m_position.X < GameState.SCREEN_WIDTH + 10)
                    Node.Value.Draw();
                Node = Node.Next;
            }
            if(Node != null)
                Node.Value.Draw();

            //GameState.spriteBatch.DrawString(GameState.font, text, CenterTextAdjust, Color.White);

        }
    }
}
