
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
    public IEnumerable<Item> GetItemsAsync()
    {
        return items;
    }

    public Item GetItemAsync(Guid id)
    {
        return items.Where(item => item.Id == id).SingleOrDefault();
    }

    public void CreateItemAsync(Item item)
    {
        items.Add(item);
    }

    public void UpdateItemAsync(Item item)
    {
        var index = items.FindIndex(existingitem => existingitem.Id == item.Id);
	items[index] = item;
    }

    public void DeleteItemAsync(Guid id)
    {
        var index = items.FindIndex(existingitem => existingitem.Id == id);
	items.RemoveAt(index);
    }
}
