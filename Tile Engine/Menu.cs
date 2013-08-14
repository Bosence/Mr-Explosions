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
using Microsoft.Xna.Framework.Net;
using TileEngine;

namespace TileEngine
{
    public class MenuScreen
    {
        public Dictionary<int, MenuItem> menuItem = new Dictionary<int, MenuItem>();
        int id;
        string name;

        public int ID
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public MenuScreen(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

    }

    public class MenuItem
    {
        string mid;
        string mname;
        string maction;

        public string mId
        {
            get { return mid; }
        }

        public string mName
        {
            get { return mname; }
        }

        public string mAction
        {
            get { return maction; }
        }

        public MenuItem(string mId, string mName, string mAction)
        {
            this.mid = mId;
            this.mname = mName;
            this.maction = mAction;
        }
    }

    public class Menu : DrawableGameComponent
    {
        public const string AudioSettings = @"Audio\Sounds.xgs";
        public const string AudioWaveBank = @"Audio\WaveBank.xwb";
        public const string AudioSoundsBank = @"Audio\SoundBank.xsb";
        public string ScreenTitle = "Bomberman";
        Map map;
        Audio music;

        Dictionary<int, MenuScreen> menu = new Dictionary<int, MenuScreen>();
        List<Character> characters = new List<Character>();
        List<Bomb> bombs = new List<Bomb>();

        SpriteBatch spriteBatch;
        int CurrentMenuItem;
        int MaxChildren;
        string CurrentMenuSelect = "0";
        string MapSelected;
        string AmountOfPlayers;
        int MenuSelected = 1;
        bool sound = true;
        bool effects = true;
        SpriteFont _font;
        KeyboardState oldState;
        Game parent;

        public Menu(string MenuFile, Game game)
            : base(game)
        {
            this.parent = game;
            MenuFile = this.Game.Content.RootDirectory + @"\Menu\" + MenuFile;

            if (!File.Exists(MenuFile))
            {
                throw new IOException("Cannot find file: " + MenuFile);
            }

            XmlTextReader xmlReader = null;
            XmlDocument xmlDocument;

            try
            {
                xmlReader = new XmlTextReader(MenuFile);
                xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlReader);

                XmlNodeList menu = xmlDocument.GetElementsByTagName("menu");

                for (int i = 0; i < menu.Count; i++)
                {
                    XmlNodeList children = menu[i].ChildNodes;
                    XmlAttributeCollection attribs = menu[i].Attributes;

                    this.menu.Add(i, new MenuScreen(Int32.Parse(attribs["id"].InnerText), attribs["name"].InnerText));

                    for (int n = 0; n < children.Count; n++)
                    {
                        this.menu[i].menuItem.Add(n, new MenuItem(children[n].Attributes["id"].InnerText, children[n].InnerText, children[n].Attributes["action"].InnerText));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
            }

        }

        public override void Initialize()
        {
            base.Initialize();

            this.oldState = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            this.spriteBatch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));

            _font = this.Game.Content.Load<SpriteFont>("fonts/default");
        }

