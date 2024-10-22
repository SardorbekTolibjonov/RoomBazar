using RoomBazar.Domain.Entites.Commons;

namespace RoomBazar.Domain.Entites.Auths;

public class Register : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
