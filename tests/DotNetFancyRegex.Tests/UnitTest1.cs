namespace DotNetFancyRegex.Tests;

public class FancyRegexTests
{
    [Fact]
    public void IsMatch_WithValidPattern_ReturnsTrue()
    {
        using var regex = new FancyRegex(@"\d+");
        Assert.True(regex.IsMatch("hello 123 world"));
    }

    [Fact]
    public void IsMatch_WithValidPattern_ReturnsFalse()
    {
        using var regex = new FancyRegex(@"\d+");
        Assert.False(regex.IsMatch("hello world"));
    }

    [Fact]
    public void Find_WithValidPattern_ReturnsMatch()
    {
        using var regex = new FancyRegex(@"\d+");
        var result = regex.Find("hello 123 world");
        Assert.Equal("123", result);
    }

    [Fact]
    public void Find_WithNoMatch_ReturnsNull()
    {
        using var regex = new FancyRegex(@"\d+");
        var result = regex.Find("hello world");
        Assert.Null(result);
    }

    [Fact]
    public void ReplaceAll_WithValidPattern_ReplacesMatches()
    {
        using var regex = new FancyRegex(@"\d+");
        var result = regex.ReplaceAll("hello 123 world 456", "XXX");
        Assert.Equal("hello XXX world XXX", result);
    }

    [Fact]
    public void Constructor_WithInvalidPattern_ThrowsException()
    {
        Assert.Throws<FancyRegexException>(() => new FancyRegex(@"(?P<unclosed"));
    }

    [Fact]
    public void FancyFeatures_Backreference_Works()
    {
        // Test backreference - match repeated word
        using var regex = new FancyRegex(@"(\w+)\s+\1");
        Assert.True(regex.IsMatch("hello hello"));
        Assert.False(regex.IsMatch("hello world"));
    }

    [Fact]
    public void FancyFeatures_Lookahead_Works()
    {
        // Test positive lookahead
        using var regex = new FancyRegex(@"\d+(?=px)");
        var result = regex.Find("width: 100px");
        Assert.Equal("100", result);
    }

    [Fact]
    public void FancyFeatures_Lookbehind_Works()
    {
        // Test positive lookbehind
        using var regex = new FancyRegex(@"(?<=\$)\d+");
        var result = regex.Find("price: $100");
        Assert.Equal("100", result);
    }
}

