
using catalog.Entities;

namespace catalog.Repositories;

public interface IInMemItemsRepository
{
    Item GetItem(Guid id);
    IEnumerable<Item> GetItems();
    void CreateItem(Item item);
    void UpdateItem(Item item);
}
