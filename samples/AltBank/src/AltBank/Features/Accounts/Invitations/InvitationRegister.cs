using Miru.Userfy;
using Miru.Userfy.Invitable;

namespace AltBank.Features.Accounts.Invitations;

public class InvitationRegister
{
    public class Query : IRequest<Command>
    {
        public string Token { get; set; }
    }
        
    public class Command : IRequest<FeatureResult>
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }

    public class Handler : 
        IRequestHandler<Query, Command>, 
        IRequestHandler<Command, FeatureResult>
    {
        private readonly AppDbContext _db;
        private readonly UserInvitation<User> _userInvitation;
        private readonly IUserLogin<User> _userLogin;
        private readonly UserfyOptions _userfyOptions;

        public Handler(
            AppDbContext db, 
            UserInvitation<User> userInvitation, 
            IUserLogin<User> userLogin, 
            UserfyOptions userfyOptions)
        {
            _db = db;
            _userInvitation = userInvitation;
            _userLogin = userLogin;
            _userfyOptions = userfyOptions;
        }
            
        public async Task<Command> Handle(Query request, CancellationToken ct)
        {
            var user = await _db.Users.ByInvitationTokenOrFailAsync(request.Token, ct);
            
            return new Command
            {
                Token = user.InvitationToken,
                Email = user.Email
            };
        }
            
        public async Task<FeatureResult> Handle(Command request, CancellationToken ct)
        {
            var user = await _db.Users.ByInvitationTokenOrFailAsync(request.Token, ct);

            user.AcceptInvitation();

            await _userInvitation.SetPasswordAsync(user, request.Password, ct);

            await _userLogin.LoginAsync(user.Email, request.Password);

            return new FeatureResult(_userfyOptions.AfterLoginFeature);
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(m => m.Token).NotEmpty();
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .Equal(x => x.PasswordConfirmation)
                .WithMessage("The passwords don't match");
            
            RuleFor(m => m.PasswordConfirmation).NotEmpty();
        }
    }
        
    public class AccountsInvitationsController : MiruController
    {
        [HttpGet("/Accounts/Invitations/{Token}")]
        public async Task<Command> Register(Query query) => await SendAsync(query);

        [HttpPost("/Accounts/Invitations/Register")]
        public async Task<FeatureResult> Register(Command command) => await SendAsync(command);
    }
}
