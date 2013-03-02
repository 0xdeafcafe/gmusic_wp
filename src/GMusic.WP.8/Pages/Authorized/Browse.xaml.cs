using GMusic.API;
using System.Collections.Generic;
using System.Globalization;
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

			// don't fucking judge me, faggot.
			List<string> days = App.ViewModel.AllSongs.Select(song => song.Title + "££$$%%$$££" + song.Artist).ToList();
			Songs.ItemsSource = days;

			// we do not want async balance since item templates are simple
			Songs.IsAsyncBalanceEnabled = false;

			// add custom group picker items, including all alphabetic characters
			List<string> groupPickerItems = new List<string>(32);
			foreach (char c in alphabet)
			{
				groupPickerItems.Add(new string(c, 1));
			}
			Songs.GroupPickerItemsSource = groupPickerItems;

			// hook the GroupPickerItemTap event to provide our custom logic since the jump list does not know how to manipulate our items
			//Songs.GroupPickerItemTap += this.OnJumpList_GroupPickerItemTap;

			// add the group and sort descriptors
			GenericGroupDescriptor<string, string> groupByFirstName = new GenericGroupDescriptor<string, string>(contact => contact.Substring(0, 1).ToLower());
			Songs.GroupDescriptors.Add(groupByFirstName);

			//GenericSortDescriptor<string, string> sort = new GenericSortDescriptor<string, string>(contact => contact);
			//Songs.SortDescriptors.Add(sort);
		}

		private void Songs_GroupPickerItemTap(object sender, GroupPickerItemTapEventArgs e)
		{
			foreach (DataGroup group in Songs.Groups)
			{
				if (object.Equals(e.DataItem, group.Key))
				{
					e.DataItemToNavigate = group;
					return;
				}
			}

			e.DataItemToNavigate = Songs.Groups[0];
			return;
		}
	}
}