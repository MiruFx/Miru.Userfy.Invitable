using Miru.Domain;
using System;

namespace Miru.Userfy.Invitable;

public interface IInvitable : IInactivable
{
    string InvitationToken { get; set; }

    DateTime? InvitationAcceptedAt { get; set; }
}