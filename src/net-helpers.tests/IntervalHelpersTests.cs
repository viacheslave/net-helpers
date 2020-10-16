using System;
using System.Collections.Generic;
using System.Linq;
using vzh.NetHelpers.intervals;
using Xunit;

namespace vzh.NetHelpers.Tests
{
  public class IntervalHelpersTests
  {
    [Theory]
    // outside
    [InlineData(1, 2, 2, 3, false)]
    [InlineData(1, 2, 3, 4, false)]
    [InlineData(2, 3, 1, 2, false)]
    [InlineData(3, 4, 1, 2, false)]
    // same
    [InlineData(1, 2, 1, 2, true)]
    // start same
    [InlineData(1, 2, 1, 3, true)]
    [InlineData(1, 3, 1, 2, true)]
    // end same
    [InlineData(0, 2, 1, 2, true)]
    [InlineData(1, 2, 0, 2, true)]
    // inside
    [InlineData(1, 2, 0, 3, true)]
    [InlineData(0, 3, 1, 2, true)]
    // intersect
    [InlineData(1, 3, 2, 3, true)]
    [InlineData(1, 3, 2, 4, true)]
    [InlineData(2, 3, 1, 3, true)]
    [InlineData(2, 4, 1, 3, true)]
    public void Intersect_Tests(int p1start, int p1end, int p2start, int p2end, bool expected)
    {
      Assert.Equal(expected,
        IntervalHelpers.Intersect(
          (p1start, p1end),
          (p2start, p2end)));
    }

    [Theory]
    // outside
    [InlineData(1, 2, 2, 3)]
    [InlineData(1, 2, 3, 4)]
    [InlineData(2, 3, 1, 2)]
    [InlineData(3, 4, 1, 2)]
    public void GetIntersection_Throws(int p1start, int p1end, int p2start, int p2end)
    {
      Assert.Throws<InvalidOperationException>(() =>
        IntervalHelpers.GetIntersection((p1start, p1end), (p2start, p2end))
      );
    }

    [Theory]
    // same
    [InlineData(1, 2, 1, 2, 1, 2)]
    // start same           
    [InlineData(1, 2, 1, 3, 1, 2)]
    [InlineData(1, 3, 1, 2, 1, 2)]
    // end same             
    [InlineData(0, 2, 1, 2, 1, 2)]
    [InlineData(1, 2, 0, 2, 1, 2)]
    // inside               
    [InlineData(1, 2, 0, 3, 1, 2)]
    [InlineData(0, 3, 1, 2, 1, 2)]
    // intersect            
    [InlineData(1, 3, 2, 3, 2, 3)]
    [InlineData(1, 3, 2, 4, 2, 3)]
    [InlineData(2, 3, 1, 3, 2, 3)]
    [InlineData(2, 4, 1, 3, 2, 3)]
    public void GetIntersection_Tests(int p1start, int p1end, int p2start, int p2end, int istart, int iend)
    {
      var intersection = IntervalHelpers.GetIntersection((p1start, p1end), (p2start, p2end));

      Assert.Equal((istart, iend), intersection);
    }

    [Theory]
    [InlineData(0, 10, 20, 1, 2, 20, 21, 22, 23)]
    [InlineData(1, 10, 20, 11, 12, 12, 13, 13, 14)]
    [InlineData(2, 10, 20, 10, 11, 12, 13, 19, 20)]
    [InlineData(3, 10, 20, 9, 11, 12, 13, 19, 21)]
    public void IntersectOver_Tests(
      int resIndex,
      int ins, int ine, 
      int p1start, int p1end,
      int p2start, int p2end,
      int p3start, int p3end)
    {
      var interval = (ins, ine);
      var storage = new List<(int, int)>()
      {
        (p1start, p1end),
        (p2start, p2end),
        (p3start, p3end),
      };

      var result = IntervalHelpers.IntersectOver(interval, storage);

      var expected = IntersectOverResults.Results[resIndex];

      AssertEqual(expected.Intersection, result.Intersection);
      AssertEqual(expected.Merge, result.Merge);

      static void AssertEqual(IReadOnlyList<(int,int)> exp, IReadOnlyList<(int,int)> res)
      {
        Assert.Equal(exp.Count, res.Count);
        for (var i = 0; i < res.Count; ++i)
          Assert.Equal(exp[i], res[i]);
      }
    }

    private static class IntersectOverResults
    {
      public static readonly IReadOnlyDictionary<int, IntersectionResult> Results =
        new Dictionary<int, IntersectionResult>
        {
          [0] = new IntersectionResult(
            new List<(int, int)>() {  },
            new List<(int, int)>() { (1,2), (20,21), (22,23) }),

          [1] = new IntersectionResult(
            new List<(int, int)>() { (11,12), (12,13), (13,14) },
            new List<(int, int)>() { (10,20) }),

          [2] = new IntersectionResult(
            new List<(int, int)>() { (10,11), (12,13), (19,20) },
            new List<(int, int)>() { (10,20) }),

          [3] = new IntersectionResult(
            new List<(int, int)>() { (10, 11), (12, 13), (19, 20) },
            new List<(int, int)>() { (9, 21) })
        };
    }
  }
}