        public void StartGame(string MenuFileSelected, string AmountOfPlayers, bool sound, bool effects)
        {
            map = new Map(@"Maps\" + MapSelected + ".xml", this.Game);
            this.Game.Components.Add(map);

            ScreenTitle += " (" + map.Name + ")";
            this.Game.Window.Title = ScreenTitle;

            music = new Audio("Menu", AudioSettings, AudioWaveBank, AudioSoundsBank, this.Game);
            this.Game.Components.Add(music);

            if (sound)
            {
                //Add sound
            }

            if (effects)
            {
                //Add effects
            }

            Controls controls1 = new Controls();
            controls1.Up = Keys.W;
            controls1.Down = Keys.S;
            controls1.Left = Keys.A;
            controls1.Right = Keys.D;
            controls1.Drop = Keys.Q;

            Controls controls2 = new Controls();
            controls2.Up = Keys.I;
            controls2.Down = Keys.K;
            controls2.Left = Keys.J;
            controls2.Right = Keys.L;
            controls2.Drop = Keys.U;

            characters.Add(new Character(map, controls1, 17, @"Sprites\White", this.Game, characters));
            this.Game.Components.Add(characters[0]);

            characters.Add(new Character(map, controls2, 29, @"Sprites\Green", this.Game, characters));
            this.Game.Components.Add(characters[1]);

            if (AmountOfPlayers == "3 players" || AmountOfPlayers == "4 players")
            {
                Controls controls3 = new Controls();
                controls3.Up = Keys.Up;
                controls3.Down = Keys.Down;
                controls3.Left = Keys.Left;
                controls3.Right = Keys.Right;
                controls3.Drop = Keys.RightControl;

                characters.Add(new Character(map, controls3, 167, @"Sprites\Red", this.Game, characters));
                this.Game.Components.Add(characters[2]);

                if (AmountOfPlayers == "4 players")
                {
                    Controls controls4 = new Controls();
                    controls4.Up = Keys.NumPad8;
                    controls4.Down = Keys.NumPad2;
                    controls4.Left = Keys.NumPad4;
                    controls4.Right = Keys.NumPad6;
                    controls4.Drop = Keys.NumPad0;

                    characters.Add(new Character(map, controls4, 179, @"Sprites\Black", this.Game, characters));
                    this.Game.Components.Add(characters[3]);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Escape))
            {
                this.Game.Exit();
            }

            if (!newState.IsKeyDown(Keys.Up))
            {
                if (oldState.IsKeyDown(Keys.Up))
                {
                    CurrentMenuItem--;
                }
            }

            if (!newState.IsKeyDown(Keys.Down))
            {
                if (oldState.IsKeyDown(Keys.Down))
                {
                    CurrentMenuItem++;
                }
            }

            if (CurrentMenuItem < 0)
            {
                CurrentMenuItem = 0;
            }

            if (CurrentMenuItem > MaxChildren - 1)
            {
                CurrentMenuItem = MaxChildren - 1;
            }

            if (!newState.IsKeyDown(Keys.Enter))
            {
                if (oldState.IsKeyDown(Keys.Enter))
                {
                    switch (CurrentMenuSelect)
                    {
                        case "quit":
                            this.Game.Exit();
                            break;
                        case "credits":
                            MenuSelected = 6;
                            break;
                        case "startGame":
                            //MenuSelected = 7;
                            this.Game.Components.Remove(this);
                            //Start Game function to be called from the parent.
                            this.StartGame(MapSelected, AmountOfPlayers, sound, effects);
                            break;
                        case "sound":
                            effects = !effects;
                            break;
                        case "music":
                            sound = !sound;
                            break;
                        default:
                            MenuSelected = Int32.Parse(CurrentMenuSelect);
                            break;
                    }

                    CurrentMenuItem = 0;
                }
            }

            oldState = newState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            int de = 200;
            int Selected = 0;


            foreach (KeyValuePair<int, MenuScreen> entry in this.menu)
            {
                if (entry.Value.ID == MenuSelected)
                {
                    MaxChildren = entry.Value.menuItem.Count;
                    spriteBatch.DrawString(_font, entry.Value.Name, new Vector2((this.Game.Window.ClientBounds.Width / 2) - 100, 150), Color.Red);
                    foreach (KeyValuePair<int, MenuItem> menutext in entry.Value.menuItem)
                    {
                        if (Selected == CurrentMenuItem)
                        {
                            spriteBatch.DrawString(_font, menutext.Value.mName, new Vector2((this.Game.Window.ClientBounds.Width / 2) - 100, de), Color.Yellow);

                            CurrentMenuSelect = menutext.Value.mAction;
                            MapSelected = menutext.Value.mName;
                            if (CurrentMenuSelect == "3")
                            {
                                AmountOfPlayers = menutext.Value.mName;
                            }
                        }
                        else
                        {
                            spriteBatch.DrawString(_font, menutext.Value.mName, new Vector2((this.Game.Window.ClientBounds.Width / 2) - 100, de), Color.White);
                        }

                                                
                        Selected++;
                        de = de + 50;
                    }
                }
            }

        
        }
    }
}
