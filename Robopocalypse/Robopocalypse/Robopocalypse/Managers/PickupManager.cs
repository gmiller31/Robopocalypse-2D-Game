using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Robopocalypse_Library;
using Microsoft.Xna.Framework.Graphics;

namespace Robopocalypse.Managers
{
    public class PickupManager
    {
        //LinkedList<Pickup> pickups;
        Pickup currentPickup;
        public PickupManager()
        {
            //currentPickup = new Pickup(0, 'e', new Vector2(0, 0));
            //GameState.objectManager.pickupsList = new LinkedList<Pickup>();
        }

        public void Add_Pickup(Pickup newPickup)
        {
            GameState.objectManager.pickupsList.AddFirst(newPickup);
        }

        public LinkedList<Pickup> Get_Pickups()
        {
            return GameState.objectManager.pickupsList;
        }

        public bool Check_Collisions(Rectangle playerRect)
        {
            foreach (Pickup p in GameState.objectManager.pickupsList)
            {
                if (p.Check_Collision(playerRect))
                    return true;
            }
            return false;
        }

        public void Clear_Pickups()
        {
            GameState.objectManager.pickupsList.Clear();
        }

        public Pickup Collided_Pickup(Rectangle playerRect)
        {
            foreach (Pickup p in GameState.objectManager.pickupsList)
            {
                if (p.Check_Collision(playerRect))
                    currentPickup = p;
            }
            GameState.objectManager.pickupsList.Remove(currentPickup);
            return currentPickup;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Pickup p in GameState.objectManager.pickupsList)
            {
                p.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (Pickup p in GameState.objectManager.pickupsList)
            {
                p.Draw(gameTime);
            }
        }
    }
}
