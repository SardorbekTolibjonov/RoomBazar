using RoomBazar.Domain.Entites.Commons;

namespace RoomBazar.Domain.Entites;

public class Product : Auditable
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
}
