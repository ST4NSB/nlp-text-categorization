using NLP.TextCategorization;
using System;
using System.Diagnostics;
using LearningSection.Data;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            TextCategorization tc = new TextCategorization();

            //tc.WipeCheckpoints();

            var watch = Stopwatch.StartNew();
            tc.Process(LearningType.NaiveBayes, @"Reuters_Big");
            watch.Stop();
            
            Console.WriteLine("Time till end: ");
            Console.WriteLine("Minutes: " + watch.Elapsed.TotalMinutes);
            Console.WriteLine("Seconds: " + watch.Elapsed.TotalSeconds);
            Console.WriteLine("Milliseconds: " + watch.Elapsed.TotalMilliseconds);
        }
    }
}
