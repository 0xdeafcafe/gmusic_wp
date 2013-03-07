using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using GMusic.API;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Phone.BackgroundAudio;
using GMusic.WP._8.Helpers;
using System.Net;

namespace GMusic.WP._8.ViewModels
{
	public class GlobalViewModel : INotifyPropertyChanged
	{
		private ObservableCollection<Models.GoogleMusicSong> _allSongs = new ObservableCollection<Models.GoogleMusicSong>();
		public ObservableCollection<Models.GoogleMusicSong> AllSongs
		{
			get { return _allSongs; }
			set 
			{ 
				_allSongs = value; 
				NotifyPropertyChanged("AllSongs");
				Save("all_songs");

                if (NowPlaying == null)
                {
                    NowPlaying = AllSongs.Take(10).ToList();
                    NextTrack();
                }

			    #region Albums
				AllAlbums.Clear();
				foreach (var song in _allSongs.Where(song => !String.IsNullOrEmpty(song.Title.Trim()) && !String.IsNullOrEmpty(song.Album.Trim())))
				{
					// Add Artist if it doesn't exist
					if (!AllAlbums.Any(album => album.Title == song.Album))
						AllAlbums.Add(new Models.GoogleMusicAlbum { Title = song.Album, AlbumArt = song.AlbumArtUrl, Artist = song.Artist });

					// Add Songs
					var albumAvaiable = AllAlbums.First(album => album.Title == song.Album);
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
						AllArtists.Add(new Models.GoogleMusicArtist { Artist = song.Artist });

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

				#region Genre
				foreach (var song in _allSongs.Where(song => !String.IsNullOrEmpty(song.Title.Trim()) && !String.IsNullOrEmpty(song.Album.Trim())))
				{
					// Add Genre if it doesn't exist
					if (AllGenres.All(genre => genre.Genre.ToLower().Trim() != song.Genre.ToLower().Trim()))
						AllGenres.Add(new Models.GoogleMusicGenre() { Genre = song.Genre });

					// Add song to that genre
					var genreToPopulate = AllGenres.First(genre => genre.Genre.ToLower().Trim() == song.Genre.ToLower().Trim());
					if (genreToPopulate.Songs == null) genreToPopulate.Songs = new List<Models.GoogleMusicSong>();
					if (!String.IsNullOrEmpty(song.Title.Trim()))
						genreToPopulate.Songs.Add(song);
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

		private ObservableCollection<Models.GoogleMusicGenre> _allGenres = new ObservableCollection<Models.GoogleMusicGenre>();
		public ObservableCollection<Models.GoogleMusicGenre> AllGenres
		{
			get { return _allGenres; }
			private set { _allGenres = value; NotifyPropertyChanged("AllGenres"); }
		}


	    public BackgroundAudioPlayer Player
	    {
            get { return BackgroundAudioPlayer.Instance; }
	    }
        public IList<Models.GoogleMusicSong> NowPlaying { get; set; }


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
			set { _playlists = value; NotifyPropertyChanged("Playlists"); Save("playlists"); }
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
        public async void NextTrack()
        {
            NowPlaying.Move(ListExtensions.Destination.End);

            if (string.IsNullOrEmpty(NowPlaying[0].Url))
            {
                var derp = await DownloadSongUrl(NowPlaying[0]);

                NowPlaying[0].Url = derp.URL;
            }

            BeginPlay();
        }

        private void BeginPlay()
        {
            var song = NowPlaying[0];

            Player.Track = new AudioTrack(new Uri(song.Url, UriKind.Absolute), song.Title, song.Artist, song.Album, new Uri(song.AlbumArtUrl, UriKind.Absolute));
            Player.Play();
        }

        public Task<Models.GoogleMusicSongUrl> DownloadSongUrl(Models.GoogleMusicSong song)
        {
            var tcs = new TaskCompletionSource<Models.GoogleMusicSongUrl>();

            if (!String.IsNullOrEmpty(song.Url.Trim()))
            {
                var url = new Uri(song.Url);
                if (!Time.Helpers.HasPassed(new QueryString(url)["expire"]))
                {
                    tcs.SetResult(new Models.GoogleMusicSongUrl
                                      {
                                          URL = song.Url
                                      });
                    return tcs.Task;
                }
            }
            App.ApiManager.OnError += tcs.SetException;
            App.ApiManager.OnGetSongURL += songurl =>
                                               {
                                                   song.Url = songurl.URL;
                                                   AddUrlToAllSongs(song);
                                                   tcs.SetResult(songurl);
                                               };
            App.ApiManager.GetSongURL(song.Id);

            return tcs.Task;
        }
        public void AddUrlToAllSongs(Models.GoogleMusicSong song)
        {
            for(var i = 0; i < AllSongs.Count(); i++)
            {
                if (AllSongs[i].Id == song.Id) continue;

                AllSongs[i].Url = song.Url;
                return;
            }
        }
		public Models.GoogleMusicArtist GetArtistFromString(string artistString)
		{
			return _allArtists.FirstOrDefault(artistT => artistT.Artist.ToString(CultureInfo.InvariantCulture).ToLower().Trim() == artistString.ToLower().Trim());
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

		public void Save(string loadId = null)
		{
			if (loadId == null)
			{
				SaveString("all_songs", JsonConvert.SerializeObject(AllSongs));
				SaveString("playlists", JsonConvert.SerializeObject(Playlists));
			}
			else
			{
				switch (loadId)
				{
					case "all_songs":
						SaveString("all_songs", JsonConvert.SerializeObject(AllSongs));
						break;
					case "playlists":
						SaveString("playlists", JsonConvert.SerializeObject(Playlists));
						break;

					default:
						break;
				}
			}

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