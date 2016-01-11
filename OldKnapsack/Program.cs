using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Knapsack.Algorithms;
using Knapsack.Algorithms.Genetic;

namespace Knapsack
{
  class Program
  {
    private static void Preheat(List<Algorithm> algorithms)
    {
      TestSuit testSuit = new TestSuit();
      testSuit.LoadTestsFromDir("../../preheat");

      foreach (var algo in algorithms)
      {
        Console.WriteLine("Preheating algorithm: " + algo.GetType().Name + " [" + algo.GetConfig() + "]");
        testSuit.RunAllTests(algo);
      }
    }

    private static void RunGeneratedTests(List<Algorithm> algorithms)
    {
      TestSuit testSuit = new TestSuit();
      Console.WriteLine("========== Generating tests ==========");
      testSuit.GenerateTests(new GeneratorSetupGenerator().GetAllSetups());

      Console.WriteLine("========== Initiating tests ==========");

      List<KeyValuePair<Algorithm, List<TestResult>>> testResults = new List<KeyValuePair<Algorithm, List<TestResult>>>();
      foreach (var algo in algorithms)
      {
        Console.WriteLine("Running algorithm: " + algo.GetType().Name + " [" + algo.GetConfig() + "]");
        testResults.Add(new KeyValuePair<Algorithm, List<TestResult>>(algo, testSuit.RunAllTests(algo)));
      }
      new ResultPrinter(testResults).Print();
    }

    private static void RunTests(List<Algorithm> algorithms)
    {
      TestSuit testSuit = new TestSuit();
      testSuit.LoadTestsFromDir("../../data");

      Console.WriteLine("========== Initiating tests ==========");

      foreach (var algo in algorithms)
      {
        Console.WriteLine("Running algorithm: " + algo.GetType().Name + " [" + algo.GetConfig() + "]");
        new ResultPrinter(testSuit.RunAllTests(algo), algo).Print();
      }
    }

    static public Genetic MakeGenetic()
    {
      return new Genetic(
        mutationPercentage: 0.2f,
        mutationCountPercentage: 1.0f,
        crossPercentage: 0.2f,
        crossCountPercentage: 1.0f,
        selectionPercentage: 0.5f,
        generationCount: 8,
        entityCount: 1000,
        iterationCount: 200
        );
    }

    static void Main(string[] args)
    {
      using (var p = Process.GetCurrentProcess())
        p.PriorityClass = ProcessPriorityClass.High;
      Genetic []g = new Genetic[21];
      for (int i = 0; i < 21; i++)
      {
        g[i] = MakeGenetic();
      }
      g[0].MutationPercentage = 0.1f;
      g[2].MutationPercentage = 0.3f;
      g[3].MutationPercentage = 0.4f;

      g[4].CrossPercentage = 0.1f;
      g[5].CrossPercentage = 0.2f;
      g[6].CrossPercentage = 0.3f;
      g[7].CrossPercentage = 0.4f;

      g[8].SelectionPercentage = 0.3f;
      g[9].SelectionPercentage = 0.4f;
      g[10].SelectionPercentage = 0.5f;
      g[11].SelectionPercentage = 0.6f;
      g[12].SelectionPercentage = 0.7f;

      g[13].EntityCount = 300;
      g[14].EntityCount = 600;
      g[15].EntityCount = 900;
      g[16].EntityCount = 1200;

      g[17].IterationCount = 100;
      g[18].IterationCount = 200;
      g[19].IterationCount = 300;
      g[20].IterationCount = 400;

      var algorithms = new List<Algorithm>
      {
        //new BBRecursiveBruteforce(),
        //new Heurestic(),
        //new CostDecomposition(),
        /*new CostFPTAS(0.04),
        new CostFPTAS(0.1),
        new CostFPTAS(0.125),
        new CostFPTAS(0.175),*/
        //new CostFPTAS(0.225),
        /*new CostFPTAS(0.275),
        new CostFPTAS(0.325),
        new CostFPTAS(0.375),*/
        //new IterativeBruteforce(),
        //new RecursiveBruteforce(),
        
      };
      /*for (int i = 0; i < 21; i++)
      {
        algorithms.Add(g[i]);
      }*/
      algorithms.Add(g[16]);
      algorithms.Add(g[17]);

      // Preheat(algorithms);
      RunTests(algorithms);
      //RunGeneratedTests(algorithms);

      Console.WriteLine("[DONE] Press any key to exit.");
      Console.ReadKey();
    }
  }
}
