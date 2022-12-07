using System.Collections.Concurrent;

namespace MathProblem.API.Repositories;

class MemoryRepository<T> : MemoryRepository<T, T>
{
    public MemoryRepository() : base((o) => o, (o) => o)
    {
        ;
    }
}

class MemoryRepository<T, TRep> : IRepository<int, T>
{ 
    protected readonly ConcurrentDictionary<int, TRep> _repo = new();
    protected readonly ConcurrentQueue<int> _orderedKeys = new();
    private readonly object _injectLock = new();
    private readonly Func<T, TRep> _toStorageType;
    private readonly Func<TRep, T> _toViewType;

    public MemoryRepository(Func<T, TRep> toStorageType, Func<TRep, T> toViewType)
    {
        _toStorageType = toStorageType;
        _toViewType = toViewType;
    }

    public int Add(T entity)
    {
        var hash = entity!.GetHashCode();
        lock (_injectLock)
        {
            if (!_orderedKeys.Contains(hash))
            {
                _orderedKeys.Enqueue(hash);
                _repo.GetOrAdd(hash, (hash) => _toStorageType(entity));
            }
        }
        return hash;
    }

    public IDictionary<int, T> GetAll()
    {
        lock (_injectLock)
        {
            return _orderedKeys.ToList().ToDictionary(k => k, k => _toViewType(_repo[k]));
        }
    }

    public bool TryGetById(int id, out T? entity)
    {
        if (_repo.TryGetValue(id, out var obj))
        {
            entity = _toViewType(obj);
            return true;
        }
        entity = default(T);
        return false;
    }
}
