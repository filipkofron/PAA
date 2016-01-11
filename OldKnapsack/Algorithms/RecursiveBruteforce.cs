using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack.Algorithms
{
  class RecursiveBruteforce : Algorithm
  {
    private int _capacity;
    private int _sumCost;
    private int _sumWeight;
    private int _idx;
    private unsafe int* items;

    private static int IntMax(int a, int b)
    {
      return a > b ? a : b;
    }

    private unsafe int RecursiveKnapsackRec()
    {
      if (_idx >= _size) return (_sumWeight > _capacity) ? -1 : _sumCost;

      int w = items[_idx * 2];
      int c = items[_idx * 2 + 1];

      _idx += 1;
      int costL = RecursiveKnapsackRec();
      
      _sumWeight += w;
      _sumCost += c;

      int costR = RecursiveKnapsackRec();

      _idx -= 1;
      _sumWeight -= w;
      _sumCost -= c;

      return IntMax(costL, costR);
    }

    public unsafe override int Solve()
    {
      _capacity = _knapsack.Capacity;
      _idx = 0;
      _sumCost = 0;
      _sumWeight = 0;
      fixed (int* itemsPtr = &_knapsack.ItemValues[0])
      {
        items = itemsPtr;
        return RecursiveKnapsackRec();
      }
    }

    public unsafe override void Clear()
    {
      items = null;
    }
  }
}
