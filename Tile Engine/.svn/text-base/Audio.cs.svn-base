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
    public class Audio : GameComponent
    {
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue sound;
        bool playing = false;
        bool paused = false;

        public bool Playing
        {
            get { return playing; }
        }

        public bool Paused
        {
            get { return paused; }
        }

        public Audio(string clip, string settings, string waveBank, string soundBank, Game game)
            : base(game)
        {
            this.audioEngine = new AudioEngine(this.Game.Content.RootDirectory + @"\" + settings);
            this.waveBank = new WaveBank(this.audioEngine, this.Game.Content.RootDirectory + @"\" + waveBank);
            this.soundBank = new SoundBank(this.audioEngine, this.Game.Content.RootDirectory + @"\" + soundBank);
            this.sound = this.soundBank.GetCue(clip);
        }

        public void Play()
        {
            if (this.Enabled)
            {
                if (!playing)
                {
                    sound.Play();
                    playing = true;
                }
            }
        }

        public void Pause()
        {
            if (this.Enabled)
            {
                if (paused)
                {
                    sound.Pause();
                    paused = true;
                    playing = false;
                }
                else
                {
                    sound.Resume();
                    paused = false;
                    playing = true;
                }
            }
        }

        public void Stop()
        {
            if (this.Enabled)
            {
                if (playing)
                {
                    sound.Stop(AudioStopOptions.Immediate);
                    playing = false;
                }

            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Enabled)
            {
                audioEngine.Update();
            }

            base.Update(gameTime);
        }
    }
}
