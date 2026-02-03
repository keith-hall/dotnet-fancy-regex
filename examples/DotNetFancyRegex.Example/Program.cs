using DotNetFancyRegex;
using System.Diagnostics;
using SystemRegex = System.Text.RegularExpressions.Regex;

Console.WriteLine("=== DotNetFancyRegex Example ===\n");

// Example 1: Basic Pattern Matching
Console.WriteLine("1. Basic Pattern Matching:");
using (var regex = new FancyRegex(@"\d+"))
{
    string text = "hello 123 world";
    Console.WriteLine($"   Text: '{text}'");
    Console.WriteLine($"   Pattern: \\d+");
    Console.WriteLine($"   IsMatch: {regex.IsMatch(text)}");
    Console.WriteLine($"   Find: '{regex.Find(text)}'");
    Console.WriteLine();
}

// Example 2: Backreferences (not supported in .NET Regex)
Console.WriteLine("2. Backreferences (fancy-regex feature):");
using (var regex = new FancyRegex(@"(\w+)\s+\1"))
{
    string text1 = "hello hello";
    string text2 = "hello world";
    Console.WriteLine($"   Pattern: (\\w+)\\s+\\1 (matches repeated words)");
    Console.WriteLine($"   '{text1}' matches: {regex.IsMatch(text1)}");
    Console.WriteLine($"   '{text2}' matches: {regex.IsMatch(text2)}");
    Console.WriteLine();
}

// Example 3: Lookahead
Console.WriteLine("3. Positive Lookahead:");
using (var regex = new FancyRegex(@"\d+(?=px)"))
{
    string text = "width: 100px, height: 200em";
    Console.WriteLine($"   Text: '{text}'");
    Console.WriteLine($"   Pattern: \\d+(?=px) (numbers followed by 'px')");
    Console.WriteLine($"   Find: '{regex.Find(text)}'");
    Console.WriteLine();
}

// Example 4: Lookbehind
Console.WriteLine("4. Positive Lookbehind:");
using (var regex = new FancyRegex(@"(?<=\$)\d+"))
{
    string text = "price: $100, cost: €200";
    Console.WriteLine($"   Text: '{text}'");
    Console.WriteLine($"   Pattern: (?<=\\$)\\d+ (numbers preceded by '$')");
    Console.WriteLine($"   Find: '{regex.Find(text)}'");
    Console.WriteLine();
}

// Example 5: Replace All
Console.WriteLine("5. Replace All:");
using (var regex = new FancyRegex(@"\d+"))
{
    string text = "I have 3 apples and 5 oranges";
    string replaced = regex.ReplaceAll(text, "many");
    Console.WriteLine($"   Original: '{text}'");
    Console.WriteLine($"   Pattern: \\d+");
    Console.WriteLine($"   Replaced: '{replaced}'");
    Console.WriteLine();
}

// Example 6: Performance Comparison
Console.WriteLine("6. Quick Performance Comparison:");
const string pattern = @"\d+";
const string testText = "hello 123 world 456 test 789";
const int iterations = 10000;

var systemRegex = new SystemRegex(pattern);
var sw = Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    systemRegex.IsMatch(testText);
}
sw.Stop();
var systemTime = sw.Elapsed;

using (var fancyRegex = new FancyRegex(pattern))
{
    sw.Restart();
    for (int i = 0; i < iterations; i++)
    {
        fancyRegex.IsMatch(testText);
    }
    sw.Stop();
    var fancyTime = sw.Elapsed;

    Console.WriteLine($"   Pattern: {pattern}");
    Console.WriteLine($"   Text: '{testText}'");
    Console.WriteLine($"   Iterations: {iterations:N0}");
    Console.WriteLine($"   .NET Regex: {systemTime.TotalMilliseconds:F2}ms");
    Console.WriteLine($"   FancyRegex: {fancyTime.TotalMilliseconds:F2}ms");
    Console.WriteLine($"   Ratio: {(fancyTime.TotalMilliseconds / systemTime.TotalMilliseconds):F2}x");
}

Console.WriteLine("\n=== Example Complete ===");

