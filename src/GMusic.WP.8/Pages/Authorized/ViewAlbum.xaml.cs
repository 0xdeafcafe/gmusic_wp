﻿using System.Windows.Controls;
using System.Windows.Navigation;
using GMusic.API;
using GMusic.WP._8.Helpers;

namespace GMusic.WP._8.Pages.Authorized
{
	public partial class ViewAlbum
	{
		public ViewAlbum()
		{
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			try
			{
				var viewId = NavigationContext.QueryString["viewId"];
				DataContext = App.ViewModel.SelectedView[viewId];
			}
			catch
			{
				VariousFunctions.TryGoingBack();
			}
			base.OnNavigatedTo(e);
		}

		private void btnViewArtist_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			VariousFunctions.ViewArtist(App.ViewModel.GetArtistFromString(((Models.GoogleMusicAlbum)(((Button)sender).DataContext)).Artist));
		}
	}
}