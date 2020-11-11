using System;
using System.Collections.Generic;
using System.Linq;
using vzh.NetHelpers.Skiplist;
using Xunit;

namespace vzh.NetHelpers.Tests
{
  public class SkipListTests
  {
    [Fact]
    public void SkipList_Adds_Searches()
    {
      var list = new Skiplist<int>(4, 0.5);
      list.Add(1);
      list.Add(2);
      list.Add(1);
      list.Add(1);
      list.Add(2);
      list.Add(3);

      Assert.True(list.Search(1));
      Assert.True(list.Search(2));
      Assert.True(list.Search(3));
    }

    [Fact]
    public void SkipList_Erases()
    {
      var list = new Skiplist<int>(4, 0.5);
      list.Add(1);
      list.Add(2);
      list.Add(1);
      list.Add(1);
      list.Add(1);
      list.Add(2);
      list.Add(3);

      // not in list
      Assert.False(list.Erase(4));

      Assert.True(list.Erase(1));
      Assert.True(list.Erase(2));
      Assert.True(list.Erase(1));
      Assert.True(list.Erase(1));
      Assert.True(list.Erase(1));
      Assert.True(list.Erase(2));
      Assert.True(list.Erase(3));

      // list should be empty
      Assert.False(list.Erase(1));
      Assert.False(list.Erase(2));
      Assert.False(list.Erase(3));

      // not in list
      Assert.False(list.Erase(4));
    }
  }
}
