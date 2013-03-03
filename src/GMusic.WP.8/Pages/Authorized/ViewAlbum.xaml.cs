using Microsoft.Phone.Controls;

namespace GMusic.WP._8.Pages.Authorized
{
	public partial class ViewAlbum : PhoneApplicationPage
	{
		public ViewAlbum()
		{
			InitializeComponent();

			DataContext = App.ViewModel.SelectedAlbum;
		}
	}
}