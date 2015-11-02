using System;
using System.Collections.Generic;
using System.Text;
using Knapsack.Algorithms;

namespace Knapsack
{
  class Program
  {
    private static void Preheat(List<Algorithm> algorithms)
    {
      TestSuit testSuit = new TestSuit();
      testSuit.LoadTestsFromDir("../../preheat");

      foreach(var algo in algorithms)
      {
        Console.WriteLine("Preheating algorithm: " + algo.GetType().Name);
        testSuit.RunAllTests(algo);
      }
    }

    private static void RunTests(List<Algorithm> algorithms)
    {
      TestSuit testSuit = new TestSuit();
      testSuit.LoadTestsFromDir("../../data");

      Console.WriteLine("========== Initiating tests ==========");

      foreach (var algo in algorithms)
      {
        Console.WriteLine("Running algorithm: " + algo.GetType().Name);
        new ResultPrinter(testSuit.RunAllTests(algo), algo.GetType().Name).Print();
      }
    }

    static void Main(string[] args)
    {
      var algorithms = new List<Algorithm>
      {
        new BBRecursiveBruteforce(),
        new CostDecomposition(),
        new CostFPTAS(1),
        new RecursiveBruteforce(),
        new IterativeBruteforce()
      };


      Preheat(algorithms);
      RunTests(algorithms);

      Console.ReadKey();
    }
  }
}
