using System;
using System.Collections.Generic;
using System.Text;

namespace vzh.NetHelpers
{
  /// <summary>
  ///   Suffix Tree (Ukkonen's) 
  /// </summary>
  public class SuffixTree
  {
    public class Node
    {
      public int Start { get; set; }
      public int End { get; set; } = int.MaxValue / 2;
      public int Link { get; set; }
      public SortedList<char, int> Next { get; set; } = new SortedList<char, int>();

      public Node(int start, int end)
      {
        Start = start;
        End = end;
      }

      public int GetEdgeLength(int pos) => Math.Min(End, pos + 1) - Start;
    }

    private readonly int _maxLength = int.MaxValue / 2;
    private readonly Node[] _nodes;
    private readonly char[] _text;

    private int _root;
    private int _position = -1;
    private int _currentNode;
    private int _needSuffixLink;
    private int _remainder;

    private int _activeNode;
    private int _activeLength;
    private int _activeEdge;

    public SuffixTree(string text)
    {
      _nodes = new Node[2 * text.Length + 2];
      _text = new char[text.Length];
      _root = _activeNode = CreateNewNode(-1, -1);

      foreach (var ch in text)
        AddChar(ch);
    }

    private void AddSuffixLink(int node)
    {
      if (_needSuffixLink > 0)
        _nodes[_needSuffixLink].Link = node;

      _needSuffixLink = node;
    }

    private char GetActiveEgde() => _text[_activeEdge];

    private bool WalkDown(int next)
    {
      if (_activeLength >= _nodes[next].GetEdgeLength(_position))
      {
        _activeEdge += _nodes[next].GetEdgeLength(_position);
        _activeLength -= _nodes[next].GetEdgeLength(_position);
        _activeNode = next;
        return true;
      }

      return false;
    }

    private int CreateNewNode(int start, int end)
    {
      _nodes[++_currentNode] = new Node(start, end);
      return _currentNode;
    }

    private void AddChar(char c)
    {
      _text[++_position] = c;
      _needSuffixLink = -1;
      _remainder++;

      while (_remainder > 0)
      {
        if (_activeLength == 0)
          _activeEdge = _position;

        if (!_nodes[_activeNode].Next.ContainsKey(GetActiveEgde()))
        {
          _nodes[_activeNode].Next[GetActiveEgde()] = CreateNewNode(_position, _maxLength);

          AddSuffixLink(_activeNode);
        }
        else
        {
          int next = _nodes[_activeNode].Next[GetActiveEgde()];
          if (WalkDown(next))
            continue;

          if (_text[_nodes[next].Start + _activeLength] == c)
          {
            _activeLength++;
            AddSuffixLink(_activeNode);
            break;
          }

          int split = CreateNewNode(
            _nodes[next].Start,
            _nodes[next].Start + _activeLength);

          _nodes[_activeNode].Next[GetActiveEgde()] = split;
          _nodes[split].Next[c] = CreateNewNode(_position, _maxLength);
          _nodes[next].Start += _activeLength;
          _nodes[split].Next[_text[_nodes[next].Start]] = next;

          AddSuffixLink(split);
        }

        _remainder--;

        if (_activeNode == _root && _activeLength > 0)
        {
          _activeLength--;
          _activeEdge = _position - _remainder + 1;
        }
        else
        {
          _activeNode = _nodes[_activeNode].Link > 0
            ? _nodes[_activeNode].Link
            : _root;
        }
      }
    }

    /// <summary>
    ///   Returns lexographically greatest suffix
    /// </summary>
    public string GetLexGreatestSubstring()
    {
      var current = _nodes[1];
      var sb = new StringBuilder();

      while (current.Next.Count > 0)
      {
        var index = current.Next[current.Next.Keys[current.Next.Keys.Count - 1]];
        current = _nodes[index];

        var start = current.Start;
        var end = current.End;

        for (var i = start; i < Math.Min(end, _text.Length); i++)
          sb.Append(_text[i]);
      }

      return sb.ToString();
    }
  }
}
