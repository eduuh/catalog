
using catalog.Dto.Geospatial;
using catalog.Entities;

namespace catalog.Repositories;

public interface IPolygonRepository
{
    Task<Polygon> GetPolygonAsync(Guid id);
    Task<IEnumerable<Polygon>> GetPolygonAsync();
    Task CreatePolygonAsync(Polygon polygon);
    Task UpdatePolygonAsync(Polygon Polygon);
    Task DeletePolygonAsync(Guid id);
}
