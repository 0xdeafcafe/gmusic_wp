/* This class was originally written by taylorfinnell (https://github.com/taylorfinnell/) for 
 * his un-official Google Music API (https://github.com/taylorfinnell/GoogleMusicAPI.NET/).
 * 
 * Modified by Alex Reed (https://github.com/Xerax) for gmusic_wp (https://github.com/Xerax/gmusic_wp)
 * 
 * yolo
 */

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GMusic.Core.Helpers;

namespace GMusic.API
{
	public class GoogleHTTP
	{
		public static String AuthroizationToken;
		public static CookieContainer AuthorizationCookieCont = new CookieContainer();
		public static CookieCollection AuthorizationCookies = new CookieCollection();

		public async Task<String> UploadDataAsync(Uri address, FormBuilder form)
		{
			return await UploadDataAsync(address, form.ContentType, form.GetBytes());
		}
		public async Task<String> UploadDataAsync(Uri address, string contentType, byte[] data)
		{
			var request = SetupRequest(address);

			if (!String.IsNullOrEmpty(contentType))
				request.ContentType = contentType;

			request.Method = "POST";
			return await new StreamReader(await request.GetRequestStreamAsync()).ReadToEndAsync();
		}


		public async Task<String> DownloadStringAsync(Uri address)
		{
			var request = SetupRequest(address);
			request.Method = "GET";
			return await new StreamReader(await request.GetRequestStreamAsync()).ReadToEndAsync();
		}
		public async Task<Stream> DownloadDataAsync(HttpWebRequest request)
		{
			return await request.GetRequestStreamAsync();
		}

		public virtual HttpWebRequest SetupRequest(Uri address)
		{
			if (address == null)
				throw new ArgumentNullException("address");

			if (address.ToString().StartsWith("https://play.google.com/music/services/"))
			{
				address = new Uri(address.OriginalString + String.Format("?u=0&xt={0}", GetCookieValue("xt")));
			}

			var request = (HttpWebRequest)WebRequest.Create(address);

			request.CookieContainer = AuthorizationCookieCont;

			if (AuthroizationToken != null)
				request.Headers[HttpRequestHeader.Authorization] = String.Format("GoogleLogin auth={0}", AuthroizationToken);

			return request;
		}
		public static String GetCookieValue(String cookieName)
		{
			return (from Cookie cookie in AuthorizationCookies where cookie.Name.Equals(cookieName) select cookie.Value).FirstOrDefault();
		}
		public static void SetCookieData(CookieContainer cont, CookieCollection coll)
		{
			AuthorizationCookieCont = cont;
			AuthorizationCookies = coll;
		}
	}
}