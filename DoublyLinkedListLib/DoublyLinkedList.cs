using System.Collections;

namespace DoublyLinkedListLib;

/// <summary>
/// Represents a generic doubly linked list where elements are added to the end.
/// Supports enumeration via <c>foreach</c>, index-based read access, and removal by index.
/// </summary>
/// <typeparam name="T">
/// The type of elements stored in the list.
/// Must implement <see cref="IComparable{T}"/> for comparison-based operations.
/// </typeparam>
public class DoublyLinkedList<T> : IEnumerable<T> where T : IComparable<T>
{
    /// <summary>Reference to the first node (head) of the list.</summary>
    private Node<T>? _head;

    /// <summary>Reference to the last node (tail) of the list.</summary>
    private Node<T>? _tail;

    /// <summary>The number of elements currently in the list.</summary>
    private int _count;

    /// <summary>
    /// Gets the number of elements currently stored in the list.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// Gets a value indicating whether the list is empty.
    /// </summary>
    public bool IsEmpty => _count == 0;

    // ─────────────────────────────────────────────────────────────────────────
    // Core operations
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Adds a new element with the specified value to the end of the list.
    /// </summary>
    /// <param name="value">The value to add to the list.</param>
    public void AddToEnd(T value)
    {
        var newNode = new Node<T>(value);

        if (_head is null)
        {
            _head = newNode;
            _tail = newNode;
        }
        else
        {
            newNode.Previous = _tail;
            _tail!.Next = newNode;
            _tail = newNode;
        }

        _count++;
    }

    /// <summary>
    /// Gets the element at the specified zero-based index (counting from the head).
    /// </summary>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="index"/> is negative or greater than or equal to <see cref="Count"/>.
    /// </exception>
    public T this[int index]
    {
        get
        {
            ValidateIndex(index);

            var current = _head;
            for (int i = 0; i < index; i++)
                current = current!.Next;

            return current!.Data;
        }
    }

    /// <summary>
    /// Removes the element at the specified zero-based index (counting from the head).
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="index"/> is negative or greater than or equal to <see cref="Count"/>.
    /// </exception>
    public void RemoveAt(int index)
    {
        ValidateIndex(index);

        var current = _head;
        for (int i = 0; i < index; i++)
            current = current!.Next;

        // Reconnect neighbours
        if (current!.Previous is not null)
            current.Previous.Next = current.Next;
        else
            _head = current.Next; // removed head

        if (current.Next is not null)
            current.Next.Previous = current.Previous;
        else
            _tail = current.Previous; // removed tail

        _count--;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Variant-specific operations
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Finds the zero-based index of the first occurrence of the specified value,
    /// searching from the head of the list.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>
    /// The zero-based index of the first occurrence if found; otherwise <c>-1</c>.
    /// </returns>
    public int FindFirstOccurrence(T value)
    {
        var current = _head;
        int index = 0;

        while (current is not null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Data, value))
                return index;

            current = current.Next;
            index++;
        }

        return -1;
    }

    /// <summary>
    /// Calculates the sum of elements located at odd positions in the list.
    /// Positions are counted with 1-based numbering starting from the head,
    /// so odd positions are 1, 3, 5, …
    /// </summary>
    /// <returns>
    /// The sum of elements at odd positions as a <see cref="double"/>.
    /// Returns <c>0</c> if the list is empty.
    /// </returns>
    public double SumAtOddPositions()
    {
        double sum = 0;
        var current = _head;
        int position = 1; // 1-based numbering

        while (current is not null)
        {
            if (position % 2 != 0) // odd position: 1, 3, 5, …
                sum += Convert.ToDouble(current.Data);

            current = current.Next;
            position++;
        }

        return sum;
    }

    /// <summary>
    /// Creates and returns a new <see cref="DoublyLinkedList{T}"/> containing only
    /// the elements from this list whose values are strictly greater than
    /// the specified threshold.
    /// </summary>
    /// <param name="threshold">The threshold value to compare each element against.</param>
    /// <returns>
    /// A new list with all elements greater than <paramref name="threshold"/>,
    /// preserving the original order.
    /// </returns>
    public DoublyLinkedList<T> GetListGreaterThan(T threshold)
    {
        var result = new DoublyLinkedList<T>();
        var current = _head;

        while (current is not null)
        {
            if (current.Data.CompareTo(threshold) > 0)
                result.AddToEnd(current.Data);

            current = current.Next;
        }

        return result;
    }

    /// <summary>
    /// Removes all elements from the list whose values are strictly greater than
    /// the arithmetic mean (average) of all elements in the list.
    /// The average is computed before any removal takes place.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the list is empty.
    /// </exception>
    public void RemoveGreaterThanAverage()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Cannot compute average of an empty list.");

        double average = ComputeAverage();

        // Collect zero-based indices of elements to remove
        var indicesToRemove = new List<int>();
        var current = _head;
        int index = 0;

        while (current is not null)
        {
            if (Convert.ToDouble(current.Data) > average)
                indicesToRemove.Add(index);

            current = current.Next;
            index++;
        }

        // Remove in reverse order so earlier indices stay valid
        for (int i = indicesToRemove.Count - 1; i >= 0; i--)
            RemoveAt(indicesToRemove[i]);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // IEnumerable<T> support
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns an enumerator that iterates through the list from head to tail.
    /// Enables use of the <c>foreach</c> statement.
    /// </summary>
    /// <returns>An enumerator over the elements of this list.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        var current = _head;
        while (current is not null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns a string representation of the list in the format
    /// <c>[a &lt;-&gt; b &lt;-&gt; c]</c>, or <c>[empty]</c> when the list has no elements.
    /// </summary>
    /// <returns>A human-readable string describing the list contents.</returns>
    public override string ToString()
    {
        if (IsEmpty)
            return "[empty]";

        return "[" + string.Join(" <-> ", this) + "]";
    }

    /// <summary>
    /// Computes the arithmetic mean of all elements in the list.
    /// </summary>
    /// <returns>The average value of the elements as a <see cref="double"/>.</returns>
    private double ComputeAverage()
    {
        double sum = 0;
        foreach (var item in this)
            sum += Convert.ToDouble(item);

        return sum / _count;
    }

    /// <summary>
    /// Validates that the given index is within the bounds of the list.
    /// </summary>
    /// <param name="index">The index to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the index is negative or greater than or equal to <see cref="Count"/>.
    /// </exception>
    private void ValidateIndex(int index)
    {
        if (index < 0 || index >= _count)
            throw new ArgumentOutOfRangeException(
                nameof(index),
                $"Index {index} is out of range. Valid range: 0 to {_count - 1}.");
    }
}
