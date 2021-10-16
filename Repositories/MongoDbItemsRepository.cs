
using catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace catalog.Repositories;

public class MongoDbItemsRepository : IInMemItemsRepository
{
   private const string databaseName = "catalog";
   private const string collectionName = "items";
   private readonly IMongoCollection<Item> itemsCollection;

   private readonly FilterDefinitionBuilder<Item> filterbuilder = Builders<Item>.Filter;

   public MongoDbItemsRepository(IMongoClient mongoClient)
   {
	IMongoDatabase database = mongoClient.GetDatabase(databaseName);
        itemsCollection  = database.GetCollection<Item>(collectionName);
   }
    public void CreateItemAsync(Item item)
    {
	itemsCollection.InsertOne(item);
    }

    public void DeleteItemAsync(Guid id)
    {
        var filter = filterbuilder.Eq(item => item.Id ,id);
	itemsCollection.DeleteOne(filter);
    }

    public Item GetItemAsync(Guid id)
    {
        var filter = filterbuilder.Eq(item => item.Id ,id);
	return itemsCollection.Find(filter).SingleOrDefault();
    }

    public IEnumerable<Item> GetItemsAsync()
    {
       return itemsCollection.Find(new BsonDocument()).ToList();
    }

    public void UpdateItem(Item item)
    {
        var filter = filterbuilder.Eq(existingitem => existingitem.Id, item.Id);
	itemsCollection.ReplaceOne(filter, item);
    }
}
