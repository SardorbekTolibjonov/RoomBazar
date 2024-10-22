namespace RoomBazar.Service.Exceptions;

public class RoomBazarException : Exception
{
    public int statusCode ;

    public RoomBazarException(int code,string message) : base(message)
    {
        this.statusCode = code;
    }
}
