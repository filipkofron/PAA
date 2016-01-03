using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;

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
    private Generation _generation;

    public override string GetConfig()
    {
      return $"mut{_mutationPercentage}_mutc{_mutationCountPercentage}_cr{_crossPercentage}_crc{_crossCountPercentage}_sel{_selectionPercentage}_it{_iterationCount}_ent{_entityCount}";
    }

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
    }

    public float MutationPercentage
    {
      get { return _mutationPercentage; }
      set { _mutationPercentage = value; }
    }

    public float MutationCountPercentage
    {
      get { return _mutationCountPercentage; }
      set { _mutationCountPercentage = value; }
    }

    public float CrossPercentage
    {
      get { return _crossPercentage; }
      set { _crossPercentage = value; }
    }

    public float CrossCountPercentage
    {
      get { return _crossCountPercentage; }
      set { _crossCountPercentage = value; }
    }

    public float SelectionPercentage
    {
      get { return _selectionPercentage; }
      set { _selectionPercentage = value; }
    }

    public int IterationCount
    {
      get { return _iterationCount; }
      set { _iterationCount = value; }
    }

    public int GenerationCount
    {
      get { return _generationCount; }
      set { _generationCount = value; }
    }

    public int EntityCount
    {
      get { return _entityCount; }
      set { _entityCount = value; }
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

    private Generation Cycle(Generation start, int cycle)
    {
      int bestCount = (int) (_entityCount * _selectionPercentage);

      Generation mutateWith =  start;
      start.MutateDuplicates(_mutator, _mutationPercentage);
      start.Cross(_mutator, _entityCount * 2, mutateWith, _crossPercentage, _crossCountPercentage);
      start.Mutate(_mutator, _entityCount * 3, _mutationPercentage, _mutationCountPercentage);
      start.Fix(_mutator);
      Generation newGeneration = start.SelectBest(_mutator, bestCount, _entityCount);

      return newGeneration;
    }

    private int BestSolution()
    {
       return _generation.SelectBest(_mutator, 1, 1).GetFirst().SumCost();
    }
    private void Out(string text)
    {
      File.AppendAllText("iterations.csv", text + "\n");
    }

    public override int Solve()
    {
      int bits = _knapsack.ItemValues.Length / 2;
      _mutator = new Mutator(bits);
      
      _generation = new Generation(_mutator, _entityCount, _knapsack);
    
      for (int i = 0; i < _iterationCount; i++)
      {
        _generation = Cycle(_generation, i);
        string text = $"{i},{_knapsack.Solution},{BestSolution()}";
        //string text = $"{i},{BestSolution()}";
        //Console.WriteLine(text);
        //Out(text);
        //Console.WriteLine($"{i}:" + BestSolution());
      }

      return BestSolution();
    }
  }
}
