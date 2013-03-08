using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GMusic.API;
using GMusic.WP._8.Helpers;
using Microsoft.Phone.BackgroundAudio;

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
        private async void btnNowPlaying_Click(object sender, RoutedEventArgs e)
        {
            var nowPlaying = App.ViewModel.AllSongs.OrderBy(song => song.Title).Take(4).ToList();

            var url = await App.ViewModel.DownloadSongUrl(nowPlaying[0]);
            App.ViewModel.Player.Track = new AudioTrack(new Uri(url.URL), nowPlaying[0].Title, "d0p3",
                                                        "swag", new Uri(nowPlaying[0].AlbumArtUrl));
            App.ViewModel.Player.Play();

            VariousFunctions.NavigateToPage(VariousFunctions.Page.Play);
        }
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