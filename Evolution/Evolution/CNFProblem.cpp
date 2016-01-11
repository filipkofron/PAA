#include "CNFProblem.h"

CNFProblem::CNFProblem(size_t varNum, size_t mulNum)
  : _varNum(varNum), _mulNum(mulNum)
{
  _weights = new int32_t[_varNum];
  _form = new int8_t*[_mulNum];
  for (size_t i = 0; i < _mulNum; i++)
  {
    _form[i] = new int8_t[_varNum];
  }
}

CNFProblem::~CNFProblem()
{
  for (size_t i = 0; i < _mulNum; i++)
  {
    delete[] _form[i];
  }
  delete[] _form;
  delete[] _weights;
}

int8_t* CNFProblem::operator[](size_t idx) const
{
  return _form[idx];
}
