// Evolution.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "CNFGenerator.h"
#include "BitGen.h"
#include <iostream>
#include "Population.h"
#include "Config.h"
#include "Cycle.h"
#include "3sat.h"


int main()
{
  const int variableNum = 40;
  const int multiplesNum = 160;

  //auto problem = CNFGenerator::GenerateOne(100, 1000);
  auto problem = gen_3_sat(variableNum, multiplesNum, 50);
  Config<CNFProblem> config;
  config._populationSize = 100;
  config._bitCount = variableNum;
  config._crossCount = 30;
  config._crossBitCount = 20;
  config._mutationCount = 30;
  config._mutationBitCount = 20;
  config._problem = problem;

  Cycle<CNFProblem> cycle(config);
  for (int i = 0; i < 500; i++)
  {
    cycle.Step();
    //cycle.Print();
    std::cout << "[" << i << "] Best: " << cycle.GetBestFitness() << std::endl;
  }
  //std::cout << " Problem: " << *problem << std::endl;
  std::cout << " Best vector: " << cycle.GetBestIndividual() << std::endl;
  std::cout << " Best weight: " << cycle.GetBestIndividual().GetBestWeight(*problem) << std::endl;
  std::cout << " Best fitness: " << cycle.GetBestFitness() << std::endl;
  
  return 0;
}
