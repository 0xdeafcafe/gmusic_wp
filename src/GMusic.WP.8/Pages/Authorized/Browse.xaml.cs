using System.Windows.Controls;
using GMusic.API;
using GMusic.WP._8.Helpers;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace GMusic.WP._8.Pages.Authorized
{
	public partial class Browse
	{
		private const string alphabet = "#abcdefghijklmnopqrstuvwxyz";

		public Browse()
		{
			InitializeComponent();

			DataContext = App.ViewModel;

			#region Artists
			// add grouping and descriptors
			var sortDescriptorsArtist = new List<DataDescriptor>();
			var sdArtist = new GenericSortDescriptor<Models.GoogleMusicArtist, char>
			{
				KeySelector = artist => artist.Artist[0]
			};
			sortDescriptorsArtist.Add(sdArtist);
			Artists.SortDescriptorsSource = sortDescriptorsArtist;
			Artists.GroupPickerItemsSource = alphabet.Select(c => new string(c, 1)).ToList();
			Artists.GroupDescriptors.Add(
				new GenericGroupDescriptor<Models.GoogleMusicArtist, string>(artist => artist.Artist.Substring(0, 1).ToLower()));
			#endregion

			#region Albums
			// add grouping and descriptors
			var sortDescriptorsAlbum = new List<DataDescriptor>();
			var sdAlbum = new GenericSortDescriptor<Models.GoogleMusicAlbum, char>
			{
				KeySelector = album => album.Title[0]
			};
			sortDescriptorsAlbum.Add(sdAlbum);
			Albums.SortDescriptorsSource = sortDescriptorsAlbum;
			Albums.GroupPickerItemsSource = alphabet.Select(c => new string(c, 1)).ToList();
			Albums.GroupDescriptors.Add(
				new GenericGroupDescriptor<Models.GoogleMusicAlbum, string>(song => song.Title.Substring(0, 1).ToLower()));
			#endregion

			#region Songs
			// add grouping and descriptors
			var sortDescriptorsSong = new List<DataDescriptor>();
			var sdSong = new GenericSortDescriptor<Models.GoogleMusicSong, char>
				         {
					         KeySelector = song => song.Title[0]
				         };
			sortDescriptorsSong.Add(sdSong);
			Songs.SortDescriptorsSource = sortDescriptorsSong;
			Songs.GroupPickerItemsSource = alphabet.Select(c => new string(c, 1)).ToList();
			Songs.GroupDescriptors.Add(
				new GenericGroupDescriptor<Models.GoogleMusicSong, string>(song => song.Title.Substring(0, 1).ToLower()));
			#endregion
		}

		private void Song_GroupPickerItemTap(object sender, GroupPickerItemTapEventArgs e)
		{
			foreach (var group in Songs.Groups.Where(group => Equals(e.DataItem, group.Key)))
			{
				e.DataItemToNavigate = group;
				return;
			}
			e.DataItemToNavigate = Songs.Groups[0];
		}
		private void Artist_GroupPickerItemTap(object sender, GroupPickerItemTapEventArgs e)
		{
			foreach (var group in Songs.Groups.Where(group => Equals(e.DataItem, group.Key)))
			{
				e.DataItemToNavigate = group;
				return;
			}
			e.DataItemToNavigate = Songs.Groups[0];
		}
		private void Album_GroupPickerItemTap(object sender, GroupPickerItemTapEventArgs e)
		{
			foreach (var group in Songs.Groups.Where(group => Equals(e.DataItem, group.Key)))
			{
				e.DataItemToNavigate = group;
				return;
			}
			e.DataItemToNavigate = Songs.Groups[0];
		}
		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			//var button = ((Button) sender);
			//var datacn = (Models.GoogleMusicSong)button.DataContext;
			//datacn.Title;
		}

		private void btnViewAlbum_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			VariousFunctions.ViewAlbum((Models.GoogleMusicAlbum)(((Button)sender).DataContext));
		}
		private void btnViewArtist_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			VariousFunctions.ViewArtist((Models.GoogleMusicArtist)(((Button)sender).DataContext));
		}
	}
}