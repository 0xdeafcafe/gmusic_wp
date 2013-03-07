using System;
using System.Collections.Generic;

namespace GMusic.WP._8.Helpers
{
    public class CharCompare : IComparer<Char>
    {
        //readonly string[] exps = new[] { @"^\d+$", @"^\d+[a-zA-Z]+$", @"^[a-zA-Z]\d+$", @"^[a-zA-Z]+\d+$" };
        public int Compare(char x, char y)
        {
            return Convert.ToInt32(x.ToString().Equals(y.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }
    }
}