using System;
using System.Linq;
using Extensions;
using System.Collections.Generic;

namespace Elements
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Write("Insert the chemical formula: ");
            string formula = Console.ReadLine();
            IDictionary<string, int> elementCount = Solve(formula);
            PrintDictionary(elementCount);
        }

        private static IDictionary<string, int> Solve(string formula)
        {

            IDictionary<string, int> ret = new Dictionary<string, int>();
            return SolveRecursively(formula, ret);
        }

        private static IDictionary<string, int> SolveRecursively(string remainingFormula,
                                        IDictionary<string, int> currentDic)
        {
            IDictionary<string, int> temp = new Dictionary<string, int>();
            string partialElement = "";
            for (int i = 0; i < remainingFormula.Length; i++)
            {
                char ch = remainingFormula[i];
                if (char.IsLower(ch) || char.IsUpper(ch))
                {
                    partialElement += ch;
                    int amount = GetAmount(remainingFormula.Substring(i + 1));
                    if (amount != -1)
                    {
                        AddElementToDict(temp, partialElement, amount);
                        partialElement = "";
                    }
                }
                else if (char.IsDigit(ch)) continue;
                else if (ch == '(')
                {
                    int numParenthesis = 1;
                    temp = SolveRecursively(remainingFormula.Substring(i + 1), temp);

                    // we have treated the next parenthesis
                    // so now we skip until the end of the parenthesis
                    while(numParenthesis != 0)
                    {
                        i++;
                        ch = remainingFormula[i];
                        if (ch == '(') numParenthesis++;
                        else if (ch == ')') numParenthesis--;
                    }
                }
                else if (ch == ')')
                {
                    int multiplier = GetAmount(remainingFormula.Substring(i + 1));
                    MultiplyElements(multiplier, temp);
                    return Combine(temp, currentDic);
                }
            }
            return Combine(temp, currentDic);
        }

        private static int GetAmount(string str)
		{
			int n = str.Length;
            char ch = str[0];
            if (n == 0 || char.IsUpper(ch)) return 1;
            else if (char.IsLower(ch)) return -1;
            else if (char.IsDigit(ch))
            {
                char next;
                string number = "";
                for (int i = 0; i < n; i++)
                {
                    next = str[i];
                    if (char.IsDigit(next))
                        number += next;
                    else break;
                }
                return number.ToIntWrapper();
            }
            else return 1;
        }

        private static IDictionary<string, int> Combine(IDictionary<string, int> dict1,
                                                        IDictionary<string, int> dict2)
        {
            IDictionary<string, int> ret = new Dictionary<string, int>(dict1);
            foreach (var item in dict2) 
                AddElementToDict(ret, item.Key, item.Value);
            return ret;

        }


        static void AddElementToDict(IDictionary<string, int> dict, string partialElement, int amount)
        {
            if (dict.ContainsKey(partialElement)) dict[partialElement] += amount;
            else dict.Add(partialElement, amount);
        }

		static void MultiplyElements(int multiplier, IDictionary<string, int> dict)
        {
            foreach (var key in dict.Keys.ToList())
                dict[key] *= multiplier;
        }

        static void PrintDictionary(IDictionary<string, int> dict)
        {
            foreach (var item in dict)
                Console.WriteLine("Element {0}: {1} atoms.", item.Key, item.Value);
        }
    }
}
