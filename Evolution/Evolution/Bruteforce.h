#pragma once

#include <cstdint>
#include <iostream>

template<typename Problem>
class Bruteforce
{
private:
  uint8_t* _present;
  uint8_t* _presentBest;
  float _weight;
  Problem& _problem;

  void Clear()
  {
    _weight = 0;
    size_t len = _problem.GetVarNum();
    for (size_t i = 0; i < len; i++)
    {
      _present[i] = 0;
    }
  }

  void Eval()
  {
    bool complete = false;
    _problem.GetSulutionPercentage(_present, complete);
    if (complete)
    {
      float newWeight = static_cast<float>(_problem.GetWeightForSulution(_present));
      if (newWeight > _weight)
      {
        _weight = newWeight;
        memcpy(_presentBest, _present, sizeof(*_present) * _problem.GetVarNum());
      }
    }
  }

  void Step()
  {
    int len = static_cast<int>(_problem.GetVarNum());
    for (int i = len - 1; i >= 0; i--)
    {
      if (_present[i])
      {
        _present[i] = 0;
      }
      else
      {
        _present[i] = !_present[i];
        break;
      }
    }
  }
public:
  Bruteforce(Problem& problem)
    : _weight(0), _problem(problem)
  {
    _present = new uint8_t[_problem.GetVarNum()];
    _presentBest = new uint8_t[_problem.GetVarNum()];
  }

  ~Bruteforce()
  {
    delete[] _presentBest;
    delete[] _present;
  }

  bool CanSolve()
  {
    return _problem.GetVarNum() < 22;
  }

  void Solve()
  {
    if (!CanSolve())
      return;

    Clear();
    uint32_t steps = 1 << _problem.GetVarNum();
    for (uint32_t i = 0; i < steps; i++)
    {
      Step();
      Eval();
    }
  }

  void PrintToOs(std::ostream& os, uint8_t* present) const
  {
    size_t len = _problem.GetVarNum();
    for (int i = 0; i < len; i++)
    {
      os << static_cast<int>(present[i]);
    }
  }

  float GetWeight() const
  {
    return _weight;
  }

  template<typename AnotherProblem>
  friend std::ostream& operator << (std::ostream& os, const Bruteforce<AnotherProblem>& brut);
};

template<typename Problem>
std::ostream& operator << (std::ostream& os, const Bruteforce<Problem>& brut)
{
  brut.PrintToOs(os, brut._presentBest);
  return os;
}
