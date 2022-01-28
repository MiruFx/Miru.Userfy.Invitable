using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Miru.Userfy.Invitable;

public static class InvitableExtensions
{
    public static bool IsInvited(this IInvitable invitable)
    {
        return invitable.InvitationToken.NotEmpty();
    }
    
    public static void Invite(this IInvitable invitable)
    {
        invitable.IsInactive = true;
        invitable.InvitationToken = Guid.NewGuid().ToString();
        invitable.InvitationAcceptedAt = null;
    }
    
    public static void AcceptInvitation(this IInvitable invitable)
    {
        invitable.InvitationToken = string.Empty;
        invitable.InvitationAcceptedAt = DateTime.Now;
        invitable.IsInactive = false;
    }
    
    public static async Task<TModel> ByInvitationTokenOrFailAsync<TModel>(
        this IQueryable<TModel> query, 
        string token, 
        CancellationToken ct = default) where TModel : IInvitable
    {
        return await query
            .Where(x => x.IsInactive)
            .Where(x => x.InvitationToken == token)
            .SingleOrFailAsync(x => x.InvitationToken == token, ct);
    }
    
    public static void ThrowDomainExceptionIfFailed(this IdentityResult result)
    {
        if (result.Succeeded == false)
            result.Errors.ThrowDomainException();
    }
}