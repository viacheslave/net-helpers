using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace vzh.NetHelpers.Tests
{
	public class RedBlackTreeTests
	{
    [Theory]
    [InlineData(1000, 1, 1)]
    [InlineData(1000, 1, 3)]
    [InlineData(1000, 1, 5)]
    [InlineData(1000, 1, -1)]
    [InlineData(1000, 1, -3)]
    [InlineData(1000, 1, -5)]
    public void Insert_ArithmeticSequence(int elements, int start, int increment)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      AssertTree(rbt, data.Keys.ToList());
    }
    
    [Theory]
    [InlineData(1000, 1, 1)]
    [InlineData(1000, 1, 3)]
    [InlineData(1000, 1, 5)]
    [InlineData(1000, 1, -1)]
    [InlineData(1000, 1, -3)]
    [InlineData(1000, 1, -5)]
    public void Delete_Egdes(int elements, int start, int increment)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var list = data.Select(c => c.Key).ToList();

      while (list.Count > 0)
      {
        int el;

        if (list.Count % 2 == 0)
        {
          el = list[0];
          list.RemoveAt(0);
        }
        else
        {
          el = list[^1];
          list.RemoveAt(list.Count - 1);
        }

        rbt.Delete(el);

        AssertTree(rbt, list);
      }
    }

    [Theory]
    [InlineData(1000, 1, 1)]
    [InlineData(1000, 1, 3)]
    [InlineData(1000, 1, 5)]
    [InlineData(1000, 1, -1)]
    [InlineData(1000, 1, -3)]
    [InlineData(1000, 1, -5)]
    public void Delete_Middle(int elements, int start, int increment)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var list = data.Select(c => c.Key).ToList();

      while (list.Count > 0)
      {
        int mid = list.Count / 2;
        int el = list[mid];

        list.RemoveAt(mid);
        rbt.Delete(el);

        AssertTree(rbt, list);
      }
    }

    [Theory]
    [InlineData(1000, 1, 1, 1, true)]
    [InlineData(1000, 1, 1, 500, true)]
    [InlineData(1000, 1, 1, 10001, false)]
    public void Find_Tests(int elements, int start, int increment, int el, bool expected)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.Find(el);
      Assert.Equal(expected, result != null);
    }

    [Theory]
    [InlineData(1000, 1, 1, 1)]
    [InlineData(1000, 1000, -1, 1)]
    public void First_Returns_Node(int elements, int start, int increment, int expected)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.First();
      Assert.NotNull(result);
      Assert.Equal(expected, result.Key);
    }

    [Fact]
    public void First_Returns_Default()
    {
      var data = new Dictionary<int, int>();
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.First();
      Assert.Null(result);
    }

    [Theory]
    [InlineData(1000, 1, 1, 1000)]
    [InlineData(1000, 1000, -1, 1000)]
    public void Last_Returns_Node(int elements, int start, int increment, int expected)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.Last();
      Assert.NotNull(result);
      Assert.Equal(expected, result.Key);
    }

    [Fact]
    public void Last_Returns_Default()
    {
      var data = new Dictionary<int, int>();
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.Last();
      Assert.Null(result);
    }

    [Theory]
    [InlineData(1000, 1, 1)]
    public void IsEmpty_Returns_False(int elements, int start, int increment)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.IsEmpty();
      Assert.False(result);
    }

    [Fact]
    public void IsEmpty_Returns_True()
    {
      var data = new Dictionary<int, int>();
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.IsEmpty();
      Assert.True(result);
    }

    [Theory]
    [InlineData(1000, 1, 1)]
    public void Clear_IsEmpty_Returns_True(int elements, int start, int increment)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      rbt.Clear();

      var result = rbt.IsEmpty();
      Assert.True(result);
    }

    [Theory]
    [InlineData(1000, 1, 1, 1000)]
    public void Count_Returns_Count(int elements, int start, int increment, int expected)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.Count();
      Assert.Equal(expected, result);
    }

    [Fact]
    public void Count_Returns_Zero()
    {
      var data = new Dictionary<int, int>();
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.Count();
      Assert.Equal(0, result);
    }

    [Theory]
    [InlineData(1000, 1, 5, 8, true, 6)]
    [InlineData(1000, 1, 5, 11, true, 11)]
    [InlineData(1000, 1, 5, 8, false, 6)]
    [InlineData(1000, 1, 5, 11, false, 6)]
    [InlineData(1000, 1, 5, 6000, false, 4996)]
    public void GetFloor_Tests(int elements, int start, int increment, int key, bool orSelf, int expected)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.GetFloor(key, orSelf);
      Assert.Equal(expected, result.Key);
    }

    [Theory]
    [InlineData(1000, 1, 5, -1, true)]
    public void GetFloor_Tests_Returns_Null(int elements, int start, int increment, int key, bool orSelf)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.GetFloor(key, orSelf);
      Assert.Null(result);
    }

    [Theory]
    [InlineData(1000, 1, 5, 8, true, 11)]
    [InlineData(1000, 1, 5, 11, true, 11)]
    [InlineData(1000, 1, 5, 8, false, 11)]
    [InlineData(1000, 1, 5, 11, false, 16)]
    [InlineData(1000, 1, 5, -1, false, 1)]
    public void GetCeiling_Tests(int elements, int start, int increment, int key, bool orSelf, int expected)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.GetCeiling(key, orSelf);
      Assert.Equal(expected, result.Key);
    }

    [Theory]
    [InlineData(1000, 1, 5, 6000, true)]
    public void GetCeiling_Tests_Returns_Null(int elements, int start, int increment, int key, bool orSelf)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.GetCeiling(key, orSelf);
      Assert.Null(result);
    }

    [Theory]
    [InlineData(1000, 1, 5, 1, true)]
    [InlineData(1000, 1, 5, 4997, false)]
    public void ContainsKey_Tests(int elements, int start, int increment, int key, bool expected)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.ContainsKey(key);
      Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(1000, 1, 5, 1, 1)]
    [InlineData(1000, 1, 5, 4996, 4996)]
    public void Indexer_Getter_Returns_Value(int elements, int start, int increment, int key, int expected)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt[key];
      Assert.Equal(result, expected);
    }

    [Theory]
    [InlineData(1000, 1, 5, 0)]
    [InlineData(1000, 1, 5, 4997)]
    public void Indexer_Getter_Throws(int elements, int start, int increment, int key)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      Assert.Throws<KeyNotFoundException>(() => rbt[key]);
    }

    [Theory]
    [InlineData(1000, 1, 5, 1, 10000)]
    public void Indexer_Setter_Updates(int elements, int start, int increment, int key, int newValue)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      rbt[key] = newValue;

      Assert.Equal(1000, rbt.Count());
      Assert.Equal(10000, rbt[key]);
    }

    [Theory]
    [InlineData(1000, 1, 5, 10000, 10000)]
    public void Indexer_Setter_Inserts(int elements, int start, int increment, int key, int newValue)
    {
      var data = BuildSUT(elements, start, increment);
      var rbt = new RedBlackTree<int, int>(data);

      rbt[key] = newValue;

      Assert.Equal(1000 + 1, rbt.Count());
      Assert.Equal(10000, rbt[key]);
    }

    private static Dictionary<int, int> BuildSUT(int elements, int start, int increment)
    {
      var data = new Dictionary<int, int>();

      while (--elements >= 0)
      {
        data.Add(start, start);
        start += increment;
      }

      return data;
    }

    private static void AssertTree<TKey, TValue>(RedBlackTree<TKey, TValue> rbt, List<TKey> keys)
      where TKey: IComparable<TKey>
    {
      var nodes = new List<Node<TKey, TValue>>();
      Traverse(rbt.Root, nodes);

      // expected nodes
      var rbtKeys = nodes.Select(n => n.Key).ToHashSet();
      var expectedKeys = keys.ToHashSet();

      var setsEqual = rbtKeys.SetEquals(expectedKeys);
      Assert.True(setsEqual);

      if (rbt.Root == null)
        return;

      // root is black
      Assert.True(rbt.Root.Color == Color.Black);

      // no adjacent red nodes
      foreach (var node in nodes)
      {
        if (node.Color == Color.Red)
        {
          Assert.False(node.Left?.Color == Color.Red);
          Assert.False(node.Right?.Color == Color.Red);
        }
      }

      // is still binary search tree
      foreach (var node in nodes)
      {
        if (node.Left != null)
          Assert.True(node.Left.Key.CompareTo(node.Key) < 0);

        if (node.Right != null)
          Assert.True(node.Right.Key.CompareTo(node.Key) > 0);
      }

      // number of black nodes is the same
      var nodesDepths = new List<(TKey key, int depth)>();
      TraverseBlackDepth(rbt.Root, nodesDepths, 0);

      Assert.True(nodesDepths.Select(c => c.depth).Distinct().Count() == 1);

      // max height is
      var nodesHeights = new List<(TKey key, int height)>();
      TraverseHeight(rbt.Root, nodesHeights, 1);

      var expectedMaxHeight = 2 * Math.Log2(nodesHeights.Count + 1);
      Assert.True(nodesHeights.Select(c => c.height).All(x => x <= expectedMaxHeight));
    }

    private static void Traverse<TKey, TValue>(Node<TKey, TValue> node, List<Node<TKey, TValue>> nodes) 
      where TKey : IComparable<TKey>
    {
      if (node == null)
        return;

      nodes.Add(node);
      Traverse(node.Left, nodes);
      Traverse(node.Right, nodes);
    }

    private static void TraverseBlackDepth<TKey, TValue>(Node<TKey, TValue> node, List<(TKey key, int depth)> nodes, int depth) 
      where TKey : IComparable<TKey>
    {
      if (node == null)
        return;

      if (node.Color == Color.Black)
        ++depth;

      if (node.Left == null && node.Right == null)
        nodes.Add((node.Key, depth));

      TraverseBlackDepth(node.Left, nodes, depth);
      TraverseBlackDepth(node.Right, nodes, depth);
    }

    private static void TraverseHeight<TKey, TValue>(Node<TKey, TValue> node, List<(TKey key, int height)> nodes, int height) 
      where TKey : IComparable<TKey>
    {
      if (node == null)
        return;

      nodes.Add((node.Key, height));
      TraverseHeight(node.Left, nodes, height + 1);
      TraverseHeight(node.Right, nodes, height + 1);
    }
  }
}
