using System;
using System.Text;
using System.Collections.Generic;

namespace Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Splits the string in words using a yield generator,
        /// returning an IEnumerable<string>.
        /// </summary>
        /// <returns>IEnumerable containing the words of the string.</returns>
        /// <param name="str">String to split in words.</param>
        public static IEnumerable<string> SplitWordsLazy(this string str)
        {
            int previousWordIndex = 0;
            int currentWordLength = 0;

            foreach (char ch in str)
            {
                if (char.IsWhiteSpace(ch))
                {
                    yield return str.Substring(previousWordIndex, currentWordLength);
                    previousWordIndex += currentWordLength + 1;
                    currentWordLength = 0;
                }
                else currentWordLength++;
            }
        }

        public static int ToIntWrapper(this string str) {
            int res;
            bool ok = int.TryParse(str, out res);
            if (!ok)
                throw new InvalidCastException();
            return res;
        }
    }
}
