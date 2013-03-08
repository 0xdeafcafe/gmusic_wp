#if WINDOWS_PHONE
namespace GMusic.Core.Resources
{
	public interface IStorage
	{
		void Load();
		void Save();
	}
}
#endif