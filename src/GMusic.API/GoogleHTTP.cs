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

namespace GMusic.API
{
	public class GoogleHTTP
	{
		public delegate void RequestCompletedEventHandler(HttpWebRequest request, HttpWebResponse response, String jsonData, Exception error);

		public static String AuthroizationToken = null;
		public static CookieContainer AuthorizationCookieCont = new CookieContainer();
		public static CookieCollection AuthorizationCookies = new CookieCollection();

		private class RequestState
		{
			public HttpWebRequest Request;
			public byte[] UploadData;
			public RequestCompletedEventHandler CompletedCallback;

			public RequestState(HttpWebRequest request, byte[] uploadData, RequestCompletedEventHandler completedCallback)
			{
				Request = request;
				UploadData = uploadData;
				CompletedCallback = completedCallback;
			}
		}

		public HttpWebRequest UploadDataAsync(Uri address, FormBuilder form, RequestCompletedEventHandler complete)
		{
			return UploadDataAsync(address, form.ContentType, form.GetBytes(), complete);
		}

		public HttpWebRequest UploadDataAsync(Uri address, string contentType, byte[] data, RequestCompletedEventHandler completedCallback)
		{
			var request = SetupRequest(address);

			if (!String.IsNullOrEmpty(contentType))
				request.ContentType = contentType;

			request.Method = "POST";
			var state = new RequestState(request, data, completedCallback);
			var result = request.BeginGetRequestStream(OpenWrite, state);

			return request;
		}


		public HttpWebRequest DownloadStringAsync(Uri address, RequestCompletedEventHandler completedCallback, int millisecondsTimeout = 10000)
		{
			var request = SetupRequest(address);
			request.Method = "GET";
			DownloadDataAsync(request, null, millisecondsTimeout, completedCallback);
			return request;
		}

		public void DownloadDataAsync(HttpWebRequest request, byte[] d, int millisecondsTimeout,
		   RequestCompletedEventHandler completedCallback)
		{
			var state = new RequestState(request, d, completedCallback);
			var result = request.BeginGetResponse(GetResponse, state);
		}


		public virtual HttpWebRequest SetupRequest(Uri address)
		{
			if (address == null)
				throw new ArgumentNullException("address");

			if (address.ToString().StartsWith("https://play.google.com/music/services/"))
			{
				address = new Uri(address.OriginalString + String.Format("?u=0&xt={0}", GoogleHTTP.GetCookieValue("xt")));
			}

			var request = (HttpWebRequest)HttpWebRequest.Create(address);

			request.CookieContainer = AuthorizationCookieCont;

			if (AuthroizationToken != null)
				request.Headers[HttpRequestHeader.Authorization] = String.Format("GoogleLogin auth={0}", AuthroizationToken);


			return request;
		}

		static void OpenWrite(IAsyncResult ar)
		{
			var state = (RequestState)ar.AsyncState;

			try
			{
				// Get the stream to write our upload to
				using (var uploadStream = state.Request.EndGetRequestStream(ar))
				{
					var buffer = new Byte[checked((uint)Math.Min(1024, (int)state.UploadData.Length))];

					var ms = new MemoryStream(state.UploadData);

					int bytesRead;
					var i = 0;
					while ((bytesRead = ms.Read(buffer, 0, buffer.Length)) != 0)
					{
						var prog = (int)Math.Floor(Math.Min(100.0,
								(((double)(bytesRead * i) / (double)ms.Length) * 100.0)));


						uploadStream.Write(buffer, 0, bytesRead);

						i++;
					}
				}

				var result = state.Request.BeginGetResponse(GetResponse, state);
			}
			catch (Exception ex)
			{
				if (state.CompletedCallback != null)
					state.CompletedCallback(state.Request, null, null, ex);
			}
		}

		static void GetResponse(IAsyncResult ar)
		{
			var state = (RequestState)ar.AsyncState;
			HttpWebResponse response = null;
			Exception error = null;
			var result = "";

			try
			{
				response = (HttpWebResponse)state.Request.EndGetResponse(ar);
				using (var responseStream = response.GetResponseStream())
				{
					var reader = new StreamReader(responseStream);

					result = reader.ReadToEnd();

				}
			}
			catch (Exception ex)
			{
				error = ex;
			}

			if (state.CompletedCallback != null)
				state.CompletedCallback(state.Request, response, result, error);
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