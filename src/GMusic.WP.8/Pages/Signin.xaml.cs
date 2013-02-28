using System.Threading;
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
					                                   var thread = new Thread(() => Dispatcher.BeginInvoke(
						                                   () =>
							                                   {
																   App.ApiManager.Login(txtEmail.Text, txtPass.Password);
								                                   App.ApiManager.OnLoginComplete(() =>
									                                                                  {
																										  PendingAnimation.Stop();
																										  EndPendingAnimation.Begin();
									                                                                  });
							                                   }));
													   thread.Start();
				                                   };
		}
	}
}