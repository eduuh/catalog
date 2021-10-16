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
    public IEnumerable<ItemDto> GetItems()
    {
        return _repository.GetItems().Select(item => item.AsDto());
    }

    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetItem(Guid id)
    {
        var item = _repository.GetItem(id);
        if (item is null) return NotFound();
        return item.AsDto();
    }

    [HttpPost]
    public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
    {
        Item item = new()
        {
            Id = Guid.NewGuid(),
            Name = itemDto.Name,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };
        _repository.CreateItem(item);

        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());
    }

    [HttpPut("{id}")]
    public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
    {
        var exstingitem = _repository.GetItem(id);

        if (exstingitem is null) return NotFound();

        Item updatedItem = exstingitem with { Name = itemDto.Name, Price = itemDto.Price };

        _repository.UpdateItem(updatedItem);

        return NoContent();

    }
     // delete /items/
    [HttpDelete("{id}")]
      public ActionResult DeleteItem(Guid id){

          var existingItem = _repository.GetItem(id);
	  if(existingItem is null) return NotFound();

	  _repository.DeleteItem(id);

	  return NoContent();
      }

}

