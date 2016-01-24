#pragma once

template<typename Problem>
struct Config
{
  size_t _bitCount;
  size_t _populationSize;

  size_t _mutationCount;
  size_t _mutationBitCount;

  size_t _crossCount;
  size_t _crossBitCount;

  std::shared_ptr<Problem> _problem;
};
