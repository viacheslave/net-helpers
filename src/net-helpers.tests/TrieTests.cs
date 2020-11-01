using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace vzh.NetHelpers.Tests
{
  public class TrieTests
  {
    [Fact]
    public void Trie_Add_Tests()
    {
      var trie = new Trie<char>();

      trie.Add("ab");
      trie.Add("abc");

      var a = trie.Root.Nodes.First(n => n.Value == 'a');

      var b = trie.Root.Nodes.First(n => n.Value == 'a')
        .Nodes.First(n => n.Value == 'b');

      var c = trie.Root.Nodes.First(n => n.Value == 'a')
        .Nodes.First(n => n.Value == 'b')
        .Nodes.First(n => n.Value == 'c');

      Assert.False(a.IsEnd);
      Assert.True(b.IsEnd);
      Assert.True(c.IsEnd);
    }
  }
}
