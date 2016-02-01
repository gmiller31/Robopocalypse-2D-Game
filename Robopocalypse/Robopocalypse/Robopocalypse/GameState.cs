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
    public class GameState
    {

        //adding this here for frost effect
        public static Texture2D frostsprite;
        public static GlobalSpriteEffect frostexplosions;

        public static Game game;
        public static GraphicsDevice graphicsDevice;
        public static GraphicsDeviceManager graphicsManager;
        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static PrimitiveBatch primitiveBatch;
        public static ScreenManager screenManager;
        public static FPS fps;
        public static KeyboardInput keyboardInput;
        public static GamePadInput gamePadInput;
        public static String[] BuildingSprite = new String[] { @"Textures\Environment\WallTile_PurpleGold", @"Textures\Environment\pitfall", @"Textures\Environment\FloorTile_PurpleGold" };
        public static Structure Structure;
        public static SpriteFont font;

        public static bool sbHasEnded; //We need to end SpriteBatch early for some PrimitiveBatch elements to render properly. This will keep track of if it has ended already to stay error free at runtime

        public static AudioEngine audioEngine;
        public static SoundBank soundBank;
        public static WaveBank waveBank;
        public static Cue cue;

        public static Managers.GameObjectsManager objectManager;
        public static Managers.InputManager inputManager;

        public static int levelLoadTimer = 0;

        public static bool debug = false;

        //public static LinkedList<AnimatedSprite> SpriteList;
        //public static LinkedList<AnimatedSprite> BackgroundElements;

        //added this to keep track of time after death so animation can play, it was the easiest way I could think of doing it
        public static int deathtimelag = 0;
        public static int freezeTime = 0;
        public static int prevGameSpeed = 1;
        public static bool gameOver;
        public static int GameSpeed = 1;
        public static int DistanceTraversed = 0;
        public static int DistanceToVictory = 1000;
        public static int PlayerCount = 1;
        public static int currentLevel = 1;
        public static bool levelStart = false;
        public static string[] LevelText = new string[] {
            "This is the default level text!",
            "If you are seeing this then the",
            "level script has no text set up!",
            "Press Accept to Continue"
            };
        public static string[] DefaultLevelText = new string[] {
            "This is the default level text!",
            "If you are seeing this then the",
            "level script has no text set up!",
            "Press Accept to Continue"
            };

        public const int SCREEN_WIDTH = 1024;
        public const int SCREEN_HEIGHT = 768;
        public const int TITLE_SAFE_AREA = 32;

        //Volume Controls
        public static float allSounds = 1;
        public static float soundEffects = 1;
        public static float music = 1;
        public static AudioCategory musicCategory;
        public static AudioCategory soundCategory;

        //Button Config
        public static Buttons b_Accept1 = Buttons.A;
        public static Buttons b_Accept2 = Buttons.A;
        public static Buttons b_Cancel1 = Buttons.B;
        public static Buttons b_Cancel2 = Buttons.B;
        public static Buttons b_LShoulder = Buttons.LeftShoulder;
        public static Buttons b_RShoulder = Buttons.RightShoulder;
        public static Buttons b_Start = Buttons.Start;
        public static Buttons b_Up1 = Buttons.DPadUp;
        public static Buttons b_Down1 = Buttons.DPadDown;
        public static Buttons b_Left1 = Buttons.DPadLeft;
        public static Buttons b_Right1 = Buttons.DPadRight;
        public static Buttons b_Up2 = Buttons.Y;
        public static Buttons b_Down2 = Buttons.A;
        public static Buttons b_Left2 = Buttons.X;
        public static Buttons b_Right2 = Buttons.B;

        //Key Config
        public static Keys k_Accept1 = Keys.Space;
        public static Keys k_Accept2 = Keys.Enter;
        public static Keys k_Cancel1 = Keys.Back;
        public static Keys k_Cancel2 = Keys.Escape;
        public static Keys k_LShoulder = Keys.D1;
        public static Keys k_RShoulder = Keys.D2;
        public static Keys k_Start = Keys.Escape;
        public static Keys k_Up1 = Keys.W;
        public static Keys k_Down1 = Keys.S;
        public static Keys k_Left1 = Keys.A;
        public static Keys k_Right1 = Keys.D;
        public static Keys k_Up2 = Keys.Up;
        public static Keys k_Down2 = Keys.Down;
        public static Keys k_Left2 = Keys.Left;
        public static Keys k_Right2 = Keys.Right;

    }
}
