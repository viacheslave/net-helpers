using System;
using System.Collections.Generic;
using System.Text;

namespace vzh.NetHelpers.intervals
{
  /// <summary>
  ///   Various interval helpers
  /// </summary>
  public static class IntervalHelpers
  {
    /// <summary>
    ///   Tells if two intervals [start, end) intersect
    /// </summary>
    /// <param name="interval">First interval</param>
    /// <param name="other">Second interval</param>
    /// <returns>True if intersect</returns>
    public static bool Intersect((int start, int end) interval, (int start, int end) other)
    {
      return interval.start < other.end && interval.end > other.start;
    }

    /// <summary>
    ///   Get intersection of two intervals [start, end).
    /// </summary>
    /// <param name="interval">First interval</param>
    /// <param name="other">Second interval</param>
    /// <exception cref="InvalidOperationException">If do not intersect</exception>
    /// <returns>Intersection interval</returns>
    public static (int start, int end) GetIntersection((int start, int end) interval, (int start, int end) other)
    {
      return !Intersect(interval, other)
        ? throw new InvalidOperationException("Intervals does not intersect")
        : (
            Math.Max(interval.start, other.start),
            Math.Min(interval.end, other.end)
          );
    }

    /// <summary>
    ///   Returns result of applying interval over an ordered set of non-intersecting intervals.
    /// </summary>
    /// <param name="interval">New interval</param>
    /// <param name="storage">Base intervals</param>
    /// <returns>Intersections and modified base</returns>
    public static IntersectionResult IntersectOver((int start, int end) interval, List<(int start, int end)> storage)
    {
      var doubled = new List<(int start, int end)>();

      var si = -1;
      var ei = -1;

      for (var index = 0; index < storage.Count; ++index)
      {
        var st = storage[index];

        if (Intersect(st, interval))
        {
          doubled.Add(GetIntersection(st, interval));

          if (si == -1)
            si = ei = index;
          else
            ei = index;
        }
      }

      if (si == -1)
        return new IntersectionResult(doubled, storage);

      var newInterval = (
        Math.Min(interval.start, storage[si].start),
        Math.Max(interval.end, storage[ei].end)
      );

      storage.Insert(ei + 1, newInterval);

      storage.RemoveRange(si, ei - si + 1);

      return new IntersectionResult(doubled, storage);
    }
  }
}
