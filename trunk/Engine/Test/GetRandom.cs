/*******************************************************************************
    Copyright 2006 Michael Welch
    
    This file is part of Loominate.

    Loominate is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    Loominate is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Loominate; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*******************************************************************************/
#if TEST

namespace Loominate.Engine
{
    using System;
    using System.Text;

    public static class GetRandom
    {

        private static Random rand;
        static string plainCharString =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
        "abcdefghijklmnopqrstuvwxyz" +
        "1234567890" + " ";

        static string funkyCharString = ",.'\"`~!@#$%^*(){}[]/=?+-_\\|<>&\n\t";
        static string randomChars;

        static string[] types = new string[] {
	        "NASDAQ",
	        "NYSE",
	        "EUREX",
	        "FUND",
	        "AMEX",
	        "CURRENCY"
	    };

        public static bool RandomCharIncludeFunkyChars
        {
            set
            {
                if (value) randomChars = plainCharString + funkyCharString;
                else randomChars = plainCharString;
            }
        }


        static GetRandom()
        {
            rand = new Random();
        }

        public static bool Boolean()
        {
            return rand.Next(2) == 1;
        }

        public static char Character()
        {
            if (randomChars == null) RandomCharIncludeFunkyChars = true;

            return randomChars[rand.Next(randomChars.Length)];

        }



        public static string StringWithout(string excludeChars)
        {
            int length;
            // 10% of the time create a large string.
            if (rand.Next(10) == 0) length = rand.Next(1000, 5000);
            else length = rand.Next(5, 20);

            StringBuilder bldr = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                char c;
                do
                {
                    c = GetRandom.Character();
                } while (excludeChars != null && excludeChars.IndexOf(c) < 0);
                bldr.Append(c);
            }
            return bldr.ToString();
        }

        public static string String()
        {
            return StringWithout(null);
        }

        public static string StringInArray(string[] strings)
        {
            if (strings == null || strings.Length == 0)
                return null;

            return strings[rand.Next(strings.Length)];
        }

        public static string CommodityNamespace()
        {
            return StringInArray(types);
        }


    }

}
#endif
