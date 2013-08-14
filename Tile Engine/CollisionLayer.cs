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
    public class CollisionLayer
    {
        string name;
        Size layersize;
        bool disabled;
        bool breakable;

        int[] tiles;

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

        public bool Disabled
        {
            get { return disabled; }
            set { disabled = value; }
        }

        public bool Breakable
        {
            get { return breakable; }
            set { breakable = value; }
        }

        public int checkTile(int ID)
        {
            return this.tiles[ID - 1];
        }

        public CollisionLayer(string name, Game game, Size layer_size, int[] layer, bool breakable)
        {
            //properites coming from map class
            this.name = name;
            this.layersize = layer_size;
            this.breakable = breakable;
            this.tiles = (int[])layer.Clone();
        }

        public void editLayer(int id, int value)
        {
            this.tiles[id] = value;
        }

        public bool CanWalk(int id)
        {
            // checking if player can move
            //bool canwalk = false;

            if (this.Disabled == false)
            {
                if (id > (tiles.Length - 1) || id < 0)
                {
                    return false;
                }
                if (tiles[id - 1] == 1)
                {
                    return false;

                }
                else
                {
                    return true;
                }
            }

            // collison layer class checks if tile is free and returns boolean
            return false;
        }
    }
}
