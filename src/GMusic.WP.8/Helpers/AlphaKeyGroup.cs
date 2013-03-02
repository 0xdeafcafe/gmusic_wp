using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Phone.Globalization;

namespace GMusic.WP._8.Helpers
{
	public class AlphaKeyGroup<T> : List<T>
	{
		const string GlobeGroupKey = "\uD83C\uDF10";

		/// <summary> 
		/// The Key of this group. 
		/// </summary> 
		public string Key { get; private set; }

		/// <summary> 
		/// Public ctor. 
		/// </summary> 
		/// <param name="key">The key for this group.</param> 
		public AlphaKeyGroup(string key)
		{
			Key = key;
		}

		/// <summary> 
		/// Create a list of AlphaGroup<T> with keys set by a SortedLocaleGrouping. 
		/// </summary> 
		/// <param name="slg">The </param> 
		/// <returns>Theitems source for a LongListSelector</returns> 
		private static List<AlphaKeyGroup<T>> CreateDefaultGroups(SortedLocaleGrouping slg)
		{
			return slg.GroupDisplayNames.Select(key => key == "..." ? new AlphaKeyGroup<T>(GlobeGroupKey) : new AlphaKeyGroup<T>(key)).ToList();
		}

		/// <summary> 
		/// Create a list of AlphaGroup<T> with keys set by a SortedLocaleGrouping  
		/// using the current threads culture to determine which alpha keys to 
		/// include. 
		/// </summary> 
		/// <param name="items">The items to place in the groups.</param> 
		/// <param name="getKey">A delegate to get the key from an item.</param> 
		/// <param name="sort">Will sort the data if true.</param> 
		/// <returns>An items source for a LongListSelector</returns> 
		public static List<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, Func<T, string> keySelector, bool sort)
		{
			return CreateGroups(
				items,
				System.Threading.Thread.CurrentThread.CurrentCulture,
				keySelector,
				sort);
		}

		/// <summary> 
		/// Create a list of AlphaGroup<T> with keys set by a SortedLocaleGrouping. 
		/// </summary> 
		/// <param name="items">The items to place in the groups.</param> 
		/// <param name="ci">The CultureInfo to group and sort by.</param> 
		/// <param name="getKey">A delegate to get the key from an item.</param> 
		/// <param name="sort">Will sort the data if true.</param> 
		/// <returns>An items source for a LongListSelector</returns> 
		public static List<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, CultureInfo ci, Func<T, string> keySelector, bool sort)
		{
			var slg = new SortedLocaleGrouping(ci);
			var list = CreateDefaultGroups(slg);

			foreach (var item in items)
			{
				int index;
				//if (slg.SupportsPhonetics) 
				//{ 
				//check if your database has yomi string for item 
				//if it does not, then do you want to generate Yomi or ask the user for this item. 
				//index = slg.GetGroupIndex(getKey(Yomiof(item))); 
				//} 
				//else 
				{
					index = slg.GetGroupIndex(keySelector(item));
				}

				if (index >= 0 && index < list.Count)
				{
					list[index].Add(item);
				}
			}

			if (sort)
			{
				foreach (var group in list)
				{
					group.Sort((c0, c1) => ci.CompareInfo.Compare(keySelector(c0), keySelector(c1)));
				}
			}

			return list;
		}
	}
}