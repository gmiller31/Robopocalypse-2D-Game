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


namespace Robopocalypse_Library
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GamePadInput : Microsoft.Xna.Framework.GameComponent
    {
        GamePadState[] gpPrev;
        GamePadState[] gpCurr;

        Vector2[] lsPrev;
        Vector2[] lsCurr;

        Vector2[] rsPrev;
        Vector2[] rsCurr;

        public GamePadInput(Game game) : base(game)
        {
            lsPrev = new Vector2[4];
            lsCurr = new Vector2[4];

            lsPrev[0] = Vector2.Zero;
            lsPrev[1] = Vector2.Zero;
            lsPrev[2] = Vector2.Zero;
            lsPrev[3] = Vector2.Zero;

            rsPrev = new Vector2[4];
            rsCurr = new Vector2[4];

            rsPrev[0] = Vector2.Zero;
            rsPrev[1] = Vector2.Zero;
            rsPrev[2] = Vector2.Zero;
            rsPrev[3] = Vector2.Zero;

            gpPrev = new GamePadState[4];
            gpCurr = new GamePadState[4];

            gpPrev[0] = gpCurr[0] = GamePad.GetState(PlayerIndex.One);
            gpPrev[1] = gpCurr[1] = GamePad.GetState(PlayerIndex.Two);
            gpPrev[2] = gpCurr[2] = GamePad.GetState(PlayerIndex.Three);
            gpPrev[3] = gpCurr[3] = GamePad.GetState(PlayerIndex.Four);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            gpPrev[0] = gpCurr[0];
            gpCurr[0] = GamePad.GetState(PlayerIndex.One);
            lsPrev[0] = lsCurr[0];
            lsCurr[0] = gpCurr[0].ThumbSticks.Left;
            rsPrev[0] = rsCurr[0];
            rsCurr[0] = gpCurr[0].ThumbSticks.Right;

            gpPrev[1] = gpCurr[1];
            gpCurr[1] = GamePad.GetState(PlayerIndex.Two);
            lsPrev[1] = lsCurr[1];
            lsCurr[1] = gpCurr[1].ThumbSticks.Left;
            rsPrev[1] = rsCurr[1];
            rsCurr[1] = gpCurr[1].ThumbSticks.Right;

            gpPrev[2] = gpCurr[2];
            gpCurr[2] = GamePad.GetState(PlayerIndex.Three);
            lsPrev[2] = lsCurr[2];
            lsCurr[2] = gpCurr[2].ThumbSticks.Left;
            rsPrev[2] = rsCurr[2];
            rsCurr[2] = gpCurr[2].ThumbSticks.Right;

            gpPrev[3] = gpCurr[3];
            gpCurr[3] = GamePad.GetState(PlayerIndex.Four);
            lsPrev[3] = lsCurr[3];
            lsCurr[3] = gpCurr[3].ThumbSticks.Left;
            rsPrev[3] = rsCurr[3];
            rsCurr[3] = gpCurr[3].ThumbSticks.Right;

            base.Update(gameTime);
        }

        public Boolean isConnected(int gamePad)
        {
            if (gamePad == -1)
                return false;
            else
                return gpCurr[gamePad].IsConnected;
        }

        public Boolean isButtonPressed(Buttons button, int gamePad)
        {
            return !gpPrev[gamePad].IsButtonDown(button) && gpCurr[gamePad].IsButtonDown(button);
        }

        public Boolean isButtonRealeased(Buttons button, int gamePad)
        {
            return gpPrev[gamePad].IsButtonDown(button) && !gpCurr[gamePad].IsButtonDown(button);
        }

        public Boolean isButtonHeld(Buttons button, int gamePad)
        {
            return gpPrev[gamePad].IsButtonDown(button) && gpCurr[gamePad].IsButtonDown(button);
        }

        public Vector2 getLeftStick(int gamePad)
        {
            return gpCurr[gamePad].ThumbSticks.Left;
        }

        public Boolean getLeftStickPressedUp(int gamePad)
        {
            if (lsCurr[gamePad].Y > 0 && lsPrev[gamePad].Y <= 0)
                return true;
            else
                return false;
        }

        public Boolean getLeftStickPressedDown(int gamePad)
        {
            if (lsCurr[gamePad].Y < 0 && lsPrev[gamePad].Y >= 0)
                return true;
            else
                return false;
        }

        public Boolean getLeftStickPressedRight(int gamePad)
        {
            if (lsCurr[gamePad].X > 0 && lsPrev[gamePad].X <= 0)
                return true;
            else
                return false;
        }

        public Boolean getLeftStickPressedLeft(int gamePad)
        {
            if (lsCurr[gamePad].X < 0 && lsPrev[gamePad].X >= 0)
                return true;
            else
                return false;
        }

        public Vector2 getRightStick(int gamePad)
        {
            return gpCurr[gamePad].ThumbSticks.Right;
        }

        public Buttons nextButtonPressed()
        {
#if WINDOWS
            var buttonList = (Buttons[])Enum.GetValues(typeof(Buttons));

            for (int i = 0; i < 4; i++)
            {
                if(isConnected(i))
                    foreach (var b in buttonList)
                    {
                        if (isConnected(i))
                            if (isButtonPressed(b, i))
                                return b;
                    }
            }

            return Buttons.Start;
#endif

#if XBOX
            //var buttonList = (Buttons[])Enum.GetValues(typeof(Buttons));
            Buttons[] buttonList2 = { Buttons.A, Buttons.B, Buttons.X, Buttons.Y, Buttons.LeftShoulder, Buttons.RightShoulder, Buttons.LeftTrigger, Buttons.RightTrigger, Buttons.DPadUp, Buttons.DPadDown, Buttons.DPadLeft, Buttons.DPadRight, Buttons.LeftStick, Buttons.RightStick };

            for (int i = 0; i < 4; i++)
            {
                if (isConnected(i))
                    foreach (var b in buttonList2)
                    {
                        if (isConnected(i))
                            if (isButtonPressed(b, i))
                                return b;
                    }
            }

            return Buttons.Start;
#endif
        }
    }
}
