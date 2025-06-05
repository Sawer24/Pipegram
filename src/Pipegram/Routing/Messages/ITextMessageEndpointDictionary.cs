using System.Diagnostics.CodeAnalysis;

namespace Pipegram.Routing.Messages;

public interface ITextMessageEndpointDictionary
{
    void Add(string[] keys, IEndpoint value);
    void Remove(string[] keys);
    bool TryGetValue(string[] keys, [MaybeNullWhen(false)] out IEndpoint endpoint,
        [MaybeNullWhen(false)] out string[] remainingKeys);
}
