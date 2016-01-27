#pragma once
#include <cstdint>
#include <iostream>

//! The weighted CNF represantation
class CNFProblem
{
private:
  size_t _varNum; //< variable number
  size_t _mulNum; //< multiple number
  int32_t* _weights; //< one dimensional array of weights, size == _varNum
  int8_t** _form; //< two dimensional array of multiples of variable coeficiens, size == _mulNum * _varNum
  int32_t _weightMax; //< precomputed max achievable weight
  int32_t _solutionMax; //< precomputed max achievable solutions (_mulNum + _mulNum * 10)
public:
  CNFProblem(size_t varNum, size_t mulNum);
  ~CNFProblem();

  //! returns the variable number
  size_t GetVarNum() const { return _varNum; }

  //! returns the multiple number
  size_t GetMulNum() const { return _mulNum; }

  //! returns the weights array
  int32_t* GetWeights() const { return _weights; }

  void PrecomputeMaxWeight();

  //! Returns an array of variable coeficient to the given multiple index
  int8_t* operator [] (size_t idx) const;

  float CalculateFitness(uint8_t* present) const;

  int32_t GetWeightForSulution(uint8_t* present) const;

  float GetSulutionPercentage(uint8_t* present, bool& complete) const;

  friend std::ostream& operator << (std::ostream& os, const CNFProblem& problem);
};

inline std::ostream& operator << (std::ostream& os, const CNFProblem& problem)
{
  for (size_t i = 0; i < problem._varNum; i++)
  {
    if (i == 0)
      os << problem._weights[i];
    else
      os << ", " << problem._weights[i];
  }

  for (size_t i = 0; i < problem._mulNum; i++)
  {
    os << std::endl;

    for (int j = 0; j < problem._varNum; j++)
    {
      if (j == 0)
        os << static_cast<int>(problem[i][j]);
      else
        os << ", " << static_cast<int>(problem[i][j]);
    }
  }
  
  return os;
}