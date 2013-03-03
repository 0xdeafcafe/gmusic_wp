using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using GMusic.API;
using System;
using System.ComponentModel;
using System.Collections.Generic;
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

				#region Albums
				AllAlbums.Clear();
				foreach (var song in _allSongs.Where(song => !String.IsNullOrEmpty(song.Title.Trim()) && !String.IsNullOrEmpty(song.Album.Trim())))
				{
					// Add Artist if it doesn't exist
					if (!AllAlbums.Any(album => album.Title == song.Album && album.Artist == song.Artist))
						AllAlbums.Add(new Models.GoogleMusicAlbum() {Title = song.Album, AlbumArt = song.ArtURL, Artist = song.Artist});

					// Add Songs
					var albumAvaiable = AllAlbums.First(album => album.Artist == song.Artist && album.Title == song.Album);
					if (albumAvaiable.Songs == null) albumAvaiable.Songs = new List<Models.GoogleMusicSong>();
					albumAvaiable.Songs.Add(song);
				}
				#endregion

				#region Artists
				AllArtists.Clear();
				foreach (var song in _allSongs.Where(song => !String.IsNullOrEmpty(song.Title.Trim()) && !String.IsNullOrEmpty(song.Album.Trim())))
				{
					// Add Artist if it doesn't exist
					if (AllArtists.All(artist => artist.Artist.ToLower().Trim() != song.Artist.ToLower().Trim()))
						AllArtists.Add(new Models.GoogleMusicArtist {Artist = song.Artist});

					// Add Songs
					var songsAvaiable = AllArtists.First(artist => artist.Artist.ToLower().Trim() == song.Artist.ToLower().Trim());
					if (songsAvaiable.Songs == null) songsAvaiable.Songs = new List<Models.GoogleMusicSong>();
					if (!String.IsNullOrEmpty(song.Title.Trim()))
						songsAvaiable.Songs.Add(song);
				}
				foreach(var artist in _allArtists)
					foreach(var album in _allAlbums.Where(album => album.Artist.ToLower().Trim() == artist.Artist.ToLower().Trim()))
					{
						if (artist.Albums == null) artist.Albums = new List<Models.GoogleMusicAlbum>();
						artist.Albums.Add(album);
					}
				#endregion
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

		public IList<Models.GoogleMusicAlbum> NewAlbums
		{
			get
			{
				return _allAlbums.OrderBy(album => album.Songs[0].CreationDate).Take(30).ToList();
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
		private Models.GoogleMusicArtist _selectedArtist;
		public Models.GoogleMusicArtist SelectedArtist
		{
			get
			{
				return _selectedArtist;
			}
			set
			{
				_selectedArtist = value;
				NotifyPropertyChanged("SelectedArtist");
			}
		}

		private Dictionary<string, object> _selectedView = new Dictionary<string, object>();
		public Dictionary<string, object> SelectedView
		{
			get { return _selectedView; }
			set
			{
				_selectedView = value;
				NotifyPropertyChanged("SelectedView");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged(String info)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(info));
		}

		#region Functions
		public Models.GoogleMusicArtist GetArtistFromString(string artistString)
		{
			return _allArtists.FirstOrDefault(artistT => artistT.Artist.ToString().ToLower().Trim() == artistString.ToLower().Trim());
		}

		#endregion

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
