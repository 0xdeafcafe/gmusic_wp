using System.Collections.ObjectModel;
using System.Windows;
using GMusic.API;

namespace GMusic.WP._8.Pages
{
	public partial class UpdatingCache
	{
		public UpdatingCache()
		{
			InitializeComponent();

			PendingAnimation.Begin();

			App.ApiManager.OnError += exception => { MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK); };
			App.ApiManager.GetAllSongs();
			App.ApiManager.OnGetAllSongsComplete += songs =>
				                                        {
					                                        App.ViewModel.AllSongs = new ObservableCollection<Models.GoogleMusicSong>(songs);

															App.ApiManager.GetPlaylists();
					                                        App.ApiManager.OnGetPlaylistsComplete += playlists =>
						                                                                                 {
																											 App.ViewModel.Playlists = (playlists);
						                                                                                 };
				                                        };
		}
	}
}