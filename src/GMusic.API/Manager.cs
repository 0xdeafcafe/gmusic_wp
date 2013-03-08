/* This class was originally written by taylorfinnell (https://github.com/taylorfinnell/) for 
 * his un-official Google Music API (https://github.com/taylorfinnell/NET/).
 * 
 * Modified by Alex Reed (https://github.com/Xerax) for gmusic_wp (https://github.com/Xerax/gmusic_wp)
 * 
 * yolo
 */

using System.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GMusic.API
{
	public class Manager
    {
        #region Events
        public EventHandler OnLoginComplete;

        public delegate void _GetAllSongs(List<Models.GoogleMusicSong> songList);
        public _GetAllSongs OnGetAllSongsComplete;

        public delegate void _AddPlaylist(Models.AddPlaylistResp resp);
        public _AddPlaylist OnCreatePlaylistComplete;

        public delegate void _Error(Exception e);
        public _Error OnError;

        public delegate void _GetPlaylists(Models.GoogleMusicPlaylists pls);
        public _GetPlaylists OnGetPlaylistsComplete;

        public delegate void _GetPlaylist(Models.GoogleMusicPlaylist pls);
        public _GetPlaylist OnGetPlaylistComplete;

        public delegate void _GetSongURL(Models.GoogleMusicSongUrl songurl);
        public _GetSongURL OnGetSongURL;

        public delegate void _DeletePlaylist(Models.DeletePlaylistResp resp);
        public _DeletePlaylist OnDeletePlaylist;

        #endregion

        #region Members
        public GoogleHTTP ConnectionClient;
        private List<Models.GoogleMusicSong> trackContainer;
        #endregion

        #region Constructor
        public Manager()
        {
            ConnectionClient = new GoogleHTTP();
            trackContainer = new List<Models.GoogleMusicSong>();
        }
        #endregion

        #region Login
        public void Login(String email, String password)
        {
            Dictionary<String, String> fields = new Dictionary<String, String>
            {
                {"service", "sj"},
                {"Email",  email},
                {"Passwd", password},
            };

            FormBuilder form = new FormBuilder();
            form.AddFields(fields);
            form.Close();

            ConnectionClient.UploadDataAsync(new Uri("https://www.google.com/accounts/ClientLogin"), form.ContentType, form.GetBytes(),  GetAuthTokenComplete);
        }

        public void Login(String authToken)
        {
            GoogleHTTP.AuthroizationToken = authToken;
            GetAuthCookies();
        }

        private void GetAuthTokenComplete(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                OnError(error);
                return;
            }

            string CountTemplate = @"Auth=(?<AUTH>(.*?))$";
            Regex CountRegex = new Regex(CountTemplate, RegexOptions.IgnoreCase);
            string auth = CountRegex.Match(jsonData).Groups["AUTH"].ToString();

            GoogleHTTP.AuthroizationToken = auth;

            GetAuthCookies();
        }

        private void GetAuthCookies()
        {
            ConnectionClient.UploadDataAsync(new Uri("https://play.google.com/music/listen?hl=en&u=0"),
                FormBuilder.Empty, GetAuthCookiesComplete);
        }

        private void GetAuthCookiesComplete(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                OnError(error);
                return;
            }

            GoogleHTTP.SetCookieData(request.CookieContainer, response.Cookies);

            if (OnLoginComplete != null)
                OnLoginComplete(this, EventArgs.Empty);
        }
        #endregion

        #region GetAllSongs
        /// <summary>
        /// Gets all the songs
        /// </summary>
        /// <param name="continuationToken"></param>
        public void GetAllSongs(String continuationToken = "")
        {
            List<Models.GoogleMusicSong> library = new List<Models.GoogleMusicSong>();

            String jsonString = "{\"continuationToken\":\"" + continuationToken + "\"}";

            Dictionary<String, String> fields = new Dictionary<String, String>
            {
               {"json", jsonString}
            };

            FormBuilder form = new FormBuilder();
            form.AddFields(fields);
            form.Close();

            ConnectionClient.UploadDataAsync(new Uri("https://play.google.com/music/services/loadalltracks"), form, TrackListChunkRecv);
        }

        private void TrackListChunkRecv(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                OnError(error);
                return;
            }

            Models.GoogleMusicPlaylist chunk = JsonConvert.DeserializeObject<Models.GoogleMusicPlaylist>(jsonData);

            trackContainer.AddRange(chunk.Songs);

            if (!String.IsNullOrEmpty(chunk.ContToken))
            {
                GetAllSongs(chunk.ContToken);
            }
            else
            {
                if (OnGetAllSongsComplete != null)
                    OnGetAllSongsComplete(trackContainer);
            }
        }
        #endregion

        #region AddPlaylist
        /// <summary>
        /// Creates a playlist with given name
        /// </summary>
        /// <param name="playlistName">Name of playlist</param>
        public void AddPlaylist(String playlistName)
        {
            String jsonString = "{\"title\":\"" + playlistName + "\"}";

            Dictionary<String, String> fields = new Dictionary<String, String>
            {
               {"json", jsonString}
            };

            FormBuilder form = new FormBuilder();
            form.AddFields(fields);
            form.Close();

            ConnectionClient.UploadDataAsync(new Uri("https://play.google.com/music/services/addplaylist"), form, PlaylistCreated);
        }

        private void PlaylistCreated(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                ThrowError(error);
                return;
            }

            Models.AddPlaylistResp resp = null;

            try
            {
				resp = JsonConvert.DeserializeObject<Models.AddPlaylistResp>(jsonData);
            }
            catch (Exception e)
            {
                ThrowError(error);
                return;
            }

            if (OnCreatePlaylistComplete != null)
                OnCreatePlaylistComplete(resp);
        }
        #endregion

        #region GetPlaylist
		/// <summary>
		/// Returns specified user/instant playlist
		/// </summary>
		public void GetPlaylist(String plID)
		{
			String jsonString = (plID.Equals("all")) ? "{}" : "{\"id\":\"" + plID + "\"}";

			Dictionary<String, String> fields = new Dictionary<String, String>() { };

			fields.Add("json", jsonString);

			FormBuilder builder = new FormBuilder();
			builder.AddFields(fields);
			builder.Close();

			if (plID.Equals("all"))
				ConnectionClient.UploadDataAsync(new Uri("https://play.google.com/music/services/loadplaylist"), builder, PlaylistRecv);
			else
				ConnectionClient.UploadDataAsync(new Uri("https://play.google.com/music/services/loadplaylist"), builder, PlaylistRecvSingle);
		}

        /// <summary>
        /// Returns all user/instant playlists
        /// </summary>
        public void GetPlaylists()
        {
	        GetPlaylist("all");
        }

        private void PlaylistRecvSingle(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                ThrowError(error);
                return;
            }

	        Models.GoogleMusicPlaylist pl;
            try
            {
				pl = JsonConvert.DeserializeObject<Models.GoogleMusicPlaylist>(jsonData);
            }
            catch (Exception e)
            {
                OnError(e);
                return;
            }

            if (OnGetPlaylistComplete != null)
                OnGetPlaylistComplete(pl);
        }
        private void PlaylistRecv(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                ThrowError(error);
                return;
            }

            Models.GoogleMusicPlaylists playlists = null;
            try
            {
				playlists = JsonConvert.DeserializeObject<Models.GoogleMusicPlaylists>(jsonData);
            }
            catch (Exception e)
            {
                ThrowError(error);
                return;
            }

            if (OnGetPlaylistsComplete != null)
                OnGetPlaylistsComplete(playlists);
        }
        #endregion

        #region GetSongURL
        public void GetSongURL(String id)
        {
            ConnectionClient.DownloadStringAsync(new Uri(String.Format("https://play.google.com/music/play?u=0&songid={0}&pt=e", id)), SongUrlRecv);
        }

        private void SongUrlRecv(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                ThrowError(error);
                return;
            }

            Models.GoogleMusicSongUrl url = null;
            try
            {
				url = JsonConvert.DeserializeObject<Models.GoogleMusicSongUrl>(jsonData);
            }
            catch (Exception e)
            {
                OnError(e);
            }

            if (OnGetSongURL != null)
                OnGetSongURL(url);

        }

        private void ThrowError(Exception error)
        {
            if (OnError != null)
                OnError(error);
        }
        #endregion

        #region DeletePlaylist
        //{"deleteId":"c790204e-1ee2-4160-9e25-7801d67d0a16"}
        public void DeletePlaylist(String id)
        {
            String jsonString = "{\"id\":\"" + id + "\"}";

            Dictionary<String, String> fields = new Dictionary<String, String>
            {
               {"json", jsonString}
            };

            FormBuilder form = new FormBuilder();
            form.AddFields(fields);
            form.Close();

            ConnectionClient.UploadDataAsync(new Uri("https://play.google.com/music/services/deleteplaylist"), form, PlaylistDeleted);
        }

        private void PlaylistDeleted(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error)
        {
            if (error != null)
            {
                ThrowError(error);
                return;
            }

            Models.DeletePlaylistResp resp = null;
            try
            {
				resp = JsonConvert.DeserializeObject<Models.DeletePlaylistResp>(jsonData);
            }
            catch (Exception ex)
            {
                ThrowError(ex);
                return;
            }

            if (OnDeletePlaylist != null)
                OnDeletePlaylist(resp);
        }
        #endregion
    }
}