﻿using System;

namespace TwistedLogik.Ultraviolet.Audio
{
    /// <summary>
    /// Represents a factory method which constructs instances of the SongPlayer class.
    /// </summary>
    /// <param name="uv">The Ultraviolet context.</param>
    /// <returns>The instance of SongPlayer that was created.</returns>
    public delegate SongPlayer SongPlayerFactory(UltravioletContext uv);

    /// <summary>
    /// Represents the method that is called when a song player raises an event.
    /// </summary>
    /// <param name="songPlayer">The song player that raised the event.</param>
    public delegate void SongPlayerEventHandler(SongPlayer songPlayer);

    /// <summary>
    /// Represents an object which plays sound effects.
    /// </summary>
    public abstract class SongPlayer : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the SongPlayer class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        protected SongPlayer(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the SongPlayer class.
        /// </summary>
        /// <returns>The instance of SongPlayer that was created.</returns>
        public static SongPlayer Create()
        {
            var uv = UltravioletContext.DemandCurrent();
            return uv.GetFactoryMethod<SongPlayerFactory>()(uv);
        }

        /// <summary>
        /// Updates the player's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last update.</param>
        public abstract void Update(UltravioletTime time);

        /// <summary>
        /// Plays the specified song.
        /// </summary>
        /// <param name="song">The song to play.</param>
        /// <param name="loop">A value indicating whether to loop the song.</param>
        /// <returns>true if the song began playing successfully; otherwise, false.</returns>
        public abstract Boolean Play(Song song, Boolean loop = false);

        /// <summary>
        /// Plays the specified song.
        /// </summary>
        /// <param name="song">The song to play.</param>
        /// <param name="volume">A value from 0.0 (silent) to 1.0 (full volume) representing the song's volume.</param>
        /// <param name="pitch">A value from -1.0 (down one octave) to 1.0 (up one octave) indicating the song's pitch adjustment.</param>
        /// <param name="pan">A value from -1.0 (full left) to 1.0 (full right) representing the song's panning position.</param>
        /// <param name="loop">A value indicating whether to loop the song.</param>
        /// <returns>true if the song began playing successfully; otherwise, false.</returns>
        public abstract Boolean Play(Song song, Single volume, Single pitch, Single pan, Boolean loop = false);

        /// <summary>
        /// Stops the song that is currently playing.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Pauses the song that is currently playing.
        /// </summary>
        public abstract void Pause();

        /// <summary>
        /// Resumes the song after it has been paused.
        /// </summary>
        public abstract void Resume();

        /// <summary>
        /// Slides the song's volume to the specified value over the specified period of time.
        /// </summary>
        /// <param name="volume">The value to which to slide the song's volume.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public abstract void SlideVolume(Single volume, TimeSpan time);

        /// <summary>
        /// Slides the song's pitch to the specified value over the specified period of time.
        /// </summary>
        /// <param name="pitch">The value to which to slide the song's pitch.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public abstract void SlidePitch(Single pitch, TimeSpan time);

        /// <summary>
        /// Slides the song's pan to the specified value over the specified period of time.
        /// </summary>
        /// <param name="pan">The value to which to slide the song's pan.</param>
        /// <param name="time">The amount of time over which to perform the slide.</param>
        public abstract void SlidePan(Single pan, TimeSpan time);

        /// <summary>
        /// Gets the song's current playback state.
        /// </summary>
        public abstract PlaybackState State
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the song is playing.
        /// </summary>
        public abstract Boolean IsPlaying
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the song is looping.
        /// </summary>
        public abstract Boolean IsLooping
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the song's current playback position.
        /// </summary>
        public abstract TimeSpan Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the song's duration.
        /// </summary>
        public abstract TimeSpan Duration
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value from 0.0 (silent) to 1.0 (full volume) representing the song's volume.
        /// </summary>
        public abstract Single Volume
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value from -1.0 (down one octave) to 1.0 (up one octave) indicating the song's pitch adjustment.
        /// </summary>
        public abstract Single Pitch
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value from -1.0 (full left) to 1.0 (full right) representing the song's panning position.
        /// </summary>
        public abstract Single Pan
        {
            get;
            set;
        }

        /// <summary>
        /// Occurs when playback starts.
        /// </summary>
        public event SongPlayerEventHandler SongStarted;

        /// <summary>
        /// Occurs when playback ends.
        /// </summary>
        public event SongPlayerEventHandler SongEnded;

        /// <summary>
        /// Occurs when the song's playback state changes.
        /// </summary>
        public event SongPlayerEventHandler StateChanged;

        /// <summary>
        /// Raises the SongStarted event.
        /// </summary>
        protected void OnSongStarted()
        {
            var temp = SongStarted;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the SongEnded event.
        /// </summary>
        protected void OnSongEnded()
        {
            var temp = SongEnded;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the StateChanged event.
        /// </summary>
        protected void OnStateChanged()
        {
            var temp = StateChanged;
            if (temp != null)
            {
                temp(this);
            }
        }
    }
}