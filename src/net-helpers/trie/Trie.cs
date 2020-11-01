using System;
using System.Collections.Generic;
using System.Text;

namespace vzh.NetHelpers
{
  public class Trie<T>
  {
    /// <summary>
    ///   Root node
    /// </summary>
    public TrieNode<T> Root { get; } = new TrieNode<T>(default);

    /// <summary>
    ///   Adds a sequence to the trie
    /// </summary>
    /// <param name="seq">Sequence</param>
    public void Add(IEnumerable<T> seq)
    {
      var current = Root;

      foreach (var element in seq)
        current = current.Add(element);

      current.SetEnd();
    }
  }
}
