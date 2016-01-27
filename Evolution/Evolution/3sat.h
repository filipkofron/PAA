#pragma once
#include "CNFProblem.h"
#include <memory>

std::shared_ptr<CNFProblem>	gen_3_sat(int n, int m, int mod, int seed);