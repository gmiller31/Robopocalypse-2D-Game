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
using Robopocalypse_Library;

namespace Robopocalypse.Managers
{
    public class InputManager
    {
        private int count;
        private int newPlayer;

        public InputManager()
        {
            count = 0;
        }

        private int IdentifyController(int player)
        {
            switch (player)
            {
                case 0:
                    if (GameState.gamePadInput.isConnected(0))
                        return 0;
                    else if (GameState.gamePadInput.isConnected(1))
                        return 1;
                    else if (GameState.gamePadInput.isConnected(2))
                        return 2;
                    else if (GameState.gamePadInput.isConnected(3))
                        return 3;
                    else
                        return - 1;

                case 1:
                    count = 0;
                    if (GameState.gamePadInput.isConnected(0))
                    {
                        if (count == player)
                            return 0;
                        else
                            count++;
                    }
                    
                    if (GameState.gamePadInput.isConnected(1))
                    {
                        if (count == player)
                            return 1;
                        else
                            count++;
                    }
                    
                    if (GameState.gamePadInput.isConnected(2))
                    {
                        if (count == player)
                            return 2;
                        else
                            count++;
                    }
                    
                    if (GameState.gamePadInput.isConnected(3))
                    {
                        if (count == player)
                            return 3;
                        else
                            count++;
                    }

                    return -1;

                case 2:
                    count = 0;
                    if (GameState.gamePadInput.isConnected(0))
                    {
                        if (count == player)
                            return 0;
                        else
                            count++;
                    }
                    
                    if (GameState.gamePadInput.isConnected(1))
                    {
                        if (count == player)
                            return 1;
                        else
                            count++;
                    }
                    
                    if (GameState.gamePadInput.isConnected(2))
                    {
                        if (count == player)
                            return 2;
                        else
                            count++;
                    }
                    
                    if (GameState.gamePadInput.isConnected(3))
                    {
                        if (count == player)
                            return 3;
                        else
                            count++;
                    }

                    return -1;

                case 3:
                    count = 0;
                    if (GameState.gamePadInput.isConnected(0))
                    {
                        if (count == player)
                            return 0;
                        else
                            count++;
                    }
                    
                    if (GameState.gamePadInput.isConnected(1))
                    {
                        if (count == player)
                            return 1;
                        else
                            count++;
                    }
                    
                    if (GameState.gamePadInput.isConnected(2))
                    {
                        if (count == player)
                            return 2;
                        else
                            count++;
                    }
                    
                    if (GameState.gamePadInput.isConnected(3))
                    {
                        if (count == player)
                            return 3;
                        else
                            count++;
                    }

                    return -1;

                default:
                    return -1;
            }
        }

        public float Vertical(int player)
        {
            player = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
            {
                //Bug 1: Check for if both up and down are pressed
                if (GameState.gamePadInput.isButtonHeld(GameState.b_Up1, player) && GameState.gamePadInput.isButtonHeld(GameState.b_Down1, player))
                    return 0;
                else if (GameState.gamePadInput.isButtonHeld(GameState.b_Up1, player))
                    return -1;
                else if (GameState.gamePadInput.isButtonHeld(GameState.b_Down1, player))
                    return 1;
                else
                    return -GameState.gamePadInput.getLeftStick(player).Y;//Bug 2: Stick Y is opposite of screen Y
            }

#if WINDOWS
            //Bug 1: Check for if both up and down are pressed
            if (GameState.keyboardInput.isKeyHeld(GameState.k_Up1) && GameState.keyboardInput.isKeyHeld(GameState.k_Down1))
                return 0;
            else if (GameState.keyboardInput.isKeyHeld(GameState.k_Up1))
                return -1;
            else if (GameState.keyboardInput.isKeyHeld(GameState.k_Down1))
                return 1;
            else
                return 0;
#endif
            //Default to 0 if on Xbox and no controller connected
#if XBOX
            return 0;
#endif
        }

