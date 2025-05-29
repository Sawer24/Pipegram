using System.Collections.Concurrent;

namespace Pipegram.Routing;

public class EndpointMetadataCollection
{
    public static EndpointMetadataCollection Empty { get; } = new EndpointMetadataCollection([]);

    private readonly object[] _items;
    private readonly ConcurrentDictionary<Type, object[]> _cache;

    public IReadOnlyCollection<object> Items => _items;

    /// <summary>
    /// Creates a new <see cref="EndpointMetadataCollection"/>.
    /// </summary>
    /// <param name="items">The metadata items.</param>
    public EndpointMetadataCollection(IEnumerable<object> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _items = [.. items];
        _cache = new ConcurrentDictionary<Type, object[]>();
    }

    /// <summary>
    /// Creates a new <see cref="EndpointMetadataCollection"/>.
    /// </summary>
    /// <param name="items">The metadata items.</param>
    public EndpointMetadataCollection(params object[] items)
        : this((IEnumerable<object>)items)
    {
    }

    /// <summary>
    /// Gets the most significant metadata item of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of metadata to retrieve.</typeparam>
    /// <returns>
    /// The most significant metadata of type <typeparamref name="T"/> or <c>null</c>.
    /// </returns>
    public T? GetMetadata<T>() where T : class
    {
        if (_cache.TryGetValue(typeof(T), out var obj))
        {
            var result = (T[])obj;
            var length = result.Length;
            return length > 0 ? result[length - 1] : default;
        }
        return GetMetadataSlow<T>();
    }

    private T? GetMetadataSlow<T>() where T : class
    {
        var result = GetOrderedMetadataSlow<T>();
        var length = result.Length;
        return length > 0 ? result[length - 1] : default;
    }

    /// <summary>
    /// Gets the metadata items of type <typeparamref name="T"/> in ascending
    /// order of precedence.
    /// </summary>
    /// <typeparam name="T">The type of metadata.</typeparam>
    /// <returns>A sequence of metadata items of <typeparamref name="T"/>.</returns>
    public IReadOnlyList<T> GetOrderedMetadata<T>() where T : class
    {
        if (_cache.TryGetValue(typeof(T), out var result))
            return (T[])result;
        return GetOrderedMetadataSlow<T>();
    }

    private T[] GetOrderedMetadataSlow<T>() where T : class
    {
        var matches = default(List<T>?);
        var items = _items;
        for (var i = 0; i < items.Length; i++)
            if (items[i] is T item)
                (matches ??= []).Add(item);
        var results = matches == null ? [] : matches.ToArray();
        _cache.TryAdd(typeof(T), results);
        return results;
    }

    /// <summary>
    /// Gets the most significant metadata item of type <typeparamref name="T"/>.
    /// Throws an <see cref="InvalidOperationException"/> if the metadata is not found.
    /// </summary>
    /// <typeparam name="T">The type of metadata to retrieve.</typeparam>
    /// <returns>
    /// The most significant metadata of type <typeparamref name="T"/>.
    /// </returns>
    public T GetRequiredMetadata<T>() where T : class
        => GetMetadata<T>() ?? throw new InvalidOperationException($"Metadata '{typeof(T)}' is not found.");
}
