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
    public class Layer : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        string name;
        Size layersize;
        Texture2D texture;
        Size spritesize;
        float alpha;
        bool disabled;
        int[] tiles;

        string textureName;
        Game game;


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Size LayerSize
        {
            get { return layersize; }
            set { layersize = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Size SpriteSize
        {
            get { return spritesize; }
            set { spritesize = value; }
        }

        public float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }

        public Layer(Game game, string name, Size layer_size, Size sprite_size, int[] layer, string textureName)
            : base(game)
        {
            this.game = game;
            // gets the layer details from the map and sets the varables
            this.name = name;
            this.layersize = layer_size;
            this.spritesize = sprite_size;

            this.textureName = textureName;
            this.tiles = (int[])layer.Clone();
        }

        public void editLayer(int id, int value)
        {
            this.tiles[id] = value;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            this.Texture = this.game.Content.Load<Texture2D>(@"Sprites/" + this.textureName);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // draw tiles
            base.Draw(gameTime);

            if (this.Disabled == false)
            {
                int column = 0;
                int row = 0;
                foreach (int i in this.tiles)
                {
                    //int tile;       


                    if (i == -1)
                    {

                    }
                    else
                    {

                        Rectangle sprite = new Rectangle(i * spritesize.Width, 0, spritesize.Width, spritesize.Height);
                        Rectangle tile = new Rectangle(column * spritesize.Height, row, spritesize.Width, spritesize.Height);
                        spriteBatch.Draw(this.Texture, tile, sprite, Color.White);
                    }




                    if (column == layersize.Width - 1)
                    {
                        row += spritesize.Height;
                        column = 0;
                    }
                    else
                    {
                        column++;
                    }
                }
            }
        }
    }
}