        public float VerticalAlt(int player)
        {
            player = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
            {
                //Bug 1: Check for if both up and down are pressed
                if (GameState.gamePadInput.isButtonHeld(GameState.b_Up2, player) && GameState.gamePadInput.isButtonHeld(GameState.b_Down2, player))
                    return 0;
                else if (GameState.gamePadInput.isButtonHeld(GameState.b_Up2, player))
                    return -1;
                else if (GameState.gamePadInput.isButtonHeld(GameState.b_Down2, player))
                    return 1;
                else
                    return -GameState.gamePadInput.getRightStick(player).Y;//Bug 2: Stick Y is opposite of screen Y
            }

#if WINDOWS
            //Bug 1: Check for if both up and down are pressed
            if (GameState.keyboardInput.isKeyHeld(GameState.k_Up2) && GameState.keyboardInput.isKeyHeld(GameState.k_Down2))
                return 0;
            else if (GameState.keyboardInput.isKeyHeld(GameState.k_Up2))
                return -1;
            else if (GameState.keyboardInput.isKeyHeld(GameState.k_Down2))
                return 1;
            else
                return 0;
#endif
            //Default to 0 if on Xbox and no controller connected
#if XBOX
            return 0;
#endif
        }

        public float Horizontal(int player)
        {
            player = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
            {
                //Bug 1: Check for if both up and down are pressed
                if (GameState.gamePadInput.isButtonHeld(GameState.b_Left1, player) && GameState.gamePadInput.isButtonHeld(GameState.b_Right1, player))
                    return 0;
                else if (GameState.gamePadInput.isButtonHeld(GameState.b_Left1, player))
                    return -1;
                else if (GameState.gamePadInput.isButtonHeld(GameState.b_Right1, player))
                    return 1;
                else
                    return GameState.gamePadInput.getLeftStick(player).X;
            }

#if WINDOWS
            //Bug 1: Check for if both up and down are pressed
            if (GameState.keyboardInput.isKeyHeld(GameState.k_Left1) && GameState.keyboardInput.isKeyHeld(GameState.k_Right1))
                return 0;
            else if (GameState.keyboardInput.isKeyHeld(GameState.k_Left1))
                return -1;
            else if (GameState.keyboardInput.isKeyHeld(GameState.k_Right1))
                return 1;
            else
                return 0;
#endif
            //Default to 0 if on Xbox and no controller connected
#if XBOX
            return 0;
#endif
        }

        public float HorizontalAlt(int player)
        {
            player = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
            {
                //Bug 1: Check for if both up and down are pressed
                if (GameState.gamePadInput.isButtonHeld(GameState.b_Left2, player) && GameState.gamePadInput.isButtonHeld(GameState.b_Right2, player))
                    return 0;
                else if (GameState.gamePadInput.isButtonHeld(GameState.b_Left2, player))
                    return -1;
                else if (GameState.gamePadInput.isButtonHeld(GameState.b_Right2, player))
                    return 1;
                else
                    return GameState.gamePadInput.getRightStick(player).X;
            }

#if WINDOWS
            //Bug 1: Check for if both up and down are pressed
            if (GameState.keyboardInput.isKeyHeld(GameState.k_Left2) && GameState.keyboardInput.isKeyHeld(GameState.k_Right2))
                return 0;
            else if (GameState.keyboardInput.isKeyHeld(GameState.k_Left2))
                return -1;
            else if (GameState.keyboardInput.isKeyHeld(GameState.k_Right2))
                return 1;
            else
                return 0;
#endif
            //Default to 0 if on Xbox and no controller connected
#if XBOX
            return 0;
#endif
        }

        public bool Accept(int player)
        {
            newPlayer = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(newPlayer))
                return GameState.gamePadInput.isButtonPressed(Buttons.A, newPlayer) || GameState.gamePadInput.isButtonPressed(Buttons.A, newPlayer);

#if WINDOWS
            if (player == 0)
                return GameState.keyboardInput.isKeyPressed(Keys.Space);
            if (player == 1)
                return GameState.keyboardInput.isKeyPressed(Keys.Enter);
#endif
            return false;
        }

