<p align="center">
    <img src="https://mirufx.github.io/Miru-Logo-Text.png" />
</p>

[![NuGet](https://img.shields.io/nuget/vpre/Miru.Userfy.Invitable.svg)](https://www.nuget.org/packages/Miru.Userfy.Invitable)
[![Build](https://github.com/MiruFx/Miru.Userfy.Invitable/workflows/CI/badge.svg)](https://github.com/MiruFx/Miru.Userfy.Invitable/actions?query=workflow%3ACI)

## About Miru Userfy Invitable

`Miru.Userfy.Invitable` is a Miru extension that provides API for user invitation.
It covers saving a new user, sending email invitation, 
updating password and other information set by the user.

Miru.Userfy is an extension component part of [Miru](https://github.com/MiruFx/Miru).
It provides authentication, authorization, and other user management features.

# Usage

## Requirements

.NET >= 6.0

Miru >= 0.9.6

## Installation

Add the library into your Miru App project:  
`miru app dotnet add package Miru.Userfy.Invitable`

Build the App project:
`miru app dotnet build`

Run UserfyInvitable installer:
`miru userfy.invitable.install`

It will add into your project:
```
/{app}/Database/Migrations/{datetime}_AlterUsersAddInvitable.cs

/{app}/Admin/Users/Invitations/_Invitation.mail.cshtml

/{app}/Users/Invitations/Register.cshtml
/{app}/Users/Invitations/UserRegister.cs
```

If you use different paths, you can move the files into your directory conventions.

## Configuration

### User Entity

Decorate your entity `User.cs` with `IInvitable`:

```csharp
public class User : UserfyUser, IInvitable
{
    public string InvitationToken { get; set; }
    public DateTime? InvitationAcceptedAt { get; set;  }
    
    // from IInactivable
    public bool IsInactive { get; set; }
}
```

Note that IInvitable depends on IInactivable.

### Configure Services

Add into your `Startup.cs` or `Program.cs` ConfigureServices:

```csharp
.AddUserfyInvitable<User>(opt =>
{
    // set true if the email will be queued and send later. otherwise, it will be sent now
    opt.SendEmailLater = true;
    
    // customize the invitation email
    opt.MailConfig((mail, user) =>
    {
        mail.TemplateAt("/Features/Accounts", "_Invitation", user);
    });
})
```

## Features

User invitation features are provided by the class `UserInvitation`.

### Add User And Send Invitation

`AddAndInviteAsync` saves a new User into `Users` table and send the email inviting the user:

```csharp
public async Task<Result> Handle(Command request, CancellationToken ct)
{
    var user = new User
    {
        Email = request.Email,
        UserName = request.Email
    };

    await _userInvitation.AddAndInviteAsync(user, ct);
    
    return new Result { Email = request.Email };
}
```

### Set User's Password

`AcceptInvitation` saves the password set by the own user:

```csharp
public async Task<FeatureResult> Handle(Command request, CancellationToken ct)
{
    var user = await _db.Users.ByInvitationTokenOrFailAsync(request.Token, ct);

    user.AcceptInvitation();

    await _userInvitation.SetPasswordAsync(user, request.Password, ct);

    await _userLogin.LoginAsync(user.Email, request.Password);

    return new FeatureResult(_userfyOptions.AfterLoginFeature);
}
```

### ResendInvitationAsync

`ResendInvitationAsync` sends a new email invitation to an user:

```csharp
public async Task<Result> Handle(Command request, CancellationToken ct)
{
    var user = await _db.Users.ByIdOrFailAsync(request.UserId, ct);

    await _userInvitation.ResendInvitationAsync(user, ct);
    
    return new Result() { UserId = user.Id };
}
```

## License

`Miru.Userfy.Invitable` is licensed under the MIT license.

