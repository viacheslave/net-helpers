using System;

namespace vzh.NetHelpers
{
  /// <summary>
  ///   Red-black tree node
  /// </summary>
  /// <typeparam name="TKey">Key type</typeparam>
  /// <typeparam name="TValue">Value type</typeparam>
  public class Node<TKey, TValue>
  {
    internal NodeData<TKey, TValue> Data { get; set; }

    /// <summary>
    ///   Node Key
    /// </summary>
    public TKey Key => Data.Key;
    
    /// <summary>
    ///   Node Value
    /// </summary>
    public TValue Value => Data.Value;

    /// <summary>
    ///   Node's left child node
    /// </summary>
    public Node<TKey, TValue> Left { get; internal set; }

    /// <summary>
    ///   Node's right child node
    /// </summary>
    public Node<TKey, TValue> Right { get; internal set; }

    /// <summary>
    ///   Node's parent node
    /// </summary>
    public Node<TKey, TValue> Parent { get; internal set; }

    /// <summary>
    ///   Node's color
    /// </summary>
    public Color Color { get; internal set; }

    public Node(TKey key, TValue value)
    {
      if (key == null)
        throw new ArgumentException(nameof(key));

      Data = new NodeData<TKey, TValue>(key, value);
    }

    /// <summary>
    ///   Node's sibling node.
    /// </summary>
    public Node<TKey, TValue> Sibling => 
      this == Parent.Left ? Parent.Right : Parent.Left;

    /// <summary>
    ///   Node's uncle node.
    ///   Other child of a grandparent.
    /// </summary>
    public Node<TKey, TValue> Uncle
    {
      get
      {
        var grandparent = GrandParent;

        return grandparent == null
            ? null
            : (Parent == grandparent.Left
                ? grandparent.Right
                : grandparent.Left);
      }
    }

    /// <summary>
    ///   Node's grandparent node.
    /// </summary>
    public Node<TKey, TValue> GrandParent 
      => Parent?.Parent;

    /// <summary>
    ///   Returns node's subtree nodes count.
    /// </summary>
    /// <return>Number of subnodes including self</return>
    public int GetVolume()
    {
      var left = Left?.GetVolume() ?? default;
      var right = Right?.GetVolume() ?? default;

      return left + right + 1;
    }

    internal void SetData(TKey key, TValue value) 
      => Data = new NodeData<TKey, TValue>(key, value);

    public override string ToString()
    {
      var leftKey = Left != null ? $"{Left.Key} <--" : string.Empty;
      var rightKey = Right != null ? $"--> {Right.Key}" : string.Empty;

      return $"{leftKey} [{Key} {Color}] {rightKey}";
    }

    internal readonly struct NodeData<TKey, TValue>
    {
      public readonly TKey Key;
      public readonly TValue Value;

      public NodeData(TKey key, TValue value)
      {
        Key = key;
        Value = value;
      }
    }
  }
}
