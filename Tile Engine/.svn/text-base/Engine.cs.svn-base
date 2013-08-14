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

namespace TileEngine
{
    public struct Size
    {
        private int width;
        private int height;

        public int Width
        {
            get { return width; }
            set { width = Math.Max(value, 0); }
        }

        public int Height
        {
            get { return height; }
            set { height = Math.Max(value, 0); }
        }

        public Size(int x, int y)
        {
            width = x;
            height = y;
        }
    }
}
