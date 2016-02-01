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
    public class SoundMenuScreen : GameScreen
    {
        private int selection = 0;
        private int blinkcounter = 0;
        private int currentSprite_bar = 0;
        private int totalSprites_bar = 6;
        private double flipCounter_bar = 0;
        private Boolean isChanged = false;
        private Boolean isWhite;
        private Texture2D energyTexture;
        private Texture2D energyBGTexture;
        private Rectangle[] energyBarPosition;
        private Rectangle[] energyBarBGPosition;
        private Rectangle source;
        Texture2D backdrop;

        public SoundMenuScreen()
        {
            backdrop = GameState.content.Load<Texture2D>(@"Textures\Menu\menu_sound");
            energyTexture = GameState.content.Load<Texture2D>(@"Textures\Player\energyBar_sprite_small");
            energyBGTexture = GameState.content.Load<Texture2D>(@"Textures\Player\battery");

            source = new Rectangle(0, 0, 32, 96);
            energyBarBGPosition = new Rectangle[3];
            energyBarPosition = new Rectangle[3];

            energyBarPosition[0] = new Rectangle(GameState.SCREEN_WIDTH / 2 + 87, GameState.SCREEN_HEIGHT / 2 - 76, 12, 80);
            energyBarBGPosition[0] = new Rectangle(GameState.SCREEN_WIDTH / 2 + 102, GameState.SCREEN_HEIGHT / 2 - 80, 20, 100);
            energyBarPosition[1] = new Rectangle(GameState.SCREEN_WIDTH / 2 + 87, GameState.SCREEN_HEIGHT / 2 - 46, 12, 80);
            energyBarBGPosition[1] = new Rectangle(GameState.SCREEN_WIDTH / 2 + 102, GameState.SCREEN_HEIGHT / 2 - 50, 20, 100);
            energyBarPosition[2] = new Rectangle(GameState.SCREEN_WIDTH / 2 + 87, GameState.SCREEN_HEIGHT / 2 - 16, 12, 80);
            energyBarBGPosition[2] = new Rectangle(GameState.SCREEN_WIDTH / 2 + 102, GameState.SCREEN_HEIGHT / 2 - 20, 20, 100);
        }

        public override void Update(GameTime gameTime)
        {
            blinkcounter += gameTime.ElapsedGameTime.Milliseconds;

            if (blinkcounter > 750)
            {
                if (isWhite)
                    isWhite = false;
                else
                    isWhite = true;
                blinkcounter = 0;
            }

            flipCounter_bar += gameTime.ElapsedGameTime.Milliseconds;
            if (flipCounter_bar > 42 && !GameState.gameOver)
            {

                currentSprite_bar++;
                flipCounter_bar = 0;
                if (currentSprite_bar > totalSprites_bar)
                    currentSprite_bar = 0;
            }

            if ((GameState.inputManager.scrollDown(0) || GameState.inputManager.scrollDown(1)) && selection < 3)
            {
                selection++;
                GameState.soundBank.PlayCue("MenuChangeSelection");
            }
            if ((GameState.inputManager.scrollUp(0) || GameState.inputManager.scrollUp(1)) && selection > 0)
            {
                selection--;
                GameState.soundBank.PlayCue("MenuChangeSelection");
            }

            switch (selection)
            {
                case 0:
                    if (GameState.inputManager.scrollLeft(0) || GameState.inputManager.scrollLeft(1))
                    {
                        if (GameState.allSounds > 0)
                        {
                            GameState.allSounds -= 0.1f;
                            isChanged = true;
                        }
                    }
                    if (GameState.inputManager.scrollRight(0) || GameState.inputManager.scrollRight(1))
                    {
                        if (GameState.allSounds < 1)
                        {
                            GameState.allSounds += 0.1f;
                            isChanged = true;
                        }
                    }
                    break;

                case 1:
                    if (GameState.inputManager.scrollLeft(0) || GameState.inputManager.scrollLeft(1))
                    {
                        if (GameState.music > 0)
                        {
                            GameState.music -= 0.1f;
                            isChanged = true;
                        }
                    }
                    if (GameState.inputManager.scrollRight(0) || GameState.inputManager.scrollRight(1))
                    {
                        if (GameState.music < 1)
                        {
                            GameState.music += 0.1f;
                            isChanged = true;
                        }
                    }
                    break;

                case 2:
                    if (GameState.inputManager.scrollLeft(0) || GameState.inputManager.scrollLeft(1))
                    {
                        if (GameState.soundEffects > 0)
                        {
                            GameState.soundEffects -= 0.1f;
                            isChanged = true;
                        }
                    }
                    if (GameState.inputManager.scrollRight(0) || GameState.inputManager.scrollRight(1))
                    {
                        if (GameState.soundEffects < 1)
                        {
                            GameState.soundEffects += 0.1f;
                            isChanged = true;
                        }
                    }
                    break;

                case 3: 
                    if (GameState.inputManager.Accept(0) || GameState.inputManager.Accept(1))
                    {
                        GameState.soundBank.PlayCue("Back");
                        GameState.screenManager.Pop();
                    }
                        break;
            }

            if (isChanged)
            {
                if (GameState.allSounds < 0)
                    GameState.allSounds = 0;
                else if (GameState.allSounds > 1)
                    GameState.allSounds = 1;

                if (GameState.music < 0)
                    GameState.music = 0;
                else if (GameState.music > 1)
                    GameState.music = 1;

                if (GameState.soundEffects < 0)
                    GameState.soundEffects = 0;
                else if (GameState.soundEffects > 1)
                    GameState.soundEffects = 1;

                GameState.musicCategory.SetVolume(GameState.music * GameState.allSounds);
                GameState.soundCategory.SetVolume(GameState.soundEffects * GameState.allSounds);
                isChanged = false;
                GameState.soundBank.PlayCue("MenuChangeSelection");
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameState.spriteBatch.Draw(backdrop, new Vector2(GameState.SCREEN_WIDTH / 2 - backdrop.Width / 2, 250), Color.White);
            //GameState.spriteBatch.DrawString(GameState.font, "Sound Menu", new Vector2(512 - GameState.font.MeasureString("Sound Menu").X / 2, 100), Color.White);

            GameState.spriteBatch.Draw(energyBGTexture, energyBarBGPosition[0], source, Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0);
            GameState.spriteBatch.Draw(energyBGTexture, energyBarBGPosition[1], source, Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0);
            GameState.spriteBatch.Draw(energyBGTexture, energyBarBGPosition[2], source, Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0);

            //All Sounds Bar
            if(GameState.allSounds == 1)
                GameState.spriteBatch.Draw(energyTexture,
                        new Rectangle(energyBarPosition[0].X, energyBarPosition[0].Y, 12, 80),
                        new Rectangle(currentSprite_bar * 12, 0, 12, 100),
                        Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0);
            else
                GameState.spriteBatch.Draw(energyTexture,
                        new Rectangle(energyBarPosition[0].X - (80 - (int)(GameState.allSounds * 80)), energyBarPosition[0].Y, 12, (int)(GameState.allSounds * 80)),
                        new Rectangle(currentSprite_bar * 12, 0, 12, (int)(GameState.allSounds * 100)),
                        Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0);

            //Music Bar
            if (GameState.music == 1)
                GameState.spriteBatch.Draw(energyTexture,
                        new Rectangle(energyBarPosition[1].X, energyBarPosition[1].Y, 12, 80),
                        new Rectangle(currentSprite_bar * 12, 0, 12, 100),
                        Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0);
            else
                GameState.spriteBatch.Draw(energyTexture,
                        new Rectangle(energyBarPosition[1].X - (80 - (int)(GameState.music * 80)), energyBarPosition[1].Y, 12, (int)(GameState.music * 80)),
                        new Rectangle(currentSprite_bar * 12, 0, 12, (int)(GameState.music * 100)),
                        Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0);

            //Sound Effects Bar
            if (GameState.soundEffects == 1)
                GameState.spriteBatch.Draw(energyTexture,
                        new Rectangle(energyBarPosition[2].X, energyBarPosition[2].Y, 12, 80),
                        new Rectangle(currentSprite_bar * 12, 0, 12, 100),
                        Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0);
            else
                GameState.spriteBatch.Draw(energyTexture,
                        new Rectangle(energyBarPosition[2].X - (80 - (int)(GameState.soundEffects * 80)), energyBarPosition[2].Y, 12, (int)(GameState.soundEffects * 80)),
                        new Rectangle(currentSprite_bar * 12, 0, 12, (int)(GameState.soundEffects * 100)),
                        Color.White, MathHelper.ToRadians(90), Vector2.Zero, SpriteEffects.None, 0);


            if (selection == 0)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "All Sounds", new Vector2(460 - GameState.font.MeasureString("All Sounds").X / 2, 300), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "All Sounds", new Vector2(460 - GameState.font.MeasureString("All Sounds").X / 2, 300), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "All Sounds", new Vector2(460 - GameState.font.MeasureString("All Sounds").X / 2, 300), Color.Aqua);

            if (selection == 1)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Music", new Vector2(460 - GameState.font.MeasureString("Music").X / 2, 330), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Music", new Vector2(460 - GameState.font.MeasureString("Music").X / 2, 330), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Music", new Vector2(460 - GameState.font.MeasureString("Music").X / 2, 330), Color.Aqua);

            if (selection == 2)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Effects", new Vector2(460 - GameState.font.MeasureString("Effects").X / 2, 360), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Effects", new Vector2(460 - GameState.font.MeasureString("Effects").X / 2, 360), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Effects", new Vector2(460 - GameState.font.MeasureString("Effects").X / 2, 360), Color.Aqua);

            if (selection == 3)
                if (isWhite)
                {
                    GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 400), Color.LightYellow);
                }
                else
                    GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 400), Color.Yellow);
            else
                GameState.spriteBatch.DrawString(GameState.font, "Exit", new Vector2(GameState.SCREEN_WIDTH / 2 - GameState.font.MeasureString("Exit").X / 2, 400), Color.Aqua);
        }
    }
}
