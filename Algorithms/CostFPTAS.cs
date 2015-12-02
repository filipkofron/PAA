using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack.Algorithms
{
  class CostFPTAS : Algorithm
  {
    private int[,] _weights;
    private unsafe int* _items;

    private int _sumAllWeights = 0;
    private int _sumAllCosts = 0;

    private int _max;

    private int _fullCost = 0;
    private double _eps;
    private int _bitsToOmit;

    public CostFPTAS(double eps)
    {
      _eps = eps;
    }

    private unsafe void SumFullCost()
    {
      fixed (int* items = &_knapsack.ItemValues[0])
      {
        _fullCost = 0;
        for (int i = 0; i < _size; i++)
        {
          _fullCost += items[i * 2 + 1];
        }
      }
    }

    private void CalculateBits()
    {
      //_bitsToOmit = (int) (_eps*_fullCost)/_size;
      _bitsToOmit = Math.Max((int) (Math.Log((_eps * _fullCost) / _size, 2.0) - 0.5), 0);
    }

    private unsafe void SumAll(int[] fptasItems)
    {
      fixed (int* items = &fptasItems[0])
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

    private unsafe int ReadSolution()
    {
      int cBest = 0;
      for (int c = _sumAllCosts; c >= 0; c--)
      {
        int val = _weights[_size, c];
        if (val <= _knapsack.Capacity)
        {
          cBest = c;
          break;
        }
      }

      int realSum = 0;
      int idx = _size - 1;
      while (idx >= 0)
      {
        if (_weights[idx + 1, cBest] != _weights[idx, cBest])
        {
          realSum += _knapsack.ItemValues[idx * 2 + 1];
          cBest -= _items[idx * 2 + 1];
        }
        idx--;
      }

      return realSum;
    }

    private int[] MakeFPTASItems()
    {
      int[] fptasItems = new int[_size * 2];
      for (int i = 0; i < _size; i++)
      {
        fptasItems[i * 2] = _knapsack.ItemValues[i * 2];
        fptasItems[i * 2 + 1] = _knapsack.ItemValues[i * 2 + 1] >> _bitsToOmit;
      }

      return fptasItems;
    }

    public unsafe override int Solve()
    {
      SumFullCost();
      CalculateBits();
      int[] fptasItems = MakeFPTASItems();
      SumAll(fptasItems);
      _max = _sumAllWeights * 2 + 1;
      _weights = new int[_size + 1, _sumAllCosts + 1];
      FillInf();

      fixed (int* itemsPtr = &fptasItems[0])
      {
        _items = itemsPtr;
        MainLoop();
      }

      return ReadSolution();
    }

    public unsafe override void Clear()
    {
      _weights = null;
      _items = null;
    }

    public override string GetConfig()
    {
      return "error: " + _eps + " omitted bits: " + _bitsToOmit;
    }
  }
}
