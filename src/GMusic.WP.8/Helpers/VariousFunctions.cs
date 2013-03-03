using System;
using GMusic.API;

namespace GMusic.WP._8.Helpers
{
	public static class VariousFunctions
	{
		public static void ViewAlbum(Models.GoogleMusicAlbum album)
		{
			App.ViewModel.SelectedAlbum = album;

			NavigateToPage(Page.ViewAlbum);
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
			MusicHub,
			ViewAlbum
		}
		public static void NavigateToPage(Page page)
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

				case Page.Browse:
					pageFriendly += "Authorized/Browse";
					break;
				case Page.MusicHub:
					pageFriendly += "Authorized/MusicHub";
					break;
				case Page.ViewAlbum:
					pageFriendly += "Authorized/ViewAlbum";
					break;
			}
			pageFriendly += ".xaml";

			App.RootFrame.Navigate(new Uri(
                pageFriendly, UriKind.Relative));

		}
	}
}
