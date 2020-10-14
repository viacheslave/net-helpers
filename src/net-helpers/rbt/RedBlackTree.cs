using System;
using System.Collections.Generic;

namespace vzh.NetHelpers
{
  /// <summary>
  ///   Red-black tree implementation
  /// </summary>
  /// <typeparam name="TKey">Key type</typeparam>
  /// <typeparam name="TValue">Value type</typeparam>
  public class RedBlackTree<TKey, TValue> where TKey : IComparable<TKey>
  {
    private Node<TKey, TValue> _root;

    public RedBlackTree()
    {
    }

    public RedBlackTree(IDictionary<TKey, TValue> entries) : this()
    {
      if (entries == null)
        throw new ArgumentNullException(nameof(entries));

      foreach (var entry in entries)
      {
        Insert(entry.Key, entry.Value);
      }
    }

    /// <summary>
    ///   Root node
    /// </summary>
    public Node<TKey, TValue> Root => _root;

    /// <summary>
    ///   Add a key/value pair
    /// </summary>
    /// <param name="key">Key</param>
    /// <param name="value">Value</param>
    public void Insert(TKey key, TValue value)
    {
      var node = new Node<TKey, TValue>(key, value);

      node = InsertAsChildOf(_root, node);

      if (_root == null)
      {
        _root = node;
        if (node != null)
          _root.Color = Color.Black;
      }

      InsertionFix(node);
    }

    /// <summary>
    ///   Deletes an entry by key
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>True if key was deleted</returns>
    public bool Delete(TKey key)
    {
      var node = GetNode(key);

      if (node != null)
      {
        if (node.Left != null && node.Right != null)
          node = CopyMaxPredecessor(node);

        var child = node.Right == null
          ? node.Left
          : node.Right;

        if (node.Color == Color.Black)
        {
          if (!child.IsBlack())
            node.Color = Color.Red;

          DeletionFix(node);
        }

        ReplaceInRotation(node, child);
      }

      return node != null;
    }

    private void DeletionFix(Node<TKey, TValue> node)
    {
      if (node.Parent != null)
      {
        var sibling = node.Sibling;

        if (sibling.IsRed())
        {
          node.Parent.SetRed();

          sibling.SetBlack();

          if (node == node.Parent.Left)
            LeftRotate(node.Parent);
          else
            RightRotate(node.Parent);
        }

        sibling = node.Sibling;

        if (node.Parent.IsBlack() &&
            sibling != null &&
            sibling.IsBlack() &&
            sibling.Left.IsBlack() &&
            sibling.Right.IsBlack())
        {
          sibling.SetRed();

          DeletionFix(node.Parent);
        }
        else
        {
          sibling = node.Sibling;

          if (node.Parent.IsRed() &&
              sibling != null &&
              sibling.IsBlack() &&
              sibling.Left.IsBlack() &&
              sibling.Right.IsBlack())
          {
            sibling.SetRed();

            node.Parent.SetBlack();
          }
          else
          {
            sibling = node.Sibling;

            if (node == node.Parent.Left &&
                sibling != null &&
                sibling.IsBlack() &&
                sibling.Left.IsRed() &&
                sibling.Right.IsBlack())
            {
              sibling.SetRed();

              if (sibling.Left != null)
                sibling.Left.SetBlack();

              RightRotate(sibling);
            }
            else if (
              node == node.Parent.Right &&
              sibling != null &&
              sibling.IsBlack() &&
              sibling.Left.IsBlack() &&
              sibling.Right.IsRed())
            {
              sibling.SetRed();

              if (sibling.Right != null)
                sibling.Right.SetBlack();

              LeftRotate(sibling);
            }

            sibling = node.Sibling;

            if (sibling != null && node.Parent != null)
            {
              sibling.Color = node.Parent.Color;
            }

            node.Parent.SetBlack();

            if (node == node.Parent.Left)
            {
              sibling.Right.SetBlack();
              LeftRotate(node.Parent);
            }
            else
            {
              sibling.Left.SetBlack();
              RightRotate(node.Parent);
            }
          }
        }
      }
    }

