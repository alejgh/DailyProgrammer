using System;
using System.Text;
using System.Collections.Generic;

namespace Extensions
{
    public static class StringExtensions
    {
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

        public static IEnumerable<byte> GetBytesLazy(this string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            foreach (var b in bytes)
            {
                yield return b;
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
