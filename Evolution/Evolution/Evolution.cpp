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

void SleepMs(unsigned long milis)
{
  std::this_thread::sleep_for(std::chrono::milliseconds(milis));
}

void Evolution(int variableNum, int multiplesNum, int seed, Config<CNFProblem>& config)
{
  Log::ResetTime();
  Log Conf("Config");
  Log Progress("Progress");
  Log Solution("Solution");

  Conf() << "Variables,Multiples,Seed" << std::endl;
  Conf() << variableNum << "," << multiplesNum << "," << seed << std::endl;

  Progress() << "time,best,sol,weight" << std::endl;
  const int avgNum = 100;
  const float avgInv = 1.0f / avgNum;
  Cycle<CNFProblem>** cycles = new Cycle<CNFProblem>*[avgNum];
  for (int i = 0; i < avgNum; i++)
    cycles[i] = new Cycle<CNFProblem>(config);

  TimeMeasure tm;
  for (int i = 0; i < 200; i++)
  {
/*    if (cycle.HasSolution())
      break;
      */
    for (int i = 0; i < avgNum; i++)
      cycles[i]->Step();
    //cycle.Print();
    float best = 0;
    float sol = 0;
    float weight = 0;
    for (int i = 0; i < avgNum; i++)
    {
      best += cycles[i]->GetBestFitness() * avgInv;
      sol += cycles[i]->GetBestIndividual().GetSolutionPercentage(*config._problem) * avgInv;
      weight += cycles[i]->GetBestIndividual().GetBestWeight(*config._problem) * avgInv;
    }
    
    std::cout << "[" << i << "] Time: " << tm.NanosFromBeginning() << " Best: " <<  best
      << " sol: " << sol
      << " weight: " << weight << std::endl;

    Progress() << tm.NanosFromBeginning() << "," << best << "," << sol << "," << weight << std::endl;
  }

  float bestFit = 0;
  float bestWeight = 0;

  for (int i = 0; i < avgNum; i++)
  {
    bestFit += cycles[i]->GetBestFitness() * avgInv;
    bestWeight += cycles[i]->GetBestIndividual().GetBestWeight(*config._problem) * avgInv;
  }

  Solution() << "time,fitness,weight" << std::endl;
  Solution() << tm.NanosFromBeginning() << ","
    << bestFit << ","
    << bestWeight << std::endl;
  std::cout << " Time: " << tm.NanosFromBeginning() << std::endl;
  std::cout << " Best weight: " << bestWeight << std::endl;
  std::cout << " Best fitness: " << bestFit << std::endl;

  for (int i = 0; i < avgNum; i++)
    delete cycles[i];
  delete[] cycles;
}

void StaticInstMain()
{
  const int variableNum = 25;
  const int multiplesNum = 50;
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

  /*for (int i = 50; i <= 300; i += 50)
  {
  config._populationSize = i;
  config._bitCount = variableNum;
  config._crossCount = config._populationSize * 2 / 3;
  config._mutationCount = config._populationSize * 2 / 3;
  config._mutationBitCount = variableNum / 30;
  config._mutationBitCount = config._mutationBitCount < 1 ? 1 : config._mutationBitCount;
  Evolution(variableNum, multiplesNum, seed, config);
  SleepMs(1001);
  }*/

  /*for (int i = 1; i <= 6; i += 1)
  {
  config._populationSize = 200;
  config._bitCount = variableNum;
  config._crossCount = config._populationSize * 2 / 3;
  config._mutationCount = config._populationSize * 2 / 3;
  config._mutationBitCount = i;
  config._mutationBitCount = config._mutationBitCount < 1 ? 1 : config._mutationBitCount;
  Evolution(variableNum, multiplesNum, seed, config);
  SleepMs(1001);
  }*/
  /*for (int i = 30; i < config._populationSize - 1; i += 30)
  {
  config._populationSize = 200;
  config._bitCount = variableNum;
  config._crossCount = i;
  config._mutationCount = i;
  config._mutationBitCount = variableNum / 30;
  config._mutationBitCount = config._mutationBitCount < 1 ? 1 : config._mutationBitCount;
  Evolution(variableNum, multiplesNum, seed, config);
  SleepMs(1001);
  }*/

  Evolution(variableNum, multiplesNum, seed, config);
  SleepMs(1001);
}

void VarInstMain(int variableNum, int multiplesNum)
{
  const int seed = 56789;

  auto problem = gen_3_sat(variableNum, multiplesNum, 50, seed);

  Config<CNFProblem> config;
  config._populationSize = 200;
  config._bitCount = variableNum;
  config._crossCount = config._populationSize * 2 / 3;
  config._mutationCount = config._populationSize * 2 / 3;
  config._mutationBitCount = variableNum / 30;
  config._mutationBitCount = config._mutationBitCount < 1 ? 1 : config._mutationBitCount;
  config._problem = problem;

  Evolution(variableNum, multiplesNum, seed, config);
}

int main()
{
  StaticInstMain();
 /* for (int i = 25; i <= 100; i += 5)
    VarInstMain(25, i);*/

  return 0;
}