        public bool Cancel(int player)
        {
            newPlayer = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(newPlayer))
                return GameState.gamePadInput.isButtonPressed(Buttons.B, newPlayer) || GameState.gamePadInput.isButtonPressed(Buttons.B, newPlayer);

#if WINDOWS
            if (player == 0)
                return GameState.keyboardInput.isKeyPressed(Keys.Escape);
            if (player == 1)
                return GameState.keyboardInput.isKeyPressed(Keys.Back);
#endif
            //Default to false if on Xbox and no controller connected or invalid player number
            return false;
        }

        public bool Start(int player)
        {
            player = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
                return GameState.gamePadInput.isButtonPressed(GameState.b_Start, player);

#if WINDOWS
            return GameState.keyboardInput.isKeyPressed(GameState.k_Start);
#endif
            //Default to false if on Xbox and no controller connected
#if XBOX
            return false;
#endif
        }

        public bool XboxButton(int player)
        {
            player = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
                return GameState.gamePadInput.isButtonPressed(Buttons.BigButton, player);
            else
                return false;
        }

        public bool scrollUp(int player)
        {
            newPlayer = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
                return GameState.gamePadInput.isButtonPressed(Buttons.DPadUp, player) || GameState.gamePadInput.getLeftStickPressedUp(player);
#if WINDOWS
            if (player == 0)
                return GameState.keyboardInput.isKeyPressed(Keys.W);
            if (player == 1)
                return GameState.keyboardInput.isKeyPressed(Keys.Up);
#endif
            return false;
        }

        public bool scrollDown(int player)
        {
            newPlayer = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
                return GameState.gamePadInput.isButtonPressed(Buttons.DPadDown, player) || GameState.gamePadInput.getLeftStickPressedDown(player);

#if WINDOWS
            if (player == 0)
                return GameState.keyboardInput.isKeyPressed(Keys.S);
            if (player == 1)
                return GameState.keyboardInput.isKeyPressed(Keys.Down);
#endif
            return false;
        }

        public bool scrollLeft(int player)
        {
            player = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
                return GameState.gamePadInput.isButtonPressed(Buttons.DPadLeft, player) || GameState.gamePadInput.getLeftStickPressedLeft(player);
#if WINDOWS
            return GameState.keyboardInput.isKeyPressed(Keys.A) || GameState.keyboardInput.isKeyPressed(Keys.Left);
#endif
            //Default to false if on Xbox and no controller connected
#if XBOX
            return false;
#endif
        }

        public bool scrollRight(int player)
        {
            player = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
                return GameState.gamePadInput.isButtonPressed(Buttons.DPadRight, player) || GameState.gamePadInput.getLeftStickPressedRight(player);

#if WINDOWS
            return GameState.keyboardInput.isKeyPressed(Keys.D) || GameState.keyboardInput.isKeyPressed(Keys.Right);
#endif
            //Default to false if on Xbox and no controller connected
#if XBOX
            return false;
#endif
        }

        public bool LeftShoulder(int player)
        {
            player = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
                return GameState.gamePadInput.isButtonPressed(GameState.b_LShoulder, player);

#if WINDOWS
            return GameState.keyboardInput.isKeyPressed(GameState.k_LShoulder);
#endif
            //Default to false if on Xbox and no controller connected
#if XBOX
            return false;
#endif
        }

        public bool RightShoulder(int player)
        {
            player = IdentifyController(player);

            if (GameState.gamePadInput.isConnected(player))
                return GameState.gamePadInput.isButtonPressed(GameState.b_RShoulder, player);

#if WINDOWS
            return GameState.keyboardInput.isKeyPressed(GameState.k_RShoulder);
#endif
            //Default to false if on Xbox and no controller connected
#if XBOX
            return false;
#endif
        }
    }
}
