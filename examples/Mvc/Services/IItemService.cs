
using Mvc.Models;

namespace Mvc.Services;

public interface IItemService
{
    Task<IReadOnlyList<Item>> GetItems();
    Task<Item?> GetItem(int id);
    Task<Item> AddItem(string name, string? description);
    Task RemoveItem(int id);
    Task<Item> UpdateItem(int id, string name, string? description);
}