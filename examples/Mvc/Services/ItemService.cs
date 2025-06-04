using Mvc.Models;

namespace Mvc.Services;

public class ItemService : IItemService
{
    private readonly List<Item> _items = [];

    public async Task<IReadOnlyList<Item>> GetItems()
    {
        await Task.Delay(10);
        return _items;
    }

    public async Task<Item?> GetItem(int id)
    {
        await Task.Delay(10);
        return _items.FirstOrDefault(i => i.Id == id);
    }

    public async Task<Item> AddItem(string name, string? description)
    {
        await Task.Delay(10);
        var item = new Item
        {
            Id = _items.Count > 0 ? _items.Max(i => i.Id) + 1 : 1,
            Name = name,
            Description = description
        };
        _items.Add(item);
        return item;
    }

    public async Task<Item> UpdateItem(int id, string name, string? description)
    {
        await Task.Delay(10);
        var item = _items.First(i => i.Id == id);
        item.Name = name;
        item.Description = description;
        return item;
    }

    public async Task RemoveItem(int id)
    {
        await Task.Delay(10);
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item != null)
            _items.Remove(item);
    }
}
