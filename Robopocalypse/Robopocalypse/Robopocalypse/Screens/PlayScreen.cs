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

namespace Robopocalypse.Screens
{
    public class PlayScreen : GameScreen
    {

        //Temporary terrain
        //private Terrain terrain;
        int[] SpawnPoint = new int[] { 0, 0, 0, 0, 0, 0 };
        int count = 0;
        Texture2D rect;
        Color color;

        public PlayScreen()
        {
            //GameState.cue.Stop(AudioStopOptions.AsAuthored);

            GameState.levelStart = false;

            //GameState.objectManager.LevelView.Reset();

            color = new Color(0, 0, 0, 200);

            rect = new Texture2D(GameState.graphicsDevice, GameState.SCREEN_WIDTH, GameState.SCREEN_HEIGHT, false, SurfaceFormat.Color);
	        Color[] colorData = new Color[GameState.SCREEN_WIDTH * GameState.SCREEN_HEIGHT];

            for (int i = 0; i < GameState.SCREEN_WIDTH * GameState.SCREEN_HEIGHT; i++)
                colorData[i] = color;
            rect.SetData<Color>(colorData);

            GameState.cue = GameState.soundBank.GetCue("MusicTrack1");
            GameState.gameOver = false;
            GameState.objectManager = new Managers.GameObjectsManager();
            GameState.objectManager.LoadManagers();

            //GameState.cue.Play();
        }

        private void Level_Ramp_Timer(GameTime gameTime)
        {
            //THIS IS EXPERIMENTAL. GIVES THE PLAYER 2 SECONDS TO LOOK AROUND AND MOVE BEFORE THE LEVEL STARTS MOVING.
            //IF WE DECIDE TO REMOVE THIS FEATURE, REMEMBER TO RESET GAMESTATE.GAMESPEED = 1 IN STRUCTURE.CS     -Cody
            if (GameState.levelLoadTimer < 2000 && GameState.levelLoadTimer >= 0)
                GameState.levelLoadTimer += gameTime.ElapsedGameTime.Milliseconds;
            else if (GameState.levelLoadTimer >= 2000)
            {
                GameState.levelLoadTimer = -1;
                GameState.GameSpeed = 1;
            }
        }

        public override void Update(GameTime gameTime)
        {

            if (GameState.cue.IsPaused) //resumes the music if it's paused
                GameState.cue.Resume();

            if(!GameState.cue.IsPlaying)
                GameState.cue.Play();

            //This keeps track of the freeze time powerup.
            if (GameState.freezeTime > 0)
            {
                GameState.freezeTime -= gameTime.ElapsedGameTime.Milliseconds;
                if (GameState.freezeTime <= 0)
                    GameState.GameSpeed = GameState.prevGameSpeed;
            }


            //Nick's stuff
            if (GameState.inputManager.Start(0) || GameState.inputManager.Start(1) || GameState.inputManager.XboxButton(0) || GameState.inputManager.XboxButton(1))
            {
                if (GameState.cue.IsPlaying) //pauses the music if it's playing
                    GameState.cue.Pause();
                GameState.soundBank.PlayCue("Pause");
                GameState.screenManager.Push(new PauseScreen());
            }

            //Checks to see if level has started in order to show story/help text
            if (!GameState.levelStart)
            {
                if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    GameState.levelStart = true;
            }
            else
            {
                Level_Ramp_Timer(gameTime);
                GameState.DistanceTraversed += GameState.GameSpeed;
                GameState.objectManager.Update(gameTime);
            }
   

        }//End Update

        public override void Draw(GameTime gameTime)
        {
            
            GameState.objectManager.Draw(gameTime);

            count = 0;
            if (!GameState.levelStart)
            {
                GameState.spriteBatch.Draw(rect, Vector2.Zero, color);
                foreach (String s in GameState.LevelText)
                {
                    GameState.spriteBatch.DrawString(GameState.font, s, new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString(s).X / 2, GameState.SCREEN_HEIGHT / 2 + 20 * count - GameState.LevelText.Length * 10), Color.White);
                    count++;
                }
            }
            GameState.objectManager.players.Draw_Line(gameTime);

            //Draw FPS to screen
            if(GameState.debug)
                GameState.spriteBatch.DrawString(GameState.font, GameState.fps.fps, new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString(GameState.fps.fps).X / 2, 700), Color.White);

                                            //spriteBatch needs to end before we begin primitiveBatch in order for the line to render properly.
            GameState.spriteBatch.End(); //If you need to use spriteBatch, please use it in the previous block.
            GameState.sbHasEnded = true;
            GameState.primitiveBatch.Begin(PrimitiveType.LineList);//If you need to use primitiveBatch, please use it in this block.
            GameState.primitiveBatch.End();
        }//End Draw
    }
}
