namespace PizzaPlace;

/// <summary>
/// Just a list with equals to match if all items match.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ComparableList<T> : IList<T>
{
    private readonly List<T> _list;

    public ComparableList()
    {
        _list = [];
    }

    public ComparableList(IEnumerable<T> values)
    {
        _list = values.ToList();
    }

    public override bool Equals(object? obj) =>
        obj is ComparableList<T> otherList &&
        Count == otherList.Count &&
        _list.Zip(otherList).All(x => x.First?.Equals(x.Second) ?? x.Second is null);

    public override int GetHashCode() =>
        _list.Aggregate(0, (hash, item) => HashCode.Combine(hash, item?.GetHashCode()));

    public override string ToString() =>
        JsonSerializer.Serialize(this);

    // IList implementation through _list

    public T this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }

    public int Count => _list.Count;

    public bool IsReadOnly => false;

    public void Add(T item) => _list.Add(item);

    public void Clear() => _list.Clear();

    public bool Contains(T item) => _list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) =>
        _list.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    public int IndexOf(T item) => _list.IndexOf(item);

    public void Insert(int index, T item) => _list.Insert(index, item);

    public bool Remove(T item) => _list.Remove(item);

    public void RemoveAt(int index) => _list.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();
}
