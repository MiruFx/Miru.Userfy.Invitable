using AltBank.Features.Accounts.Passwords;

namespace AltBank.Tests.Features.Accounts.Passwords;

public class PasswordForgotTest : FeatureTest
{
    [Test]
    public async Task Can_generate_reset_email()
    {
        // arrange
        var user = await _.MakeUserAsync<User>();

        // act
        await _.SendAsync(new PasswordForgot.Command
        {
            Email = user.Email
        });

        // assert
        var job = _.LastEmailSent();
        job.ToAddresses.ShouldContainEmail(user.Email);
        job.Body.ShouldContain("password reset");
    }

    public class Validations : ValidationTest<PasswordForgot.Command>
    {
        [Test]
        public void Email_is_required_and_valid()
        {
            ShouldBeValid(m => m.Email, Request.Email);

            ShouldBeInvalid(m => m.Email, string.Empty);
            ShouldBeInvalid(m => m.Email, "admin!.admin");
        }
    }
}
