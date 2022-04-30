using Miru.Scopables;
using Miru.Userfy;

namespace AltBank.Features;

public class CurrentAttributes : ICurrentAttributes
{
    private bool _initialized;
    
    private readonly Current _current;
    private readonly AppDbContext _db;
    private readonly ICurrentUser _currentUser;

    public CurrentAttributes(
        Current current,
        AppDbContext db, 
        ICurrentUser currentUser)
    {
        _current = current;
        _db = db;
        _currentUser = currentUser;
    }

    public async Task BeforeAsync<TRequest>(TRequest request, CancellationToken ct)
    {
        if (_initialized == false)
        {
            _current.IsAuthenticated = _currentUser.IsAuthenticated;
        
            if (_currentUser.IsAuthenticated)
            {
                _current.User = await _db.Users
                    .AsNoTracking()
                    .ByIdAsync(_currentUser.Id, ct);
            }
        
            _initialized = true;
        }
    }
}