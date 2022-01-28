using Microsoft.AspNetCore.Identity;
using Miru.Mailing;
using System.Threading;
using System.Threading.Tasks;

namespace Miru.Userfy.Invitable;

public class UserInvitation<TUser> 
    where TUser : UserfyUser, IInvitable
{
    private readonly UserManager<TUser> _userManager;
    private readonly IMailer _mailer;
    private readonly UserfyInvitableOptions<TUser> _options;

    public UserInvitation(
        UserManager<TUser> userManager, 
        IMailer mailer,
        UserfyInvitableOptions<TUser> options)
    {
        _userManager = userManager;
        _mailer = mailer;
        _options = options;
    }

    public async Task SetPasswordAsync(
        TUser user, 
        string password, 
        CancellationToken ct = default)
    {
        var result = await _userManager.AddPasswordAsync(user, password);
        
        result.ThrowDomainExceptionIfFailed();
    }

    public async Task AddAndInviteAsync(
        TUser user,
        CancellationToken ct = default)
    {
        user.Invite();
        
        var result = await _userManager.CreateAsync(user);

        result.ThrowDomainExceptionIfFailed();

        if (_options.SendEmailLater)
            await _mailer.SendLaterAsync(new UserInvitationMail<TUser>(user, _options));
        else
            await _mailer.SendNowAsync(new UserInvitationMail<TUser>(user, _options));
    }
}