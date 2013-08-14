using System;
using System.IO;
using System.Xml;
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
    public class Map : DrawableGameComponent
    {
        // declaring map size and name, and tile size
        SpriteBatch spriteBatch;
        string name;
        Size mapsize;
        Size spritesize;
        Dictionary<string, Layer> texLayers = new Dictionary<string, Layer>();
        Dictionary<string, CollisionLayer> colLayers = new Dictionary<string, CollisionLayer>();
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Size MapSize
        {
            get { return mapsize; }
        }

        public Size SpriteSize
        {
            get { return spritesize; }
        }

        public Map(string mapxml, Game game)
            : base(game)
        {



            XmlTextReader mapreader = new XmlTextReader(game.Content.RootDirectory + " \\" + mapxml);
            XmlDocument mapdocument = new XmlDocument();
            mapdocument.Load(mapreader);

            name = mapdocument.SelectSingleNode("/map").Attributes["name"].InnerText;
            mapsize = new Size(Convert.ToInt32(mapdocument.SelectSingleNode("/map/mapsize").Attributes["x"].InnerText),
                               Convert.ToInt32(mapdocument.SelectSingleNode("/map/mapsize").Attributes["y"].InnerText));

            spritesize = new Size(Convert.ToInt32(mapdocument.SelectSingleNode("/map/spritesize").Attributes["x"].InnerText),
                   Convert.ToInt32(mapdocument.SelectSingleNode("/map/spritesize").Attributes["y"].InnerText));

            XmlNodeList xmlLayers = mapdocument.SelectNodes("/map/layer");
            for (int i = 0; i < xmlLayers.Count; i++)
            {
                if (xmlLayers.Item(i).Attributes["type"].InnerText == "Texture")
                {
                    int[] layerArray = Array.ConvertAll<string, int>(xmlLayers.Item(i).SelectSingleNode("data").InnerText.Split(' '), Convert.ToInt32);
                    string layerName = xmlLayers.Item(i).Attributes["name"].InnerText;
                    this.texLayers.Add(xmlLayers.Item(i).Attributes["name"].InnerText, new Layer(game, layerName, mapsize, spritesize, layerArray, xmlLayers.Item(i).SelectSingleNode("sprite").InnerText));
                    this.Game.Components.Add(this.texLayers[layerName]);
                }
                else if (xmlLayers.Item(i).Attributes["type"].InnerText == "Collision")
                {
                    int[] layerArray = Array.ConvertAll<string, int>(xmlLayers.Item(i).SelectSingleNode("data").InnerText.Split(' '), Convert.ToInt32);
                    string layerName = xmlLayers.Item(i).Attributes["name"].InnerText;
                    this.colLayers.Add(layerName, new CollisionLayer(layerName, game, mapsize, layerArray, Convert.ToBoolean(xmlLayers.Item(i).Attributes["breakable"].InnerText)));
                }
                else
                {
                    //unknown layer
                }


            }



            // Console.Write("");
        }

        public void AddLayer(string key, Layer layer)
        {
            this.texLayers.Add(key, layer);
        }
        public void AddColLayer(string key, CollisionLayer colLayer)
        {
            this.colLayers.Add(key, colLayer);
        }



        public void RemoveLayer(string key)
        {
            this.texLayers.Remove(key);
        }
        public void RemoveColLayer(string key)
        {
            this.colLayers.Remove(key);
        }



        public void EnableLayer(string key)
        {
            this.texLayers[key].Disabled = false;
        }
        public void EnableColLayer(string key)
        {
            this.colLayers[key].Disabled = false;
        }

        public void DisableLayer(string key)
        {
            this.texLayers[key].Disabled = true;
        }
        public void DisableColLayer(string key)
        {
            this.colLayers[key].Disabled = true;
        }

        public void editTexLayer(int id, string layername, int texValue)
        {

            this.texLayers[layername].editLayer(id, texValue);
        }

        public void editColLayer(int id, string layername, int colValue)
        {

            this.colLayers[layername].editLayer(id, colValue);

        }

        public void clearTile(int ID, string layername)
        {
            this.editTexLayer(ID, layername, -1);
            this.editColLayer(ID, layername, -1);
        }

        public bool CanWalk(int id)
        {
            bool canwalk = true;



            foreach (KeyValuePair<string, CollisionLayer> i in this.colLayers)
            {
                if (i.Value.CanWalk(id) == false)
                {
                    return false;
                }
            }
            // player class checks if tile is free and return boolean
            return canwalk;
        }

        public bool CanBreak(int id, string layername)
        {
            if (id > ((this.colLayers[layername].LayerSize.Height * this.colLayers[layername].LayerSize.Width) - 1) || id < 0)
            {
                return false;
            }

            if (this.colLayers[layername].checkTile(id) == 1)
            {
                return this.colLayers[layername].Breakable;
            }
            else
            {
                return false;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
