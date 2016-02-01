using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Robopocalypse
{
    public class Structure
    {
        private StreamReader reader;
        //private LinkedList<AnimatedSprite> list;
        int x = 0, y = 0, height = 0, width = 0, FinishHeight = 400, FinishWidth = 400, i = 0, j = 0;
        String type;

        String enemyType;
        String enemyDirection;
        String zoneType;

        String[] stringArray;

        public void LoadLevel(String FileName, String[] BuildingSprite)
        {
            String line = String.Empty;
            reader = new StreamReader(FileName);
            GameState.objectManager.Reset();

            do
            {
                line = reader.ReadLine();
                if (!line.StartsWith("//") || !line.StartsWith("/"))            //Ignores comments denoted by // or /
                {
                    type = line.Split(',')[0];

                    stringArray = line.Split(',');
                    
                    if (type.Equals("S"))                                       //reading the type additional types may be added for objectso ther than structures.
                    {
                        if (line.Split(',')[1] == "T")
                        {
                            x = int.Parse(line.Split(',')[2]);
                            y = int.Parse(line.Split(',')[3]);
                            height = int.Parse(line.Split(',')[4]);
                            width = int.Parse(line.Split(',')[5]);
                            for (i = 0; i < width; i += 32)
                                for (j = 0; j < height; j += 32)
                                    GameState.objectManager.SpriteList.AddLast(new AnimatedSprite(
                                        BuildingSprite[0], 1, 32, 32, 60, new Vector2(x + i, y + j)));
                        }
                        else
                        {
                            x = int.Parse(line.Split(',')[1]);
                            y = int.Parse(line.Split(',')[2]);
                            height = int.Parse(line.Split(',')[3]);
                            width = int.Parse(line.Split(',')[4]);
                            GameState.objectManager.SpriteList.AddLast(new AnimatedSprite(
                                BuildingSprite[0], 1, height, width, 60, new Vector2(x, y)));
                        }
                    }
                    else if(type.Equals("A"))
                    {
                        if (line.Split(',')[1] == "T")
                        {
                            x = int.Parse(line.Split(',')[2]);
                            y = int.Parse(line.Split(',')[3]);
                            height = int.Parse(line.Split(',')[4]);
                            width = int.Parse(line.Split(',')[5]);
                            for (i = 0; i < width; i += 32)
                                for (j = 0; j < height; j += 32)
                                    GameState.objectManager.SpriteList.AddLast(new AnimatedSprite(
                                        BuildingSprite[1], 1, 32, 32, 60, new Vector2(x + i, y + j), true));
                        }
                        else
                        {
                            x = int.Parse(line.Split(',')[1]);
                            y = int.Parse(line.Split(',')[2]);
                            height = int.Parse(line.Split(',')[3]);
                            width = int.Parse(line.Split(',')[4]);
                            GameState.objectManager.SpriteList.AddLast(new AnimatedSprite(
                                BuildingSprite[1], 1, height, width, 60, new Vector2(x, y), true));
                        }
                    }
                    else if (type.Equals("B"))
                    {
                        if (line.Split(',')[1] == "T")
                        {
                            x = int.Parse(line.Split(',')[2]);
                            y = int.Parse(line.Split(',')[3]);
                            height = int.Parse(line.Split(',')[4]);
                            width = int.Parse(line.Split(',')[5]);
                            for (i = 0; i < width; i += 32)
                                for (j = 0; j < height; j += 32)
                                    GameState.objectManager.BackgroundList.AddLast(new AnimatedSprite(
                                        BuildingSprite[2], 1, 32, 32, 60, new Vector2(x + i, y + j)));
                        }
                        else
                        {
                            x = int.Parse(line.Split(',')[1]);
                            y = int.Parse(line.Split(',')[2]);
                            height = int.Parse(line.Split(',')[3]);
                            width = int.Parse(line.Split(',')[4]);
                            GameState.objectManager.BackgroundList.AddLast(new AnimatedSprite(
                                BuildingSprite[2], 1, height, width, 60, new Vector2(x, y)));
                        }
                    }
                    else if(type.Equals("F"))
                    {
                        x = int.Parse(line.Split(',')[1]);
                        y = int.Parse(line.Split(',')[2]);


                        //adding finish zone instead of finsih block
                        GameState.objectManager.AddZone(new Zone(new Vector2(x, y), "Victory"));
                       // GameState.objectManager.BackgroundList.AddLast(new AnimatedSprite(
                        //    BuildingSprite[1], 1, FinishHeight, FinishWidth, 60, new Vector2(x, y)));

                    }
                    else if (type.Equals("E"))
                    {
                        enemyType = line.Split(',')[1];
                        enemyDirection = line.Split(',')[2];
                        x = int.Parse(line.Split(',')[3]);
                        y = int.Parse(line.Split(',')[4]);

                        if (enemyDirection == "Aimed") //if the enemy has the direction "aimed", it's an aimed turret, so use the overload
                            GameState.objectManager.AddEnemy(new Enemy(enemyType, enemyDirection, new Vector2(x, y), int.Parse(line.Split(',')[5])));
                        else if (enemyType == "Laser")
                        {
                            if (stringArray.Length == 9)
                                GameState.objectManager.AddEnemy(new Enemy(enemyType, enemyDirection, new Vector2(x, y),
                                                                 new Vector2(float.Parse(line.Split(',')[5]), float.Parse(line.Split(',')[6])), float.Parse(line.Split(',')[7]), float.Parse(line.Split(',')[8]), 0));
                            else
                            GameState.objectManager.AddEnemy(new Enemy(enemyType, enemyDirection, new Vector2(x, y),
                                 new Vector2(float.Parse(line.Split(',')[5]), float.Parse(line.Split(',')[6])), float.Parse(line.Split(',')[7]), float.Parse(line.Split(',')[8]), float.Parse(line.Split(',')[9])));
                        }
                        else
                            GameState.objectManager.AddEnemy(new Enemy(enemyType, enemyDirection, new Vector2(x, y)));
                    }
                    else if (type.Equals("Z"))
                    {
                        zoneType = line.Split(',')[1];
                        x = int.Parse(line.Split(',')[2]);
                        y = int.Parse(line.Split(',')[3]);
                        GameState.objectManager.AddZone(new Zone(new Vector2(x, y), zoneType));
                    }
                    else if (type.Equals("P"))
                    {
                        zoneType = line.Split(',')[1];
                        int strength = int.Parse(line.Split(',')[2]);
                        x = int.Parse(line.Split(',')[3]);
                        y = int.Parse(line.Split(',')[4]);
                        GameState.objectManager.AddPickup(new Pickup(strength,zoneType[0],new Vector2(x, y)));
                    }
                    else if(type.Equals("T"))
                    {
                        GameState.LevelText = line.Split(',')[1].Split(';');
                    }
                    else if(type.Equals("H"))
                    {
                        GameState.objectManager.players.Set_Player_Positions(new Vector2(int.Parse(line.Split(',')[1]), int.Parse(line.Split(',')[2])), new Vector2(int.Parse(line.Split(',')[3]), int.Parse(line.Split(',')[4])));
                    }
                    else if(type.Equals("X"))
                    {
                        GameState.objectManager.AddStopPoint(int.Parse(line.Split(',')[1]));
                    }
                    else
                    {
                        GameState.objectManager.SpriteList.AddLast(new AnimatedSprite(
                            BuildingSprite[0], 1, height, width, 60, new Vector2(x, y)));
                    }
                }
            }
            while (!reader.EndOfStream);
            reader.Close();
            //return GameState.objectManager.SpriteList;
        }
    }
}
