#pragma once

#include <cstdint>
#include <cstring>
#include "BitGen.h"

template<typename Problem>
struct Individual
{
  size_t _length;
  uint8_t* _present;
  float _fitness;
  bool _fintessDirty;

  Individual(size_t length)
    : _length(length), _fitness(0.0f), _fintessDirty(true)
  {
    _present = new uint8_t[length];
    memset(_present, 0, _length);
  }

  Individual(const Individual& individual)
    : _length(individual._length), _fitness(individual._fitness), _fintessDirty(individual._fintessDirty)
  {
    _present = new uint8_t[individual._length];
    memcpy(_present, individual._present, _length);
  }

  ~Individual()
  {
    delete[] _present;
  }

  uint8_t operator [] (int idx) const
  {
    return _present[idx];
  }

  Individual& operator = (const Individual& ind)
  {
    if (this == &ind)
      return *this;

    memcpy(_present, ind._present, _length);

    return *this;
  }

  void Mutate(size_t count)
  {
    BitGen::FlipRandBytes(_present, _length, count);
    _fintessDirty = true;
  }

  void Cross(const Individual& individual, size_t count)
  {
    BitGen::SetRandBytes(_present, individual._present, _length, count);
    _fintessDirty = true;
  }

  void RecalculateFitness(const Problem& problem)
  {
    _fitness = problem.CalculateFitness(_present);
  }

  float GetFitness(const Problem& problem)
  {
    if (_fintessDirty)
      RecalculateFitness(problem);
    return _fitness;
  }

  template<typename ProblemAnother>
  friend std::ostream& operator << (std::ostream& os, const Individual<ProblemAnother>& ind);
};

template<typename Problem>
std::ostream& operator << (std::ostream& os, const Individual<Problem>& ind)
{
  for (size_t i = 0; i < ind._length; i++)
  {
    os << static_cast<int>(ind._present[i]);
  }
  return os;
}
