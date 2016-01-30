// Evolution.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Log.h"
#include "CNFGenerator.h"
#include "BitGen.h"
#include <iostream>
#include "Population.h"
#include "Config.h"
#include "Cycle.h"
#include "3sat.h"
#include "Clock.h"
#include "Bruteforce.h"

Log Conf("Config");
Log Progress("Progress");
Log Solution("Solution");

void Evolution(int variableNum, int multiplesNum, int seed, Config<CNFProblem>& config)
{
  Conf() << "Variables,Multiples,Seed" << std::endl;
  Conf() << variableNum << "," << multiplesNum << "," << seed << std::endl;

  Progress() << "time,best,sol,weight" << std::endl;

  Cycle<CNFProblem> cycle(config);
  TimeMeasure tm;
  for (int i = 0; i < 200; i++)
  {
/*    if (cycle.HasSolution())
      break;
      */
    cycle.Step();
    //cycle.Print();
    float best = cycle.GetBestFitness();
    float sol = cycle.GetBestIndividual().GetSolutionPercentage(*config._problem);
    float weight = cycle.GetBestIndividual().GetBestWeight(*config._problem);
    std::cout << "[" << i << "] Time: " << tm.NanosFromBeginning() << " Best: " <<  best
      << " sol: " << sol
      << " weight: " << weight << std::endl;

    Progress() << tm.NanosFromBeginning() << "," << best << "," << sol << "," << weight << std::endl;
  }
  //std::cout << " Problem: " << *problem << std::endl;
  Solution() << "time,fitness,weight,vector" << std::endl;
  Solution() << tm.NanosFromBeginning() << ","
    << cycle.GetBestFitness() << ","
    << cycle.GetBestIndividual().GetBestWeight(*config._problem) << ","
    << cycle.GetBestIndividual() << std::endl;
  std::cout << " Time: " << tm.NanosFromBeginning() << std::endl;
  std::cout << " Best vector: " << cycle.GetBestIndividual() << std::endl;
  std::cout << " Best weight: " << cycle.GetBestIndividual().GetBestWeight(*config._problem) << std::endl;
  std::cout << " Best fitness: " << cycle.GetBestFitness() << std::endl;
}

int main()
{
  const int variableNum = 20;
  const int multiplesNum = 40;
  const int seed = 56789;

  //auto problem = CNFGenerator::GenerateOne(100, 1000);
  auto problem = gen_3_sat(variableNum, multiplesNum, 50, seed);

  Bruteforce<CNFProblem> brutforce(*problem);
  if (brutforce.CanSolve())
  {
    std::cout << std::endl;
    Log BestSolution("BestSolution");
    BestSolution() << "solution,weight" << std::endl;
    std::cout << "solution,weight" << std::endl;
    brutforce.Solve();
    BestSolution() << brutforce << "," << brutforce.GetWeight() << std::endl;
    std::cout << brutforce << "," << brutforce.GetWeight() << std::endl;
    std::cout << std::endl;
  }

  Config<CNFProblem> config;
  config._populationSize = 200;
  config._bitCount = variableNum;
  config._crossCount = config._populationSize * 2 / 3;
  config._mutationCount = config._populationSize * 2 / 3;
  config._mutationBitCount = variableNum / 30;
  config._mutationBitCount = config._mutationBitCount < 1 ? 1 : config._mutationBitCount;
  config._problem = problem;

  Evolution(variableNum, multiplesNum, seed, config);
  
  return 0;
}
