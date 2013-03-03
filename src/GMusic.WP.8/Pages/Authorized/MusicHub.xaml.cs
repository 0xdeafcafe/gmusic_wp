using System;
using System.Windows;
using System.Windows.Controls;
using GMusic.API;
using GMusic.WP._8.Helpers;

namespace GMusic.WP._8.Pages.Authorized
{
	public partial class MusicHub
	{
		public MusicHub()
		{
			InitializeComponent();

			DataContext = App.ViewModel;
		}

		// collection
		private void btnMusicCollection_Click(object sender, RoutedEventArgs e)
		{
			VariousFunctions.NavigateToPage(VariousFunctions.Page.Browse);
		}
		private void btnPlaylistCollection_Click(object sender, RoutedEventArgs e)
		{
			VariousFunctions.NavigateToPage(VariousFunctions.Page.BrowsePlaylists);
		}

		// history


		// new
		private void btnViewAlbum_Click(object sender, RoutedEventArgs e)
		{
			VariousFunctions.ViewAlbum((Models.GoogleMusicAlbum)(((Button)sender).DataContext));
		}

		// appbar
		private void btnAbout_Click(object sender, EventArgs e)
		{
			VariousFunctions.NavigateToPage(VariousFunctions.Page.About);
		}
	}
}