using System.Collections.Generic;

namespace vzh.NetHelpers
{
  /// <summary>
  ///   Single trie node
  /// </summary>
  /// <typeparam name="T">Trie node generic type</typeparam>
  public sealed class TrieNode<T>
  {
    /// <summary>
    ///   Node value
    /// </summary>
    public T Value { get; }

    /// <summary>
    ///   Indication of terminated node
    /// </summary>
    public bool IsEnd { get; private set; }

    /// <summary>
    ///   Node's children
    /// </summary>
    public ICollection<TrieNode<T>> Nodes => _nodes.Values;

    private Dictionary<T, TrieNode<T>> _nodes { get; } = new Dictionary<T, TrieNode<T>>();

    public TrieNode(T value)
    {
      Value = value;
    }

    /// <summary>
    ///   Terminates the node
    /// </summary>
    public void SetEnd() => IsEnd = true;

    /// <summary>
    ///   Adds a child to the node
    /// </summary>
    /// <param name="value">Child's value</param>
    /// <returns>New node</returns>
    public TrieNode<T> Add(T value)
    {
      if (_nodes.ContainsKey(value))
        return _nodes[value];

      var node = new TrieNode<T>(value);
      _nodes[value] = node;

      return node;
    }
  }
}
