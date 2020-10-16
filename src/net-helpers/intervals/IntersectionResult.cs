using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vzh.NetHelpers.intervals
{
  public class IntersectionResult
  {
    /// <summary>
    ///   Intersections
    /// </summary>
    public IReadOnlyList<(int start, int end)> Intersection { get; }

    /// <summary>
    ///   New base
    /// </summary>
    public IReadOnlyList<(int start, int end)> Merge { get; }

    public IntersectionResult(IList<(int start, int end)> intersection, IList<(int start, int end)> baseline)
    {
      Intersection = intersection.ToList().AsReadOnly();
      Merge = baseline.ToList().AsReadOnly();
    }
  }
}
