namespace vzh.NetHelpers
{
  /// <summary>
  ///   Red-black tree node extensions
  /// </summary>
  internal static class NodeExtensions
  {
    public static bool IsBlack<TKey, TValue>(this Node<TKey, TValue> node) 
      => node == null || node.Color == Color.Black;

    public static bool IsRed<TKey, TValue>(this Node<TKey, TValue> node) 
      => node == null || node.Color == Color.Red;

    public static void SetBlack<TKey, TValue>(this Node<TKey, TValue> node)
    {
      if (node != null)
        node.Color = Color.Black;
    }

    public static void SetRed<TKey, TValue>(this Node<TKey, TValue> node)
    {
      if (node != null)
        node.Color = Color.Red;
    }
  }
}
