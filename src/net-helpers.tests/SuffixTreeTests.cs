using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace vzh.NetHelpers.Tests
{
  public class SuffixTreeTests
  {
    [Theory]
    [InlineData("leetcode", "tcode")]
    [InlineData("acdabcd", "dabcd")]
    [InlineData("abab", "bab")]
    public void SuffxiTree_LexGreatest_Tests(string input, string output)
    {
      var tree = new SuffixTree(input);

      Assert.Equal(output, tree.GetLexGreatestSubstring());
    }
  }
}
