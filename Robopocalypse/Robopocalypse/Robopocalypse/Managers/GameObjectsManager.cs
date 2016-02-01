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

namespace Robopocalypse.Managers
{
    public class GameObjectsManager
    {
        //Object Lists
        public LinkedList<AnimatedSprite> SpriteList;
        public LinkedList<AnimatedSprite> BackgroundList;
        public LinkedList<Pickup> pickupsList;
        public LinkedList<Enemy> enemyList;
        public LinkedList<Zone> zoneList;
        public LinkedList<Enemy> deadEnemiesList;
        public LinkedList<Enemy> enemiesToAdd;
        public LinkedList<Explosion> explosionList;
        public LinkedList<Explosion> fizzledExplosionList;
        public LinkedList<StopPoint> stopPointList;
        public LinkedList<Enemy> bulletList;
        public LinkedList<Enemy> fizzledBulletList;
        public LinkedList<Laser> laserList;
        public LinkedList<Laser> deadLaserList;

        //Object Managers
        public Managers.PickupManager pickupManager;
        public Managers.PlayerPair players;
        public LevelViewer LevelView;

        public GameObjectsManager()
        {
            SpriteList = new LinkedList<AnimatedSprite>();
            BackgroundList = new LinkedList<AnimatedSprite>();
            pickupsList = new LinkedList<Pickup>();
            enemyList = new LinkedList<Enemy>();
            enemiesToAdd = new LinkedList<Enemy>();
            zoneList = new LinkedList<Zone>();
            deadEnemiesList = new LinkedList<Enemy>();
            explosionList = new LinkedList<Explosion>();
            stopPointList = new LinkedList<StopPoint>();
            fizzledExplosionList = new LinkedList<Explosion>();
            bulletList = new LinkedList<Enemy>();
            fizzledBulletList = new LinkedList<Enemy>();
            laserList = new LinkedList<Laser>();
            deadLaserList = new LinkedList<Laser>();
        }

        public void LoadManagers()
        {
            pickupManager = new Managers.PickupManager();
            players = new Managers.PlayerPair();
            LevelView = new LevelViewer();
        }

        public Boolean AddEnemy(Enemy enemy)
        {
            Boolean result = true;

            enemiesToAdd.AddLast(enemy);

            return result;
        }

        public Boolean AddBullet(Enemy enemy)
        {
            Boolean result = true;

            bulletList.AddLast(enemy);

            return result;
        }

        public Boolean AddDeadEnemy(Enemy enemy)
        {
            Boolean result = true;

            deadEnemiesList.AddLast(enemy);

            return result;
        }

        public Boolean AddStopPoint(int distance)
        {
            Boolean result = true;
            stopPointList.AddLast(new StopPoint(distance));
            return result;
        }

        public Boolean AddFizzledExplosion(Explosion explosion)
        {
            Boolean result = true;
            fizzledExplosionList.AddLast(explosion);
            return result;
        }

        public Boolean RemoveEnemy(Enemy enemy)
        {
            return enemyList.Remove(enemy);
        }

        public Boolean RemoveBullet(Enemy enemy)
        {
            fizzledBulletList.AddLast(enemy);

            return true;
        }

        public Boolean AddLaser(Laser laser)
        {
            Boolean result = true;

            laserList.AddLast(laser);

            return result;
        }

        public Boolean RemoveLaser(Laser laser)
        {
            deadLaserList.AddLast(laser);

            return true;
        }

        private void ClearBullets()
        {
            foreach (Enemy b in fizzledBulletList)
            {
                bulletList.Remove(b);

            }
        }

        private void ClearLasers()
        {
            foreach (Laser l in deadLaserList)
            {
                laserList.Remove(l);
            }
        }

        private void ClearDeadLists()
        {
            deadLaserList.Clear();
            fizzledBulletList.Clear();
        }

        public Boolean AddZone(Zone zone)
        {
            Boolean result = true;

            zoneList.AddLast(zone);

            return result;
        }

        public Boolean RemoveZone(Zone zone)
        {
            return zoneList.Remove(zone);
        }

        public Boolean AddPickup(Pickup pickup)
        {
            Boolean result = true;
            pickupManager.Add_Pickup(pickup);
            return result;
        }

        public Boolean AddExplosion(Explosion explo)
        {
            Boolean result = true;
            explosionList.AddLast(explo);
            return result;
        }

        private void Remove_Enemies_Explosions()
        {
            foreach (Enemy e in deadEnemiesList)
            {
                enemyList.Remove(e);
                explosionList.AddLast(new Explosion(e.Type(),e.myPos));
            }
            foreach (Explosion e in fizzledExplosionList)
            {
                explosionList.Remove(e);
            }
            deadEnemiesList.Clear();
            fizzledExplosionList.Clear();
        }

        public void Reset()
        {
            SpriteList.Clear();
            BackgroundList.Clear();
            enemyList.Clear();
            pickupsList.Clear();
            zoneList.Clear();
            deadEnemiesList.Clear();
            explosionList.Clear();
            stopPointList.Clear();
            bulletList.Clear();
            laserList.Clear();
            enemiesToAdd.Clear();
            fizzledExplosionList.Clear();
            fizzledBulletList.Clear();
            deadLaserList.Clear();
            GameState.DistanceTraversed = 0;
            GameState.GameSpeed = 0;
            GameState.levelLoadTimer = 0;
            GameState.LevelText = GameState.DefaultLevelText;

            //Trying to fix the memory leak
            GC.Collect();
        }

        public void Update(GameTime gameTime)
        {
            if (GameState.freezeTime > 0)
            {
                GameState.frostexplosions.Update(gameTime);
            }

            foreach (Zone z in zoneList)
            {
                z.Update(gameTime);
            }

            LevelView.Update(gameTime);

            pickupManager.Update(gameTime);

            players.Update(gameTime);

            foreach (Enemy e in enemyList)
            {
                e.Update(gameTime);
            }
            foreach(Explosion e in explosionList)
            {
                e.Update(gameTime);
            }
            foreach(StopPoint p in stopPointList)
            {
                p.Update(gameTime);
            }
            foreach(Enemy b in bulletList)
            {
                b.Update(gameTime);
            }
            foreach(Enemy e in enemiesToAdd)
            {
                enemyList.AddLast(e);
            }
            foreach (Laser l in laserList)
            {
                l.Update(gameTime);
            }
            enemiesToAdd.Clear();

            Remove_Enemies_Explosions();
            ClearBullets();
            ClearLasers();
            ClearDeadLists();
        }

        public void Draw(GameTime gameTime)
        {
            LevelView.Draw(gameTime);
            foreach (Zone z in zoneList)
            {
                z.Draw(gameTime);
            }

            pickupManager.Draw(gameTime);

            

            foreach (Enemy e in enemyList)
            {
                e.Draw(gameTime);
            }

           

            foreach (Enemy b in bulletList)
            {
                b.Draw(gameTime);
            }

            foreach (Laser l in laserList)
            {
                l.Draw(gameTime);
            }

            players.Draw(gameTime);

            //Moved explosions to be in front of enemies and players
            foreach (Explosion e in explosionList)
            {
                e.Draw(gameTime);
            }

            if (GameState.freezeTime > 0)
            {
                GameState.spriteBatch.Draw(GameState.frostsprite, Vector2.Zero, new Rectangle(0, 0, GameState.SCREEN_WIDTH, GameState.SCREEN_HEIGHT), Color.White);
            }
        }
    }
}
