namespace AltBank.Domain;

public class Current
{
    public User User { get; set; }
    
    public bool IsAuthenticated { get; set; }

    public bool IsAnonymous => IsAuthenticated == false;
}
