
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
    public IEnumerable<Item> GetItems()
    {
        return items;
    }

    public Item GetItem(Guid id)
    {
        return items.Where(item => item.Id == id).SingleOrDefault();
    }

    public void CreateItem(Item item)
    {
        items.Add(item);
    }

    public void UpdateItem(Item item)
    {
        var index = items.FindIndex(existingitem => existingitem.Id == item.Id);
	items[index] = item;
    }
}
