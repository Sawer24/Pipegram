using System.Diagnostics.CodeAnalysis;

namespace Pipegram.Routing;

public interface IMessageEndpointDictionary
{
    void Add(string[] keys, IEndpoint value);
    void Remove(string[] keys);
    bool TryGetValue(string[] keys, [MaybeNullWhen(false)] out IEndpoint endpoint,
        [MaybeNullWhen(false)] out string[] remainingKeys);
}
