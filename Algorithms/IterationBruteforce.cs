using System;
using System.Collections.Generic;
using System.Text;

namespace Knapsack
{
  class IterativeBruteforce : Algorithm
  {
    private static int IndexFromBits(ulong bits)
    {
      int r = 0; // result of log2(v) will go here

      if ((bits & 0xFFFFFFFF00000000) != 0)
      {
        bits >>= 32;
        r |= 32;
      }

      if ((bits & 0xFFFF0000) != 0)
      {
        bits >>= 16;
        r |= 16;
      }

      if ((bits & 0xFF00) != 0)
      {
        bits >>= 8;
        r |= 8;
      }

      if ((bits & 0xF0) != 0)
      {
        bits >>= 4;
        r |= 4;
      }

      if ((bits & 0xC) != 0)
      {
        bits >>= 2;
        r |= 2;
      }

      if ((bits & 0x2) != 0)
      {
        bits >>= 1;
        r |= 1;
      }

      return r;

      /*return (int) (Math.Log(bits, 2) + 0.5);*/
    }

    public override unsafe int Solve()
    {
      var n = _knapsack.ItemValues.Length/2;
      var capacity = _knapsack.Capacity;
      ulong test = 0;
      ulong prev = 0;
      int sumWeight = 0;
      int maxSumCost = 0;
      int sumCost = 0;
      ulong mask = 0;
      int idx = 0;
      int cycles = 1 << n;

      fixed (int* ptr = &_knapsack.ItemValues[0])
      {
        for (int i = 0; i < cycles; i++)
        {
          prev = test++;
          mask = prev ^ test;
          if ((prev & mask) != 0)
          {
            ulong temp = prev & mask;
            while (temp != 0)
            {
              int pos = IndexFromBits(temp);
              temp = temp & ~((ulong) 1 << pos);
              idx = pos * 2;
              sumWeight -= ptr[idx];
              sumCost -= ptr[idx + 1];
            }
          }

          if ((test & mask) != 0)
          {
            idx = IndexFromBits(test & mask) * 2;
            sumWeight += ptr[idx];
            sumCost += ptr[idx + 1];
          }

          if (sumCost > maxSumCost && sumWeight <= capacity)
            maxSumCost = sumCost;
        }
      }

      return maxSumCost;
    }
  }
}
