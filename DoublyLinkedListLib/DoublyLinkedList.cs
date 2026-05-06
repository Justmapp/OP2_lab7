using System.Collections;

namespace DoublyLinkedListLib;

public class DoublyLinkedList : IEnumerable<int>
{
    private Node? _head;
    private Node? _tail;
    private int _count;

    public int Count => _count;
    public bool IsEmpty => _count == 0;

    public void AddToEnd(int value)
    {
        var newNode = new Node(value);

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

    public int this[int index]
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

    public void RemoveAt(int index)
    {
        ValidateIndex(index);

        var current = _head;
        for (int i = 0; i < index; i++)
            current = current!.Next;

        if (current!.Previous is not null)
            current.Previous.Next = current.Next;
        else
            _head = current.Next;

        if (current.Next is not null)
            current.Next.Previous = current.Previous;
        else
            _tail = current.Previous;

        _count--;
    }

    public int FindFirstOccurrence(int value)
    {
        var current = _head;
        int index = 0;

        while (current is not null)
        {
            if (current.Data == value)
                return index;

            current = current.Next;
            index++;
        }

        return -1;
    }

    public double SumAtOddPositions()
    {
        double sum = 0;
        var current = _head;
        int position = 1;

        while (current is not null)
        {
            if (position % 2 != 0)
                sum += current.Data;

            current = current.Next;
            position++;
        }

        return sum;
    }

    public DoublyLinkedList GetListGreaterThan(int threshold)
    {
        var result = new DoublyLinkedList();
        var current = _head;

        while (current is not null)
        {
            if (current.Data > threshold)
                result.AddToEnd(current.Data);

            current = current.Next;
        }

        return result;
    }

    public void RemoveGreaterThanAverage()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Cannot compute average of an empty list.");

        double average = ComputeAverage();

        var indicesToRemove = new List<int>();
        var current = _head;
        int index = 0;

        while (current is not null)
        {
            if (current.Data > average)
                indicesToRemove.Add(index);

            current = current.Next;
            index++;
        }

        for (int i = indicesToRemove.Count - 1; i >= 0; i--)
            RemoveAt(indicesToRemove[i]);
    }

    public IEnumerator<int> GetEnumerator()
    {
        var current = _head;
        while (current is not null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString()
    {
        if (IsEmpty)
            return "[empty]";

        return "[" + string.Join(" <-> ", this) + "]";
    }

    private double ComputeAverage()
    {
        double sum = 0;
        foreach (int item in this)
            sum += item;

        return sum / _count;
    }

    private void ValidateIndex(int index)
    {
        if (index < 0 || index >= _count)
            throw new ArgumentOutOfRangeException(
                nameof(index),
                $"Index {index} is out of range. Valid range: 0 to {_count - 1}.");
    }
}
