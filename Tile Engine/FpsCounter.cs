using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;

namespace TileEngine
{
    public class FpsCounter : GameComponent
    {
        float updateInterval = 1.0f;
        float timeSinceLastUpdate = 0.0f;
        float framecount = 0;

        float fps = 0;

        public float FPS
        {
            get { return fps; }
        }

        public FpsCounter(Game game)
            : base(game)
        {

        }

        public event EventHandler<EventArgs> Updated;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float elapsed = (float)gameTime.ElapsedRealTime.TotalSeconds;

            framecount++;
            timeSinceLastUpdate += elapsed;

            if (timeSinceLastUpdate > updateInterval)
            {
                fps = framecount / timeSinceLastUpdate; //mean fps over updateIntrval
                framecount = 0;
                timeSinceLastUpdate -= updateInterval;

                if (Updated != null)
                {
                    Updated(this, new EventArgs());
                }
            }
        }
    }
}
