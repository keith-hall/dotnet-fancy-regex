using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DotNetFancyRegex;
using SystemRegex = System.Text.RegularExpressions.Regex;

BenchmarkRunner.Run<RegexBenchmarks>();

[MemoryDiagnoser]
public class RegexBenchmarks
{
    private const string Pattern = @"\d+";
    private const string Text = "hello 123 world 456 test 789";
    
    private SystemRegex? _systemRegex;
    private FancyRegex? _fancyRegex;

    [GlobalSetup]
    public void Setup()
    {
        _systemRegex = new SystemRegex(Pattern);
        _fancyRegex = new FancyRegex(Pattern);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _fancyRegex?.Dispose();
    }

    [Benchmark]
    public bool SystemRegex_IsMatch()
    {
        return _systemRegex!.IsMatch(Text);
    }

    [Benchmark]
    public bool FancyRegex_IsMatch()
    {
        return _fancyRegex!.IsMatch(Text);
    }

    [Benchmark]
    public string? SystemRegex_Find()
    {
        return _systemRegex!.Match(Text).Value;
    }

    [Benchmark]
    public string? FancyRegex_Find()
    {
        return _fancyRegex!.Find(Text);
    }

    [Benchmark]
    public string SystemRegex_Replace()
    {
        return _systemRegex!.Replace(Text, "XXX");
    }

    [Benchmark]
    public string FancyRegex_Replace()
    {
        return _fancyRegex!.ReplaceAll(Text, "XXX");
    }
}

[MemoryDiagnoser]
public class BackreferenceBenchmarks
{
    private const string Pattern = @"(\w+)\s+\1";
    private const string Text = "hello hello world world test test";
    
    private FancyRegex? _fancyRegex;

    [GlobalSetup]
    public void Setup()
    {
        _fancyRegex = new FancyRegex(Pattern);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _fancyRegex?.Dispose();
    }

    [Benchmark]
    public bool FancyRegex_BackreferenceMatch()
    {
        return _fancyRegex!.IsMatch(Text);
    }
}

