#pragma once

#if _WIN32 || _WIN64
#include <windows.h>
#include <cstdint>
class TimeMeasure
{
private:
  LARGE_INTEGER _frequency;
  LARGE_INTEGER _start;
  LARGE_INTEGER _end;

public:
  TimeMeasure()
  {
    QueryPerformanceFrequency(&_frequency);
    QueryPerformanceCounter(&_start);
  }

  int64_t NanosFromBeginning()
  {
    QueryPerformanceCounter(&_end);
    return (_end.QuadPart - _start.QuadPart) / (_frequency.QuadPart / 1000000);
  }
};
#elif __linux__ || __unix__ || defined(_POSIX_VERSION)
#include <time.h>
class TimeMeasure
{
private:
  struct timespec _startTime;
public:
  TimeMeasure()
  {
    clock_gettime(CLOCK_PROCESS_CPUTIME_ID, &_startTime);
  }

  int64_t NanosFromBeginning()
  {
    struct timespec endTime;
    clock_gettime(CLOCK_PROCESS_CPUTIME_ID, &endTime);
    return endTime.tv_nsec - _startTime.tv_nsec;
  }
};
#elif // platform
#error "Platform not supported (Not POSIX or WIN32)"
#endif // platform
