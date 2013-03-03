using System.Reflection;
using Microsoft.Phone.Controls;

namespace GMusic.WP._8.Pages
{
	public partial class About : PhoneApplicationPage
	{
		public About()
		{
			InitializeComponent();

			lblAppVer.Text = "GOOGLE MUSIC - " + GetVersionNumber();
		}
		private static string GetVersionNumber()
		{
			var asm = Assembly.GetExecutingAssembly();
			var parts = asm.FullName.Split(',');
			return parts[1].Split('=')[1];
		}
	}
}