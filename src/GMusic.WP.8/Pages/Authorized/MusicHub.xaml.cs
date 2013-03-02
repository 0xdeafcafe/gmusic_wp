using System;
using System.Windows;

namespace GMusic.WP._8.Pages.Authorized
{
	public partial class MusicHub
	{
		public MusicHub()
		{
			InitializeComponent();
		}

		private void btnMusicCollection_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new Uri("/Pages/Authorized/Browse.xaml", UriKind.Relative));
		}
	}
}