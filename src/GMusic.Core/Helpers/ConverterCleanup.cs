using System.Collections.Generic;

namespace GMusic.Core.Helpers
{
	public class ConverterCleanup
	{
		public Dictionary<string, string> CharacterErrorDictionary = new Dictionary<string, string>();

		public ConverterCleanup()
		{
			CharacterErrorDictionary.Clear();
			CharacterErrorDictionary.Add("2", "3");
		}
	}
}
