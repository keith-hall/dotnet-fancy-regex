```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.3 LTS (Noble Numbat)
AMD EPYC 7763 3.07GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.102
  [Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3


```
| Method              | Mean | Error |
|-------------------- |-----:|------:|
| SystemRegex_IsMatch |   NA |    NA |
| FancyRegex_IsMatch  |   NA |    NA |
| SystemRegex_Find    |   NA |    NA |
| FancyRegex_Find     |   NA |    NA |
| SystemRegex_Replace |   NA |    NA |
| FancyRegex_Replace  |   NA |    NA |

Benchmarks with issues:
  RegexBenchmarks.SystemRegex_IsMatch: DefaultJob
  RegexBenchmarks.FancyRegex_IsMatch: DefaultJob
  RegexBenchmarks.SystemRegex_Find: DefaultJob
  RegexBenchmarks.FancyRegex_Find: DefaultJob
  RegexBenchmarks.SystemRegex_Replace: DefaultJob
  RegexBenchmarks.FancyRegex_Replace: DefaultJob
