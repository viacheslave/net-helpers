using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace vzh.NetHelpers.Tests
{
	public class RedBlackTreeTests
	{
    /// <summary>
    ///   Insert API.
    /// </summary>
    /// <param name="elements">Number of elements</param>
    /// <param name="start">Start index</param>
    /// <param name="increment">Increment</param>
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
    
    /// <summary>
    ///   Delete functionality.
    ///   Deletes are from list edges.
    /// </summary>
    /// <param name="elements">Number of elemetns</param>
    /// <param name="start">Start index of insert elements</param>
    /// <param name="increment">Increment of insert elements</param>
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

    /// <summary>
    ///   Delete functionality.
    ///   Deletes are at the middle.
    /// </summary>
    /// <param name="elements">Number of elemetns</param>
    /// <param name="start">Start index of insert elements</param>
    /// <param name="increment">Increment of insert elements</param>
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
      Assert.Equal(result != null, expected);
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
      Assert.Equal(result.Key, expected);
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
      Assert.Equal(result.Key, expected);
    }

    [Fact]
    public void Last_Returns_Default()
    {
      var data = new Dictionary<int, int>();
      var rbt = new RedBlackTree<int, int>(data);

      var result = rbt.Last();
      Assert.Null(result);
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
