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

		private void btnMusicCollection_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/Authorized/Browse.xaml", UriKind.Relative));
		}

		private void btnAbout_Click(object sender, EventArgs e)
		{
			VariousFunctions.NavigateToPage(VariousFunctions.Page.About);
		}

		private void btnViewAlbum_Click(object sender, RoutedEventArgs e)
		{
			VariousFunctions.ViewAlbum((Models.GoogleMusicAlbum)(((Button)sender).DataContext));
		}
	}
}