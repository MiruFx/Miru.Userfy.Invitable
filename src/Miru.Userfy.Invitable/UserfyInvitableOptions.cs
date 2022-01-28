using Miru.Mailing;
using System;

namespace Miru.Userfy.Invitable;

public class UserfyInvitableOptions<TUser> where TUser : UserfyUser
{
    public Action<Email, TUser> MailConfiguration { get; internal set; }

    public bool SendEmailLater { get; set; } = true;
    
    public void MailConfig(Action<Email> action)
    {
        MailConfiguration = (email, user) => action(email);
    }
    
    public void MailConfig(Action<Email, TUser> action)
    {
        MailConfiguration = action;
    }
}