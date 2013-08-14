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
    public enum CharDirection
    {
        Up,
        Down,
        Left,
        Right,
        Drop
    }

    public struct Controls
    {
        Keys up;
        Keys down;
        Keys left;
        Keys right;
        Keys drop;

        public Keys Up
        {
            get { return up; }
            set { up = value; }
        }

        public Keys Down
        {
            get { return down; }
            set { down = value; }
        }

        public Keys Left
        {
            get { return left; }
            set { left = value; }
        }

        public Keys Right
        {
            get { return right; }
            set { right = value; }
        }

        public Keys Drop
        {
            get { return drop; }
            set { drop = value; }
        }
    }

    public class Character : DrawableGameComponent
    {
        Game parent;
        Map map;
        CharDirection direction;
        int charLives = 3;
        int spawnTime = 0;
        GameTime gametime;
        int charSpawnLocation;
        SpriteBatch spriteBatch;
        SpriteSheet spriteSheet;

        bool moving = false;
        const float charCount = 20f;
        float gameCount = 0f;
        int steps = 0;
        Bomb bomb;
        public bool isBombDown = false;
        int bombDown = 0;
        string sprite;

        List<Character> characterList;
        int currentTile;

        Controls controls;

        float x;
        float y;
        float baseX;
        float baseY;
        bool reset = false;

        public byte alpha = 255;

        public Character(Map map, Controls controls, int currentTile, string sprite, Game game, List<Character> characterList)
            : base(game)
        {
            this.characterList = characterList;
            this.map = map;
            this.controls = controls;
            this.parent = game;
            this.currentTile = currentTile;
            this.sprite = sprite;
            this.direction = CharDirection.Down;
            this.charSpawnLocation = currentTile;
            this.x = ((currentTile % this.map.MapSize.Width) - 1) * this.map.SpriteSize.Width;
            this.y = (float)Math.Floor((decimal)(currentTile / this.map.MapSize.Width)) * this.map.SpriteSize.Height;
            this.baseX = this.x;
            this.baseY = this.y;
        }

        public int CurrentTile
        {
            get { return currentTile; }
            set { CurrentTile = value; }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            this.spriteBatch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            this.spriteSheet = new SpriteSheet(this.parent.Content.Load<Texture2D>(this.sprite), new Size(this.map.SpriteSize.Width, this.map.SpriteSize.Height));
        }

        public override void Update(GameTime gameTime)
        {
            if (this.charLives == 0)
            {
                this.Game.Components.Remove(this);
            }

            if (this.spawnTime > 0)
            {
                this.spawnTime += gameTime.ElapsedGameTime.Milliseconds;

                if (this.spawnTime > 2000 && this.spawnTime <= 4500)
                {
                    if (!this.reset)
                    {
                        this.alpha = 125;
                        this.currentTile = this.charSpawnLocation;
                        this.x = this.baseX;
                        this.y = this.baseY;

                        this.reset = true;
                    }
                }
                else if (this.spawnTime > 4500)
                {
                    this.alpha = 255;
                    this.isBombDown = false;
                    this.spawnTime = 0;
                }
            }

            this.gameCount += gameTime.ElapsedGameTime.Milliseconds;

            this.bombDown += gameTime.ElapsedGameTime.Milliseconds;
            if (!this.moving)
            {
                KeyboardState kb = Keyboard.GetState();

                if (this.bombDown >= 2800)
                {
                    this.isBombDown = false;
                    this.bombDown = 0;
                }

                if (kb.IsKeyDown(controls.Drop) && !this.isBombDown)
                {
                    this.bomb = new Bomb(this.parent, this.currentTile, this.map, this.characterList);
                    this.parent.Components.Add(this.bomb);
                    this.isBombDown = true;
                }

                if (kb.IsKeyDown(controls.Up) && !kb.IsKeyDown(controls.Down))
                {
                    this.moving = true;
                    this.direction = CharDirection.Up;
                }
                else if (kb.IsKeyDown(controls.Down) && !kb.IsKeyDown(controls.Up))
                {
                    this.moving = true;
                    this.direction = CharDirection.Down;
                }
                else if (kb.IsKeyDown(controls.Left) && !kb.IsKeyDown(controls.Right))
                {
                    this.moving = true;
                    this.direction = CharDirection.Left;
                }
                else if (kb.IsKeyDown(controls.Right) && !kb.IsKeyDown(controls.Left))
                {
                    this.moving = true;
                    this.direction = CharDirection.Right;
                }

                if (this.moving)
                {
                    switch (this.direction)
                    {
                        case CharDirection.Up:
                            if (!this.map.CanWalk(this.currentTile - this.map.MapSize.Width))
                            {
                                this.moving = false;
                            }
                            else
                            {
                                currentTile -= this.map.MapSize.Width;
                            }
                            break;
                        case CharDirection.Down:
                            if (!this.map.CanWalk(this.currentTile + this.map.MapSize.Width))
                            {
                                this.moving = false;
                            }
                            else
                            {
                                currentTile += this.map.MapSize.Width;
                            }
                            break;
                        case CharDirection.Left:
                            if (!this.map.CanWalk(this.currentTile - 1))
                            {
                                this.moving = false;
                            }
                            else
                            {
                                currentTile -= 1;
                            }
                            break;
                        case CharDirection.Right:
                            if (!this.map.CanWalk(this.currentTile + 1))
                            {
                                this.moving = false;
                            }
                            else
                            {
                                currentTile += 1;
                            }
                            break;
                        case CharDirection.Drop:
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                if (this.gameCount > charCount)
                {
                    this.steps++;

                    if (this.direction == CharDirection.Up)
                    {
                        this.y -= 4f;
                    }
                    else if (this.direction == CharDirection.Down)
                    {
                        this.y += 4f;
                    }
                    else if (this.direction == CharDirection.Right)
                    {
                        this.x += 4f;
                    }
                    else if (this.direction == CharDirection.Left)
                    {
                        this.x -= 4f;
                    }

                    this.gameCount = 0;

                    if (this.steps >= 11)
                    {
                        this.moving = false;
                        this.steps = 0;
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.gametime = gameTime;
            Rectangle sprite = new Rectangle();

            if (this.direction == CharDirection.Drop)
            {
                sprite = this.spriteSheet.GetSprite(3);
            }

            if (this.direction == CharDirection.Up)
            {
                if (steps < 5)
                {
                    sprite = this.spriteSheet.GetSprite(1);
                }
                else if (steps > 7)
                {
                    sprite = this.spriteSheet.GetSprite(2);
                }
                else
                {
                    sprite = this.spriteSheet.GetSprite(0);
                }

                if (!moving)
                {
                    sprite = this.spriteSheet.GetSprite(0);
                }
            }

            if (this.direction == CharDirection.Down)
            {
                if (steps < 5)
                {
                    sprite = this.spriteSheet.GetSprite(4);
                }
                else if (steps > 7)
                {
                    sprite = this.spriteSheet.GetSprite(5);
                }
                else
                {
                    sprite = this.spriteSheet.GetSprite(3);
                }

                if (!moving)
                {
                    sprite = this.spriteSheet.GetSprite(3);
                }
            }

            if (this.direction == CharDirection.Left)
            {
                if (steps < 5)
                {
                    sprite = this.spriteSheet.GetSprite(7);
                }
                else if (steps > 7)
                {
                    sprite = this.spriteSheet.GetSprite(8);
                }
                else
                {
                    sprite = this.spriteSheet.GetSprite(6);
                }

                if (!moving)
                {
                    sprite = this.spriteSheet.GetSprite(6);
                }
            }

            if (this.direction == CharDirection.Right)
            {
                if (steps < 5)
                {
                    sprite = this.spriteSheet.GetSprite(10);
                }
                else if (steps > 7)
                {
                    sprite = this.spriteSheet.GetSprite(11);
                }
                else
                {
                    sprite = this.spriteSheet.GetSprite(9);
                }

                if (!moving)
                {
                    sprite = this.spriteSheet.GetSprite(9);
                }
            }

            this.spriteBatch.Draw(this.spriteSheet.Texture, new Rectangle((int)this.x, (int)this.y - 8, sprite.Width, sprite.Height), sprite, new Color(255, 255, 255, this.alpha));

            base.Draw(gameTime);
        }

        public void KillCharacter()
        {
            this.reset = false;
            this.spawnTime = 1;
            this.alpha = 0;
            this.isBombDown = true;
            this.charLives--;
        }
    }
}
