using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack
{
  class Knapsack
  {
    public int Capacity { get; }
    public int[] ItemValues { get; }

    public int Solution { get; set; }

    public Knapsack(int[] itemValues, int capacity, int solution)
    {
      ItemValues = itemValues;
      Capacity = capacity;
      Solution = solution;
    }

    public int Solve(Algorithm algorithm)
    {
      algorithm.Knapsack = this;
      return algorithm.Solve();
    }
  }
}
