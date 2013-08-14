using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    public class SpriteSheet
    {
        Texture2D texture;
        Size sprite;

        public Texture2D Texture
        {
            get { return this.texture; }
            set { this.texture = value; }
        }

        public Size SpriteSize
        {
            get { return this.sprite; }
            set { this.sprite = value; }
        }

        public SpriteSheet(Texture2D texture, Size spriteSize)
        {
            this.texture = texture;
            this.sprite = spriteSize;
        }

        public Rectangle GetSprite(int id)
        {
            int cols = (this.texture.Width / this.sprite.Width);
            int rows = (this.texture.Height / this.sprite.Height);

            if ((id + 1) > (cols * rows))
            {
                throw new OverflowException("ID is outside of sprite sheet bounds");
            }

            Rectangle sprite = new Rectangle(-1, -1, this.sprite.Width, this.sprite.Height);

            if ((id + 1) > cols)
            {
                sprite.X = ((id % cols) * this.sprite.Width);
                sprite.Y = ((id / cols) * this.sprite.Height);
            }
            else
            {
                sprite.X = (id * this.sprite.Width);
                sprite.Y = 0;
            }

            return sprite;
        }
    }
}
