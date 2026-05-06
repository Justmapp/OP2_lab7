namespace DoublyLinkedListLib;

/// <summary>
/// Represents a single node in a doubly linked list.
/// Each node stores a data value and references to the previous and next nodes.
/// </summary>
/// <typeparam name="T">The type of data stored in the node.</typeparam>
public class Node<T>
{
    /// <summary>
    /// Gets or sets the data value stored in this node.
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// Gets or sets the reference to the next node in the list.
    /// <c>null</c> if this node is the tail.
    /// </summary>
    public Node<T>? Next { get; set; }

    /// <summary>
    /// Gets or sets the reference to the previous node in the list.
    /// <c>null</c> if this node is the head.
    /// </summary>
    public Node<T>? Previous { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="Node{T}"/> with the specified data value.
    /// </summary>
    /// <param name="data">The data value to store in the node.</param>
    public Node(T data)
    {
        Data = data;
        Next = null;
        Previous = null;
    }
}
