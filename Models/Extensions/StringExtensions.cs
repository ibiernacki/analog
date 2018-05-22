using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Extensions
{
    public static class StringExtensions
    {
        public static int FirstNotOf(this string str, char[] chars, int starIndex = 0)
        {
            for (var i = starIndex; i < str.Length; i++)
            {
                if (chars.All(c => c != str[i]))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
