using catalog.Dtos;
using catalog.Entities;
using catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace catalog.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{

    // GET /items
    private readonly IInMemItemsRepository _repository;

    public ItemsController(IInMemItemsRepository repository)
    {
        _repository = repository;
    }


    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetItemsAsync()
    {
        return (await _repository.GetItemsAsync())
		.Select(item => item.AsDto());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
    {
        var item = await _repository.GetItemAsync(id);
        if (item is null) return NotFound();
        return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItem(CreateItemDto itemDto)
    {
        Item item = new()
        {
            Id = Guid.NewGuid(),
            Name = itemDto.Name,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };
        await _repository.CreateItemAsync(item);

        return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateItem(Guid id, UpdateItemDto itemDto)
    {
        var exstingitem = await _repository.GetItemAsync(id);

        if (exstingitem is null) return NotFound();

        Item updatedItem = exstingitem with { Name = itemDto.Name, Price = itemDto.Price };

        await _repository.UpdateItemAsync(updatedItem);

        return NoContent();

    }
     // delete /items/
    [HttpDelete("{id}")]
      public async  Task<ActionResult> DeleteItem(Guid id){

          var existingItem = _repository.GetItemAsync(id);
	  if(existingItem is null) return NotFound();
	  await _repository.DeleteItemAsync(id);
	  return NoContent();
      }

}

