using GMusic.API;
using System;
using System.Windows;

namespace GMusic.WP._8.Pages
{
	public partial class Signin
	{
		public Signin()
		{
			InitializeComponent();
		}

		private void btnSignIn_Click(object sender, RoutedEventArgs e)
		{
			StartPendingAnimation.Begin();
			StartPendingAnimation.Completed += (o, args) =>
													{
														PendingAnimation.Begin();

														App.ApiManager.Login(txtEmail.Text, txtPass.Password);
														App.ApiManager.OnError += exception => Dispatcher.BeginInvoke(() =>
																												{
																													EndPendingAnimation.Begin();

																													if (exception.Message.Contains("The remote server returned an error:"))
																														MessageBox.Show("Unable to connect to Google Music", "Connection Error", MessageBoxButton.OK);
																													else
																														MessageBox.Show("Unable to Sign In to Google Music.", "Unknown Error", MessageBoxButton.OK);
																												});
														App.ApiManager.OnLoginComplete += (sender1, eventArgs) => Dispatcher.BeginInvoke(() => NavigationService.
															                                                                                       Navigate(
																                                                                                       new Uri(
																	                                                                                       "/Pages/UpdatingCache.xaml",
																	                                                                                       UriKind.Relative)));
													};
		}
	}
}