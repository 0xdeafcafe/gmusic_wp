using System.IO.IsolatedStorage;

namespace GMusic.WP._8.Resources
{
	public class IsolatedStorage : IStorage
	{
		public IsolatedStorage()
		{
			Load();
		}

		private readonly IsolatedStorageSettings _appSettings = IsolatedStorageSettings.ApplicationSettings;

		private string _googleAuthToken;
		public string GoogleAuthToken
		{ 
			get
			{
				return _googleAuthToken;
			}
			set 
			{
				_googleAuthToken = value;
				Save();
			}
		}

		public void Load()
		{
			GoogleAuthToken = LoadString("google_auth_token");
		}
		private string LoadString(string key)
		{
			return _appSettings.Contains(key) ? (string)_appSettings[key] : null;
		}

		public void Save()
		{
			SaveString("google_auth_token", GoogleAuthToken);

			_appSettings.Save();
		}
		private void SaveString(string key, object value)
		{
			if (_appSettings.Contains(key))
				_appSettings[key] = value;
			else
				_appSettings.Add(key, value);
		}
	}
}