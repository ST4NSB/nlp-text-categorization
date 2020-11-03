using NLP.TextCategorization;
using System;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            TextCategorization tc = new TextCategorization();
            tc.Process("Reuters_Big");



            foreach(var item in tc.ShowGlobalListOfWords())
            {
                Console.WriteLine(item);
            }

            Console.WriteLine();


            foreach (var item in tc.ShowAllDocumentsCategoryAndWordFrequence())
            {
                Console.WriteLine(item);
            }
        }
    }
}
