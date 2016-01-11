using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knapsack.Algorithms.Genetic
{
  class Mutator
  {
    private static readonly Random _random = new Random();
    private List<int> _mutationIndexes;

    public Mutator(int size)
    {
      _mutationIndexes = new List<int>(size);
      for (int i = 0; i < size; i++)
      {
        _mutationIndexes.Add(i);
      }
    }

    public static void Shuffle<T>(IList<T> list)
    {
      int n = list.Count;
      while (n > 1)
      {
        int k = _random.Next(n);
        n--;
        T value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }

    public void Mutate(bool[] presenceArray, int count)
    {
      Shuffle(_mutationIndexes);

      for (int i = 0; i < count; i++)
      {
        presenceArray[_mutationIndexes[i]] = !presenceArray[_mutationIndexes[i]];
      }
    }

    public int Rand(int maxPlusOne)
    {
      return _random.Next(maxPlusOne);
    }
  }
}
