using System.Diagnostics.CodeAnalysis;

namespace Pipegram.Routing.Messages;

public class TextMessageEndpointDictionary : ITextMessageEndpointDictionary
{
    private struct TrieNode()
    {
        public readonly Dictionary<string, TrieNode> Children { get; } = [];
        public IEndpoint? Endpoint { get; set; }
    }

    private readonly TrieNode _root = new();

    public void Add(string[] keys, IEndpoint value)
    {
        var children = _root.Children;
        for (var i = 0; i < keys.Length; i++)
        {
            var key = keys[i];
            if (!children.TryGetValue(key, out var next))
            {
                next = new TrieNode();
                if (i == keys.Length - 1)
                    next.Endpoint = value;
                children[key] = next;
            }
            children = next.Children;
        }
    }

    public bool TryGetValue(string[] keys, [MaybeNullWhen(false)] out IEndpoint endpoint, [MaybeNullWhen(false)] out string[] remainingKeys)
    {
        var node = _root;
        var lastMatched = default(IEndpoint);
        var depth = 0;

        for (var i = 0;; i++)
        {
            if (node.Endpoint != null)
            {
                lastMatched = node.Endpoint;
                depth = i;
            }
            if (i == keys.Length || !node.Children.TryGetValue(keys[i], out node))
                break;
        }

        endpoint = lastMatched;
        remainingKeys = lastMatched != null ? keys[depth..] : null;
        return lastMatched != null;
    }

    public void Remove(string[] keys) => Remove(_root, keys, 0);

    private static bool Remove(TrieNode node, string[] keys, int index)
    {
        if (index == keys.Length)
        {
            if (node.Endpoint == null)
                return false;
            node.Endpoint = null;
            return node.Children.Count == 0;
        }

        var key = keys[index];
        if (!node.Children.TryGetValue(key, out var child))
            return false;

        if (Remove(child, keys, index + 1))
        {
            node.Children.Remove(key);
            return node.Children.Count == 0 && node.Endpoint == null;
        }
        return false;
    }
}