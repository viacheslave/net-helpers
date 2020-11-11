using System;

namespace vzh.NetHelpers.Skiplist
{
  /// <summary>
  ///   Basic Skip List implementation.
  ///   Supports duplicates.
  /// </summary>
  /// <typeparam name="T">Type if items</typeparam>
  public class Skiplist<T>
    where T: IComparable<T>
  {
    private class Node
    {
      public T Key { get; }
      public Node[] ForwardNodes { get; }

      public Node(T key, int level)
      {
        Key = key;
        ForwardNodes = new Node[level + 1];
      }
    }

    private readonly int _maxLevel;
    private readonly double _p;

    private int _level;
    private readonly Node _head;
    private readonly Random _rnd = new Random(DateTime.Now.Millisecond);

    /// <summary>
    ///   Initializes a skip list.
    /// </summary>
    /// <param name="maxLevel">Max levels to support</param>
    /// <param name="p">Probability of node on level I having nodes on level (i+1)</param>
    public Skiplist(int maxLevel, double p)
    {
      _level = 0;
      _p = p;
      _maxLevel = maxLevel;
      _head = new Node(default, _maxLevel);
    }

    /// <summary>
    ///   Searches for a key.
    /// </summary>
    /// <param name="key">Key value</param>
    /// <returns>Trus if found</returns>
    public bool Search(T key)
    {
      (Node current, _) = GetUpdateNodes(key);
      return current?.Key.Equals(key) == true;
    }

    /// <summary>
    ///   Adds a key.
    /// </summary>
    /// <param name="key">Key value</param>
    public void Add(T key)
    {
      (_, Node[] updateNodes) = GetUpdateNodes(key);

      var randomLevel = GetRandomLevel();
      if (randomLevel > _level)
      {
        for (var i = _level + 1; i < randomLevel + 1; i++)
          updateNodes[i] = _head;

        _level = randomLevel;
      }

      var node = new Node(key, randomLevel);

      for (int i = 0; i <= randomLevel; i++)
      {
        node.ForwardNodes[i] = updateNodes[i].ForwardNodes[i];
        updateNodes[i].ForwardNodes[i] = node;
      }
    }

    /// <summary>
    ///   Removes a key.
    /// </summary>
    /// <param name="key">Key value</param>
    /// <returns>True if found</returns>
    public bool Erase(T key)
    {
      (Node current, Node[] updateNodes) = GetUpdateNodes(key);

      if (current?.Key.Equals(key) == true)
      {
        for (var i = 0; i <= _level; i++)
        {
          if (updateNodes[i].ForwardNodes[i] != current)
            break;

          updateNodes[i].ForwardNodes[i] = current.ForwardNodes[i];
        }

        while (_level > 0 && _head.ForwardNodes[_level] == null)
          _level--;

        return true;
      }

      return false;
    }

    private (Node current, Node[] updateNodes) GetUpdateNodes(T key)
    {
      var current = _head;
      var updateNodes = new Node[_maxLevel + 1];

      for (var i = _level; i >= 0; i--)
      {
        while (current.ForwardNodes[i]?.Key.CompareTo(key) < 0)
          current = current.ForwardNodes[i];

        updateNodes[i] = current;
      }

      return (current.ForwardNodes[0], updateNodes);
    }

    private int GetRandomLevel()
    {
      var level = 0;
      var rnd = _rnd.NextDouble();

      while (rnd < _p && level < _maxLevel)
      {
        level++;
        rnd = _rnd.NextDouble();
      }

      return level;
    }
  }
}
