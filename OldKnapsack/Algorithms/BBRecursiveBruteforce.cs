using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack.Algorithms
{
  class BBRecursiveBruteforce : Algorithm
  {
    private int _capacity;
    private int _sumCost;
    private int _sumWeight;
    private int _currMaxCost;
    private int _idx;
    private unsafe int* _items;
    private unsafe int* _postCostSums;

    private static int IntMax(int a, int b)
    {
      return a > b ? a : b;
    }

    private unsafe int RecursiveKnapsackRec()
    {
      if (_idx >= _size) return (_sumWeight > _capacity) ? -1 : _sumCost;
      if (_sumWeight > _capacity) return -1;
      if (_currMaxCost > _postCostSums[_idx] + _sumCost) return -1;

      int w = _items[_idx * 2];
      int c = _items[_idx * 2 + 1];

      _idx += 1;
      int costL = RecursiveKnapsackRec();

      _sumWeight += w;
      _sumCost += c;

      int costR = RecursiveKnapsackRec();

      _currMaxCost = IntMax(IntMax(_currMaxCost, costL), costR);

      _idx -= 1;
      _sumWeight -= w;
      _sumCost -= c;

      return IntMax(costL, costR);
    }

    private unsafe void CalculatePostCostSums()
    {
      _postCostSums[_size - 1] = _items[(_size - 1) * 2 + 1];
      for (int i = _size - 1; i >= 0; i--)
      {
        _postCostSums[i] = _postCostSums[i + 1] + _items[i * 2 + 1];
      }
    }

    public unsafe override int Solve()
    {
      _capacity = _knapsack.Capacity;
      _idx = 0;
      _sumCost = 0;
      _sumWeight = 0;
      _currMaxCost = 0;
      fixed (int* itemsPtr = &_knapsack.ItemValues[0])
      {
        _items = itemsPtr;
        int[] postSums = new int[_size];
        fixed (int* postSumsPtr = &postSums[0])
        {
          _postCostSums = postSumsPtr;
          CalculatePostCostSums();
          return RecursiveKnapsackRec();
        }
      }
    }

    public unsafe override void Clear()
    {
      _items = null;
      _postCostSums = null;
    }
  }
}
