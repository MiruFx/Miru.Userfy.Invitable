using Miru.Userfy.Invitable;

namespace AltBank.Features.Accounts.Invitations;

public class InvitationNew
{
    public class Query : IRequest<Command>
    {
    }
        
    public class Command : IRequest<Result>
    {
        public string Email { get; set; }
    }

    public class Result
    {
        public string Email { get; set; }
    }

    public class Handler : 
        IRequestHandler<Query, Command>, 
        IRequestHandler<Command, Result>
    {
        private readonly UserInvitation<User> _userInvitation;

        public Handler(UserInvitation<User> userInvitation)
        {
            _userInvitation = userInvitation;
        }
            
        public async Task<Command> Handle(Query request, CancellationToken ct)
        {
            await Task.CompletedTask;

            return new Command();
        }
            
        public async Task<Result> Handle(Command request, CancellationToken ct)
        {
            var user = new User
            {
                Email = request.Email,
                UserName = request.Email
            };

            await _userInvitation.AddAndInviteAsync(user, ct);
            
            return new Result { Email = request.Email };
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Email).NotEmpty().EmailAddress();
        }
    }
        
    public class AccountsInvitationsController : MiruController
    {
        [HttpGet("/Accounts/Invitations/New")]
        public async Task<Command> New(Query query) => await SendAsync(query);

        [HttpPost("/Accounts/Invitations/New")]
        public async Task<Result> New(Command command) => await SendAsync(command);
    }
}