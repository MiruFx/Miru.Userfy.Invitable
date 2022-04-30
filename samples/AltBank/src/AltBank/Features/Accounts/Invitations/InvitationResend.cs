using Miru.Userfy.Invitable;

namespace AltBank.Features.Accounts.Invitations;

public class InvitationResend
{
    public class Command : IRequest<Result>
    {
        public long UserId { get; set; }
    }

    public class Result
    {
        public long UserId { get; set; }
    }
    
    public class Handler : 
        IRequestHandler<Command, Result>
    {
        private readonly AppDbContext _db;
        private readonly UserInvitation<User> _userInvitation;

        public Handler(
            AppDbContext db, 
            UserInvitation<User> userInvitation)
        {
            _db = db;
            _userInvitation = userInvitation;
        }
          
        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            var user = await _db.Users.ByIdOrFailAsync(request.UserId, ct);

            await _userInvitation.ResendInvitationAsync(user, ct);
            
            return new Result() { UserId = user.Id };
        }
    }

    public class AccountsInvitationsController : MiruController
    {
        [HttpPost("/Accounts/Invitations/Register")]
        public async Task<Result> Resend(Command command) => await SendAsync(command);
    }
}
