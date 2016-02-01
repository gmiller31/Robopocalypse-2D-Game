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
    public class Enemy
    {

        private string myDir; //direction: left/right/up/down/homing1/homing2/etc.
        public string myType;

        //adding this in to try getting boss explosion to work
        public String Type()
        {
            return myType;
        }

        public Vector2 myPos;
        private Vector2 nextPos;
        public bool isDead;
        public int deathTimer;
        private double bossTargetTimer;
        private bool fallingPlayed = false;

        //determined by the type
        private float speed;
        private int health;
        public Rectangle myRect;
        private Texture2D myTexture;
        private Texture2D secondaryTexture;
        private int secondaryCurrentSprite;
        
        //Variables specifically for jumper
        private double jumpStartTime;
        private double jumpEndTime;
        private float jumpY;

        //Variables specifically for Spawner
        private float Spawnertimer = 0; float Spawnertimer2 = 0;
        private int DronesCreated = 10;
        private Boolean Spawned = false, ResetTimerSpawner=false, TurretsPlaced=false;
        private Vector2 SpriteOffset = new Vector2(90, 112);
        int randomAttack2;
        Vector2 MaxDistance = new Vector2(50,40);

        //these will keep track of our animated sprites
        private int currentSprite;
        private int totalSprites;
        private double flipCounter;
        //Adding in sprite column stats
        private int currentSpriteColumn;
        private int totalSpriteColumn;


        //Variables specifically for Bomber
        private Vector2 myTarget;
        private Boolean isHit;

        private Vector2[] playerPositions = new Vector2[2];
        private Vector2 target;
        private int targetPlayer;
        private double angle;
        private float deltaY, deltaX;

        public bool isDangerous;
        public bool isTraversable;
        public bool isInvincible;

        private bool wallCollision;

        private bool takingDamage = false;
        private Vector2 shake = new Vector2 (0,0);
        private float shakeOffset = 2.5f; //how much it shakes

        //Turrets and Bullets
        private float RoF; //Rate of Fire: lower, the faster it attacks
        private float attackTimer;
        private Vector2 bulletDirection;
        private float bulletLife;
        private Vector2 turretOffset = new Vector2 (8,1);

        //homing++
        private Vector2 HomingBossOffset = new Vector2(45, 67);
        private int fireballs = 3;
        private Boolean FireballSpawned = false;
        private int currentFBcount = 0;

        //laser stuff
        private Vector2 laserOffset = new Vector2(15, 15);
        private Vector2 laserBossOffset = new Vector2(48, 48); //center of the enemy
        private Vector2 targetPos = new Vector2(0, 0);
        private float cooldown = 0.0f;
        Random rand = new Random();
        int randomAttack;

        //For gattling gun
        private Boolean isFiring=false; // Whether the turret is firing
        private int coolDown=0; // Time between whether or not it's firing
        private int FireFrames = 0; //for switching spritesheet for firing
        
        //variable for Turret boss
        public Enemy[] SubEnemies; //sub enemies for a boss type, this could be used for other bosses but mostly for the turret for now
        private Boolean poweredOn=true;
        private Rectangle bounds; //for wondering area
        private int switchTime; // Time to switch directions
        private int switchTimeCounter = 0;
        private int currentCornerTarget;
        private Random randomTimeSwitch;
        private int movebullet=0;
        private Vector2 moveToTarget;
        private Boolean[] recentDeath;
        private Boolean speedUpPeriod;
        private int startingHealth;

        //For deh lines
        Texture2D lineTexture;
        Texture2D lineTexture_med;
        Texture2D lineTexture_short;
        float[] distance;
        float[] lineAngle;
        Vector2 origin = new Vector2(0, 8 / 2);
        int lineflipcounter=0;
        int linecurrentsprite=0;
        int linetotalsprite=7;



        Texture2D invincibilityShield;
        int shieldSprite;

        public Enemy(string type, Vector2 dir, Vector2 pos) //bullets
        {
            isInvincible = false;
            isDangerous = true;
            myType = type;
            bulletDirection = dir;
            myPos = pos;
            myDir = "Bullet";
            Set_Rectangle_Position();
            

            if(type=="Bullet")
            {
                myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\round_bullet");
                totalSprites = 3;
                myRect.Width = 15;
                myRect.Height = 15;
            }
            else if (type == "GattlingBullet")
            {
                myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\bullet");
                totalSprites = 3;
                myRect.Width = 15;
                myRect.Height = 15;
            }
            else if (type == "Fireball")
            {
                myDir = "Fireball";
                myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\fireball");
                totalSprites = 3;
                myRect.Width = 32;
                myRect.Height = 32;
            }
            
            bulletLife = 500;
            health = 100;

            currentSprite = 0;
            
        }

        public Enemy(string type, string dir, Vector2 pos, Vector2 tar) //bombs and lasers
        {
            if (type == "Bomb")
            {
                isInvincible = false;
                isDangerous = true;
                isTraversable = true;
                isHit = false;
                myType = type;
                myDir = dir;
                myPos = pos;
                myTarget = tar;
                Set_Rectangle_Position();

                myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\Bomb");
                health = 200;
                speed = 1.5f;
                currentSprite = 0;
                totalSprites = 7;
                myRect.Width = 32;
                myRect.Height = 32;
            }
            else if(type=="Laser")
            {
                isDangerous = true;

                myType = type;
                myDir = dir;
                myPos = pos;
                myTarget = targetPos;
                Set_Rectangle_Position();

                myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\laser_enemy_pink");
                health = 1000;
                speed = 0;
                totalSprites = 7;
                myRect.Width = 32;
                myRect.Height = 32;
                currentSprite = 0;
                isDangerous = false;
                RoF = 300;
                attackTimer = 100;
                isTraversable = false;
                isInvincible = true;

                angle = (float)(Math.PI * 1.5) - (float)Math.Atan2(myPos.X - targetPos.X, myPos.Y - targetPos.Y);
                angle = MathHelper.ToDegrees((float)angle);
            }
        }

        public Enemy(string type, string dir, Vector2 pos, Vector2 pos2, float cd, float timer, float delay) //laser
        {
            myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\laser_enemy_pink");
            myType = type;
            myDir = dir;
            health = 750;
            speed = 0;
            totalSprites = 7;
            currentSprite = 0;
            isDangerous = false;
            RoF = timer;
            attackTimer = cd;
            cooldown = cd;
            attackTimer = attackTimer + delay;
            isTraversable = false;
            myRect.Width = 32;
            myRect.Height = 32;
            targetPos = pos2;
            myPos = pos;
            Set_Rectangle_Position();
            isInvincible = true;


            //added this bit
            angle = (float)(Math.PI * 1.5) - (float)Math.Atan2(myPos.X - targetPos.X, myPos.Y - targetPos.Y);
            angle = MathHelper.ToDegrees((float)angle);

            GameState.objectManager.AddEnemy(new Enemy("Laser", "LaserEnd", targetPos, myPos)); //creates the end point
        }


        

        public Enemy(string type, string dir, Vector2 pos, int turretDirection) //non-rotating turrets
        {
            isInvincible = false;
            isDangerous = true;
            isTraversable = false;
            myType = type;
            myDir = dir;
            myPos = pos;
            Set_Rectangle_Position();

            myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\turret_spin_tall");
            health = 500;
            speed = 0;
            currentSprite = turretDirection;
            myRect.Width = 32;
            myRect.Height = 32;
            totalSprites = 7;
            targetPlayer = -1;
            RoF = 200;
            attackTimer = 0;
            bulletDirection = new Vector2(0, 0);

            switch (currentSprite)
            { 
                case 2:
                    bulletDirection.X = -1;
                    bulletDirection.Y = 0;
                    break;
                case 3:
                    bulletDirection.X = -1;
                    bulletDirection.Y = -1;
                    break;
                case 4:
                    bulletDirection.X = 0;
                    bulletDirection.Y = -1;
                    break;
                case 5:
                    bulletDirection.X = 1;
                    bulletDirection.Y = -1;
                    break;
                case 6:
                    bulletDirection.X = 1;
                    bulletDirection.Y = 0;
                    break;
                case 7:
                    bulletDirection.X = 1;
                    bulletDirection.Y = 1;
                    break;
                case 0:
                    bulletDirection.X = 0;
                    bulletDirection.Y = 1;
                    break;
                case 1:
                    bulletDirection.X = -1;
                    bulletDirection.Y = 1;
                    break;
                default:
                    break;
            }
        }

        public Enemy(string type, string dir, Vector2 pos)
        {
            isInvincible = false;
            isDangerous = true;
            isTraversable = true;
            myType = type;
            myDir = dir;
            myPos = pos;
            Set_Rectangle_Position();
            shieldSprite = 0;
            invincibilityShield = GameState.content.Load<Texture2D>(@"Textures\Enemy\sheildBubbleBig");
            switch (myType)
            {
                case "SpinningBlade":
                    myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\blade_spin_4Frame");
                    health = 1000;
                    speed = 2;
                    currentSprite = 0;
                    totalSprites = 3;
                    myRect.Width = 26;
                    myRect.Height = 26;
                    break;
                case "Turret":
                    myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\turret_spin_tall");
                    health = 1000;
                    speed = 0;
                    currentSprite = 0;
                    myRect.Width = 32;
                    myRect.Height = 32;
                    totalSprites = 7;
                    targetPlayer = -1;
                    RoF = 200;
                    attackTimer = 0;
                    bulletDirection = new Vector2(0, 0);
                    break;
                case "Spawner":
                    randomTimeSwitch = new Random();//adding this on for EXPLOSIONS
                    myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\Spawner_Boss3.0");
                    health = 5000;
                    startingHealth = 5000;
                    speed = 0;
                    currentSprite = 0;
                    myRect.Width = 170;
                    myRect.Height = 224;
                    totalSprites = 7;
                    targetPlayer = 1;
                    isDangerous = true;
                    isTraversable = false;
                    currentSpriteColumn = 0;
                    break;
                case "Homing":
                    myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\Homing");
                    health = 800;
                    speed = 0.75f;
                    currentSprite = 0;
                    totalSprites = 7;
                    myRect.Width = 32;
                    myRect.Height = 32;
                    targetPlayer = -1; //doesn't start with a target: -1 = no target
                    break;
                case "BossHoming":
                    randomTimeSwitch = new Random(); //adding this on for EXPLOSIONS
                    myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\homing_boss");
                    health = 4000;
                    speed = 1.02f;
                    currentSprite = 0;
                    secondaryCurrentSprite = 0;
                    totalSprites = 7;
                    isInvincible = true;
                    myRect.Width = 96;
                    myRect.Height = 96;
                    targetPlayer = 1;
                    bossTargetTimer = 0;
                    break;
                case "BossHoming++":
                    randomTimeSwitch = new Random(); //adding this on for EXPLOSIONS
                    myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\homing_boss_enrage");
                    health = 5000;
                    speed = 1f;
                    isInvincible = true;
                    currentSprite = 0;
                    secondaryCurrentSprite = 0;
                    totalSprites = 7;
                    myRect.Width = 96;
                    myRect.Height = 96;
                    targetPlayer = 1;
                    bossTargetTimer = 0;
                    break;
                case "BossBomber":
                    myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\bombing_boss");//TODO: Make Art
                    health = 12000;
                    speed = 0f;
                    currentSprite = 0;
                    secondaryCurrentSprite = 0;
                    totalSprites = 7;
                    myRect.Width = 96;
                    myRect.Height = 96;
                    targetPlayer = 1;
                    bossTargetTimer = 0;
                    isDangerous = true;
                    isTraversable = false;
                    break;
                case "BossJumper":
                    randomTimeSwitch = new Random(); //adding this on for EXPLOSIONS
                    myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\jumper_boss");
                    secondaryTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\shadow");
                    health = 10000;
                    speed = 0f;
                    currentSprite = 0;
                    secondaryCurrentSprite = 0;
                    totalSprites = 10;
                    myRect.Width = 64;
                    myRect.Height = 64;
                    targetPlayer = 1;
                    bossTargetTimer = 0;
                    isDangerous = true;
                    break;
                case "Rubble":
                    //myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\rubble");
                    myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\breakable_rubble");
                    secondaryTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\shadow");
                    currentSpriteColumn = 0;
                    health = 950;
                    speed = 0;
                    currentSprite = 0;
                    secondaryCurrentSprite = 5;
                    totalSprites = 2;
                    myRect.Width = 32;
                    myRect.Height = 32;
                    targetPlayer = 0;
                    bossTargetTimer = 0;
                    jumpY = -1200;
                    isDangerous = false;
                    isInvincible = true;
                    attackTimer = 3500;
                    if (GameState.GameSpeed == 0)
                        attackTimer = 1800;
                    break;
                case "BossTurret":
                    BossTurretInitialize();
                    break;
                case "MovingTurret":
                    MovingTurretInitialize();
                    break;
                case "GattlingGun":
                    GattlingGunInitialize();
                    break;
                case "BossLaser":
                    randomTimeSwitch = new Random();
                    myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\laser_boss");
                    health = 20000;
                    speed = 0f;
                    currentSprite = 0;
                    totalSprites = 7;
                    myRect.Width = 96;
                    myRect.Height = 96;
                    isDangerous = false;
                    isTraversable = false;
                    cooldown = 500;
                    break;
                case "Laser":
                    myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\laser_enemy_pink");
                    health = 1000;
                    speed = 0;
                    totalSprites = 7;
                    myRect.Width = 32;
                    myRect.Height = 32;
                    currentSprite = 0;
                    isDangerous = false;
                    RoF = 300;
                    attackTimer = 100;
                    isTraversable = false;
                    isInvincible = true;
                    break;
                default:
                    break;
            }
        }

        private void Set_Rectangle_Position()
        {
            myRect.X = (int)myPos.X;
            myRect.Y = (int)myPos.Y;
        }

        public bool Check_Collision(Rectangle playerRect)
        {
            if (!(playerRect.Left > myRect.Right
                    || playerRect.Right < myRect.Left
                    || playerRect.Top > myRect.Bottom
                    || playerRect.Bottom < myRect.Top))
            {
                if(myType == "Homing")
                {
                    Death();
                }
                if (myType == "Spawner")
                {
                    if (targetPlayer == 0)
                        targetPlayer = 1;
                    else
                        targetPlayer = 0;
                }
                if (myType == "Bomb")
                {
                    isHit = true;
                    Death();
                }
                else if (myType == "Bullet" || myType == "GattlingBullet" || myType == "Fireball")
                {
                    GameState.objectManager.AddExplosion(new Explosion("spark", myPos));
                    GameState.soundBank.PlayCue("Explosion");
                    Death();
                }
                else if (myType == "BossHoming" || myType == "BossHoming++")
                {
                    if (targetPlayer == 0)
                        targetPlayer = 1;
                    else
                        targetPlayer = 0;
                    bossTargetTimer = 0;
                }
  
                return true;
            }
            return false;
        }

        private void Update_Sprite(GameTime gameTime)
        {
            if(flipCounter>28 && isInvincible)
            {
                shieldSprite++;
                if(shieldSprite>3)
                {
                    shieldSprite = 0;
                }
            }
            if (myType == "Turret")
            {
                if (myDir == "None")
                {
                    Detect_Angle(gameTime);

                    if (angle < 0)
                        angle = angle + 360;

                    if ((angle > 0 && angle <= 22.5) || (angle > 337.5 && angle <= 360))
                    {
                        currentSprite = 2;
                        bulletDirection.X = -1;
                        bulletDirection.Y = 0;
                    }
                    else if (angle > 22.5 && angle <= 67.5)
                    {
                        currentSprite = 3;
                        bulletDirection.X = -1;
                        bulletDirection.Y = -1;
                    }
                    else if (angle > 67.5 && angle <= 112.5)
                    {
                        currentSprite = 4;
                        bulletDirection.X = 0;
                        bulletDirection.Y = -1;
                    }
                    else if (angle > 112.5 && angle <= 157.5)
                    {
                        currentSprite = 5;
                        bulletDirection.X = 1;
                        bulletDirection.Y = -1;
                    }
                    else if (angle > 157.5 && angle <= 202.5)
                    {
                        currentSprite = 6;
                        bulletDirection.X = 1;
                        bulletDirection.Y = 0;
                    }
                    else if (angle > 202.5 && angle <= 247.5)
                    {
                        currentSprite = 7;
                        bulletDirection.X = 1;
                        bulletDirection.Y = 1;
                    }
                    else if (angle > 247.5 && angle <= 292.5)
                    {
                        currentSprite = 0;
                        bulletDirection.X = 0;
                        bulletDirection.Y = 1;
                    }
                    else if (angle > 292.5 && angle <= 337.5)
                    {
                        currentSprite = 1;
                        bulletDirection.X = -1;
                        bulletDirection.Y = 1;
                    }
                }

            }
            else if (myType =="Laser" || myType== "BossLaser")
            {
            //TODO: things

                if(angle<=36)
                {
                    currentSpriteColumn = 0;
                }
                else if(angle<=72)
                {
                    currentSpriteColumn = 8;
                }
                else if (angle <= 108)
                {
                    currentSpriteColumn = 9;
                }
                else if (angle <= 144)
                {
                    currentSpriteColumn = 7;
                }
                else if (angle <= 180)
                {
                    currentSpriteColumn = 6;
                }
                else if (angle <= 216)
                {
                    currentSpriteColumn = 5;
                }
                else if (angle <= 252)
                {
                    currentSpriteColumn = 4;
                }
                else if (angle <= 288)
                {
                    currentSpriteColumn = 3;
                }
                else if (angle <= 324)
                {
                    currentSpriteColumn = 2;
                }
                else
                {
                    currentSpriteColumn = 1;
                }


                flipCounter += gameTime.ElapsedGameTime.Milliseconds;

                if (flipCounter > 40)
                {
                    currentSprite++;
                    flipCounter = 0;
                    if (currentSprite > totalSprites)
                        currentSprite = 0;
                }



            }
            else if (myType == "Spawner")
            {
                if ((float)health / startingHealth > .83f)
                {
                    currentSpriteColumn = 0;
                }
                else if ((float)health / startingHealth > .55f)
                {
                    currentSpriteColumn = 1;

                }
                else if ((float)health / startingHealth > .25f)
                {
                    currentSpriteColumn = 2;

                }
                else
                {
                    currentSpriteColumn = 3;

                }


                bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (bossTargetTimer >= 1000)  //We want the boss to switch targets every few seconds.
                {
                    Spawnertimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (Spawnertimer < 700)
                    {
                        currentSprite = (int)(Spawnertimer / 100);
                    }
                    else if (Spawnertimer >= 700 && Spawned == false)
                    {
                        for (int i = 0; i < DronesCreated; i++)
                        {
                            randomAttack = rand.Next((int)MaxDistance.X * 2);       //randomize numbers for mob spawn
                            if (randomAttack <= MaxDistance.X)
                                randomAttack = -randomAttack;
                            else if (randomAttack > MaxDistance.X)
                                randomAttack = randomAttack - (int)MaxDistance.X;

                            randomAttack2 = rand.Next((int)MaxDistance.Y*2);
                            if (randomAttack2 <= MaxDistance.Y)
                                randomAttack2 = -randomAttack2;
                            else if (randomAttack2 > MaxDistance.Y)
                                randomAttack2 = randomAttack2 - (int)MaxDistance.Y;

                            GameState.objectManager.AddEnemy(new Enemy("Homing", "Homing",
                               new Vector2(myPos.X + SpriteOffset.X + randomAttack,
                                   myPos.Y + SpriteOffset.Y + randomAttack2)));
                        }
                        Spawned = true;
                    }
                    else if(ResetTimerSpawner == false && Spawned == true)
                    {
                        Spawnertimer2 = 0;
                        ResetTimerSpawner = true;
                    }
                    else if (Spawned == true)
                    {
                        Spawnertimer2 += gameTime.ElapsedGameTime.Milliseconds;
                        if (Spawnertimer2 < 700)
                            currentSprite = (int)(7 - (Spawnertimer2 / 100));
                        else
                        {
                            Spawned = false;
                            bossTargetTimer = 0;
                            Spawnertimer = 0;
                            Spawnertimer2 = 0;
                            ResetTimerSpawner = false;
                        }
                            
                    }
                }

            }
            else if (myType == "BossJumper")
            {
                if (isDangerous) //If jumper is grounded
                {
                    currentSprite = (int)(bossTargetTimer / 800);
                }
                else //If the jumper is airborn
                {
                    if (bossTargetTimer <= 4050)
                    {
                        currentSprite = 6;
                        secondaryCurrentSprite = 0;
                    }
                    else if (bossTargetTimer <= 4100)
                    {
                        currentSprite = 7;
                        secondaryCurrentSprite = 0;
                    }
                    else
                    {
                        currentSprite = 7;
                        if (bossTargetTimer <= 4200)
                            secondaryCurrentSprite = 1;
                        else if (bossTargetTimer <= 4400)
                            secondaryCurrentSprite = 2;
                        else if (bossTargetTimer <= 4600)
                            secondaryCurrentSprite = 3;
                        else if (bossTargetTimer <= 4800)
                            secondaryCurrentSprite = 4;
                        else if (bossTargetTimer <= 5900)
                            secondaryCurrentSprite = 5;
                        else if (bossTargetTimer <= 6100)
                            secondaryCurrentSprite = 4;
                        else if (bossTargetTimer <= 6300)
                            secondaryCurrentSprite = 3;
                        else if (bossTargetTimer <= 6500)
                            secondaryCurrentSprite = 2;
                        else if (bossTargetTimer <= 6700)
                            secondaryCurrentSprite = 1;
                    }
                }
            }
            else if (myType == "Rubble")
            {
                if (isInvincible)
                {
                    secondaryCurrentSprite = (int)(-jumpY / 200);
                    currentSprite = 1;
                    currentSpriteColumn = 0;
                }
                else
                {
                    
                    if((float)health/950>.75)
                    {
                        currentSpriteColumn = 0;
                    }
                    else if ((float)health / 950 > .5)
                    {
                        currentSpriteColumn = 1;
                    }
                    else if ((float)health / 950 > .25)
                    {
                        currentSpriteColumn = 2;
                    }
                    else
                    {
                        currentSpriteColumn = 3;
                    }

                    secondaryCurrentSprite = 0;
                    currentSprite = 0;
                }
            }
            else if (myType == "BossTurret")
            {
               //TODO: add in sprite thingings
                
                //THIS
                if ((float)health / startingHealth > .83f)
                {
                    currentSpriteColumn = 0;
                }
                else if ((float)health / startingHealth > .55f)
                {
                    currentSpriteColumn = 1;

                }
                else if ((float)health / startingHealth > .37f)
                {
                    currentSpriteColumn = 2;

                }
                else if ((float)health / startingHealth > .19f)
                {
                    currentSpriteColumn = 3;

                }
                else
                    currentSpriteColumn = 4;
                if (poweredOn)
                {



                    

                    flipCounter += gameTime.ElapsedGameTime.Milliseconds;

                    if (flipCounter > 40)
                    {
                        currentSprite++;
                        flipCounter = 0;
                        if (currentSprite > totalSprites)
                            currentSprite = 0;
                    }


                    lineflipcounter += gameTime.ElapsedGameTime.Milliseconds;

                    //Switched this over so it will play idle animation instead of no animation for not moving
                    if (lineflipcounter > 20 && !GameState.gameOver)
                    {

                        linecurrentsprite++;
                        lineflipcounter = 0;
                        if (linecurrentsprite > linetotalsprite)
                            linecurrentsprite = 0;
                    }
                }
                else
                    currentSprite = 8;
            }
            else if (myType == "GattlingGun")
            {
                if (poweredOn)
                {
                    if(currentSprite == 8)
                        currentSprite = 0;
                    if (isFiring)
                    {
                        if (currentSpriteColumn == 0)
                        {
                            currentSpriteColumn = 1;
                        }
                        if (FireFrames >= RoF) //switches which barrel is firing based on the firerate
                        {

                            currentSpriteColumn++;
                            FireFrames = 0;
                            if (currentSpriteColumn > totalSpriteColumn)
                            {
                                currentSpriteColumn = 1;
                            }
                        }
                        else
                        {
                            FireFrames += gameTime.ElapsedGameTime.Milliseconds;
                        }
                    }
                    else
                    {
                        currentSpriteColumn = 0; //not firing
                    }

                    //just the base going through the animations 
                    flipCounter += gameTime.ElapsedGameTime.Milliseconds;

                    if (flipCounter > 40)
                    {
                        currentSprite++;
                        flipCounter = 0;
                        if (currentSprite > totalSprites)
                            currentSprite = 0;
                    }
                }
                else
                {
                    currentSprite = 8;

                }
            }
            else if (myType =="MovingTurret") //updated the column level (where it's pointing) and the move passive sprite effects
            {
                if (poweredOn)
                {
                    Detect_Angle(gameTime);
                    if (currentSprite == 8)
                        currentSprite = 0;
                    if (angle < 0)
                        angle = angle + 360;

                    if ((angle > 0 && angle <= 22.5) || (angle > 337.5 && angle <= 360))
                    {
                        currentSpriteColumn = 2;
                        bulletDirection.X = -1;
                        bulletDirection.Y = 0;
                    }
                    else if (angle > 22.5 && angle <= 67.5)
                    {
                        currentSpriteColumn = 3;
                        bulletDirection.X = -1;
                        bulletDirection.Y = -1;
                    }
                    else if (angle > 67.5 && angle <= 112.5)
                    {
                        currentSpriteColumn = 4;
                        bulletDirection.X = 0;
                        bulletDirection.Y = -1;
                    }
                    else if (angle > 112.5 && angle <= 157.5)
                    {
                        currentSpriteColumn = 5;
                        bulletDirection.X = 1;
                        bulletDirection.Y = -1;
                    }
                    else if (angle > 157.5 && angle <= 202.5)
                    {
                        currentSpriteColumn = 6;
                        bulletDirection.X = 1;
                        bulletDirection.Y = 0;
                    }
                    else if (angle > 202.5 && angle <= 247.5)
                    {
                        currentSpriteColumn = 7;
                        bulletDirection.X = 1;
                        bulletDirection.Y = 1;
                    }
                    else if (angle > 247.5 && angle <= 292.5)
                    {
                        currentSpriteColumn = 0;
                        bulletDirection.X = 0;
                        bulletDirection.Y = 1;
                    }
                    else if (angle > 292.5 && angle <= 337.5)
                    {
                        currentSpriteColumn = 1;
                        bulletDirection.X = -1;
                        bulletDirection.Y = 1;
                    }
                    flipCounter += gameTime.ElapsedGameTime.Milliseconds;

                    if (flipCounter > 40)
                    {
                        currentSprite++;
                        flipCounter = 0;
                        if (currentSprite > totalSprites)
                            currentSprite = 0;
                    }
                }
                else
                    currentSprite = 8; //powered off state

            }
            else
            {
                flipCounter += gameTime.ElapsedGameTime.Milliseconds;

                if (flipCounter > 40)
                {
                    currentSprite++;
                    flipCounter = 0;
                    if (currentSprite > totalSprites)
                        currentSprite = 0;
                }
            }
        } //End Update_Sprite

        private double Detect_Angle(GameTime gameTime)
        {
            playerPositions = GameState.objectManager.players.Get_Player_Positions(); //gets both player positions
            target = playerPositions[targetPlayer];

            deltaY = myPos.Y - target.Y;
            deltaX = myPos.X - target.X;
            angle = Math.Atan2(deltaY, deltaX) * 180 / Math.PI;

            return angle;
        }

        private bool Collides(GameTime gameTime) //wall collision for spinningblades
        {
            wallCollision = false;

            foreach(AnimatedSprite wall in GameState.objectManager.SpriteList)
            {
                if (nextPos.X > wall.m_position.X - myRect.Width && nextPos.X < (wall.m_position.X + wall.m_width) &&
                    nextPos.Y > wall.m_position.Y - myRect.Height && nextPos.Y < (wall.m_position.Y + wall.m_height))
                {
                    wallCollision = true;
                }
            }

            return wallCollision;
        }

        //Method for initializing the boss turret subtype
        //I'll see how this goes..
        
        public void BossTurretInitialize() 
        {
            //TODO: lines?
            //TODO: initialization for base
            myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\TURRETBOSS_Body_medium"); //finish and replace art
            health = 25500;
            startingHealth = 20500;
            speed = 1.06f;
            currentSprite = 0;
            currentSpriteColumn = 0;
            totalSprites = 7;
            totalSpriteColumn = 4;
            myRect.Width = 64;
            myRect.Height = 64;
            bossTargetTimer = 0;
            RoF = 400;//NEED TO MAKE ANIMATION FOR MISSLE FIRING
            //subtypes for shoulders and turret 0 being the left should, 1 being the right, 2 being the turret
            SubEnemies = new Enemy[5];
            SubEnemies[0] = new Enemy("MovingTurret", "MovingTurret", new Vector2(myPos.X - 128, myPos.Y + 128));
            SubEnemies[0].health = 6000;
            SubEnemies[1] = new Enemy("MovingTurret", "MovingTurret", new Vector2(myPos.X -128, myPos.Y -128));
            SubEnemies[1].health = 6000;
            SubEnemies[2] = new Enemy("GattlingGun", "GattlingGun", new Vector2(myPos.X, myPos.Y + 64));
            SubEnemies[2].health = 6000;
            SubEnemies[3] = new Enemy("MovingTurret", "MovingTurret", new Vector2(myPos.X + 128, myPos.Y - 128));
            SubEnemies[3].health = 6000;
            SubEnemies[4] = new Enemy("MovingTurret", "MovingTurret", new Vector2(myPos.X + 128, myPos.Y + 128));
            SubEnemies[4].health = 6000;

            recentDeath = new Boolean[5];
            recentDeath[0] = true;
            recentDeath[1] = true;
            recentDeath[2] = true;
            recentDeath[3] = true;
            recentDeath[4] = true;

            distance = new float[4];
            lineAngle = new float[4];


            lineTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\TransparentBar_longwavy_boss");
            lineTexture_med = GameState.content.Load<Texture2D>(@"Textures\Enemy\TransparentBar_wavyshortpink_boss");
            lineTexture_short = GameState.content.Load<Texture2D>(@"Textures\Enemy\TransparentBar_wavyshortpurple_boss");
            speedUpPeriod = false;

            randomTimeSwitch = new Random();

        }

        public void MovingTurretInitialize()
        {
            //TODO: moving turret initialize
            myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\TURRETBOSS_shoulder");
            health = 1000;
            speed = 1.03f;
            currentSprite = 0;
            currentSpriteColumn = 0;
            myRect.Width = 32;
            myRect.Height = 32;
            totalSprites = 7;
            totalSpriteColumn = 7;
            targetPlayer = 1;
            moveToTarget = myPos * 1.1f;
            RoF = 200;
            attackTimer = 0;
            switchTime = 3000; // Time to switch directions
            switchTimeCounter = 0;
            currentCornerTarget=3;
            randomTimeSwitch = new Random();
            bounds = new Rectangle((int)myPos.X, (int)myPos.Y, 96, 96);
            bulletDirection = new Vector2(0, 0);

        }

        public void GattlingGunInitialize()
        {
            turretOffset = new Vector2(-20, 12);
            myTexture = GameState.content.Load<Texture2D>(@"Textures\Enemy\TURRETBOSS_turret");
            health = 2000;
            speed = 0f;
            currentSprite = 0;
            currentSpriteColumn = 0;
            coolDown = 0;
            myRect.Width = 32;
            myRect.Height = 32;
            totalSprites = 7;
            totalSpriteColumn = 6;
            RoF = 120;
            attackTimer = 0;
            bulletDirection = new Vector2(-1, 0);

        }

        //method for managing the turret bosses and such
        public void BossTurretUpdate(GameTime gameTime)
        {
            if (!isDead && poweredOn)
            {
                bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (bossTargetTimer >= 3000)  //just grabbing this from homing as to move the target to that player
                {
                    if (targetPlayer == 0)
                        targetPlayer = 1;
                    else
                        targetPlayer = 0;
                    bossTargetTimer = 0;
                }


                
                
                playerPositions = GameState.objectManager.players.Get_Player_Positions(); //gets both player positions
                target = playerPositions[targetPlayer] - myPos;
                target.Normalize();
                if(!speedUpPeriod  && myPos.X<700)
                    nextPos = new Vector2(myPos.X+speed, myPos.Y + (target.Y * speed)); //simply moves up and down while moving to the right
                else if(speedUpPeriod || myPos.X<300)
                {
                    coolDown+=gameTime.ElapsedGameTime.Milliseconds;
                    nextPos = new Vector2(myPos.X + speed*2, myPos.Y + (target.Y * speed));
                    if(coolDown>2500)
                    {
                        coolDown = 0;
                        speedUpPeriod = false;
                    }
                }
                else if(myPos.X > 700)
                {
                    nextPos = new Vector2(myPos.X+target.X*speed*.25f, myPos.Y+ (target.Y  * speed));

                }
                myPos = nextPos;
                if (!SubEnemies[0].isDead)
                {
                    SubEnemies[0].bounds.X = (int)myPos.X - 128;
                    SubEnemies[0].bounds.Y = (int)myPos.Y + 128;
                    SubEnemies[0].MovingTurretUpdate(gameTime);
                    SubEnemies[0].myPos.X = SubEnemies[0].myPos.X - GameState.GameSpeed*.25f;
                    SubEnemies[0].Set_Rectangle_Position();

                }
                else
                {
                    if (recentDeath[0])
                    {
                        GameState.objectManager.AddExplosion(new Explosion("green", new Vector2(myPos.X - 32, myPos.Y + 32)));
                        recentDeath[0] = false;
                        poweredOn = false;
                        health -= 1600;
                        coolDown = 0;
                        
                    }
                    health -= 1;
                }
                if (!SubEnemies[1].isDead)
                {
                    SubEnemies[1].bounds.X = (int)myPos.X - 128;
                    SubEnemies[1].bounds.Y = (int)myPos.Y - 128;
                    SubEnemies[1].MovingTurretUpdate(gameTime);
                    SubEnemies[1].myPos.X = SubEnemies[1].myPos.X - GameState.GameSpeed * .25f;
                    SubEnemies[1].Set_Rectangle_Position();
                }
                else
                {
                    if (recentDeath[1])
                    {
                        GameState.objectManager.AddExplosion(new Explosion("green", new Vector2(myPos.X-32,myPos.Y-32)));
                        recentDeath[1] = false;
                        poweredOn = false;
                        health -= 1600;
                        coolDown = 0;
                    }
                    health -= 1;
                }
                if (!SubEnemies[2].isDead)
                {
                    SubEnemies[2].GattlingGunUpdate(gameTime);
                    SubEnemies[2].myPos.X = myPos.X + 40;
                    SubEnemies[2].myPos.Y = myPos.Y + 50;
                    SubEnemies[2].myPos.X = myPos.X - GameState.GameSpeed;

                    SubEnemies[2].Set_Rectangle_Position();
                }
                else
                {
                    if (recentDeath[2])
                    {
                        recentDeath[2] = false;
                        poweredOn = false;
                        health -= 1600;
                        coolDown = 0;
                    }
                   
                }
                if (!SubEnemies[3].isDead)
                {
                    
                    SubEnemies[3].bounds.X = (int)myPos.X + 128;
                    SubEnemies[3].bounds.Y = (int)myPos.Y - 128;
                    SubEnemies[3].MovingTurretUpdate(gameTime);
                    SubEnemies[3].myPos.X = SubEnemies[3].myPos.X - GameState.GameSpeed * .25f;
                    //SubEnemies[3].myPos.X = myPos.X - GameState.GameSpeed;
                    SubEnemies[3].Set_Rectangle_Position();
                }
                else
                {
                    if (recentDeath[3])
                    {
                        GameState.objectManager.AddExplosion(new Explosion("green", new Vector2(myPos.X + 32, myPos.Y - 32)));
                        recentDeath[3] = false;
                        poweredOn = false;
                        health -= 1600;
                        coolDown = 0;
                    }
                    health -= 1;
                }
                if (!SubEnemies[4].isDead)
                {

                    SubEnemies[4].bounds.X = (int)myPos.X + 128;
                    SubEnemies[4].bounds.Y = (int)myPos.Y + 128;
                    SubEnemies[4].MovingTurretUpdate(gameTime);
                    SubEnemies[4].myPos.X = SubEnemies[4].myPos.X - GameState.GameSpeed * .25f;
                    //SubEnemies[4].myPos.X = myPos.X - GameState.GameSpeed;
                    SubEnemies[4].Set_Rectangle_Position();
                }
                else
                {
                    if (recentDeath[4])
                    {
                        GameState.objectManager.AddExplosion(new Explosion("green", new Vector2(myPos.X + 32, myPos.Y + 32)));
                        recentDeath[4] = false;
                        poweredOn = false;
                        health -= 1600;
                        coolDown = 0;
                    }
                    health -= 1;
                }

                  SubEnemies[0].Update_Sprite(gameTime);
                  SubEnemies[1].Update_Sprite(gameTime);
                  SubEnemies[2].Update_Sprite(gameTime);
                  SubEnemies[3].Update_Sprite(gameTime);
                  SubEnemies[4].Update_Sprite(gameTime);

                  distance[0] = Vector2.Distance(myPos, SubEnemies[0].myPos);
                  lineAngle[0] = (float)(Math.PI * 1.5) - (float)Math.Atan2(myPos.X - SubEnemies[0].myPos.X+16, myPos.Y - SubEnemies[0].myPos.Y);

                  distance[1] = Vector2.Distance(myPos, SubEnemies[1].myPos)+16;
                  lineAngle[1] = (float)(Math.PI * 1.5) - (float)Math.Atan2(myPos.X - SubEnemies[1].myPos.X, myPos.Y - SubEnemies[1].myPos.Y);

                  distance[2] = Vector2.Distance(myPos, SubEnemies[3].myPos);
                  lineAngle[2] = (float)(Math.PI * 1.5) - (float)Math.Atan2(myPos.X - SubEnemies[3].myPos.X, myPos.Y - SubEnemies[3].myPos.Y+16);

                  distance[3] = Vector2.Distance(myPos, SubEnemies[4].myPos)-16;
                  lineAngle[3] = (float)(Math.PI * 1.5) - (float)Math.Atan2(myPos.X - SubEnemies[4].myPos.X, myPos.Y - SubEnemies[4].myPos.Y);
                

            }
            else if (!poweredOn && !isDead)
            {

                SubEnemies[0].myPos.X -=  GameState.GameSpeed;
                //SubEnemies[0].Set_Rectangle_Position();

                SubEnemies[1].myPos.X -=  GameState.GameSpeed;
               // SubEnemies[1].Set_Rectangle_Position();

                SubEnemies[3].myPos.X -=  GameState.GameSpeed;
               // SubEnemies[3].Set_Rectangle_Position();

                SubEnemies[4].myPos.X -=  GameState.GameSpeed;
                //SubEnemies[4].Set_Rectangle_Position();
                SubEnemies[2].myPos.X = myPos.X + 40;
                SubEnemies[2].myPos.Y = myPos.Y + 50;
                
                

                coolDown += gameTime.ElapsedGameTime.Milliseconds;
                if(coolDown>3000)
                {
                    poweredOn = true;
                    speedUpPeriod = true;
                    coolDown = 0;
                }
            }
            else if(isDead)
            {
                SubEnemies[0].myPos.X -= GameState.GameSpeed;
                //SubEnemies[0].Set_Rectangle_Position();

                SubEnemies[1].myPos.X -= GameState.GameSpeed;
                // SubEnemies[1].Set_Rectangle_Position();

                SubEnemies[3].myPos.X -= GameState.GameSpeed;
                // SubEnemies[3].Set_Rectangle_Position();

                SubEnemies[4].myPos.X -= GameState.GameSpeed;
                SubEnemies[2].myPos.X = myPos.X + 40;
                SubEnemies[2].myPos.Y = myPos.Y + 50;

                bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                deathTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (bossTargetTimer >= 50)
                {
                    GameState.soundBank.PlayCue("Explosion");
                    float randomx= myPos.X + randomTimeSwitch.Next(-64, 96);
                    float randomy=myPos.Y + randomTimeSwitch.Next(-64, 96);
                    GameState.objectManager.AddExplosion(new Explosion("purple", new Vector2(randomx,randomy)));
                    for (int i = 0; i < SubEnemies.Length;i++ )
                    {
                        if (!SubEnemies[i].isDead)
                        {
                            randomx = SubEnemies[i].myPos.X + randomTimeSwitch.Next(-16, 16);
                            randomy = SubEnemies[i].myPos.Y + randomTimeSwitch.Next(-16, 16);
                            GameState.objectManager.AddExplosion(new Explosion("purple", new Vector2(randomx, randomy)));
                        }

                    }
                        bossTargetTimer = 0;
                }
                if (deathTimer >= 3000)
                {
                    GameState.objectManager.AddExplosion(new Explosion("Explosion3", myPos));
                    GameState.objectManager.AddDeadEnemy(this);
                    GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X - myPos.X % 32, myPos.Y - myPos.Y % 32), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X - myPos.X % 32, myPos.Y - 96 - myPos.Y % 32), "Victory"));

                    GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X - myPos.X % 32 + 32, myPos.Y - myPos.Y % 32), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X - myPos.X % 32 + 32, myPos.Y - myPos.Y % 32 - 96), "Victory"));

                    GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X - myPos.X % 32, myPos.Y + 32 - myPos.Y % 32), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X - myPos.X % 32, myPos.Y - 128 - myPos.Y % 32), "Victory"));

                    GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X - myPos.X % 32 + 32, myPos.Y + 32 - myPos.Y % 32), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X - myPos.X % 32 + 32, myPos.Y - 128 - myPos.Y % 32), "Victory"));
                     GameState.objectManager.AddStopPoint(1);
                    
                }
            }

        }

        public void MovingTurretUpdate(GameTime gameTime) 
        {
            // The area it is allowed to move in
            // Movement *rules*
            Vector2 temptarget = moveToTarget - myPos;
            temptarget.Normalize();
            if (bounds.Contains((int)(myPos.X + speed*temptarget.X), (int)(myPos.Y + speed*temptarget.Y)))
            {
                switchTimeCounter+=gameTime.ElapsedGameTime.Milliseconds;
                if(switchTimeCounter>switchTime)
                {
                    switchTimeCounter = 0;
                    switchTime = randomTimeSwitch.Next(300, 3000);
                    currentCornerTarget++;
                    if(currentCornerTarget>3)
                    {
                        currentCornerTarget = 0;
                    }
                    switch(currentCornerTarget)
                    {
                        case 0: //aim for left top corner with a degree of randomness
                            moveToTarget.X = bounds.X + randomTimeSwitch.Next(-1, 1) * bounds.Width * .6f;
                            moveToTarget.Y = bounds.Y + randomTimeSwitch.Next(-2, -1) * bounds.Height;
                            break;
                        case 1: //aim for right top with a degree of randomness
                            moveToTarget.X = bounds.Width + randomTimeSwitch.Next(-1, 1) * bounds.Width;
                            moveToTarget.Y = bounds.Y + randomTimeSwitch.Next(-2, -1) * bounds.Height;
                            break;
                        case 2: //aim for bottom left corner with a degree of randomness
                            moveToTarget.X = bounds.X + randomTimeSwitch.Next(-1, 1) * bounds.Width * .6f;
                            moveToTarget.Y = bounds.Height + randomTimeSwitch.Next(1, 2) * bounds.Height;
                            break;
                        case 3: //aim for bottom right corner with a degree of randomness
                            moveToTarget.X = bounds.Width + randomTimeSwitch.Next(-1, 1) * bounds.Width * .6f;
                            moveToTarget.Y = bounds.Height + randomTimeSwitch.Next(1, 2) * bounds.Height;
                            break;
                        default:
                            break;

                    }
                }
                
                
                    
            }
            else
            {
                if (myPos.X + speed * temptarget.X > bounds.Width + bounds.X)
                {
                    moveToTarget.X = bounds.X - 50 - myPos.X;
                   
                }
                else if (myPos.X + speed * temptarget.X < bounds.X)
                {
                    moveToTarget.X = bounds.Width + bounds.X + 50 - myPos.X;
                }

                if (myPos.Y + speed * temptarget.Y > bounds.Height + bounds.Y)
                {
                    moveToTarget.Y = bounds.Y - 50 - myPos.Y;
                   //target.Y = -1;
                    
                }
                else if (myPos.Y + speed * temptarget.Y < bounds.Y)
                {
                    moveToTarget.Y = bounds.Height + bounds.Y + 50 - myPos.Y;
                    //myPos.Y += 1;
                }

                
                
                
            }
            moveToTarget.Normalize();

            nextPos = new Vector2(myPos.X + moveToTarget.X * speed, myPos.Y + (moveToTarget.Y * speed)); 
            myPos = nextPos;
            //firerate
            attackTimer = attackTimer + 1;
            if (attackTimer >= RoF)
            {
                GameState.objectManager.AddBullet(new Enemy("Bullet", bulletDirection, (myPos + turretOffset)));
                attackTimer = 0;
                GameState.soundBank.PlayCue("Shot");
            }
            
        }

        public void GattlingGunUpdate(GameTime gameTime)
        {
            coolDown += gameTime.ElapsedGameTime.Milliseconds;
            if(coolDown>12000 && isFiring==false)
            {
                isFiring = true;
                coolDown = 0;
                attackTimer = 0;
            }
            else if(coolDown>6000 && isFiring==true)
            {
                isFiring = false;
                coolDown = 0;
                attackTimer = 0;
            }
            if(isFiring)
            {
                if (attackTimer >= RoF)
                {
                    GameState.objectManager.AddBullet(new Enemy("GattlingBullet", bulletDirection, new Vector2((myPos.X + turretOffset.X), (myPos.Y + turretOffset.Y + movebullet))));
                    attackTimer = 0;
                    GameState.soundBank.PlayCue("Shot");
                }
                else
                    attackTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (movebullet < 12)
                {
                    movebullet += 4;
                }
                else
                    movebullet = 0;
                
             }
        }

        public void DrawCollisionBox(GameTime gameTime) //for testing the collision
        {
            GameState.primitiveBatch.AddVertex(myPos, Color.Red);
            GameState.primitiveBatch.AddVertex(myPos + new Vector2(32,0), Color.Red);

            GameState.primitiveBatch.AddVertex(myPos + new Vector2(32, 0), Color.Red);
            GameState.primitiveBatch.AddVertex(myPos + new Vector2(32, 32), Color.Red);

            GameState.primitiveBatch.AddVertex(myPos + new Vector2(32, 32), Color.Red);
            GameState.primitiveBatch.AddVertex(myPos + new Vector2(0, 32), Color.Red);

            GameState.primitiveBatch.AddVertex(myPos + new Vector2(0, 32), Color.Red);
            GameState.primitiveBatch.AddVertex(myPos, Color.Red);

        }

        private void Death()
        {
            switch (myType)
            {
                case "BossHoming":
                    isDead = true;
                    isDangerous = false;
                    GameState.soundBank.PlayCue("Explosion");
                    break;
                case "BossHoming++":
                    isDead = true;
                    isDangerous = false;
                    GameState.soundBank.PlayCue("Explosion");
                    break;
                case "Spawner":
                    isDead = true;
                    isDangerous = false;
                    GameState.soundBank.PlayCue("Explosion");
                    break;
                case "BossLaser":
                    isDead = true;
                    isDangerous = false;
                    GameState.soundBank.PlayCue("Explosion");
                    break;
                case "BossBomber":
                    isDead = true;
                    isDangerous = false;
                    GameState.soundBank.PlayCue("Explosion");
                    GameState.objectManager.AddZone(new Zone(new Vector2(6880 - GameState.DistanceTraversed, 224), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(6880 - GameState.DistanceTraversed, 256), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(6880 - GameState.DistanceTraversed, 288), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(6880 - GameState.DistanceTraversed, 320), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(6912 - GameState.DistanceTraversed, 224), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(6912 - GameState.DistanceTraversed, 256), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(6912 - GameState.DistanceTraversed, 288), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(6912 - GameState.DistanceTraversed, 320), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(6944 - GameState.DistanceTraversed, 224), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(6944 - GameState.DistanceTraversed, 256), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(6944 - GameState.DistanceTraversed, 288), "Victory"));
                    GameState.objectManager.AddZone(new Zone(new Vector2(6944 - GameState.DistanceTraversed, 320), "Victory"));
                    break;
                case "Bomb":
                    if(isHit)
                    {
                        isDead = true;
                        isDangerous = false;
                        GameState.soundBank.PlayCue("Explosion");
                        GameState.objectManager.AddDeadEnemy(this);
                    }
                    else
                    {
                        isDead = true;
                        isDangerous = false;
                        GameState.soundBank.PlayCue("Explosion");
                        GameState.objectManager.AddDeadEnemy(this);

                        playerPositions = GameState.objectManager.players.Get_Player_Positions();
                        if (MathHelper.Distance(myPos.X, playerPositions[0].X) < 32 && MathHelper.Distance(myPos.Y, playerPositions[0].Y) < 32 && MathHelper.Distance(myPos.X, playerPositions[1].X) < 32 && MathHelper.Distance(myPos.Y, playerPositions[1].Y) < 32)
                        {
                            //Do not spawn the pit
                        }
                        else
                        {
                            GameState.objectManager.SpriteList.AddLast(new AnimatedSprite(GameState.BuildingSprite[1], 1, 32, 32, 60, new Vector2(myPos.X, myPos.Y), true));
                        }
                    }
                    break;
                case "Bullet":
                    GameState.objectManager.RemoveBullet(this);
                    isDead = true;
                    break;
                case "Fireball":
                    GameState.objectManager.RemoveBullet(this);
                    isDead = true;
                    break;
                case "GattlingBullet":
                    GameState.objectManager.RemoveBullet(this);
                    isDead = true;
                    break;
                case "MovingTurret":
                    GameState.soundBank.PlayCue("Explosion");
                    GameState.objectManager.AddExplosion(new Explosion("Explosion2",myPos));
                    GameState.objectManager.AddDeadEnemy(this);
                    isDead = true;
                    break;
                case "BossTurret":
                    isDead = true;
                    isDangerous = false;
                    GameState.soundBank.PlayCue("Explosion");
                    break;
                case "BossJumper":
                    isDead = true;
                    isDangerous = false;
                    GameState.soundBank.PlayCue("Explosion");
                    bossTargetTimer = 0;
                    break;
                default:
                    GameState.soundBank.PlayCue("Explosion");
                    GameState.objectManager.AddDeadEnemy(this);
                    isDead = true;
                    break;
            }
        }

        public void Take_Damage( int damageTaken )
        {

            GameState.soundBank.PlayCue("energyFry");


            //testing adding in sparks, maybe should have an add explosio
            GameState.objectManager.AddExplosion(new Explosion("spark", new Vector2(myPos.X + 8, myPos.Y + 8)));


            if (!isDead)
            {
                health -= damageTaken;
                if (health <= 0)
                {
                    Death();
                    if (myType != "Rubble")
                    {
                        GameState.objectManager.players.players[0].data_enemiesDefeated++;
                        GameState.objectManager.players.players[1].data_enemiesDefeated++;
                    }
                }
            }

            //testing to make enemies shake when taking damage
            if (shake.X == shakeOffset)
                shake.X = -shakeOffset;
            else
                shake.X = shakeOffset;

            takingDamage = true;

        }

        public void Take_Damage(int damageTaken, String impactPoints)
        {
            GameState.soundBank.PlayCue("energyFry");


            if (impactPoints == "TL")
                GameState.objectManager.AddExplosion(new Explosion("spark", new Vector2(myPos.X + 4 * (myRect.Width / 16), myPos.Y + 4 * (myRect.Height / 16))));
            else if (impactPoints == "TR")
                GameState.objectManager.AddExplosion(new Explosion("spark", new Vector2(myPos.X + myRect.Width - 4 * (myRect.Width / 16) - 16, myPos.Y + 4 * (myRect.Height / 16))));
            else if (impactPoints == "BL")
                GameState.objectManager.AddExplosion(new Explosion("spark", new Vector2(myPos.X + 4 * (myRect.Width / 16), myPos.Y + myRect.Height - 4 * (myRect.Height / 16) - 16)));
            else if (impactPoints == "RB")
                GameState.objectManager.AddExplosion(new Explosion("spark", new Vector2(myPos.X + myRect.Width - (4 * (myRect.Width / 16)) - 16, myPos.Y + myRect.Height - (4 * (myRect.Height / 16)) - 16)));
            else
                GameState.objectManager.AddExplosion(new Explosion("spark", new Vector2(myPos.X + (myRect.Width / 2) - 8, myPos.Y + (myRect.Height / 2) - 8)));

            if (!isDead)
            {
                health -= damageTaken;
                if (health <= 0)
                {
                    Death();
                    if (myType != "Rubble")
                    {
                        GameState.objectManager.players.players[0].data_enemiesDefeated++;
                        GameState.objectManager.players.players[1].data_enemiesDefeated++;
                    }
                }
            }

            //testing to make enemies shake when taking damage
            if (shake.X == shakeOffset)
                shake.X = -shakeOffset;
            else
                shake.X = shakeOffset;

            takingDamage = true;

        }

        public void Update(GameTime gameTime)
        {
            if ((myPos.X > -250 && myPos.X < 1070) || myType == "BossHoming" || myType == "BossHoming++" || myType == "Laser") //if it's near the screen, it updates
            {
                if (targetPlayer == -1) //if the target player is DEFAULT, then find the closest player and set them to default
                    findTarget(gameTime);

                if (takingDamage)
                {
                    takingDamage = false;
                }
                else
                {
                    shake.X = 0;
                }
                    

                switch (myDir)
                {
                    case "Left":
                        nextPos = myPos;
                        nextPos.X = nextPos.X - speed;
                        if (!Collides(gameTime))
                            myPos = nextPos;
                        else
                            myDir = "Right";
                        break;
                    case "Right":
                        nextPos = myPos;
                        nextPos.X = nextPos.X + speed;
                        if (!Collides(gameTime))
                            myPos = nextPos;
                        else
                            myDir = "Left";
                        break;
                    case "Up":
                        nextPos = myPos;
                        nextPos.Y = nextPos.Y - speed;
                        if (!Collides(gameTime))
                            myPos = nextPos;
                        else
                            myDir = "Down";
                        break;
                    case "Down":
                        nextPos = myPos;
                        nextPos.Y = nextPos.Y + speed;
                        if (!Collides(gameTime))
                            myPos = nextPos;
                        else
                            myDir = "Up";
                        break;
                    case "None":
                        if (attackTimer >= RoF)
                        {
                            GameState.objectManager.AddBullet(new Enemy("Bullet", bulletDirection, (myPos + turretOffset) ));
                            attackTimer = 0;
                            GameState.soundBank.PlayCue("Shot");
                        }
                        else
                            attackTimer = attackTimer + 1;
                        break;
                    case "Aimed":
                        if (attackTimer >= RoF)
                        {
                            GameState.objectManager.AddBullet(new Enemy("Bullet", bulletDirection, (myPos + turretOffset)));
                            attackTimer = 0;
                            GameState.soundBank.PlayCue("Shot");
                        }
                        else
                            attackTimer = attackTimer + 1;
                        break;
                    case "Homing":
                        playerPositions = GameState.objectManager.players.Get_Player_Positions(); //gets both player positions
                        target = playerPositions[targetPlayer] - myPos;
                        target.Normalize();
                        nextPos = myPos + (target * speed);
                        myPos = nextPos;
                        break;
                    case "Bullet":
                        bulletLife--;
                        if (bulletLife <= 0)
                            Death();
                        myPos = myPos + bulletDirection;  
                        break;
                    case "Fireball":
                        bulletLife--;
                        if (bulletLife <= 0)
                            Death();
                        myPos = myPos + bulletDirection*2;
                        break;
                    case "GattlingBullet": //Adding Gatling Bullet
                        bulletLife--;
                        if (bulletLife <= 0)
                            Death();
                        myPos = myPos + bulletDirection*4;  
                        break;
                    case "Spawner":
                        if (!isDead)
                        {
                            if(TurretsPlaced == false)
                            {
                                GameState.objectManager.AddEnemy(new Enemy("Turret", "None",
                               new Vector2(myPos.X + 32,
                                   myPos.Y + 32)));
                                GameState.objectManager.AddEnemy(new Enemy("Turret", "None",
                               new Vector2(myPos.X + 96,
                                   myPos.Y + 32)));
                                TurretsPlaced = true;
                            }
                        }
                        else
                        {
                            bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                            deathTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (bossTargetTimer >= 500)
                            {
                                GameState.soundBank.PlayCue("Explosion");
                                float randomx = myPos.X + randomTimeSwitch.Next(-32, 128);
                                float randomy = myPos.Y + randomTimeSwitch.Next(-32, 128);
                                GameState.objectManager.AddExplosion(new Explosion(new Vector2(randomx, randomy)));
                                bossTargetTimer = 0;
                            }
                            if (deathTimer >= 3000)
                            {
                                GameState.objectManager.AddDeadEnemy(this);
                                GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X, myPos.Y), "Victory"));
                                GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X, myPos.Y + 96), "Victory"));
                                GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X + 96, myPos.Y), "Victory"));
                                GameState.objectManager.AddZone(new Zone(new Vector2(myPos.X + 96, myPos.Y + 96), "Victory"));
                            }
                        }
                        break;

                    case "BossHoming":
                        if (!isDead)
                        {
                            if (GameState.freezeTime <= 0 && GameState.GameSpeed == 0)
                                isInvincible = false;
                            else
                                isInvincible = true;

                            bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (bossTargetTimer >= 3000)  //We want the boss to switch targets every few seconds.
                            {
                                if (targetPlayer == 0)
                                    targetPlayer = 1;
                                else
                                    targetPlayer = 0;
                                bossTargetTimer = 0;
                            }
                            playerPositions = GameState.objectManager.players.Get_Player_Positions(); //gets both player positions
                            target = playerPositions[targetPlayer] - myPos;
                            target.Normalize();
                            nextPos = myPos + (target * speed);
                            myPos = nextPos;
                        }
                        else
                        {
                            bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                            deathTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if(bossTargetTimer >= 500)
                            {
                                GameState.soundBank.PlayCue("Explosion");
                                float randomx = myPos.X + randomTimeSwitch.Next(-32, 128);
                                float randomy = myPos.Y + randomTimeSwitch.Next(-32, 128);
                                GameState.objectManager.AddExplosion(new Explosion("green", new Vector2(randomx, randomy)));
                                bossTargetTimer = 0;
                            }
                            if(deathTimer >= 3000)
                            {
                                GameState.objectManager.AddDeadEnemy(this);
                                GameState.objectManager.AddZone(new Zone(new Vector2(4736 - GameState.DistanceTraversed, 288), "Victory"));
                                GameState.objectManager.AddZone(new Zone(new Vector2(4736 - GameState.DistanceTraversed, 448), "Victory"));
                            }
                        }
                        break;
                    case "BossHoming++":
                        if (!isDead)
                        {
                            

                            bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (bossTargetTimer >= 3000)  //We want the boss to switch targets every few seconds.
                            {
                                if (targetPlayer == 0)
                                    targetPlayer = 1;
                                else
                                    targetPlayer = 0;
                                bossTargetTimer = 0;
                            }
                            if (GameState.freezeTime <= 0 && GameState.GameSpeed == 0)
                                isInvincible = false;
                            else
                                isInvincible = true;
                            if (GameState.freezeTime > 0)
                                speed = 0.5f;
                            else
                                speed = 1f;
                            playerPositions = GameState.objectManager.players.Get_Player_Positions(); //gets both player positions
                            target = playerPositions[targetPlayer] - myPos;
                            target.Normalize();
                            nextPos = myPos + (target * speed);
                            myPos = nextPos;

                            if (currentSprite == 4 && FireballSpawned == false)
                            {
                                //Trying something -Cody
                                /*
                                currentFBcount = 0;
                                while (currentFBcount < fireballs)
                                {
                                    GameState.objectManager.AddBullet(new Enemy("Fireball", target, (myPos + HomingBossOffset)));
                                    currentFBcount++;
                                }*/
                                if(currentFBcount> fireballs)
                                {
                                    currentFBcount = 0;
                                    GameState.objectManager.AddBullet(new Enemy("Fireball", target, (myPos + HomingBossOffset)));
                                }
                                currentFBcount++;

                                FireballSpawned = true;
                            }
                            if (currentSprite == 5 || currentSprite == 3)
                                FireballSpawned = false;
                        }
                        else
                        {
                            bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                            deathTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (bossTargetTimer >= 500)
                            {
                                GameState.soundBank.PlayCue("Explosion");
                                float randomx = myPos.X + randomTimeSwitch.Next(-32, 128);
                                float randomy = myPos.Y + randomTimeSwitch.Next(-32, 128);
                                GameState.objectManager.AddExplosion(new Explosion("green", new Vector2(randomx, randomy)));
                                bossTargetTimer = 0;
                            }
                            if (deathTimer >= 3000)
                            {
                                GameState.objectManager.AddDeadEnemy(this);
                                GameState.objectManager.AddZone(new Zone(new Vector2(5888 - GameState.DistanceTraversed, 256), "Victory"));
                                GameState.objectManager.AddZone(new Zone(new Vector2(5888 - GameState.DistanceTraversed, 352), "Victory"));
                            }
                        }
                        break;
                    case "BossBomber":
                        if (!isDead)
                        {
                            bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (bossTargetTimer >= 3000)  //We want the boss to attack every few seconds.
                            {
                                playerPositions = GameState.objectManager.players.Get_Player_Positions(); //gets both player positions

                                target = playerPositions[0];
                                GameState.objectManager.AddEnemy(new Enemy("Bomb", "Bomb", new Vector2(myPos.X, myPos.Y), target));
                                GameState.objectManager.AddEnemy(new Enemy("Bomb", "Bomb", new Vector2(myPos.X + 64, myPos.Y + 64), target));

                                target = playerPositions[1];
                                GameState.objectManager.AddEnemy(new Enemy("Bomb", "Bomb", new Vector2(myPos.X + 63, myPos.Y), target));
                                GameState.objectManager.AddEnemy(new Enemy("Bomb", "Bomb", new Vector2(myPos.X, myPos.Y + 64), target));

                                bossTargetTimer = 0;
                            }
                        }
                        else
                        {
                            bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                            deathTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (bossTargetTimer >= 500)
                            {
                                GameState.soundBank.PlayCue("Explosion");
                                bossTargetTimer = 0;
                            }
                            if (deathTimer >= 3000)
                            {
                                GameState.objectManager.AddDeadEnemy(this);
                                GameState.objectManager.AddZone(new Zone(new Vector2(512 - GameState.DistanceTraversed, 288), "Victory"));
                                GameState.objectManager.AddZone(new Zone(new Vector2(512 - GameState.DistanceTraversed, 448), "Victory"));
                            }
                        }
                        break;
                    case "Bomb":
                        target = myTarget - myPos;
                        if (Math.Abs(target.X) < Math.Abs(speed) && Math.Abs(target.Y) < Math.Abs(speed) && !isDead)
                        {
                            Death();
                        }
                        else
                        {
                            target.Normalize();
                            nextPos = myPos + (target * speed);
                            myPos = nextPos;
                        }
                        break;
                    case "BossJumper":
                        if (!isDead)
                        {
                            bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (isDangerous) //If the jumper is grounded
                            {
                                if (bossTargetTimer >= 4000) //The jumper will remain grounded for 6 seconds then quickly leap into the air. This triggers when he is prepared to leap.
                                {
                                    targetPlayer = 1;
                                    if ((int)gameTime.TotalGameTime.TotalMilliseconds % 2 == 0) // 50:50 chance of targeting each player.
                                        targetPlayer = 0;
                                    isDangerous = false;
                                    jumpStartTime = gameTime.TotalGameTime.TotalMilliseconds;
                                    jumpEndTime = gameTime.TotalGameTime.TotalMilliseconds + 1000;
                                    isInvincible = true;
                                }
                            }
                            else //If the jumper is airborn
                            {
                                if (bossTargetTimer < 5250) //The start of jumper's ascension
                                {
                                    jumpY = MathHelper.Lerp(0, -1200, (float)((gameTime.TotalGameTime.TotalMilliseconds - jumpStartTime) / (jumpEndTime - jumpStartTime)));
                                }
                                else if (bossTargetTimer >= 5250 && bossTargetTimer <= 6000)  //The jumper will have completely left the screen by 7000 and will re-emerge at 8000.
                                //During this time, his position needs to change to the new targeted player.
                                {
                                    myPos = GameState.objectManager.players.Get_Player_Positions()[targetPlayer];
                                    myPos += new Vector2(-40, -40);
                                    jumpY = -800;
                                    jumpStartTime = gameTime.TotalGameTime.TotalMilliseconds;
                                    jumpEndTime = gameTime.TotalGameTime.TotalMilliseconds + 1000;
                                }
                                else if (bossTargetTimer > 6000 && bossTargetTimer <= 7000) //The start of jumper's descension
                                {
                                    if(!fallingPlayed && bossTargetTimer >= 6500)
                                    {
                                        fallingPlayed = true;
                                        GameState.soundBank.PlayCue("Fall");
                                    }
                                    jumpY = MathHelper.Lerp(-800, 0, (float)((gameTime.TotalGameTime.TotalMilliseconds - jumpStartTime) / (jumpEndTime - jumpStartTime)));
                                }
                                else if (bossTargetTimer > 7000)
                                {
                                    fallingPlayed = false;
                                    jumpY = 0;
                                    bossTargetTimer = 0;
                                    isInvincible = false;
                                    isDangerous = true;
                                    GameState.soundBank.PlayCue("Thud");
                                    GameState.objectManager.AddEnemy(new Enemy("Rubble", "Rubble", GameState.objectManager.players.Get_Player_Positions()[0]));
                                    GameState.objectManager.AddEnemy(new Enemy("Rubble", "Rubble", GameState.objectManager.players.Get_Player_Positions()[1]));
                                    GameState.objectManager.AddEnemy(new Enemy("Rubble", "Rubble", new Vector2(myPos.X + 120, myPos.Y - 130)));
                                    GameState.objectManager.AddEnemy(new Enemy("Rubble", "Rubble", new Vector2(myPos.X + 150, myPos.Y + 90)));
                                    GameState.objectManager.AddEnemy(new Enemy("Rubble", "Rubble", new Vector2(myPos.X - 70, myPos.Y - 140)));
                                    GameState.objectManager.AddEnemy(new Enemy("Rubble", "Rubble", new Vector2(myPos.X - 80, myPos.Y + 110)));
                                    GameState.objectManager.AddEnemy(new Enemy("Rubble", "Rubble", new Vector2(myPos.X + 150, myPos.Y)));
                                    GameState.objectManager.AddEnemy(new Enemy("Rubble", "Rubble", new Vector2(myPos.X, myPos.Y + 90)));
                                    GameState.objectManager.AddEnemy(new Enemy("Rubble", "Rubble", new Vector2(myPos.X, myPos.Y - 110)));
                                    GameState.objectManager.AddEnemy(new Enemy("Rubble", "Rubble", new Vector2(myPos.X - 70, myPos.Y)));
                                }
                            }
                        }
                        else
                        {
                            bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                            deathTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if (bossTargetTimer >= 500)
                            {
                                GameState.soundBank.PlayCue("Explosion");
                                float randomx = myPos.X + randomTimeSwitch.Next(-32, 128);
                                float randomy = myPos.Y + randomTimeSwitch.Next(-32, 128);
                                GameState.objectManager.AddExplosion(new Explosion("green", new Vector2(randomx, randomy)));
                                bossTargetTimer = 0;
                            }
                            if (deathTimer >= 3000)
                            {
                                GameState.objectManager.AddDeadEnemy(this);
                                GameState.objectManager.AddZone(new Zone(new Vector2(512, 256), "Victory"));
                                GameState.objectManager.AddZone(new Zone(new Vector2(512, 352), "Victory"));
                            }
                        }
                        break;
                    case "Rubble":
                        if (jumpY < 0)
                        {
                            if (jumpStartTime == 0)
                            {
                                jumpStartTime = gameTime.TotalGameTime.TotalMilliseconds;
                                jumpEndTime = gameTime.TotalGameTime.TotalMilliseconds + attackTimer;
                            }
                            if(jumpY > -500 && !fallingPlayed)
                            {
                                fallingPlayed = true;
                                GameState.soundBank.PlayCue("Fall");
                            }
                            jumpY = MathHelper.Lerp(-attackTimer / 1.8f, 0, (float)((gameTime.TotalGameTime.TotalMilliseconds - jumpStartTime) / (jumpEndTime - jumpStartTime)));
                            if (jumpY >= 0)
                            {
                                jumpY = 0;
                                GameState.soundBank.PlayCue("Thud");
                                isDangerous = true;
                                isTraversable = false;
                                isInvincible = false;
                            }
                        }
                        else
                        {
                            isDangerous = false;
                        }
                        break;
                    case "BossTurret":
                        BossTurretUpdate(gameTime);
                        break;
                    case "MovingTurret":
                        MovingTurretUpdate(gameTime);
                        break;
                    case "GattlingGun":
                        GattlingGunUpdate(gameTime);
                        break;
                    case "BossLaser":
                        if (!isDead)
                        {
                            angle = (float)(Math.PI * 1.5) - (float)Math.Atan2(myPos.X - targetPos.X, myPos.Y - targetPos.Y);
                            angle = MathHelper.ToDegrees((float)angle);

                            if (cooldown <= 0) //if it's ready to attack
                            {
                                randomAttack = rand.Next(8);

                                if (randomAttack == 0) 
                                {
                                    target.X = myPos.X + laserBossOffset.X + 448;
                                    target.Y = myPos.Y + laserBossOffset.Y - 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, -896f, 0f));
                                    target.X = myPos.X + laserBossOffset.X - 448;
                                    target.Y = myPos.Y + laserBossOffset.Y + 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, 896f, 0f));
                                    cooldown = 400;
                                }
                                else if (randomAttack == 1)
                                {
                                    target.X = myPos.X + laserBossOffset.X + 448;
                                    target.Y = myPos.Y + laserBossOffset.Y + 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, -896f, 0f));
                                    target.X = myPos.X + laserBossOffset.X - 448;
                                    target.Y = myPos.Y + laserBossOffset.Y - 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, 896f, 0f));
                                    cooldown = 400;
                                }
                                else if (randomAttack == 2) 
                                {
                                    target.X = myPos.X + laserBossOffset.X + 448;
                                    target.Y = myPos.Y + laserBossOffset.Y - 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, 0f, 480f));
                                    target.X = myPos.X + laserBossOffset.X - 448;
                                    target.Y = myPos.Y + laserBossOffset.Y + 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, 0f, -480f));
                                    cooldown = 400;
                                }
                                else if (randomAttack == 3)
                                {
                                    target.X = myPos.X + laserBossOffset.X - 448;
                                    target.Y = myPos.Y + laserBossOffset.Y - 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, -480f, 0f));
                                    target.X = myPos.X + laserBossOffset.X + 448;
                                    target.Y = myPos.Y + laserBossOffset.Y + 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, 480f, 0f));
                                    cooldown = 400;
                                }
                                else if (randomAttack == 4) 
                                {
                                    target.X = myPos.X + laserBossOffset.X + 448;
                                    target.Y = myPos.Y + laserBossOffset.Y - 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, 0f, 480f));
                                    target.X = myPos.X + laserBossOffset.X - 448;
                                    target.Y = myPos.Y + laserBossOffset.Y - 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, 0f, 480f));
                                    cooldown = 400;
                                }
                                else if (randomAttack == 5)
                                {
                                    target.X = myPos.X + laserBossOffset.X + 448;
                                    target.Y = myPos.Y + laserBossOffset.Y + 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, 0f, -480f));
                                    target.X = myPos.X + laserBossOffset.X - 448;
                                    target.Y = myPos.Y + laserBossOffset.Y + 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, 0f, -480f));
                                    cooldown = 400;
                                }
                                else if (randomAttack == 6)
                                {
                                    target.X = myPos.X + laserBossOffset.X + 448;
                                    target.Y = myPos.Y + laserBossOffset.Y - 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, -896f, 0f));
                                    target.X = myPos.X + laserBossOffset.X + 448;
                                    target.Y = myPos.Y + laserBossOffset.Y + 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, -896f, 0f));
                                    cooldown = 400;
                                }
                                else if (randomAttack == 7)
                                {
                                    target.X = myPos.X + laserBossOffset.X - 448;
                                    target.Y = myPos.Y + laserBossOffset.Y - 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, 896f, 0f));
                                    target.X = myPos.X + laserBossOffset.X - 448;
                                    target.Y = myPos.Y + laserBossOffset.Y + 240;
                                    GameState.objectManager.AddLaser(new Laser((myPos + laserBossOffset), target, 100, 896f, 0f));
                                    cooldown = 400;
                                }
                            }
                            cooldown--;
                        }
                        else
                        {
                            bossTargetTimer += gameTime.ElapsedGameTime.Milliseconds;
                            deathTimer += gameTime.ElapsedGameTime.Milliseconds;
                            if(bossTargetTimer >= 500)
                            {
                                GameState.soundBank.PlayCue("Explosion");
                                float randomx = myPos.X + randomTimeSwitch.Next(-32, 96);
                                float randomy = myPos.Y + randomTimeSwitch.Next(-32, 96);
                                GameState.objectManager.AddExplosion(new Explosion("green", new Vector2(randomx, randomy)));
                                bossTargetTimer = 0;
                            }
                            if(deathTimer >= 3000)
                            {
                                GameState.objectManager.AddDeadEnemy(this);
                                GameState.objectManager.AddZone(new Zone(new Vector2(480, 288), "Victory"));
                                GameState.objectManager.AddZone(new Zone(new Vector2(608, 288), "Victory"));
                            }
                        }
                        break;
                    case "Laser":
                        if (attackTimer <= 0)
                        {
                            
                            GameState.objectManager.AddLaser(new Laser(myPos + laserOffset, targetPos + laserOffset, RoF));
                           // angle =  MathHelper.ToDegrees(GameState.objectManager.laserList.Last.Value.lineAngle);
                            angle = (float)(Math.PI * 1.5) - (float)Math.Atan2(myPos.X - targetPos.X, myPos.Y - targetPos.Y);
                            angle = MathHelper.ToDegrees((float)angle);
                           
                            attackTimer = cooldown + RoF;
                        }
                        attackTimer--;
                        break;
                    case "LaserEnd":
                        //sits there and looks pretty. might need to add animation/effects here, or maybe just get rid of this
                        break;
                    default:
                        break;
                }

                Update_Sprite(gameTime);
            }

            myPos.X = myPos.X - GameState.GameSpeed; //autoscrolling
            targetPos.X = targetPos.X - GameState.GameSpeed;
            bounds.X = bounds.X - GameState.GameSpeed;
            Set_Rectangle_Position();


        }

        public void findTarget(GameTime gameTime)
        {
            playerPositions = GameState.objectManager.players.Get_Player_Positions();

            if (Vector2.Distance(playerPositions[0], myPos) < Vector2.Distance(playerPositions[1], myPos))
                targetPlayer = 0;
            else
                targetPlayer = 1;
        }
        public void Draw(GameTime gameTime)
        {
            if ((myPos.X > -250 && myPos.X < 1070) || myType == "BossHoming" || (myPos.X < 1070 && myType == "BossJumper")) //if it's near the screen, it updates
            {
                if (myType == "Bullet" || myType == "GattlingBullet")
                    GameState.spriteBatch.Draw(myTexture, myPos, new Rectangle(currentSprite * 16, 0, 16, 16), Color.White);
                else if (myType == "BossHoming" || myType == "BossHoming++" || myType == "BossLaser") //added this to make boss draw  //G: Threw my boss in here too, same size and such
                {
                    GameState.spriteBatch.Draw(myTexture, (myPos + new Vector2(5, 5) + shake), new Rectangle(currentSprite * 96, 96 * currentSpriteColumn, 96, 96), Color.White);
                    if (isInvincible)
                    {
                        GameState.spriteBatch.Draw(invincibilityShield, (myPos + shake), new Rectangle(shieldSprite * 106, 0, 106, 106), Color.White);
                    }
                }
                else if (myType == "Spawner") //added this to make boss draw
                    GameState.spriteBatch.Draw(myTexture, (myPos + shake), new Rectangle(currentSprite * 170, currentSpriteColumn * 224, 170, 224), Color.White);
                else if (myType == "BossBomber")
                    GameState.spriteBatch.Draw(myTexture, (myPos + shake), new Rectangle(currentSprite * 96, 0, 96, 96), Color.White);
                else if (myType == "BossJumper")
                {
                    GameState.spriteBatch.Draw(secondaryTexture, new Rectangle((int)(myPos.X + shake.X), (int)(myPos.Y + shake.Y + 24), 64, 64), new Rectangle(secondaryCurrentSprite * 32, 0, 32, 32), Color.White);
                    GameState.spriteBatch.Draw(myTexture, (myPos + shake + new Vector2(0, jumpY)), new Rectangle(currentSprite * 64, 0, 64, 64), Color.White);
                }
                else if (myType == "Rubble")
                {
                    if (jumpY > 0)
                        GameState.spriteBatch.Draw(secondaryTexture, (myPos + shake), new Rectangle(secondaryCurrentSprite * 32, 0, 32, 32), Color.White);
                    GameState.spriteBatch.Draw(myTexture, (myPos + shake + new Vector2(0, jumpY)), new Rectangle(currentSprite * 32, 032 * currentSpriteColumn, 32, 32), Color.White);
                }
                else if (myType == "MovingTurret")
                {
                    GameState.spriteBatch.Draw(myTexture, (myPos + shake), new Rectangle(currentSprite * 32, 32 * currentSpriteColumn, 32, 32), Color.White);
                }
                else if (myType == "GattlingGun")
                    GameState.spriteBatch.Draw(myTexture, (myPos + shake), new Rectangle(currentSprite * 32, 32 * currentSpriteColumn, 32, 32), Color.White);
                else if (myType == "BossTurret")
                {
                    //Draw Turret
                    if (!SubEnemies[2].isDead)
                        GameState.spriteBatch.Draw(SubEnemies[2].myTexture, (SubEnemies[2].myPos + shake), new Rectangle(SubEnemies[2].currentSprite * 32, 32 * SubEnemies[2].currentSpriteColumn, 32, 32), Color.White);

                    //Draw shoulder 1
                    if (!SubEnemies[0].isDead)
                        GameState.spriteBatch.Draw(SubEnemies[0].myTexture, (SubEnemies[0].myPos + shake), new Rectangle(SubEnemies[0].currentSprite * 32, 32 * SubEnemies[0].currentSpriteColumn, 32, 32), Color.White);
                    //Draw shoulder 2
                    if (!SubEnemies[1].isDead)
                        GameState.spriteBatch.Draw(SubEnemies[1].myTexture, (SubEnemies[1].myPos + shake), new Rectangle(SubEnemies[1].currentSprite * 32, 32 * SubEnemies[1].currentSpriteColumn, 32, 32), Color.White);

                    //Draw shoulder 3
                    if (!SubEnemies[3].isDead)
                        GameState.spriteBatch.Draw(SubEnemies[3].myTexture, (SubEnemies[3].myPos + shake), new Rectangle(SubEnemies[3].currentSprite * 32, 32 * SubEnemies[3].currentSpriteColumn, 32, 32), Color.White);
                    //Draw shoulder 4
                    if (!SubEnemies[4].isDead)
                        GameState.spriteBatch.Draw(SubEnemies[4].myTexture, (SubEnemies[4].myPos + shake), new Rectangle(SubEnemies[4].currentSprite * 32, 32 * SubEnemies[4].currentSpriteColumn, 32, 32), Color.White);


                    for (int i = 0; i < distance.Length; i++)
                    {
                        int temp = i;
                        if (temp >= 2)
                            temp++;
                        if (!SubEnemies[temp].isDead && poweredOn)
                        {
                            if (distance[i] < 100)
                                GameState.spriteBatch.Draw(lineTexture_short, new Rectangle((int)myPos.X + 32, (int)myPos.Y + 32, (int)distance[i], 8), new Rectangle(80 * linecurrentsprite, 0, (int)distance[i], 8), Color.White, lineAngle[i], origin, SpriteEffects.None, 0);
                            else if (distance[i] < 150)
                                GameState.spriteBatch.Draw(lineTexture_med, new Rectangle((int)myPos.X + 32, (int)myPos.Y + 32, (int)distance[i], 8), new Rectangle(100 * linecurrentsprite, 0, (int)distance[i], 8), Color.White, lineAngle[i], origin, SpriteEffects.None, 0);
                            else
                                GameState.spriteBatch.Draw(lineTexture, new Rectangle((int)myPos.X + 32, (int)myPos.Y + 32, (int)distance[i], 8), new Rectangle(150 * linecurrentsprite, 0, (int)distance[i], 8), Color.White, lineAngle[i], origin, SpriteEffects.None, 0);
                        }
                    }
                    //Draw base
                    GameState.spriteBatch.Draw(myTexture, (myPos + shake), new Rectangle(currentSprite * 64, 64 * currentSpriteColumn, 64, 64), Color.White);
                }
                else if (myType=="Laser")
                {
                    GameState.spriteBatch.Draw(myTexture, (myPos + shake), new Rectangle(currentSprite * 32, currentSpriteColumn*32, 32, 32), Color.White);
                }
                else
                    GameState.spriteBatch.Draw(myTexture, (myPos + shake), new Rectangle(currentSprite * 32, 0, 32, 32), Color.White);
                //DrawCollisionBox(gameTime); //for testing collision   Breaks the game when turned on?
            }
        }
    }
}
