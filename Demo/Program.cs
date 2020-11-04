using NLP.TextCategorization;
using System;
using System.Diagnostics;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            TextCategorization tc = new TextCategorization();

            var watch = Stopwatch.StartNew();
            tc.Process(@"test");
            watch.Stop();
            Console.WriteLine("Time till end: ");
            Console.WriteLine("Minutes: " + watch.Elapsed.TotalMinutes);
            Console.WriteLine("Seconds: " + watch.Elapsed.TotalSeconds);
            Console.WriteLine("Milliseconds: " + watch.Elapsed.TotalMilliseconds);


            //foreach (var item in tc.ShowGlobalListOfWords())
            //{
            //    Console.WriteLine(item);
            //}

            //Console.WriteLine();


            //foreach (var item in tc.ShowAllDocumentsCategoryAndWordFrequence())
            //{
            //    Console.WriteLine(item);
            //}
        }
    }
}
