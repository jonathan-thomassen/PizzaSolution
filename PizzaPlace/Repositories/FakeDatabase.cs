using PizzaPlace.Models;

namespace PizzaPlace.Repositories;

public abstract class FakeDatabase<T> where T : Dto
{
    private static readonly ConcurrentDictionary<long, T> _dictionary = new();
    private static long _currentId = 1;
    private readonly static object _idLock = new();

    protected long Insert(T item)
    {
        var id = GetNextId();
        _dictionary[id] = item with { Id = id };

        return id;
    }

    protected T? Get(long id) =>
        _dictionary.TryGetValue(id, out var value) ? value : null;

    protected IEnumerable<T> Get(Func<T, bool> pickItems) =>
        _dictionary.Values.Where(pickItems);

    protected void Update(T item, long id)
    {
        if (_dictionary.ContainsKey(id))
        {
            _dictionary[id] = item with { Id = id };
        }
        else
        {
            throw new PizzaException($"Item with id {id} does not exists - unable to update.");
        }
    }

    protected void Delete(long id)
    {
        _dictionary.TryRemove(id, out var _);
    }

    private static long GetNextId()
    {
        long id;
        lock (_idLock)
        {
            id = _currentId++;
        }

        return id;
    }
}
