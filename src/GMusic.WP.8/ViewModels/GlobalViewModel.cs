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
			set 
			{ 
				_allSongs = value; 
				NotifyPropertyChanged("AllSongs"); 
				Save();

				AllAlbums.Clear();
				foreach (var song in _allSongs.Where(song => !String.IsNullOrEmpty(song.Title.Trim()) && !String.IsNullOrEmpty(song.Album.Trim())))
				{
					#region Albums
					// Add Artist if it doesn't exist
					if (!AllAlbums.Any(album => album.Title == song.Album && album.Artist == song.Artist))
						AllAlbums.Add(new Models.GoogleMusicAlbum() {Title = song.Album, AlbumArt = song.ArtURL, Artist = song.Artist});

					// Add Songs
					var albumAvaiable = AllAlbums.First(album => album.Artist == song.Artist && album.Title == song.Album);
					if (albumAvaiable.Songs == null) albumAvaiable.Songs = new List<Models.GoogleMusicSong>();
					albumAvaiable.Songs.Add(song);
					#endregion

					#region Artists
					// Add Artist if it doesn't exist
					if (AllArtists.All(artist => artist.Artist != song.Artist))
						AllArtists.Add(new Models.GoogleMusicArtist { Artist = song.Artist });

					// Add Songs
					var songsAvaiable = AllArtists.First(artist => artist.Artist == song.Artist);
					if (songsAvaiable.Songs == null) songsAvaiable.Songs = new List<Models.GoogleMusicSong>();
					if (!String.IsNullOrEmpty(song.Title.Trim()))
						songsAvaiable.Songs.Add(song);

					// Add Albums
					var albumsAvaiable = AllArtists.First(artist => artist.Artist == song.Artist);
					if (albumsAvaiable.Albums == null) albumsAvaiable.Albums = new List<Models.GoogleMusicAlbum>();
					var albumsToAdd = AllAlbums.Where(album => album.Artist == song.Artist && album.Title == song.Album && !String.IsNullOrEmpty(song.Title.Trim()) && !String.IsNullOrEmpty(song.Album.Trim()));
					if (albumsToAdd.Any()) albumsAvaiable.Albums.AddRange(albumsToAdd);
					#endregion
				}
			}
		}

		private ObservableCollection<Models.GoogleMusicArtist> _allArtists = new ObservableCollection<Models.GoogleMusicArtist>();
		public ObservableCollection<Models.GoogleMusicArtist> AllArtists
		{
			get { return _allArtists; }
			private set { _allArtists = value; NotifyPropertyChanged("AllArtists"); }
		}

		private ObservableCollection<Models.GoogleMusicAlbum> _allAlbums = new ObservableCollection<Models.GoogleMusicAlbum>();
		public ObservableCollection<Models.GoogleMusicAlbum> AllAlbums
		{
			get { return _allAlbums; }
			private set { _allAlbums = value; NotifyPropertyChanged("AllAlbums"); }
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

		private Models.GoogleMusicAlbum _selectedAlbum;
		public Models.GoogleMusicAlbum SelectedAlbum
		{
			get
			{
				return _selectedAlbum;
			}
			set 
			{ 
				_selectedAlbum = value; 
				NotifyPropertyChanged("SelectedAlbum"); 
			}
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
