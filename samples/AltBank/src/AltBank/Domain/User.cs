using AltBank.Features.Accounts.Invitations;
using Miru.Behaviors.TimeStamp;
using Miru.Userfy;
using Miru.Userfy.Invitable;

namespace AltBank.Domain;

public class User : UserfyUser, ITimeStamped, IInvitable
{
    public override string Display => Email;
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public bool IsInactive { get; set; }
    public string InvitationToken { get; set; }
    public DateTime? InvitationAcceptedAt { get; set; }
}
