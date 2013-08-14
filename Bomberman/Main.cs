using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using TileEngine;

namespace Bomberman
{
    public class Main : Game
    {
        public string ScreenTitle = "Bomberman";
        public const int ScreenWidth = 660;
        public const int ScreenHeight = 572;
        public const string AudioSettings = @"Audio\Sounds.xgs";
        public const string AudioWaveBank = @"Audio\WaveBank.xwb";
        public const string AudioSoundsBank = @"Audio\SoundBank.xsb";

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Menu menu;
        Audio music;
        List<Character> characters = new List<Character>();

#if DEBUG
        FpsCounter fps;
#endif

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;

            menu = new Menu("default.xml", this);
            this.Components.Add(menu);

            this.Window.AllowUserResizing = false;
            ((System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(this.Window.Handle)).ShowIcon = false;
        }

        void Main_myEvent(Character src)
        {
            throw new Exception("");
        }

        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), spriteBatch);

#if DEBUG
            fps = new FpsCounter(this);
            this.Components.Add(fps);

            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();
#endif

            base.Initialize();
        }

        protected override void LoadContent()
        {

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

#if DEBUG
            this.Window.Title = ScreenTitle + fps.FPS.ToString(" - FPS: 0");
#endif

            spriteBatch.Begin();
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
