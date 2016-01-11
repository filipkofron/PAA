using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack.Algorithms
{
  class CostDecomposition : Algorithm
  {
    private int[,] _weights;
    private unsafe int* _items;

    private int _sumAllWeights = 0;
    private int _sumAllCosts = 0;
    private int _max;

    private unsafe void SumAll()
    {
      fixed (int* items = &_knapsack.ItemValues[0])
      {
        _sumAllWeights = 0;
        _sumAllCosts = 0;
        for (int i = 0; i < _size; i++)
        {
          _sumAllWeights += items[i * 2];
          _sumAllCosts += items[i * 2 + 1];
        }
      }
    }

    private void FillInf()
    {
      for (int i = 0; i < _sumAllCosts + 1; i++)
      {
        for (int j = 0; j < _size + 1; j++)
        {
          _weights[j, i] = _max;
        }
      }
    }

    private static int Min(int a, int b)
    {
      return a < b ? a : b;
    }

    private unsafe void MainLoop()
    {
      _weights[0, 0] = 0;

      for (int c = 0; c < _sumAllCosts + 1; c++)
      {
        for (int i = 0; i < _size; i++)
        {
          int left = _weights[i, c];
          int cIdx = c - _items[i * 2 + 1];
          int right;
          if (cIdx < 0)
            right = _max;
          else
            right = _weights[i, cIdx] + _items[i * 2];
          
          _weights[i + 1, c] = Min(left, right);
        }
      }
    }

    private int ReadSolution()
    {
      for (int c = _sumAllCosts; c >= 0; c--)
      {
        int val = _weights[_size, c];
        if (val <= _knapsack.Capacity)
          return c;
      }

      return 0;
    }

    public unsafe override int Solve()
    {
      SumAll();
      _max = _sumAllWeights*2 + 1;
      _weights = new int[_size + 1, _sumAllCosts + 1];
      FillInf();

      fixed (int* itemsPtr = &_knapsack.ItemValues[0])
      {
        _items = itemsPtr;
        MainLoop();
      }

      return ReadSolution();
    }

    public unsafe override void Clear()
    {
      _items = null;
      _weights = null;
    }
  }
}
