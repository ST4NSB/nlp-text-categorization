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

            var watch = Stopwatch.StartNew();
            var accuracy = 0.0f;
            tc.Process(LearningType.NaiveBayes, @"Reuters_Big", out accuracy);
            watch.Stop();

            Console.WriteLine("\r\nAccuracy: " + accuracy * 100.0f + " %\r\n");
            
            //Console.WriteLine("Time till end: ");
            //Console.WriteLine("Minutes: " + watch.Elapsed.TotalMinutes);
            //Console.WriteLine("Seconds: " + watch.Elapsed.TotalSeconds);
            //Console.WriteLine("Milliseconds: " + watch.Elapsed.TotalMilliseconds);
        }
    }
}
