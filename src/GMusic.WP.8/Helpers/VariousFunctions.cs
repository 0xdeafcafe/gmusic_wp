using System;
using GMusic.API;
using System.Collections.Generic;
using System.Windows;

namespace GMusic.WP._8.Helpers
{
	public static class VariousFunctions
	{
		public static void ViewAlbum(Models.GoogleMusicAlbum album)
		{
			var viewId = GenerateRandomViewId();
			App.ViewModel.SelectedView.Add(viewId, album);

			NavigateToPage(Page.ViewAlbum, new Dictionary<string, string> { { "viewId", viewId } });
		}
		public static void ViewArtist(Models.GoogleMusicArtist artist)
		{
			var viewId = GenerateRandomViewId();
			App.ViewModel.SelectedView.Add(viewId, artist);

			NavigateToPage(Page.ViewArtist, new Dictionary<string, string> { { "viewId", viewId } });
		}
		public static void ViewGenre(Models.GoogleMusicGenre genre)
		{
			var viewId = GenerateRandomViewId();
			App.ViewModel.SelectedView.Add(viewId, genre);

			NavigateToPage(Page.ViewGenre, new Dictionary<string, string> { { "viewId", viewId } });
		}
		public static void ViewPlaylist(Models.GoogleMusicPlaylist playlist)
		{
			var viewId = GenerateRandomViewId();
			App.ViewModel.SelectedView.Add(viewId, playlist);

			NavigateToPage(Page.ViewPlaylist, new Dictionary<string, string> { { "viewId", viewId } });
		}
		public static void TryGoingBack()
		{
			MessageBox.Show("There was an unknown error loading the specfied information. Sorry about that.", "Unknown Error",
			                MessageBoxButton.OK);

			if (App.RootFrame.CanGoBack)
				App.RootFrame.GoBack();
			else
				NavigateToPage(Page.MusicHub);
		}

		public static string GenerateRandomViewId()
		{
			var random = new Random();
			return string.Format("{0}::{1}", random.Next(0, 10000), random.Next(0, 10000));
		}

		public enum Page
		{
			// Un Authorized
			About,
			LoginSplash,
			Signin,
			UpdatingCache,

			// Authorized
			Browse,
			BrowsePlaylists,
			MusicHub,
			ViewAlbum,
			ViewArtist,
			ViewGenre,
			ViewPlaylist
		}
		public static void NavigateToPage(Page page, Dictionary<string, string> queryStringParams = null)
		{
			var pageFriendly = "/Pages/";
			switch (page)
			{
				case Page.About:
					pageFriendly += "About";
					break;
				case Page.LoginSplash:
					pageFriendly += "LoginSplash";
					break;
				case Page.Signin:
					pageFriendly += "Signin";
					break;
				case Page.UpdatingCache:
					pageFriendly += "UpdatingCache";
					break;

				case Page.BrowsePlaylists:
					pageFriendly += "Authorized/BrowsePlaylists";
					break;
				case Page.Browse:
					pageFriendly += "Authorized/Browse";
					break;
				case Page.MusicHub:
					pageFriendly += "Authorized/MusicHub";
					break;
				case Page.ViewAlbum:
					pageFriendly += "Authorized/ViewAlbum";
					break;
				case Page.ViewArtist:
					pageFriendly += "Authorized/ViewArtist";
					break;
				case Page.ViewGenre:
					pageFriendly += "Authorized/ViewGenre";
					break;
				case Page.ViewPlaylist:
					pageFriendly += "Authorized/ViewPlaylist";
					break;
			}
			pageFriendly += ".xaml";

			if (queryStringParams != null)
			{
				var i = 0;
				foreach (var entry in queryStringParams)
				{
					var toAdd = "";
					if (i == 0)
						toAdd += "?";
					else
						toAdd += "&";

					toAdd += string.Format("{0}={1}", entry.Key, entry.Value);
					pageFriendly += toAdd;

					i++;
				}
			}

			App.RootFrame.Navigate(new Uri(
                pageFriendly, UriKind.Relative));

		}
	}
}