using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Knapsack.Algorithms.Genetic
{
  class Genetic : Algorithm
  {
    private float _mutationPercentage;
    private float _mutationCountPercentage;
    private float _crossPercentage;
    private float _crossCountPercentage;
    private float _selectionPercentage;
    private int _iterationCount;
    private int _generationCount;
    private int _entityCount;

    private Mutator _mutator;
    private Generation[] _generations;

    public Genetic(
      float mutationPercentage,
      float mutationCountPercentage,

      float crossPercentage,
      float crossCountPercentage,

      float selectionPercentage,

      int generationCount,
      int iterationCount,
      int entityCount)
    {
      _mutationPercentage = mutationPercentage;
      _mutationCountPercentage = mutationCountPercentage;
      _crossPercentage = crossPercentage;
      _crossCountPercentage = crossCountPercentage;
      _selectionPercentage = selectionPercentage;
      _generationCount = generationCount;
      _iterationCount = iterationCount;
      _entityCount = entityCount;

      _generations = new Generation[_generationCount];
    }

    public class DuplicateKeyComparer<TKey>
                :
             IComparer<TKey> where TKey : IComparable
    {
      #region IComparer<TKey> Members

      public int Compare(TKey x, TKey y)
      {
        int result = x.CompareTo(y);

        if (result == 0)
          return 1;   // Handle equality as beeing greater
        else
          return result;
      }

      #endregion
    }

    private Generation Cycle(Generation start, int cycle, int gener)
    {
      int bestCount = (int) (_entityCount * _selectionPercentage);
      Generation mutateWith = _mutator.Rand(4) == 0 ? start : _generations[_mutator.Rand(_generationCount)];
      start.Cross(_mutator, _entityCount, mutateWith, _crossPercentage, _crossCountPercentage);
      start.Mutate(_mutator, _entityCount, _mutationPercentage, _mutationCountPercentage);
      Generation newGeneration = start.SelectBest(_mutator, bestCount, _entityCount);

      Configuration best = newGeneration.SelectBest(_mutator, 1, 1).GetFirst();
      //Console.WriteLine($"[{cycle}:{gener}] " + best.Evaluate() + " " + best.SumCost());

      return newGeneration;
    }

    private SortedList<int, int> SortedSolutions()
    {
      SortedList<int, int> sorted = new SortedList<int, int>(new DuplicateKeyComparer<int>());
      for (int g = 0; g < _generationCount; g++)
      {
        int sol = (int) _generations[g].SelectBest(_mutator, 1, 1).GetFirst().SumCost();
        sorted.Add(sol, sol);
      }
      return sorted;
    }

    private int BestSolution()
    {
      int max = 0;
      for (int g = 0; g < _generationCount; g++)
      {
        int curr = _generations[g].SelectBest(_mutator, 1, 1).GetFirst().SumCost();
        if (curr > max)
          max = curr;
      }
      return max;
    }

    public override int Solve()
    {
      int bits = _knapsack.ItemValues.Length / 2;
      _mutator = new Mutator(bits);

      for (int g = 0; g < _generationCount; g++)
      {
        _generations[g] = new Generation(_mutator, _entityCount, _knapsack);
      }

      for (int i = 0; i < _iterationCount; i++)
      {
        for (int g = 0; g < _generationCount; g++)
        {
          _generations[g] = Cycle(_generations[g], i, g);
        }
        string text = $"{i}({_knapsack.Solution}):";
        foreach (var item in SortedSolutions())
        {
          text += $" {item.Key}";
        }
        Console.WriteLine(text);
        //Console.WriteLine($"{i}:" + BestSolution());
      }

      return BestSolution();
    }
  }
}
