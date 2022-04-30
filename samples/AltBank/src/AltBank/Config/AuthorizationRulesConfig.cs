using Miru.Security;
using Miru.Userfy;

namespace AltBank.Config;

public class AuthorizationRulesConfig : IAuthorizationRules
{
    public async Task<AuthorizationResult> Evaluate<TRequest>(TRequest request, FeatureInfo feature)
    {
        await Task.CompletedTask;

        return AuthorizationResult.Succeed();
    }
}
