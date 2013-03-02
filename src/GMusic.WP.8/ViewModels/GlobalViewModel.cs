using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using GMusic.API;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using GMusic.WP._8.Helpers;
using GMusic.WP._8.Resources;
using Newtonsoft.Json;

namespace GMusic.WP._8.ViewModels
{
	public class GlobalViewModel : INotifyPropertyChanged, IStorage
	{
		private ObservableCollection<Models.GoogleMusicSong> _allSongs = new ObservableCollection<Models.GoogleMusicSong>();
		public ObservableCollection<Models.GoogleMusicSong> AllSongs
		{
			get { return _allSongs; }
			set { _allSongs = value; NotifyPropertyChanged("AllSongs"); Save(); }
		}
		public List<AlphaKeyGroup<Models.GoogleMusicSong>> GroupedSongs
		{
			get
			{
				return AlphaKeyGroup<Models.GoogleMusicSong>.CreateGroups(
					AllSongs,
					s => s.Name,
					true);
			}
		} 


		public IList<Models.GoogleMusicSong> NewSongs
		{
			get
			{
				return _allSongs.OrderBy(song => song.CreationDate).Take(30).ToList();
			}
		}
		public IList<Models.GoogleMusicSong> RecentPlayed
		{
			get
			{
				return _allSongs.OrderBy(song => song.LastPlayed).Take(30).ToList();
			}
		}

		private Models.GoogleMusicPlaylists _playlists = new Models.GoogleMusicPlaylists();
		public Models.GoogleMusicPlaylists Playlists
		{
			get { return _playlists; }
			set { _playlists = value; NotifyPropertyChanged("Playlists"); Save(); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(info));
		}


		#region Storage
		private readonly IsolatedStorageSettings _appSettings = IsolatedStorageSettings.ApplicationSettings;

		public void Load()
		{
			AllSongs = JsonConvert.DeserializeObject<ObservableCollection<Models.GoogleMusicSong>>(LoadString("all_songs"));
			Playlists = JsonConvert.DeserializeObject<Models.GoogleMusicPlaylists>(LoadString("playlists"));
		}
		private string LoadString(string key)
		{
			return _appSettings.Contains(key) ? (string)_appSettings[key] : null;
		}

		public void Save()
		{
			SaveString("all_songs", JsonConvert.SerializeObject(AllSongs));
			SaveString("playlists", JsonConvert.SerializeObject(Playlists));

			_appSettings.Save();
		}
		private void SaveString(string key, object value)
		{
			if (_appSettings.Contains(key))
				_appSettings[key] = value;
			else
				_appSettings.Add(key, value);
		}
		#endregion
	}
}
