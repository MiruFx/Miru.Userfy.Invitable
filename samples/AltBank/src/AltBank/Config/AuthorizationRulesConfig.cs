using Miru.Security;
using Miru.Userfy;

namespace AltBank.Config;

public class AuthorizationRulesConfig : IAuthorizationRules
{
    private readonly IUserSession<User> _userSession;
        
    public AuthorizationRulesConfig(IUserSession<User> userSession) => _userSession = userSession;

    public AuthorizationResult Evaluate<TRequest>(TRequest request, FeatureInfo feature)
    {
        // if (_userSession.IsAuthenticated)
        // {
        //     var user = await _userSession.GetUserAsync();
        //     
        //     if (feature.IsIn("Admin") && user?.IsAdmin == false)
        //         return AuthorizationResult.Fail();
        // }
        //
        // if (feature.Implements<IMustBeAuthenticated>() && _userSession.IsAnonymous)
        //     return AuthorizationResult.Fail("Authentication is required");
            
        // return AuthorizationResult.Succeed();

        return AuthorizationResult.Succeed();
    }
}