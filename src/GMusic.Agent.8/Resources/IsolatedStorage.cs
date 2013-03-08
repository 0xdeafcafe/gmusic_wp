using System.IO.IsolatedStorage;
using GMusic.Core.Resources;

namespace GMusic.Agent._8.Resources
{
	public class IsolatedStorage : IStorage
	{
		public IsolatedStorage()
		{
			Load();
		}

		private readonly IsolatedStorageSettings _appSettings = IsolatedStorageSettings.ApplicationSettings;

	    public string GoogleAuthToken { get; private set; }

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
            
        }
	}
}