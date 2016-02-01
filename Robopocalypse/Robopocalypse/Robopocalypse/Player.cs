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
    public class Player
    {
        public static Line2D line;
        private static float lineLength;
        private float speed;
        private float accel;
        private static bool isConnected;

        private double stickTiltX2;
        private double stickTiltX1;
        private double stickTiltY2;
        private double stickTiltY1;

        //These hold the four corners of the structures we're checking intersection against.
        private static Vector2 structUpperLeft;
        private static Vector2 structLowerLeft;
        private static Vector2 structUpperRight;
        private static Vector2 structLowerRight;

        //These are the distances for different line lengths/strengths
        private static float minDistance;
        private static float maxDistance;
        private static float strongDistance;
        private static float mediumDistance;

        //This holds the point of our drone (line end point)
        public Vector2 myPos;
        private Rectangle myRect;

        //This is just a Vector2 holding 16,16 to offset easily without making a new vector2
        private static Vector2 posOffSet = new Vector2(11, 11);
        private static Vector2 imageOffSet = new Vector2(-6, -4);

        //This is the sprite sheet for the spider drone
        private Texture2D droneTexture;

        //This is the texture and rectangles for the energy bars.
        private Texture2D energyTexture;
        private Texture2D energyBGTexture;
        private Rectangle[] energyBarPosition;
        private Rectangle[] energyBarBGPosition;

        //Number of the player. 0 == player 1, 1 == player 2
        private int playerNum;

        //Velocity for speed up / slow down.
        private Vector2 velocity;

        //These keep track of our invincibility information
        public bool isInvincible;
        public double invTime;
        public bool isBlinking;
        public double blinkTimer;
        public int blinkTimeLength;
        public bool invFromDamage = true; //This will make sure we're not giving penalties to those who got an invincibility pickup.

        public bool onVictoryTile;

        //baseAccel is the baseline for how quickly a player will accelerate and decelerate
        private float baseAccel;

        //maxSpeed is the baseline maximum speed a spider droid can move.
        private float maxSpeed;

        private Boolean speedChanged;

        //If you get hit by an untraversable enemy, you'll be able to walk through untraversable enemies until you are no longer touching an untraversable enemy
        private bool hitByUntraversable = false;

        //This is just an int to use for random things so a new variable doesn't need to be declared
        private static int tempInt;

        //These hold the current and max energy totals for the player.
        public float baseMaxEnergy;
        public float maxEnergy;
        public float currentEnergy;
        private float energyDrainSpeed;
        private float energyRechargeSpeed;

        //This holds the pickup we find if we get one
        private Pickup foundPickup;

        //These hold the level up data
        public int playerCash;
        public int cashForPartner;
        public int playerCashPickedUp;
        private float maxEnergyBonus;
        public int maxEnergyLevel; 
        private float energyDrainBonus;
        public int energyDrainLevel;
        private float energyRechargeBonus;
        public int energyRechargeLevel;
        private float accelBonus;
        public int accelLevel;
        private float speedBonus;
        public int speedLevel;
        private static float damageBonus = 0;
        public int damageLevel;
        public int healthPickupLevel;
        public int invTimeLevel;
        private int healthPickupBonus;
        private int invTimeBonus;

        //these will keep track of our animated sprites
        private int currentSprite;
        private int totalSprites;
        private double flipCounter;

        //current sprites for the energy bar
        private int currentSprite_bar=0;
        private int totalSprites_bar=6;
        private double flipCounter_bar=0;


        //Added sprite column to keep track of which place on the sprite sheet it needs to draw
        private int spritecolumn;

        //These are random things we can store for fun stats
        public float data_distanceTraveled;
        public float data_timeSpentInFront;
        public float data_timeSpentIdle;
        public float data_timeWalkingIntoWalls;
        public float data_energyDrained;
        public int data_pickupsGrabbed;
        public int data_crushDeaths;
        public int data_batteryDeaths;
        public int data_damageDeaths;

        public float data_distanceTraveledTotal;
        public float data_timeSpentInFrontTotal;
        public float data_timeSpentIdleTotal;
        public float data_timeWalkingIntoWallsTotal;
        public float data_energyDrainedTotal;
        public int data_pickupsGrabbedTotal;
        public int data_crushDeathsTotal;
        public int data_batteryDeathsTotal;
        public int data_damageDeathsTotal;
        public int[] data_deathsPerLevel;
        public int data_timeSpentSpeedingScreen;
        public int data_timesSpeedActivated;
        public int data_totalCashEarned;
        public int data_enemiesDefeated;

        //For the speed zones
        public int speedModifier; //- if slow, 0 if normal, + is boost


        //variables for sheild
        private int sheild_totalsprite;
        private int sheild_currentsprite;
        private Texture2D sheildSprite;

        public Player(int newPlayerNum)
        {
            //just initialziing the shiled here because;
            sheild_currentsprite = 0;
            sheild_totalsprite = 3;
            sheildSprite = GameState.content.Load<Texture2D>(@"Textures\Player\sheildBubble");

            //Base initialization
            line = new Line2D(new Vector2(150, 80), new Vector2(150, 180), Color.White);
            playerNum = newPlayerNum;
            myRect.Width = (int)posOffSet.X * 2;
            myRect.Height = (int)posOffSet.Y * 2;
            foundPickup = new Pickup(0, 'e', new Vector2(0, 0));
            energyBarBGPosition = new Rectangle[10];
            energyBarPosition = new Rectangle[10];

            //Speed and movement initialization
            speed = 1;
            baseAccel = 0.15f;
            maxSpeed = 2.0f;
            velocity = new Vector2(0, 0);
            speedChanged = false;

            onVictoryTile = false;

            //Invincibility initializations
            invTime = 0;
            isInvincible = false;
            blinkTimer = 0;
            isBlinking = false;
            blinkTimeLength = 20;

            //Sprites counts
            currentSprite = 0;
            totalSprites = 27;
            spritecolumn = 0;

            //Energy initializations
            baseMaxEnergy = 100;
            maxEnergy = 100;
            currentEnergy = maxEnergy;
            energyDrainSpeed = 16;
            energyRechargeSpeed = 0.5f;

            //Speed Zones
            speedModifier = 0;

            //Distance declarations
            minDistance = 65;
            strongDistance = 130;
            mediumDistance = 200;
            maxDistance = 320;

            data_deathsPerLevel = new int[16];
            for (int i = 0; i < 16; i++)
            {
                data_deathsPerLevel[i] = 0;
            }

            //Player Specific positions and textures.
            if (newPlayerNum == 0)
            {
                myPos = line.StartPosition;
                droneTexture = GameState.content.Load<Texture2D>(@"Textures\Player\spider_blue_trimmed_shrinked");
                energyTexture = GameState.content.Load<Texture2D>(@"Textures\Player\energyBar_sprite_small");
                energyBGTexture = GameState.content.Load<Texture2D>(@"Textures\Player\battery");
                energyBarPosition[0] = new Rectangle(44, GameState.SCREEN_HEIGHT - 120, 12, 80);
                energyBarBGPosition[0] = new Rectangle(40, GameState.SCREEN_HEIGHT - 130, 20, 100);
                for (int i = 0; i < 10; i++)
                {
                    energyBarPosition[i] = energyBarPosition[0];
                    energyBarBGPosition[i] = energyBarBGPosition[0];
                    energyBarPosition[i].X += 26 * i;
                    energyBarBGPosition[i].X += 26 * i;
                }
            }
            else
            {
                myPos = line.EndPosition;
                droneTexture = GameState.content.Load<Texture2D>(@"Textures\Player\spider_orange_trimmed_shrinked");
                energyTexture = GameState.content.Load<Texture2D>(@"Textures\Player\energybar_sprite_small2");
                energyBGTexture = GameState.content.Load<Texture2D>(@"Textures\Player\battery2");
                energyBarPosition[0] = new Rectangle(GameState.SCREEN_WIDTH - 52, GameState.SCREEN_HEIGHT - 120, 12, 80);
                energyBarBGPosition[0] = new Rectangle(GameState.SCREEN_WIDTH - 56, GameState.SCREEN_HEIGHT - 130, 20, 100);
                for (int i = 0; i < 10; i++)
                {
                    energyBarPosition[i] = energyBarPosition[0];
                    energyBarBGPosition[i] = energyBarBGPosition[0];
                    energyBarPosition[i].X -= 26 * i;
                    energyBarBGPosition[i].X -= 26 * i;
                }
            }

            Set_Rectangle();

            //Fun statistics initializations
            data_distanceTraveled = 0.0f;
            data_timeSpentInFront = 0.0f;
            data_timeSpentIdle = 0.0f;
            data_timeWalkingIntoWalls = 0.0f;
            data_energyDrained = 0.0f;
            data_pickupsGrabbed = 0;
            data_crushDeaths = 0;
            data_batteryDeaths = 0;
            data_damageDeaths = 0;

            //These are the level up bonuses
            playerCash = 0;
            cashForPartner = 0;
            playerCashPickedUp = 0;
            maxEnergyBonus = 0;
            maxEnergyLevel = 0;
            energyDrainBonus = 0;
            energyDrainLevel = 0;
            energyRechargeBonus = 0;
            energyRechargeLevel = 0;
            damageLevel = 0;
            accelBonus = 0;
            accelLevel = 0;
            speedBonus = 0;
            speedLevel = 0;
            invTimeLevel = 0;
            invTimeBonus = 0;
            healthPickupLevel = 0;
            healthPickupBonus = 0;
        }

        public float Get_posX()
        {
            return myPos.X;
        }
        public void Set_PlayerPosition(int x, int y)
        {
            myPos.X = x;
            myPos.Y = y;
        }
        public void Set_PlayerPosition(Vector2 newPos)
        {
            myPos = newPos;
        }
        public void LevelUp(int selection, int cost)
        {
            /* 0 = Acceleration
             * 1 = Max Speed
             * 2 = Max Energy
             * 3 = Recharge Speed
             * 4 = Energy Drain Speed
             */
            playerCash -= cost;
            switch (selection)
            {
                case 0:
                    accelLevel++;
                    accelBonus += 0.1f;
                    break;
                case 1:
                    speedLevel++;
                    speedBonus += 0.2f;
                    break;
                case 2:
                    maxEnergyLevel++;
                    maxEnergyBonus += 20;
                    break;
                case 3:
                    energyRechargeLevel++;
                    energyRechargeBonus += 0.25f;
                    break;
                case 4:
                    energyDrainLevel++;
                    energyDrainBonus -= 2;
                    break;
                case 5:
                    damageLevel++;
                    damageBonus += 0.5f;
                    break;
                case 6:
                    healthPickupLevel++;
                    healthPickupBonus += 20;
                    break;
                case 7:
                    invTimeLevel++;
                    invTimeBonus += 250;
                    break;
            }
            
        }

        private void Set_Rectangle()
        {
            myRect.X = (int)myPos.X - (int)(myRect.Width / 2);
            myRect.Y = (int)myPos.Y - (int)(myRect.Height / 2);
        }

        public void Reset_For_New_Level()
        {
            velocity = Vector2.Zero;
        }

        public void Move_Line()
        {

            myPos.X += velocity.X;
            myPos.Y += velocity.Y;
            Set_Rectangle();
            Check_Crushed();

            //Makes Players scroll with the screen
            myPos.X -= GameState.GameSpeed;

            if (playerNum == 0)
            {
                line.StartPosition = myPos;
            }
            else
            {
                line.EndPosition = myPos;
            }


        } //End Move_Line

        private void Update_Velocity()
        {
            //accel will be the speed in which we accelerate.
            accel = baseAccel + accelBonus;

            //Player 0's input.
            if (playerNum == 0 && currentEnergy > 0)
            {
                stickTiltX1 = GameState.inputManager.Horizontal(playerNum);
                if (stickTiltX1 != 0)
                {
                    if (velocity.X < 0)
                        accel *= 2.8f * Math.Abs((float)stickTiltX1);
                }
                else
                {
                    if (velocity.X > 0)
                    {
                        velocity.X -= speed * accel;
                        if (velocity.X < 0)
                            velocity.X = 0;
                    }
                    else if (velocity.X < 0)
                    {
                        velocity.X += speed * accel;
                        if (velocity.X > 0)
                            velocity.X = 0;
                    }
                }
                velocity.X += (float)stickTiltX1 * speed * accel;

                accel = baseAccel + accelBonus;

                stickTiltY1 = GameState.inputManager.Vertical(playerNum);

                if (stickTiltY1 != 0)
                {
                    if (velocity.Y < 0)
                        accel *= 1.8f * Math.Abs((float)stickTiltY1);
                }
                else
                {
                    if (velocity.Y > 0)
                    {
                        velocity.Y -= speed * accel;
                        if (velocity.Y < 0)
                            velocity.Y = 0;
                    }
                    else if (velocity.Y < 0)
                    {
                        velocity.Y += speed * accel;
                        if (velocity.Y > 0)
                            velocity.Y = 0;
                    }
                }
                velocity.Y += (float)stickTiltY1 * speed * accel;
            }

            //Player 1's input.
            else if (playerNum == 1 && currentEnergy > 0)
            {
                if (GameState.PlayerCount == 1) // Spider 2 Movement if Single Player
                {
                    stickTiltX2 = GameState.inputManager.HorizontalAlt(playerNum - 1);

                    if (stickTiltX2 != 0)
                    {
                        if (velocity.X < 0)
                            accel *= 2.8f * Math.Abs((float)stickTiltX2);
                    }
                    else
                    {
                        if (velocity.X > 0)
                        {
                            velocity.X -= speed * accel;
                            if (velocity.X < 0)
                                velocity.X = 0;
                        }
                        else if (velocity.X < 0)
                        {
                            velocity.X += speed * accel;
                            if (velocity.X > 0)
                                velocity.X = 0;
                        }
                    }
                    velocity.X += (float)stickTiltX2 * speed * accel;

                    accel = baseAccel + accelBonus;

                    stickTiltY2 = GameState.inputManager.VerticalAlt(playerNum - 1);

                    if (stickTiltY2 != 0)
                    {
                        if (velocity.Y < 0)
                            accel *= 1.8f * Math.Abs((float)stickTiltY2);
                    }
                    else
                    {
                        if (velocity.Y > 0)
                        {
                            velocity.Y -= speed * accel;
                            if (velocity.Y < 0)
                                velocity.Y = 0;
                        }
                        else if (velocity.Y < 0)
                        {
                            velocity.Y += speed * accel;
                            if (velocity.Y > 0)
                                velocity.Y = 0;
                        }
                    }
                    velocity.Y += (float)stickTiltY2 * speed * accel;
                }
                else // Spider 2 Movement if Multiplayer
                {
#if WINDOWS
                    stickTiltX2 = GameState.inputManager.Horizontal(playerNum);
                    if (stickTiltX2 != 0)
                    {
                        if (velocity.X < 0)
                            accel *= 2.8f * Math.Abs((float)stickTiltX2);
                    }
                    else
                    {
                        if (velocity.X > 0)
                        {
                            velocity.X -= speed * accel;
                            if (velocity.X < 0)
                                velocity.X = 0;
                        }
                        else if (velocity.X < 0)
                        {
                            velocity.X += speed * accel;
                            if (velocity.X > 0)
                                velocity.X = 0;
                        }
                    }
                    velocity.X += (float)stickTiltX2 * speed * accel;

                    accel = baseAccel + accelBonus;

                    stickTiltY2 = GameState.inputManager.Vertical(playerNum);

                    if (stickTiltY2 != 0)
                    {
                        if (velocity.Y < 0)
                            accel *= 1.8f * Math.Abs((float)stickTiltY2);
                    }
                    else
                    {
                        if (velocity.Y > 0)
                        {
                            velocity.Y -= speed * accel;
                            if (velocity.Y < 0)
                                velocity.Y = 0;
                        }
                        else if (velocity.Y < 0)
                        {
                            velocity.Y += speed * accel;
                            if (velocity.Y > 0)
                                velocity.Y = 0;
                        }
                    }
                    velocity.Y += (float)stickTiltY2 * speed * accel;
#endif

#if XBOX
                    stickTiltX2 = GameState.inputManager.Horizontal(playerNum);
                    if (stickTiltX2 != 0)
                    {
                        if (velocity.X < 0)
                            accel *= 2.8f * Math.Abs((float)stickTiltX2);
                    }
                    else
                    {
                        if (velocity.X > 0)
                        {
                            velocity.X -= speed * accel;
                            if (velocity.X < 0)
                                velocity.X = 0;
                        }
                        else if (velocity.X < 0)
                        {
                            velocity.X += speed * accel;
                            if (velocity.X > 0)
                                velocity.X = 0;
                        }
                    }
                    velocity.X += (float)stickTiltX2 * speed * accel;

                    accel = baseAccel + accelBonus;

                    stickTiltY2 = GameState.inputManager.Vertical(playerNum);

                    if (stickTiltY2 != 0)
                    {
                        if (velocity.Y < 0)
                            accel *= 1.8f * Math.Abs((float)stickTiltY2);
                    }
                    else
                    {
                        if (velocity.Y > 0)
                        {
                            velocity.Y -= speed * accel;
                            if (velocity.Y < 0)
                                velocity.Y = 0;
                        }
                        else if (velocity.Y < 0)
                        {
                            velocity.Y += speed * accel;
                            if (velocity.Y > 0)
                                velocity.Y = 0;
                        }
                    }
                    velocity.Y += (float)stickTiltY2 * speed * accel;
#endif
                }
            }


            accel = baseAccel + accelBonus;

            //Checking to see if either both up/down (or left/right) are held or neither are held.
            //If so, the spider will decelerate.
            //The conditions will also be met if the player is out of energy.
            if (currentEnergy <= 0)
            {
                if (velocity.X > 0)
                {
                    velocity.X -= speed * accel;
                    if (velocity.X < 0)
                        velocity.X = 0;
                }
                else if (velocity.X < 0)
                {
                    velocity.X += speed * accel;
                    if (velocity.X > 0)
                        velocity.X = 0;
                }
            }

            if (currentEnergy <= 0)
            {
                if (velocity.Y > 0)
                {
                    velocity.Y -= speed * accel;
                    if (velocity.Y < 0)
                        velocity.Y = 0;
                }
                else if (velocity.Y < 0)
                {
                    velocity.Y += speed * accel;
                    if (velocity.Y > 0)
                        velocity.Y = 0;
                }
            }

            //We're making sure we are not exceeding the robot's max speed.
            if (velocity.Y > maxSpeed + speedBonus)
                velocity.Y = maxSpeed + speedBonus;
            else if (velocity.Y < -maxSpeed - speedBonus)
                velocity.Y = -(maxSpeed + speedBonus);

            if (velocity.X * 0.8f > maxSpeed + speedBonus)
                velocity.X = (maxSpeed + speedBonus) / 0.8f;
            else if (velocity.X * 0.8f < -maxSpeed - speedBonus)
                velocity.X = -(maxSpeed + speedBonus) / 0.8f;

            float totalSpeed = Math.Abs(velocity.Y) + Math.Abs(velocity.X * 0.8f);
            if (totalSpeed > (maxSpeed + speedBonus) * 1.65f)
            {
                velocity.Y *= ((maxSpeed + speedBonus) * 1.65f) / totalSpeed;
                velocity.X *= ((maxSpeed + speedBonus) * 1.65f) / totalSpeed;
            }
        } //End Update_Velocity

        private void Update_Sprite(GameTime gameTime)
        {
            flipCounter += gameTime.ElapsedGameTime.Milliseconds;

            //switching sheild sprite
            if(flipCounter>28)
            {
                sheild_currentsprite++;
                if (sheild_currentsprite > sheild_totalsprite)
                {
                    sheild_currentsprite = 0;
                }
            }
            //Switched this over so it will play idle animation instead of no animation for not moving
            if (velocity.X == 0 && velocity.Y == 0 && !GameState.gameOver)
            {
                spritecolumn = 1;
                if (flipCounter > 28)
                {
                    currentSprite++;
                    flipCounter = 0;
                    if (currentSprite > 15)
                        currentSprite = 0;
                }
            }
            else if (flipCounter > 28 && !GameState.gameOver)
            {
                spritecolumn = 0;
                currentSprite++;
                flipCounter = 0;
                if (currentSprite > totalSprites)
                    currentSprite = 0;
            }
            else if(GameState.gameOver && flipCounter > 28 && (currentEnergy <= 0 || myPos.X < 0))
            {
                //Death animation loop
                if (GameState.deathtimelag == 0)
                {
                    currentSprite = 0;
                }
                GameState.deathtimelag++;
                spritecolumn = 2;
                currentSprite++;
                flipCounter = 0;
            }


            flipCounter_bar += gameTime.ElapsedGameTime.Milliseconds;
            if (flipCounter_bar > 42 && !GameState.gameOver)
            {
                
                currentSprite_bar++;
                flipCounter_bar = 0;
                if (currentSprite_bar > totalSprites_bar)
                    currentSprite_bar = 0;
            }
        } //End Update_Sprite

        public void New_Level()
        {
            maxEnergy = baseMaxEnergy + maxEnergyBonus;
            currentEnergy = maxEnergy;
            playerCashPickedUp = 0;
            cashForPartner = 0;
        }

        private static Boolean Intersects(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            return ((relativeCCW(x1, y1, x2, y2, x3, y3) *
                 relativeCCW(x1, y1, x2, y2, x4, y4) <= 0)
                && (relativeCCW(x3, y3, x4, y4, x1, y1) *
                    relativeCCW(x3, y3, x4, y4, x2, y2) <= 0));
        }

        public static int relativeCCW(double x1, double y1,
                      double x2, double y2,
                      double px, double py)
        {
            x2 -= x1;
            y2 -= y1;
            px -= x1;
            py -= y1;
            double ccw = px * y2 - py * x2;
            if (ccw == 0.0)
            {
                // The point is colinear, classify based on which side of
                // the segment the point falls on.  We can calculate a
                // relative value using the projection of px,py onto the
                // segment - a negative value indicates the point projects
                // outside of the segment in the direction of the particular
                // endpoint used as the origin for the projection.
                ccw = px * x2 + py * y2;
                if (ccw > 0.0)
                {
                    // Reverse the projection to be relative to the original x2,y2
                    // x2 and y2 are simply negated.
                    // px and py need to have (x2 - x1) or (y2 - y1) subtracted
                    //    from them (based on the original values)
                    // Since we really want to get a positive answer when the
                    //    point is "beyond (x2,y2)", then we want to calculate
                    //    the inverse anyway - thus we leave x2 & y2 negated.
                    px -= x2;
                    py -= y2;
                    ccw = px * x2 + py * y2;
                    if (ccw < 0.0)
                    {
                        ccw = 0.0;
                    }
                }
            }
            return (ccw < 0.0) ? -1 : ((ccw > 0.0) ? 1 : 0);
        }

        public static Boolean Intersects(Line2D l1, Line2D l2)
        {
            return Intersects(l1.StartPosition.X, l1.StartPosition.Y, l1.EndPosition.X, l1.EndPosition.Y, l2.StartPosition.X, l2.StartPosition.Y, l2.EndPosition.X, l2.EndPosition.Y);
        }

        private static void Damage_Enemy(Enemy enemy)
        {
            tempInt = (int)damageBonus + 10 + (int)((maxDistance - lineLength) / 24);

            if (GameState.objectManager.players.Get_isInvincible(0) && GameState.objectManager.players.Get_invFromDamage(0))
                tempInt /= 2;
            if (GameState.objectManager.players.Get_isInvincible(1) && GameState.objectManager.players.Get_invFromDamage(1))
                tempInt /= 2;
            if (lineLength > maxDistance)
                tempInt -= (int)damageBonus;
            if (tempInt < 0)
                tempInt = 0;

            enemy.Take_Damage(tempInt);
        }

        private static void Damage_Enemy(Enemy enemy, string impactPoints)
        {
            tempInt = (int)damageBonus + 10 + (int)((maxDistance - lineLength) / 24);

            if (GameState.objectManager.players.Get_isInvincible(0) && GameState.objectManager.players.Get_invFromDamage(0))
                tempInt /= 2;
            if (GameState.objectManager.players.Get_isInvincible(1) && GameState.objectManager.players.Get_invFromDamage(1))
                tempInt /= 2;
            if (lineLength > maxDistance)
                tempInt -= (int)damageBonus;
            if (tempInt < 0)
                tempInt = 0;

            enemy.Take_Damage(tempInt, impactPoints);
        }

        public static void Check_Line_Weapon()
        {
            foreach (Enemy enemy in GameState.objectManager.enemyList)
            {
                if (!enemy.isInvincible)
                {
                    // if it's even possible for our line to intersect the enemy
                    if (!((line.StartPosition.X < enemy.myPos.X && line.EndPosition.X < enemy.myPos.X) ||
                        (line.StartPosition.Y < enemy.myPos.Y && line.EndPosition.Y < enemy.myPos.Y) ||
                        (line.StartPosition.X > enemy.myPos.X + enemy.myRect.Width && line.EndPosition.X > enemy.myPos.X + enemy.myRect.Width) ||
                        (line.StartPosition.Y > enemy.myPos.Y + enemy.myRect.Height && line.EndPosition.Y > enemy.myPos.Y + enemy.myRect.Height)))
                    {
                        structUpperLeft.X = enemy.myPos.X;
                        structUpperLeft.Y = enemy.myPos.Y;
                        structUpperRight.X = enemy.myPos.X + enemy.myRect.Width;
                        structUpperRight.Y = enemy.myPos.Y;
                        structLowerLeft.X = enemy.myPos.X;
                        structLowerLeft.Y = enemy.myPos.Y + enemy.myRect.Height;
                        structLowerRight.X = enemy.myPos.X + enemy.myRect.Width;
                        structLowerRight.Y = enemy.myPos.Y + enemy.myRect.Height;

                        string tempString = "";
                        if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structUpperLeft.X, structUpperLeft.Y, structUpperRight.X, structUpperRight.Y))
                            tempString += "T";
                        if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structUpperRight.X, structUpperRight.Y, structLowerRight.X, structLowerRight.Y))
                            tempString += "R";
                        if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structLowerRight.X, structLowerRight.Y, structLowerLeft.X, structLowerLeft.Y))
                            tempString += "B";
                        if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structLowerLeft.X, structLowerLeft.Y, structUpperLeft.X, structUpperLeft.Y))
                            tempString += "L";
                        if (tempString != "")
                            Damage_Enemy(enemy, tempString);
                    }
                    if (enemy.SubEnemies != null) //Adding this here to account for subenemies
                        for (int i = 0; i < enemy.SubEnemies.Length; i++)
                        {
                            // if it's even possible for our line to intersect the structure
                            if (!((line.StartPosition.X < enemy.SubEnemies[i].myPos.X && line.EndPosition.X < enemy.SubEnemies[i].myPos.X) ||
                                (line.StartPosition.Y < enemy.SubEnemies[i].myPos.Y && line.EndPosition.Y < enemy.SubEnemies[i].myPos.Y) ||
                                (line.StartPosition.X > enemy.SubEnemies[i].myPos.X + enemy.SubEnemies[i].myRect.Width && line.EndPosition.X > enemy.SubEnemies[i].myPos.X + enemy.SubEnemies[i].myRect.Width) ||
                                (line.StartPosition.Y > enemy.SubEnemies[i].myPos.Y + enemy.SubEnemies[i].myRect.Height && line.EndPosition.Y > enemy.SubEnemies[i].myPos.Y + enemy.SubEnemies[i].myRect.Height)))
                            {
                                structUpperLeft.X = enemy.SubEnemies[i].myPos.X;
                                structUpperLeft.Y = enemy.SubEnemies[i].myPos.Y;
                                structUpperRight.X = enemy.SubEnemies[i].myPos.X + enemy.SubEnemies[i].myRect.Width;
                                structUpperRight.Y = enemy.SubEnemies[i].myPos.Y;
                                structLowerLeft.X = enemy.SubEnemies[i].myPos.X;
                                structLowerLeft.Y = enemy.SubEnemies[i].myPos.Y + enemy.SubEnemies[i].myRect.Height;
                                structLowerRight.X = enemy.SubEnemies[i].myPos.X + enemy.SubEnemies[i].myRect.Width;
                                structLowerRight.Y = enemy.SubEnemies[i].myPos.Y + enemy.SubEnemies[i].myRect.Height;
                                if (!enemy.SubEnemies[i].isDead)
                                {
                                    if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structUpperLeft.X, structUpperLeft.Y, structUpperRight.X, structUpperRight.Y))
                                        enemy.SubEnemies[i].Take_Damage((int)damageBonus + 10 + (int)((maxDistance - lineLength) / 24));
                                    else if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structUpperRight.X, structUpperRight.Y, structLowerRight.X, structLowerRight.Y))
                                        enemy.SubEnemies[i].Take_Damage((int)damageBonus + 10 + (int)((maxDistance - lineLength) / 24));
                                    else if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structLowerRight.X, structLowerRight.Y, structLowerLeft.X, structLowerLeft.Y))
                                        enemy.SubEnemies[i].Take_Damage((int)damageBonus + 10 + (int)((maxDistance - lineLength) / 24));
                                    else if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structLowerLeft.X, structLowerLeft.Y, structUpperLeft.X, structUpperLeft.Y))
                                        enemy.SubEnemies[i].Take_Damage((int)damageBonus + 10 + (int)((maxDistance - lineLength) / 24));
                                }
                            }
                        }
                    //End of test
                }
            }
        }

        public static bool Update_Line_Intersect()
        {
            isConnected = true;

            //Get our list of structures from... Level Maker? Not sure where it is currently******************************************
            foreach (AnimatedSprite structure in GameState.objectManager.SpriteList)
            {
                if (!structure.Lineable)
                {
                    // if it's even possible for our line to intersect the structure
                    if (!((line.StartPosition.X < structure.m_position.X && line.EndPosition.X < structure.m_position.X) ||
                        (line.StartPosition.Y < structure.m_position.Y && line.EndPosition.Y < structure.m_position.Y) ||
                        (line.StartPosition.X > structure.m_position.X + structure.m_width && line.EndPosition.X > structure.m_position.X + structure.m_width) ||
                        (line.StartPosition.Y > structure.m_position.Y + structure.m_height && line.EndPosition.Y > structure.m_position.Y + structure.m_height)))
                    {
                        structUpperLeft.X = structure.m_position.X;
                        structUpperLeft.Y = structure.m_position.Y;
                        structUpperRight.X = structure.m_position.X + structure.m_width;
                        structUpperRight.Y = structure.m_position.Y;
                        structLowerLeft.X = structure.m_position.X;
                        structLowerLeft.Y = structure.m_position.Y + structure.m_height;
                        structLowerRight.X = structure.m_position.X + structure.m_width;
                        structLowerRight.Y = structure.m_position.Y + structure.m_height;

                        if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structUpperLeft.X, structUpperLeft.Y, structUpperRight.X, structUpperRight.Y))
                            isConnected = false;
                        else if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structUpperRight.X, structUpperRight.Y, structLowerRight.X, structLowerRight.Y))
                            isConnected = false;
                        else if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structLowerRight.X, structLowerRight.Y, structLowerLeft.X, structLowerLeft.Y))
                            isConnected = false;
                        else if (Intersects(line.StartPosition.X, line.StartPosition.Y, line.EndPosition.X, line.EndPosition.Y, structLowerLeft.X, structLowerLeft.Y, structUpperLeft.X, structUpperLeft.Y))
                            isConnected = false;
                    }
                }
            }
            return isConnected;
        } //End Check_Line_Intersect

        private static void Update_Line_Length()
        {
            lineLength = Vector2.Distance(line.StartPosition, line.EndPosition);
        }

        public static void Update_Line_Color()
        {
            Update_Line_Length();

            if (lineLength < strongDistance)
                line.Color = Color.IndianRed;
            else if (lineLength < mediumDistance)
                line.Color = Color.DarkBlue;
            else if (lineLength < maxDistance)
                line.Color = Color.LightYellow;

        }

        private bool Check_Single_Collision(Rectangle blockadeRect)
        {
            bool collisionDetected = false;
            //If the current player is trying to walk into this structure along the X axis. (We'll check Y axis next)
            if (myPos.X + velocity.X + posOffSet.X > blockadeRect.X && myPos.X + velocity.X - posOffSet.X < blockadeRect.X + blockadeRect.Width)
            {
                if (myPos.Y + posOffSet.Y > blockadeRect.Y && myPos.Y - posOffSet.Y < blockadeRect.Y + blockadeRect.Height)
                {
                    if (velocity.X > 0)
                    {
                        if (myPos.X + velocity.X + posOffSet.X > blockadeRect.X)
                        {
                            velocity.X = 0;
                            myPos.X = -posOffSet.X + blockadeRect.X;
                        }
                    }
                    else if (velocity.X < 0)
                    {
                        if (myPos.X + velocity.X - posOffSet.X < blockadeRect.X + blockadeRect.Width)
                        {
                            velocity.X = 0;
                            myPos.X = blockadeRect.X + posOffSet.X + blockadeRect.Width;
                        }
                    }
                    collisionDetected = true;
                }
            }

            //In this if statement we'll be checking the y axis collision
            if (myPos.X + posOffSet.X > blockadeRect.X && myPos.X - posOffSet.X < blockadeRect.X + blockadeRect.Width)
            {
                if (myPos.Y + velocity.Y + posOffSet.Y > blockadeRect.Y && myPos.Y + velocity.Y - posOffSet.Y < blockadeRect.Y + blockadeRect.Height)
                {
                    if (velocity.Y > 0)
                    {
                        if (myPos.Y + velocity.Y + posOffSet.Y > blockadeRect.Y)
                        {
                            velocity.Y = 0;
                            myPos.Y = -posOffSet.Y + blockadeRect.Y;
                        }
                    }
                    else if (velocity.Y < 0)
                    {
                        if (myPos.Y + velocity.Y - posOffSet.Y < blockadeRect.Y + blockadeRect.Height)
                        {
                            velocity.Y = 0;
                            myPos.Y = blockadeRect.Y + posOffSet.Y + blockadeRect.Height;
                        }
                    }
                    collisionDetected = true;
                }
            }
            return collisionDetected;
        }

        private bool Check_Untraversable_Enemy_Collision(Rectangle blockadeRect)
        {
            bool collisionDetected = false;
            //If the current player is trying to walk into this structure along the X axis. (We'll check Y axis next)
            if (myPos.X + velocity.X + posOffSet.X > blockadeRect.X && myPos.X + velocity.X - posOffSet.X < blockadeRect.X + blockadeRect.Width)
            {
                if (myPos.Y + posOffSet.Y > blockadeRect.Y && myPos.Y - posOffSet.Y < blockadeRect.Y + blockadeRect.Height)
                {
                    
                    collisionDetected = true;
                }
            }

            //In this if statement we'll be checking the y axis collision
            if (myPos.X + posOffSet.X > blockadeRect.X && myPos.X - posOffSet.X < blockadeRect.X + blockadeRect.Width)
            {
                if (myPos.Y + velocity.Y + posOffSet.Y > blockadeRect.Y && myPos.Y + velocity.Y - posOffSet.Y < blockadeRect.Y + blockadeRect.Height)
                {
                    
                    collisionDetected = true;
                }
            }

            if (!isInvincible)
                Take_Damage();

            return collisionDetected;
        }

        private void Check_Collisions()
        {
#if XBOX
            if (velocity.X + myPos.X > GameState.SCREEN_WIDTH - GameState.TITLE_SAFE_AREA)
                velocity.X = GameState.SCREEN_WIDTH - myPos.X - GameState.TITLE_SAFE_AREA;
            if (velocity.X + myPos.X < GameState.TITLE_SAFE_AREA)
                velocity.X = -myPos.X + GameState.TITLE_SAFE_AREA;
            if (velocity.Y + myPos.Y > GameState.SCREEN_HEIGHT - GameState.TITLE_SAFE_AREA)
                velocity.Y = GameState.SCREEN_HEIGHT - myPos.Y -GameState.TITLE_SAFE_AREA;
            if (velocity.Y + myPos.Y < GameState.TITLE_SAFE_AREA)
                velocity.Y = -myPos.Y + GameState.TITLE_SAFE_AREA;
#endif
#if WINDOWS
            if (velocity.X + myPos.X > GameState.SCREEN_WIDTH)
                velocity.X = GameState.SCREEN_WIDTH - myPos.X;
            if (velocity.X + myPos.X < 0)
                velocity.X = -myPos.X;
            if (velocity.Y + myPos.Y > GameState.SCREEN_HEIGHT)
                velocity.Y = GameState.SCREEN_HEIGHT - myPos.Y;
            if (velocity.Y + myPos.Y < 0)
                velocity.Y = -myPos.Y;
#endif
            Rectangle checkRect = new Rectangle();
            //Get our list of structures
            foreach (AnimatedSprite structure in GameState.objectManager.SpriteList)
            {
                if (structure.m_position.X > -400 && structure.m_position.X < GameState.SCREEN_WIDTH + 10)
                {
                    checkRect.X = (int)structure.m_position.X;
                    checkRect.Y = (int)structure.m_position.Y;
                    checkRect.Width = structure.m_width;
                    checkRect.Height = structure.m_height;
                    Check_Single_Collision(checkRect);
                }
            }
            bool becomeTraversable = true;
            foreach (Enemy enemy in GameState.objectManager.enemyList)
            {
                if (!enemy.isTraversable)
                {
                    if (!hitByUntraversable)
                    {
                     //   if (Check_Single_Collision(enemy.myRect))
                       //     hitByUntraversable = true;
                        if (Check_Single_Collision(enemy.myRect))
                            becomeTraversable = false;
                    }
                    else
                    {
                        if (Check_Untraversable_Enemy_Collision(enemy.myRect))
                        {
                            becomeTraversable = false;
                        }
                    }
                    
                }
            }
            if (becomeTraversable)
                hitByUntraversable = false;
        } //End Check_Collisions

        private void Update_Energy(GameTime gameTime, bool isConnected)
        {
            if (isConnected && !GameState.gameOver)
            {
                currentEnergy += (float)((energyRechargeSpeed + energyRechargeBonus) * gameTime.ElapsedGameTime.Milliseconds / 200);
                if (currentEnergy > maxEnergy)
                    currentEnergy = maxEnergy;
            }
            else
            {
                currentEnergy -= (float)((energyDrainSpeed + energyDrainBonus) * gameTime.ElapsedGameTime.Milliseconds / 200);
                data_energyDrainedTotal -= (float)((energyDrainSpeed + energyDrainBonus) * gameTime.ElapsedGameTime.Milliseconds / 200);
                if (currentEnergy <= 0)
                {
                    currentEnergy = 0;
                }
            }
        }

        private void Check_Max_Distance()
        {
            if (playerNum == 0 && Vector2.Distance(myPos + velocity, line.EndPosition) > maxDistance)
            {
                /*
                while (Vector2.Distance(myPos + velocity, line.EndPosition) > maxDistance)
                {
                    velocity *= .98f;

                    if (Math.Abs(velocity.X) <= .0001f && Math.Abs(velocity.Y) <= .0001f)
                    {
                        //myPos += (line.EndPosition - myPos) * .01f;
                        velocity.X = 0;
                        velocity.Y = 0;
                        break;
                    }
                }*/
                velocity *= 0.7f;
            }
            else if (playerNum == 1 && Vector2.Distance(myPos + velocity, line.StartPosition) > maxDistance)/* && ((myPos + velocity) - line.StartPosition).Length() > (myPos - line.StartPosition).Length()*/
            {
                /*
                while (Vector2.Distance(myPos + velocity, line.StartPosition) > maxDistance)
                {
                    velocity *= .98f;

                    if (Math.Abs(velocity.X) <= .0001f && Math.Abs(velocity.Y) <= .0001f)
                    {
                        //myPos += (line.StartPosition - myPos) * .01f;
                        velocity.X = 0;
                        velocity.Y = 0;
                        break;
                    }
                }*/
                velocity *= 0.7f;
            }
        }

        private void Check_Min_Distance()
        {
            if(playerNum == 0)
            {
                while(Vector2.Distance(myPos + velocity, line.EndPosition) < minDistance)
                {
                    if (line.StartPosition == line.EndPosition)
                        myPos.Y += .01f;

                    velocity *= .96f;

                    if (Math.Abs(velocity.X) <= .00001f && Math.Abs(velocity.Y) <= .00001f)
                    {
                        velocity = (myPos - line.EndPosition) * .02f;

                        while(Vector2.Distance(myPos + velocity, line.EndPosition) < minDistance)
                        {
                            velocity *= 1.01f;
                        }
                    }
                }
            }
            else
            {
                while(Vector2.Distance(myPos + velocity, line.StartPosition) < minDistance)
                {
                    if (line.StartPosition == line.EndPosition)
                        myPos.Y -= .01f;

                    velocity *= .96f;

                    if(Math.Abs(velocity.X) <= .00001f && Math.Abs(velocity.Y) <= .00001f)
                    {
                        velocity = (myPos - line.StartPosition) * .02f;

                        while (Vector2.Distance(myPos + velocity, line.StartPosition) < minDistance)
                        {
                            velocity *= 1.01f;
                        }
                    }
                }
            }
        }

        public static void Game_Over()
        {
            if(!GameState.gameOver)
            {
                //GameState.currentLevel = 1;
                GameState.soundBank.PlayCue("Explosion");
            }
            GameState.gameOver = true;
            

            //moving these into the update loop for now so there is time for a death animation
            //GameState.screenManager.Pop();
            //GameState.screenManager.Push(new Screens.GameOverScreen());
        }

        private void Check_Crushed()
        {
            //if the player got crushed into the side of the screen
#if XBOX
            if (myPos.X < GameState.TITLE_SAFE_AREA)
            {
                if (!GameState.gameOver)
                {
                    data_crushDeaths++;
                    data_crushDeathsTotal++;
                    data_deathsPerLevel[GameState.currentLevel1]++;
                }
                maxEnergy = 0;
                currentEnergy = 0;
                Game_Over();
            }
#endif
#if WINDOWS
            if (myPos.X < 0)
            {
                if (!GameState.gameOver)
                {
                    data_crushDeaths++;
                    data_crushDeathsTotal++;
                    data_deathsPerLevel[GameState.currentLevel]++;
                }
                maxEnergy = 0;
                currentEnergy = 0;
                Game_Over();
            }
#endif
        }

        private void Check_Pickups()
        {
            if (GameState.objectManager.pickupManager.Check_Collisions(myRect))
            {
                foundPickup = GameState.objectManager.pickupManager.Collided_Pickup(myRect);
                if(foundPickup.effect == 'm')
                {
                    GameState.soundBank.PlayCue("Coin");
                    GameState.objectManager.AddExplosion(new Explosion("coin", new Vector2(myPos.X , myPos.Y)));
                    playerCashPickedUp += (int)((double)foundPickup.amount * 0.6);
                    data_totalCashEarned += (int)((double)foundPickup.amount * 0.6);
                    cashForPartner += (int)((double)foundPickup.amount * 0.4);
                    data_pickupsGrabbedTotal++;
                }
                else if(foundPickup.effect == 'e')
                {
                    GameState.soundBank.PlayCue("Heal");
                    GameState.objectManager.AddExplosion(new Explosion("health", new Vector2(myPos.X, myPos.Y)));
                    maxEnergy += 20 + healthPickupBonus;
                    currentEnergy += (float)(20 + healthPickupBonus) * 0.5f;
                    if (maxEnergy > baseMaxEnergy + maxEnergyBonus)
                        maxEnergy = baseMaxEnergy + maxEnergyBonus;
                    data_pickupsGrabbedTotal++;
                    if(currentEnergy > maxEnergy)
                    {
                        currentEnergy = maxEnergy;
                    }
                }
                else if(foundPickup.effect == 'i')
                {
                    GameState.soundBank.PlayCue("Invincibility");
                    GameState.objectManager.AddExplosion(new Explosion("invuln", new Vector2(myPos.X, myPos.Y)));
                    isInvincible = true;
                    invFromDamage = false;
                    blinkTimer = blinkTimeLength;
                    invTime = foundPickup.amount + invTimeBonus;
                    data_pickupsGrabbedTotal++;
                }
                else if(foundPickup.effect == 'f')
                {
                    GameState.soundBank.PlayCue("TimeStop");
                    GameState.objectManager.AddExplosion(new Explosion("freeze", new Vector2(myPos.X, myPos.Y)));
                    if(GameState.freezeTime <= 0)
                        GameState.prevGameSpeed = GameState.GameSpeed;
                    GameState.freezeTime = foundPickup.amount;
                    GameState.GameSpeed = 0;
                    data_pickupsGrabbedTotal++;
                }
            }
        }

        private void Take_Damage()
        {
            if (!isInvincible)
            {
                GameState.soundBank.PlayCue("Damage");
                isInvincible = true;
                blinkTimer = blinkTimeLength;
                invTime = 750 + invTimeBonus;
                invFromDamage = true;
                maxEnergy -= 20;
                if (currentEnergy > maxEnergy)
                    currentEnergy = maxEnergy;
            }
        }

        private void Take_Damage(int dmgAmount)
        {
            isInvincible = true;
            blinkTimer = blinkTimeLength;
            invTime = 750 + invTimeBonus;
            maxEnergy -= dmgAmount;
            if (currentEnergy > maxEnergy)
                currentEnergy = maxEnergy;
        }

        private void Check_Enemies()
        {
            if (!isInvincible)
            {
                foreach (Enemy enemy in GameState.objectManager.enemyList)
                {
                    if (enemy.isDangerous)
                    {
                        if (enemy.Check_Collision(myRect))
                        {
                            Take_Damage();
                            if (!enemy.isTraversable)
                                hitByUntraversable = true;
                            else if (enemy.myType == "Rubble")
                                hitByUntraversable = true;
                        }
                        if (enemy.SubEnemies != null) //Adding this here to account for subenemies
                            for (int i = 0; i < enemy.SubEnemies.Length; i++)
                            {
                                if(enemy.SubEnemies[i].Check_Collision(myRect))
                                    if(!enemy.SubEnemies[i].isDead)//added this in for now
                                        Take_Damage();
                            }
                    }
                }
                foreach (Enemy bullet in GameState.objectManager.bulletList)
                {
                    if (bullet.isDangerous)
                    {
                        if (bullet.Check_Collision(myRect))
                        {
                            Take_Damage();
                        }
                    }
                }
            }
        }

        private void Check_Zones()
        {
            speedChanged = false;
            onVictoryTile = false;

            foreach(Zone zone in GameState.objectManager.zoneList)
            {
                if(zone.Check_Collision(myRect))
                {
                    if(zone.type == "Spikes" && !isInvincible)
                    {
                        Take_Damage();
                    }

                    if(zone.type == "SpeedUp")
                    {
                        maxSpeed = 3.0f;
                        speedChanged = true;
                    }

                    if(zone.type == "SpeedDown")
                    {
                        maxSpeed = 1.0f;
                        speedChanged = true;
                    }

                    if(zone.type == "Victory")
                    {
                        onVictoryTile = true;
                    }
                }

            }


            if (speedChanged == false && maxSpeed != 2.0f) //how speedChanged works: assume speed hasn't changed. if speed changes due to speed zone, then don't reset to default speed,
            {                          //until they are no longer in a speed change zone
                maxSpeed = 2.0f;
            }
        }

        private void Update_Invincibility(GameTime gameTime)
        {
            invTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (invTime < 0)
            {
                invTime = 0;
                isInvincible = false;
                isBlinking = false;
            }
            if (invTime > 0) //If we were hit and are now in our period of invincibility...
            {
                blinkTimer -= gameTime.ElapsedGameTime.Milliseconds;
                if (blinkTimer <= 0)
                {
                    blinkTimer = 20;
                    isBlinking = !isBlinking;
                }
            }
        }

        public void Check_Laser_Collision() 
        {
            foreach (Laser laser in GameState.objectManager.laserList)
            {


                // if it's even possible for our line to intersect the structure
                if (!((laser.startPoint.X < myPos.X && laser.endPoint.X < myPos.X) ||
                    (laser.startPoint.Y < myPos.Y && laser.endPoint.Y < myPos.Y) ||
                    (laser.startPoint.X > myPos.X + myRect.Width && laser.endPoint.X > myPos.X + myRect.Width) ||
                    (laser.startPoint.Y > myPos.Y + myRect.Height && laser.endPoint.Y > myPos.Y + myRect.Height)))
                {
                    structUpperLeft.X = myPos.X;
                    structUpperLeft.Y = myPos.Y;
                    structUpperRight.X = myPos.X + myRect.Width;
                    structUpperRight.Y = myPos.Y;
                    structLowerLeft.X = myPos.X;
                    structLowerLeft.Y = myPos.Y + myRect.Height;
                    structLowerRight.X = myPos.X + myRect.Width;
                    structLowerRight.Y = myPos.Y + myRect.Height;

                    if (Intersects(laser.startPoint.X, laser.startPoint.Y, laser.endPoint.X, laser.endPoint.Y, structUpperLeft.X, structUpperLeft.Y, structUpperRight.X, structUpperRight.Y))
                        Take_Damage();
                    else if (Intersects(laser.startPoint.X, laser.startPoint.Y, laser.endPoint.X, laser.endPoint.Y, structUpperRight.X, structUpperRight.Y, structLowerRight.X, structLowerRight.Y))
                        Take_Damage();
                    else if (Intersects(laser.startPoint.X, laser.startPoint.Y, laser.endPoint.X, laser.endPoint.Y, structLowerRight.X, structLowerRight.Y, structLowerLeft.X, structLowerLeft.Y))
                        Take_Damage();
                    else if (Intersects(laser.startPoint.X, laser.startPoint.Y, laser.endPoint.X, laser.endPoint.Y, structLowerLeft.X, structLowerLeft.Y, structUpperLeft.X, structUpperLeft.Y))
                        Take_Damage();
                }

            }
        }

        public void Stat_Updates(GameTime gameTime)
        {
            if(velocity == Vector2.Zero)
                data_timeSpentIdleTotal += gameTime.ElapsedGameTime.Milliseconds;
            else
                data_distanceTraveledTotal += velocity.Length();
            if(myPos.X > GameState.objectManager.players.players[0].myPos.X)
                data_timeSpentInFrontTotal += gameTime.ElapsedGameTime.Milliseconds;
            else if(myPos.X > GameState.objectManager.players.players[1].myPos.X)
                data_timeSpentInFrontTotal += gameTime.ElapsedGameTime.Milliseconds;
            if (GameState.GameSpeed == 2)
                data_timeSpentSpeedingScreen += gameTime.ElapsedGameTime.Milliseconds;
        }

        public void Update(GameTime gameTime, bool isConnected)
        {
            if (GameState.gameOver)
            {
                //checks how many frames have passed since death
                if (GameState.deathtimelag > 14)
                {
                    //GameState.screenManager.Pop();
                    GameState.screenManager.Push(new Screens.GameOverScreen());
                }
            }

            Update_Velocity();
            Stat_Updates(gameTime);
            Check_Min_Distance();
            Check_Max_Distance();
            Check_Collisions();
            Update_Sprite(gameTime);
            if (isInvincible)
                Update_Invincibility(gameTime);
            Check_Pickups();
            Check_Enemies();
            Check_Zones();
            Update_Energy(gameTime, isConnected);

            if (!isInvincible) //if you can take damage, check laser collision
                Check_Laser_Collision();

        }//End Update

        public void Draw(GameTime gameTime)
        {
            if (!isBlinking)
                GameState.spriteBatch.Draw(droneTexture, myPos - posOffSet + imageOffSet, new Rectangle(currentSprite * 32, spritecolumn*32, 32, 32), Color.White);
            for(int i = 0; i < 10; i++)
            {
                if(i * 20 < maxEnergy) //This will make it so we only show the batteries in which the player has access to.
                    GameState.spriteBatch.Draw(energyBGTexture, energyBarBGPosition[i], Color.White);
                else if(i * 20 < baseMaxEnergy + maxEnergyBonus)
                    GameState.spriteBatch.Draw(energyBGTexture, energyBarBGPosition[i], Color.Red);
                if ((i + 1) * 20 <= currentEnergy) //This shows the inner energy bar
                    //GameState.spriteBatch.Draw(energyTexture, energyBarPosition[i], Color.White);

                    //Drawing animated energy bar
                    GameState.spriteBatch.Draw(energyTexture,
                        new Rectangle(energyBarPosition[i].X, energyBarPosition[i].Y+5, 12, 80),
                        new Rectangle(currentSprite_bar * 12, 0, 12, 100),
                        Color.White);
                else if (i * 20 <= currentEnergy && currentEnergy != maxEnergy) //This is for an energy bar that is not full. 
                {
                    GameState.spriteBatch.Draw(energyTexture,
                        new Rectangle(energyBarPosition[i].X, energyBarPosition[i].Y  +5+(20 - (int)((float)currentEnergy % 20)) * 4, 12, (int)((float)currentEnergy % 20) * 4),
                        new Rectangle(currentSprite_bar*12, 0, 12, (int)((float)currentEnergy % 20) * 4),
                        Color.White);
                }
            }
            if (isInvincible && !invFromDamage)
            {
                GameState.spriteBatch.Draw(sheildSprite, myPos - posOffSet + imageOffSet, new Rectangle(sheild_currentsprite * 32, 0, 32, 32), Color.White);
            }
        }//End Draw
    }
}
