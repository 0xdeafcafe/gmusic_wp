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
		};


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


			public string TrackString
			{
				get { return Songs.Count + " tracks"; }
			}
		}


		public class GoogleMusicSong
		{
			string albumart;

			[JsonProperty("genre")]
			public string Genre { get; set; }


			[JsonProperty("beatsPerMinute")]
			public int BPM { get; set; }


			[JsonProperty("albumArtistNorm")]
			public string AlbumArtistNorm { get; set; }


			[JsonProperty("artistNorm")]
			public string ArtistNorm { get; set; }


			[JsonProperty("album")]
			public string Album { get; set; }


			[JsonProperty("lastPlayed")]
			public double LastPlayed { get; set; }


			[JsonProperty("type")]
			public int Type { get; set; }


			[JsonProperty("disc")]
			public int Disc { get; set; }


			[JsonProperty("id")]
			public string ID { get; set; }


			[JsonProperty("composer")]
			public string Composer { get; set; }


			[JsonProperty("title")]
			public string Title { get; set; }


			[JsonProperty("albumArtist")]
			public string AlbumArtist { get; set; }


			[JsonProperty("totalTracks")]
			public int TotalTracks { get; set; }


			[JsonProperty("name")]
			public string Name { get; set; }


			[JsonProperty("totalDiscs")]
			public int TotalDiscs { get; set; }


			[JsonProperty("year")]
			public int Year { get; set; }


			[JsonProperty("titleNorm")]
			public string TitleNorm { get; set; }


			[JsonProperty("artist")]
			public string Artist { get; set; }


			[JsonProperty("albumNorm")]
			public string AlbumNorm { get; set; }


			[JsonProperty("track")]
			public int Track { get; set; }


			[JsonProperty("durationMillis")]
			public long Duration { get; set; }


			[JsonProperty("albumArt")]
			public string AlbumArt { get; set; }


			[JsonProperty("deleted")]
			public bool Deleted { get; set; }


			[JsonProperty("url")]
			public string URL { get; set; }


			[JsonProperty("creationDate")]
			public float CreationDate { get; set; }


			[JsonProperty("playCount")]
			public int Playcount { get; set; }


			[JsonProperty("rating")]
			public int Rating { get; set; }


			[JsonProperty("comment")]
			public string Comment { get; set; }


			[JsonProperty("albumArtUrl")]
			public string ArtURL
			{
				get
				{
					return (albumart != null && !albumart.StartsWith("http:")) ? "http:" + albumart : albumart;
				}
				set { albumart = value; }
			}


			public string ArtistAlbum
			{
				get
				{
					return Artist + ", " + Album;
				}
			}
		}
	}
}
