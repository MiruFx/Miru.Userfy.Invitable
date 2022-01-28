using Miru.Mailing;
using Newtonsoft.Json;

namespace Miru.Userfy.Invitable;

public class UserInvitationMail<TUser> : Mailable where TUser : UserfyUser
{
    [JsonIgnore]
    private readonly TUser _user;

    private readonly UserfyInvitableOptions<TUser> _options;

    public UserInvitationMail(TUser user, UserfyInvitableOptions<TUser> options)
    {
        _user = user;
        _options = options;
    }

    public override void Build(Email mail)
    {
        mail.To(_user.Email, _user.Display)
            .Subject("Invitation")
            .Template($"_Invited", _user);
        
        _options.MailConfiguration?.Invoke(mail, _user);
    }
}
