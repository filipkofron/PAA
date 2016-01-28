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
  _solutionMax = static_cast<int32_t>(_mulNum + _mulNum * 10);
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

void CNFProblem::PrecomputeMaxWeight()
{
  _weightMax = 0;
  for (size_t i = 0; i < _varNum; i++)
  {
    _weightMax += _weights[i];
  }
}

int8_t* CNFProblem::operator[](size_t idx) const
{
  return _form[idx];
}

float CNFProblem::CalculateFitness(uint8_t* present) const
{
  const float normalizedMin = 0.00001f;
  float weight = static_cast<float>(GetWeightForSulution(present));
  bool complete = false;
  float solPoints = GetSulutionPercentage(present, complete);

  //solPoints += sol ? _mulNum * 10 : 0;
  // return (weightMax / _weightMax) + (solPoints / _solutionMax) * 5.0f;
  //return (solPoints / _solutionMax) + normalizedMin;

  // return (weight / _weightMax) * (solPoints) + normalizedMin;
  //return ((weight / _weightMax) + (solPoints)) * 0.5f + normalizedMin;

  // return solPoints + normalizedMin;
  //float res = solPoints + normalizedMin;
  // float res = ((weight / _weightMax) + (solPoints)) * 0.5f;
  float res = ((weight / _weightMax) * 0.3f + (solPoints) * 0.7f) * 0.5f;
  /*if (res < 0.9f)
  {
    res *= 0.1f;
  }
  else
  {
    res -= 0.9f;
    res /= 0.1f;
  }*/
  float diffFromOne = 1.0f - res;
  if (diffFromOne > 0.00001f)
    res /= 1.0f - res;;

  return res + normalizedMin;
}

int32_t CNFProblem::GetWeightForSulution(uint8_t* present) const
{
  int weightMax = 0;
  for (size_t i = 0; i < _varNum; i++)
  {
    if (present[i])
      weightMax += _weights[i];
  }
  return weightMax;
}

float CNFProblem::GetSulutionPercentage(uint8_t* present, bool& complete) const
{
  float solPoints = 0;
  bool sol = true;
  for (size_t i = 0; i < _mulNum; i++)
  {
    int8_t* vars = this->_form[i];
    bool val = false;
    for (size_t j = 0; j < _varNum; j++)
    {
      if (vars[j] != 0)
      {
        int8_t varSign = present[j] ? vars[j] : -vars[j];
        val |= varSign == 1;
      }
    }
    solPoints += val ? 1 : 0;
    sol &= val;
  }

  complete = sol;

  return solPoints / _mulNum;
}