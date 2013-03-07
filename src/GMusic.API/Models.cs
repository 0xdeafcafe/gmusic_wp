/* This class was originally written by taylorfinnell (https://github.com/taylorfinnell/) for 
 * his un-official Google Music API (https://github.com/taylorfinnell/GoogleMusicAPI.NET/).
 * 
 * Modified by Alex Reed (https://github.com/Xerax) for gmusic_wp (https://github.com/Xerax/gmusic_wp)
 * 
 * yolo
 */

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GMusic.API
{
	public class Models
	{
		public class GoogleMusicSongUrl
		{
			[JsonProperty("url")]
			public String URL { get; set; }
		}
		public class AddPlaylistResp
		{
			[JsonProperty("id")]
			public String ID { get; set; }


			[JsonProperty("title")]
			public String Title { get; set; }


			[JsonProperty("success")]
			public bool Success { get; set; }
		}
		public class DeletePlaylistResp
		{
			[JsonProperty("deleteId")]
			public String ID { get; set; }
		}

		public class GoogleMusicPlaylists
		{
			[JsonProperty("playlists")]
			public List<GoogleMusicPlaylist> UserPlaylists { get; set; }


			[JsonProperty("magicPlaylists")]
			public List<GoogleMusicPlaylist> InstantMixes { get; set; }
		}
		public class GoogleMusicPlaylist
		{
			[JsonProperty("title")]
			public string Title { get; set; }


			[JsonProperty("playlistId")]
			public string PlaylistId { get; set; }


			[JsonProperty("requestTime")]
			public double RequestTime { get; set; }


			[JsonProperty("continuationToken")]
			public string ContToken { get; set; }


			[JsonProperty("differentialUpdate")]
			public bool DiffUpdate { get; set; }


			[JsonProperty("playlist")]
			public List<GoogleMusicSong> Songs { get; set; }


			[JsonProperty("continuation")]
			public bool Cont { get; set; }


			public string SongCount
			{
				get { return string.Format("{0:n0} songs", Songs.Count); }
			}
		}

		public class GoogleMusicSong
		{
            public string Genre { get; set; }
            public int BeatsPerMinute { get; set; }
            public string AlbumArtistNorm { get; set; }
            public string ArtistNorm { get; set; }
            public string Album { get; set; }
            public object LastPlayed { get; set; }
            public int Type { get; set; }
            public object RecentTimestamp { get; set; }
            public int Disc { get; set; }
            public string Id { get; set; }
            public string Composer { get; set; }
            public string Title { get; set; }
            public string AlbumArtist { get; set; }
            public string ArtistMatchedId { get; set; }
            public int TotalTracks { get; set; }
            public bool SubjectToCuration { get; set; }
            public string Name { get; set; }
            public int TotalDiscs { get; set; }
            public int Year { get; set; }
            public string TitleNorm { get; set; }
            public string Artist { get; set; }
            public string AlbumNorm { get; set; }
            public int Track { get; set; }
            public int DurationMillis { get; set; }
            public string MatchedId { get; set; }
            public string AlbumArtUrl { get; set; }
            public bool Deleted { get; set; }
            public string Url { get; set; }
            public object CreationDate { get; set; }
            public int PlayCount { get; set; }
            public int Bitrate { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; }
		}
		public class GoogleMusicArtist
		{
			[JsonProperty("artist")]
			public string Artist { get; set; }

			[JsonProperty("albums")]
			public List<GoogleMusicAlbum> Albums { get; set; }

			[JsonProperty("songs")]
			public List<GoogleMusicSong> Songs { get; set; }
		}
		public class GoogleMusicAlbum
		{
			[JsonProperty("artist")]
			public string Artist { get; set; }

			[JsonProperty("title")]
			public string Title { get; set; }

			[JsonProperty("album_art")]
			public string AlbumArt { get; set; }

			[JsonProperty("songs")]
			public List<GoogleMusicSong> Songs { get; set; }
		}
		public class GoogleMusicGenre
		{
			[JsonProperty("genre")]
			public string Genre { get; set; }

			[JsonProperty("songs")]
			public List<GoogleMusicSong> Songs { get; set; }
		}
	}
}