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

namespace TileEngine
{
    public enum BombExplosions
    {
        bombNormal = 0,
        bombBig,
        bombSmall,

        firstExplosionMid,
        secondExplosionMid,
        thirdExplosionMid,
        fourthExplosionMid,

        firstExplosionVertical,
        secondExplosionVertical,
        thirdExplosionVertical,
        fourthExplosionVertical,

        firstExplosionHorizontal,
        secondExplosionHorizontal,
        thirdExplosionHorizontal,
        fourthExplosionHorizontal,

        firstExplosionUp,
        secondExplosionUp,
        thirdExplosionUp,
        fourthExplosionUp,

        firstExplosionRight,
        secondExplosionRight,
        thirdExplosionRight,
        fourthExplosionRight,

        firstExplosionDown,
        secondExplosionDown,
        thirdExplosionDown,
        fourthExplosionDown,

        firstExplosionLeft,
        secondExplosionLeft,
        thirdExplosionLeft,
        fourthExplosionLeft
    }

    public class Bomb : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteSheet spriteSheet;

        public bool isBombDown = false;
        int currentTile;
        Map map;
        Game game;
        int bombTime = 0;
        bool startUp = true;
        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;
        int cycle = 0;
        int stepTime = 100;
        List<Character> characterList;

        public Bomb(Game game, int currentTile, Map map, List<Character> characterList)
            : base(game)
        {
            this.characterList = characterList;
            this.currentTile = currentTile;
            this.map = map;
            this.game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            this.spriteBatch = (SpriteBatch)this.Game.Services.GetService(typeof(SpriteBatch));
            this.spriteSheet = new SpriteSheet(this.Game.Content.Load<Texture2D>(@"Sprites\Bomb"), new Size(44, 44));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            this.bombTime += gameTime.ElapsedGameTime.Milliseconds;

            if (this.bombTime < 500)
            {
                DrawTile(BombExplosions.bombNormal, this.currentTile, false);
            }
            else if (this.bombTime >= 500 && this.bombTime < 1000)
            {
                DrawTile(BombExplosions.bombSmall, this.currentTile, false);
            }
            else if (this.bombTime >= 1000 && this.bombTime < 1500)
            {
                DrawTile(BombExplosions.bombBig, this.currentTile, false);
            }

            if (this.bombTime > 1500 && this.bombTime < (1500 + (this.stepTime)))
            {
                this.cycle = 0;
            }
            else if (this.bombTime >= (1500 + this.stepTime) && this.bombTime < (1500 + (this.stepTime * 2)))
            {
                this.cycle = 1;
            }
            else if (this.bombTime >= (1500 + (this.stepTime * 2)) && this.bombTime < (1500 + (this.stepTime * 3)))
            {
                this.cycle = 2;
            }
            else if (this.bombTime >= (1500 + (this.stepTime * 3)) && this.bombTime < (1500 + (this.stepTime * 4)))
            {
                this.cycle = 3;
            }
            else if (this.bombTime >= (1500 + (this.stepTime * 4)))
            {
                this.Dispose();
            }

            if (this.bombTime >= 1500)
            {
                DrawTile(BombExplosions.firstExplosionMid + this.cycle, this.currentTile, false);

                if (this.startUp)
                {
                    this.up = DrawTile(BombExplosions.firstExplosionVertical + this.cycle, this.currentTile - this.map.MapSize.Width, false);
                    this.right = DrawTile(BombExplosions.firstExplosionHorizontal + this.cycle, this.currentTile + 1, false);
                    this.down = DrawTile(BombExplosions.firstExplosionVertical + this.cycle, this.currentTile + this.map.MapSize.Width, false);
                    this.left = DrawTile(BombExplosions.firstExplosionHorizontal + this.cycle, this.currentTile - 1, false);

                    this.startUp = false;
                }
                else
                {
                    DrawTile(BombExplosions.firstExplosionVertical + this.cycle, this.currentTile - this.map.MapSize.Width, false);
                    DrawTile(BombExplosions.firstExplosionHorizontal + this.cycle, this.currentTile + 1, false);
                    DrawTile(BombExplosions.firstExplosionVertical + this.cycle, this.currentTile + this.map.MapSize.Width, false);
                    DrawTile(BombExplosions.firstExplosionHorizontal + this.cycle, this.currentTile - 1, false);
                }

                DrawTile(BombExplosions.firstExplosionUp + this.cycle, this.currentTile - (this.map.MapSize.Width * 2), this.up);
                DrawTile(BombExplosions.firstExplosionRight + this.cycle, this.currentTile + 2, this.right);
                DrawTile(BombExplosions.firstExplosionDown + this.cycle, this.currentTile + (this.map.MapSize.Width * 2), this.down);
                DrawTile(BombExplosions.firstExplosionLeft + this.cycle, this.currentTile - 2, this.left);
            }
        }

        private bool DrawTile(BombExplosions tile, int id, bool broken)
        {
            bool breaking = false;

            if (broken == false)
            {
                if (!this.map.CanWalk(id))
                {
                    this.map.clearTile(id - 1, "Barrels");
                    breaking = true;
                }
                else
                {
                    Rectangle sprite = this.spriteSheet.GetSprite((int)tile);
                    Vector2 location = TileToPixel(id);
                    this.spriteBatch.Draw(this.spriteSheet.Texture, new Rectangle((int)location.X, (int)location.Y, sprite.Width, sprite.Height), sprite, Color.White);
                }
                              
                foreach(Character i in this.characterList)
                { 
                    if (i.CurrentTile == id && !this.startUp && i.alpha == 255)
                    {
                        i.KillCharacter();
                    }
                }
            }
            return breaking;
        }

        private Vector2 TileToPixel(int id)
        {
            return new Vector2(((id % this.map.MapSize.Width) - 1) * this.map.SpriteSize.Width, (float)Math.Floor((decimal)(id / this.map.MapSize.Width)) * this.map.SpriteSize.Height);
        }
    }
}
