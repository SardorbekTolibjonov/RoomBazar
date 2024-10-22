using RoomBazar.Domain.Entites.Commons;

namespace RoomBazar.Domain.Entites.Auths;

public class RefreshToken : Auditable
{
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }

    public int RegisterId { get; set; }
    public Register Register { get; set; }
}
