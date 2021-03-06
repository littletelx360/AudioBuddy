﻿using FilenameBuddy;
using InputHelper;
using MenuBuddy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AudioBuddy
{
	/// <summary>
	/// This is a screen where you can do a music test.
	/// </summary>
	public class SoundTestScreen : MenuScreen
	{
		#region Properties

		/// <summary>
		/// The index of the music to play
		/// </summary>
		private int _musicIndex;

		/// <summary>
		/// the index of the sound to play
		/// </summary>
		private int _soundIndex;

		/// <summary>
		/// list of all the music cue names
		/// Add all your music cue name to this list
		/// </summary>
		private List<Filename> MusicNames { get; set; }

		/// <summary>
		/// list of all the sounds fx cues
		/// Add all your sound effect cues to this list
		/// </summary>
		private List<string> SoundEffectNames { get; set; }

		private List<SoundEffect> SoundEffects { get; set; }

		private MenuEntry MusicMenuEntry { get; set; }

		private MenuEntry SoundFxMenuEntry { get; set; }

		#endregion //Properties

		#region Methods

		public SoundTestScreen()
			: base("Sound Test")
		{
			//quiet please
			AudioManager.StopMusic();

			//set up the lists
			MusicNames = new List<Filename>();
			SoundEffectNames = new List<string>() { "None" };
			SoundEffects = new List<SoundEffect>();
			_musicIndex = 0;
			_soundIndex = 0;
		}

		public override async Task LoadContent()
		{
			await base.LoadContent();

			//setup the music option
			MusicMenuEntry = new MenuEntry(MusicText(), Content)
			{
				IsQuiet = true
			};

			MusicMenuEntry.OnClick += NextMusic;

			AddMenuEntry(MusicMenuEntry);

			//Setup the sound fx option
			SoundFxMenuEntry = new MenuEntry(SoundText(), Content)
			{
				IsQuiet = true
			};

			SoundFxMenuEntry.OnClick += NextSound;

			AddMenuEntry(SoundFxMenuEntry);

			var backMenuEntry = new MenuEntry("Back", Content)
			{
				IsQuiet = true
			};
			backMenuEntry.OnClick += Cancelled;
			AddMenuEntry(backMenuEntry);
		}

		/// <summary>
		/// Add all the music to this screen after it has been initialized.
		/// </summary>
		/// <param name="music"></param>
		public void AddMusic(List<Filename> music)
		{
			MusicNames.AddRange(music);
		}

		/// <summary>
		/// Add all the sound effect to this screen
		/// </summary>
		/// <param name="soundfx"></param>
		public void AddSoundFx(List<Filename> soundfx)
		{
			foreach (var sound in soundfx)
			{
				SoundEffectNames.Add(sound.GetFileNoExt());
				SoundEffects.Add(AudioManager.GetSoundEffect(sound));
			}
		}

		#region Music

		/// <summary>
		/// get the text for the music menu option
		/// </summary>
		/// <returns></returns>
		private string MusicText()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Music: ");
			sb.Append(AudioManager.CurrentMusicFile());
			return sb.ToString();
		}

		/// <summary>
		/// Increment the music that is playing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NextMusic(object sender, EventArgs e)
		{
			//increment the music to play
			_musicIndex++;
			if (_musicIndex > MusicNames.Count)
			{
				_musicIndex = 0;
			}
			PlayMusic();
		}

		/// <summary>
		/// decrement the music that is playing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PrevMusic(object sender, EventArgs e)
		{
			//decrement the music to play
			_musicIndex--;
			if (_musicIndex < 0)
			{
				_musicIndex = MusicNames.Count;
			}
			PlayMusic();
		}

		/// <summary>
		/// Play (or stop) the music.
		/// </summary>
		private void PlayMusic(object sender, ClickEventArgs e)
		{
			PlayMusic();
		}

		private void PlayMusic()
		{
			if (0 == _musicIndex)
			{
				//check if trying to stop the music
				AudioManager.StopMusic();
			}
			else
			{
				//play the selected song
				AudioManager.PlayMusic(MusicNames[_musicIndex - 1]);
			}

			//set the text of the menu option
			MusicMenuEntry.Text = MusicText();
		}

		#endregion //Music

		#region Sound Fx

		/// <summary>
		/// get the text for the sound effect menu entry
		/// </summary>
		/// <returns></returns>
		private string SoundText()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Sound: ");
			sb.Append(SoundEffectNames[_soundIndex]);
			return sb.ToString();
		}

		/// <summary>
		/// Increment the Sound that is playing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NextSound(object sender, EventArgs e)
		{
			//increment the Sound to play
			_soundIndex++;
			if (_soundIndex >= SoundEffectNames.Count)
			{
				_soundIndex = 0;
			}

			//set the text of the menu option
			SoundFxMenuEntry.Text = SoundText();

			//Play the sound fx
			PlaySound(sender, new ClickEventArgs(SoundFxMenuEntry.Position.ToVector2(), MouseButton.Left, null));
		}

		/// <summary>
		/// decrement the Sound that is playing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PrevSound(object sender, EventArgs e)
		{
			//decrement the Sound to play
			_soundIndex--;
			if (_soundIndex < 0)
			{
				_soundIndex = SoundEffectNames.Count - 1;
			}

			//set the text of the menu option
			SoundFxMenuEntry.Text = SoundText();

			//Play the sound fx
			PlaySound(sender, new ClickEventArgs(SoundFxMenuEntry.Position.ToVector2(), MouseButton.Left, null));
		}

		/// <summary>
		/// Play (or stop) the Sound.
		/// </summary>
		private void PlaySound(object sender, ClickEventArgs e)
		{
			if (0 != _soundIndex)
			{
				//play the selected song
				SoundEffects[_soundIndex - 1].Play();
			}
		}

		#endregion //Sound Fx

		#endregion //Methods
	}
}