    private Node<TKey, TValue> CopyMaxPredecessor(Node<TKey, TValue> node)
    {
      var predecessor = GetMaxPredecessorOf(node);

      node.SetData(predecessor.Key, predecessor.Value);

      return predecessor;
    }

    private Node<TKey, TValue> GetMaxPredecessorOf(Node<TKey, TValue> node)
    {
      node = node.Left;

      while (node.Right != null)
        node = node.Right;

      return node;
    }

    private void InsertionFix(Node<TKey, TValue> node)
    {
      if (node.Parent == null)
      {
        node.Color = Color.Black;
        return;
      }

      if (node.Parent.Color == Color.Red)
      {
        Node<TKey, TValue> uncle = node.Uncle;

        if (uncle != null && uncle.Color == Color.Red)
        {
          node.Parent.Color = Color.Black;
          uncle.Color = Color.Black;

          var gp = node.GrandParent;
          gp.Color = Color.Red;

          InsertionFix(gp);
        }
        else
        {
          var gp = node.GrandParent;
          if (node == node.Parent.Right && node.Parent == gp.Left)
          {
            LeftRotate(node.Parent);
            node = node.Left;
          }
          else if (node == node.Parent.Left && node.Parent == gp.Right)
          {
            RightRotate(node.Parent);
            node = node.Right;
          }

          gp = node.GrandParent;
          node.Parent.Color = Color.Black;

          gp.Color = Color.Red;
          if (node == node.Parent.Left && node.Parent == gp.Left)
          {
            RightRotate(gp);
          }
          else if (node == node.Parent.Right && node.Parent == gp.Right)
          {
            LeftRotate(gp);
          }
        }
      }
    }

    private Node<TKey, TValue> InsertAsChildOf(Node<TKey, TValue> parent, Node<TKey, TValue> node)
    {
      if (parent == null)
      {
        node.Color = Color.Black;
        return node;
      }

      if (node.Key.CompareTo(parent.Key) < 0)
      {
        if (parent.Left == null)
        {
          node.Parent = parent;
          parent.Left = node;
        }
        else
        {
          node = InsertAsChildOf(parent.Left, node);
        }
      }
      else
      {
        if (parent.Right == null)
        {
          node.Parent = parent;
          parent.Right = node;
        }
        else
        {
          node = InsertAsChildOf(parent.Right, node);
        }
      }

      return node;
    }

    private void LeftRotate(Node<TKey, TValue> node)
    {
      if (node != null)
      {
        var right = node.Right;

        ReplaceInRotation(node, right);

        node.Right = right?.Left;

        if (right != null)
        {
          if (right.Left != null)
          {
            right.Left.Parent = node;
          }
          right.Left = node;
        }

        node.Parent = right;
      }
    }

    private void RightRotate(Node<TKey, TValue> node)
    {
      if (node != null)
      {
        var left = node.Left;

        ReplaceInRotation(node, left);

        node.Left = left?.Right;

        if (left != null)
        {
          if (left.Right != null)
          {
            left.Right.Parent = node;
          }
          left.Right = node;
        }

        node.Parent = left;
      }
    }

    private void ReplaceInRotation(Node<TKey, TValue> node, Node<TKey, TValue> replacement)
    {
      if (node.Parent == null)
      {
        _root = replacement;
        if (replacement != null)
          _root.Color = Color.Black;
      }
      else
      {
        if (node == node.Parent.Left)
          node.Parent.Left = replacement;
        else
          node.Parent.Right = replacement;
      }

      if (replacement != null)
        replacement.Parent = node.Parent;
    }

    private Node<TKey, TValue> GetNode(TKey key)
    {
      var curNode = _root;

      while (curNode != null)
      {
        if (curNode.Key.Equals(key))
        {
          return curNode;
        }

        curNode = key.CompareTo(curNode.Key) < 0
          ? curNode.Left
          : curNode.Right;
      }

      return null;
    }
  }
}
