using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using System;

namespace Miru.Userfy.Invitable;

public static class InvitationRegistry
{
    public static IServiceCollection AddUserfyInvitable<TUser>(
        this IServiceCollection services,
        Action<UserfyInvitableOptions<TUser>> optionsAction = null)
            where TUser : UserfyUser, IInvitable
    {
        var options = new UserfyInvitableOptions<TUser>();
        
        optionsAction?.Invoke(options);
        
        // TODO: try to add Inactivable

        services.AddConsolable<InvitableInstallConsolable>();
        
        return services
            .AddSingleton(options)
            .AddScoped<UserInvitation<TUser>>();
    }
}