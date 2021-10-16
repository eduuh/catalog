
using catalog.Entities;

namespace catalog.Repositories;

public class InMemItemsRepository : IInMemItemsRepository
{
    private readonly List<Item> items = new()
    {
        new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 8, CreatedDate = DateTimeOffset.UtcNow },
        new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 10, CreatedDate = DateTimeOffset.UtcNow },
        new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 16, CreatedDate = DateTimeOffset.UtcNow },
        new Item { Id = Guid.NewGuid(), Name = "Knife", Price = 20, CreatedDate = DateTimeOffset.UtcNow },
    };
    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
        return await Task.FromResult(items);
    }

    public async Task<Item> GetItemAsync(Guid id)
    {
        var item =  items.Where(item => item.Id == id).SingleOrDefault();
	return await Task.FromResult(item);
    }

    public async Task CreateItemAsync(Item item)
    {
        items.Add(item);
	await Task.CompletedTask;
    }

    public async Task UpdateItemAsync(Item item)
    {
        var index = items.FindIndex(existingitem => existingitem.Id == item.Id);
	items[index] = item;
	await Task.CompletedTask;
    }

    public async Task DeleteItemAsync(Guid id)
    {
        var index = items.FindIndex(existingitem => existingitem.Id == id);
	items.RemoveAt(index);
	await Task.CompletedTask;
    }
}
