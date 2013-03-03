using System.Windows.Navigation;
using GMusic.WP._8.Helpers;

namespace GMusic.WP._8.Pages.Authorized
{
	public partial class ViewGenre
	{
		public ViewGenre()
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
	}
}