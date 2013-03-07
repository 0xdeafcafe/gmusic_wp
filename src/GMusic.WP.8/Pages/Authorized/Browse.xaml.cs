using System.Globalization;
using System.Windows.Controls;
using GMusic.API;
using GMusic.WP._8.Helpers;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using System;

namespace GMusic.WP._8.Pages.Authorized
{
	public partial class Browse
	{
		private const string alphabet = "#abcdefghijklmnopqrstuvwxyz";
        private List<string> inUse = new List<string>();

		public Browse()
		{
			InitializeComponent();

			DataContext = App.ViewModel;

            #region Artists
            inUse = new List<string>();
		    Artists.ItemsSource = App.ViewModel.AllArtists;
		    Artists.SortDescriptors.Add(new GenericSortDescriptor<Models.GoogleMusicArtist, char>
		                                    {
                                                KeySelector = artist => AddDescriptior(artist.Artist)
		                                    });
            Artists.GroupDescriptors.Add(new GenericGroupDescriptor<Models.GoogleMusicArtist, char>
		                                    {
                                                KeySelector = artist => AddDescriptior(artist.Artist)
		                                    });
		    FinishInUseList();
		    Artists.GroupPickerItemsSource = string.Join("", inUse).Select(c => new string(c, 1)).ToList();
			#endregion

			#region Albums
            inUse = new List<string>();
            Albums.ItemsSource = App.ViewModel.AllAlbums;
            Albums.SortDescriptors.Add(new GenericSortDescriptor<Models.GoogleMusicAlbum, string>
            {
                KeySelector = album => AddDescriptior(album.Artist).ToString(CultureInfo.InvariantCulture)
            });
            Albums.GroupDescriptors.Add(new GenericGroupDescriptor<Models.GoogleMusicAlbum, char>
            {
                KeySelector = album => AddDescriptior(album.Artist)
            });
            FinishInUseList();
            Albums.GroupPickerItemsSource = string.Join("", inUse).Select(c => new string(c, 1)).ToList();
			#endregion

            #region Songs
            inUse = new List<string>();
            Songs.ItemsSource = App.ViewModel.AllSongs;
            Songs.SortDescriptors.Add(new GenericSortDescriptor<Models.GoogleMusicSong, string>
            {
                KeySelector = song => AddDescriptior(song.Title).ToString(CultureInfo.InvariantCulture)
            });
            Songs.GroupDescriptors.Add(new GenericGroupDescriptor<Models.GoogleMusicSong, char>
            {
                KeySelector = song => AddDescriptior(song.Title)
            });
            FinishInUseList();
            Songs.GroupPickerItemsSource = string.Join("", inUse).Select(c => new string(c, 1)).ToList();
			#endregion

            #region Genre
            inUse = new List<string>();
            Genres.ItemsSource = App.ViewModel.AllGenres;
            Genres.SortDescriptors.Add(new GenericSortDescriptor<Models.GoogleMusicGenre, string>
            {
                KeySelector = genre => AddDescriptior(genre.Genre).ToString(CultureInfo.InvariantCulture)
            });
            Genres.GroupDescriptors.Add(new GenericGroupDescriptor<Models.GoogleMusicGenre, char>
            {
                KeySelector = genre => AddDescriptior(genre.Genre)
            });
            FinishInUseList();
            Genres.GroupPickerItemsSource = string.Join("", inUse).Select(c => new string(c, 1)).ToList();
			#endregion
		}

		private void View_GroupPickerItemTap(object sender, GroupPickerItemTapEventArgs e)
		{
		    var index = 0;
		    var alphaList = alphabet.ToList();
            for (var i = 0; i < alphaList.Count(); i++)
                if (alphaList[i] == ((string)e.DataItem)[0])
                    index = i;
		    e.DataItemToNavigate = Artists.Groups[index];
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
		private void btnViewGenre_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			VariousFunctions.ViewGenre((Models.GoogleMusicGenre)(((Button)sender).DataContext));
		}

        public char AddDescriptior(string alpha)
        {
            var beta = alpha.ToLower().StartsWith("the ")
                ? Char.IsLetter(alpha[4]) ? Char.ToLower(alpha[4]) : '#'
                : Char.IsLetter(alpha[0]) ? Char.ToLower(alpha[0]) : '#';

            if (inUse.Count(use => Char.ToLowerInvariant(use[0]) == beta) <= 0)
                inUse.Add(Char.ToLowerInvariant(beta).ToString());

            return beta;
        }
        public void FinishInUseList()
        {
            foreach (var letter in alphabet.Where(letter => inUse.Count(use => use[0] == letter) <= 0))
                inUse.Add(Char.ToUpperInvariant(letter).ToString());

            inUse.Sort();
        }
	}
}