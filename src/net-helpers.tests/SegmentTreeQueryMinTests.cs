using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace vzh.NetHelpers.Tests
{
  public class SegmentTreeQueryMinTests
  {
    [Theory]
    [InlineData(0, 0, 8)]
    [InlineData(0, 1, 4)]
    [InlineData(0, 2, 0)]
    [InlineData(2, 3, 0)]
    [InlineData(1, 3, 0)]
    [InlineData(2, 4, -1)]
    [InlineData(0, 5, -1)]
    public void SegmentTreeQueryMin_GetMin_Tests(int from, int to, int expected)
    {
      var arr = new int[] { 8, 4, 0, 0, -1, 4 };

      var tree = new SegmentTreeQueryMin(arr);

      Assert.Equal(expected, tree.GetMin((from, to)));
    }
  }
}
