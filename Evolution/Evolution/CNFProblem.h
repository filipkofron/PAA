#pragma once
#include <cstdint>

// The weighted CNF represantation
class CNFProblem
{
private:
  size_t _varNum; //< variable number
  size_t _mulNum; //< multiple number
  int32_t* _weights; //< one dimensional array of weights, size == _varNum
  int8_t** _form; //< two dimensional array of multiples of variable coeficiens, size == _mulNum * _varNum
public:
  CNFProblem(size_t varNum, size_t mulNum);
  ~CNFProblem();

  // returns the variable number
  size_t GetVarNum() const { return _varNum; }

  // returns the multiple number
  size_t GetMulNum() const { return _mulNum; }

  // Returns a array of variable coeficient to the given multiple index
  int8_t* operator [] (size_t idx) const;
};
