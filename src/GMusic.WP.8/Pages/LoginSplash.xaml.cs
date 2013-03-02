using System;
using System.Windows;
namespace GMusic.WP._8.Pages
{
	public partial class LoginSplash
	{
		public LoginSplash()
		{
			InitializeComponent();

			PendingAnimation.Begin();
			App.ApiManager.OnError += exception => MessageBox.Show(exception.ToString(), "Error", MessageBoxButton.OK);
			App.ApiManager.Login(App.IsolatedStorage.GoogleAuthToken);
			App.ApiManager.OnLoginComplete +=
				(sender, args) =>
				Dispatcher.BeginInvoke(
					() =>
						{
							App.ViewModel.Load();
							NavigationService.Navigate(new Uri("/Pages/Authorized/MusicHub.xaml", UriKind.Relative));
						});
		}
	}
}