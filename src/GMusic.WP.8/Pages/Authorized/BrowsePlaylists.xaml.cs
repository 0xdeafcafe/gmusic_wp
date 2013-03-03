namespace GMusic.WP._8.Pages.Authorized
{
	public partial class BrowsePlaylists
	{
		public BrowsePlaylists()
		{
			InitializeComponent();

			DataContext = App.ViewModel.Playlists;
		}

		private void btnViewUserPlaylist_Click(object sender, System.Windows.RoutedEventArgs e)
		{

		}
		private void btnViewInstantMix_Click(object sender, System.Windows.RoutedEventArgs e)
		{

		}
	}
}