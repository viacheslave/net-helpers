using System;

namespace vzh.NetHelpers
{
  /// <summary>
  ///   Segement Tree that supports queries for min element value.
  /// </summary>
  public class SegmentTreeQueryMin
  {
    private readonly int[] _data;
    private readonly int _length;
    
    public SegmentTreeQueryMin(int[] arr)
    {
      var height = (int)Math.Ceiling(Math.Log(arr.Length) / Math.Log(2));

      var maxSize = 2 * (int)Math.Pow(2, height) - 1;
      _data = new int[maxSize];

      _length = arr.Length;

      Build(arr, (0, arr.Length - 1), 0);
    }

    /// <summary>
    ///   Query range
    /// </summary>
    /// <param name="range">Range from-to inclusive</param>
    /// <returns>Min value</returns>
    public int GetMin((int from, int to) range)
    {
      return GetMin((0, _length - 1), range, 0);
    }

    private int GetMin((int from, int to) dr, (int from, int to) qr, int index)
    {
      if (qr.from <= dr.from && qr.to >= dr.to)
        return _data[index];

      if (qr.from > dr.to || qr.to < dr.from)
        return int.MaxValue;

      var mid = dr.from + (dr.to - dr.from) / 2;

      return Math.Min(
        GetMin((dr.from, mid), qr, 2 * index + 1),
        GetMin((mid + 1, dr.to), qr, 2 * index + 2));
    }

    private int Build(int[] arr, (int from, int to) dr, int index)
    {
      if (dr.from == dr.to)
      {
        _data[index] = arr[dr.from];
        return arr[dr.from];
      }

      var mid = dr.from + (dr.to - dr.from) / 2;

      _data[index] = Math.Min(
        Build(arr, (dr.from, mid), index * 2 + 1),
        Build(arr, (mid + 1, dr.to), index * 2 + 2));

      return _data[index];
    }
  }
}
