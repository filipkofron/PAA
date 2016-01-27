#pragma once
#include <climits>
#include <random>
#include <unordered_map>
#include <thread>

namespace BitGen
{
  static uint32_t x = static_cast<uint32_t>(666 * time(nullptr)), y = 362436069, z = 521288629;

  inline uint32_t xorshf96(void)
  {
    uint32_t t;
    x ^= x << 16;
    x ^= x >> 5;
    x ^= x << 1;

    t = x;
    x = y;
    y = z;
    z = t ^ x ^ y;

    return z;
  }

  struct ThreadLocal
  {
    uint8_t* bufferA;
    uint8_t* bufferB;

    static const int BufferLenMax = 32 * 1024;
    
    ThreadLocal()
    {
      bufferA = new uint8_t[BufferLenMax];
      bufferB = new uint8_t[BufferLenMax];
    }

    ~ThreadLocal()
    {
      delete[] bufferA;
      delete[] bufferB;
    }
  };
  static std::unordered_map<std::thread::id, ThreadLocal> _threadLocal;

  static size_t CeilDiv32(size_t nom)
  {
    return (nom >> 5) + (nom & 0x0000001F ? 1 : 0);
  }

  static size_t CeilDiv2(size_t nom)
  {
    return (nom >> 1) + (nom & 0x00000001);
  }

  inline uint32_t GetRandomUInt32()
  {
    return static_cast<uint32_t>(xorshf96());
  }

  inline int32_t GetRandomInt32()
  {
    return static_cast<int32_t>(xorshf96());
  }

  inline float GetRandomFloat01()
  {
    return static_cast<uint32_t>(xorshf96()) / static_cast<float>(UINT32_MAX);
  }

  inline void FillRandomBytesAsBits(uint8_t* dest, size_t length)
  {
    size_t intsToGen = CeilDiv32(length);
    uint8_t* curr = dest;
    uint8_t* max = dest + length;

    for (size_t i = 0; i < intsToGen; i++)
    {
      uint32_t randInt = GetRandomUInt32();
      for (size_t b = 0; b < 32 && curr < max; b++)
      {
        *dest++ = randInt & 1;
        randInt >>= 1;
      }
    }
  }

  inline void FillRandUInt16Mod(uint16_t* dest, size_t length, uint16_t mod)
  {
    size_t intsToGen = CeilDiv2(length);
    uint16_t* max = dest + length;
    for (size_t i = 0; i < intsToGen; i++)
    {
      uint16_t* destCurr = dest + (i << 1);
      uint32_t val = GetRandomUInt32();
      *destCurr++ = reinterpret_cast<uint16_t*>(&val)[0] % mod;

      if (destCurr < max)
        *destCurr = reinterpret_cast<uint16_t*>(&val)[1] % mod;
    }
  }

  inline void FlipRandBytes(uint8_t* dest, size_t length, size_t count)
  {
    auto& threadLocal = _threadLocal[std::this_thread::get_id()];
    uint16_t* indexes = reinterpret_cast<uint16_t*>(threadLocal.bufferA);
    FillRandUInt16Mod(indexes, static_cast<uint16_t>(count), static_cast<uint16_t>(length));

    for (size_t i = 0; i < count; i++)
    {
      uint16_t idx = indexes[i];
      dest[idx] = !dest[idx];
    }
  }

  inline void SetRandBytes(uint8_t* dest, const uint8_t* src, size_t length, size_t count)
  {
    auto& threadLocal = _threadLocal[std::this_thread::get_id()];
    uint16_t* indexes = reinterpret_cast<uint16_t*>(threadLocal.bufferA);
    FillRandUInt16Mod(indexes, count, static_cast<uint16_t>(length));

    for (size_t i = 0; i < count; i++)
    {
      uint16_t idx = indexes[i];
      dest[idx] = src[idx];
    }
  }
}
