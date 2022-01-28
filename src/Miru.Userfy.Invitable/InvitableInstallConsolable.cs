using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;
using Miru.Userfy.Invitable;
using System;
using Maker = Miru.Userfy.Invitable.Maker;

public class InvitableInstallConsolable : Consolable
{
    public InvitableInstallConsolable()
        : base("userfy.invitable.install", "Install files to use Userfy Invitable")
    {
    }

    public class ConsolableHandler : IConsolableHandler
    {
        private readonly MiruSolution _solution;

        public ConsolableHandler(MiruSolution solution)
        {
            _solution = solution;
        }

        public async Task Execute()
        {
            var maker = new Maker(_solution, typeof(IInvitable).Assembly);

            Console2.WhiteLine("Installing Miru.Userfy.Invitable");
            Console2.BreakLine();

            MakeMigration(maker);
            // make.Query(In, Name, Action, "Show");

            Console2.BreakLine();
            Console2.WhiteLine("Done. Good luck!");
            Console2.BreakLine();

            await Task.CompletedTask;
        }

        private void MakeMigration(Maker maker)
        {
            var input = new
            {
                Version = DateTime.Now.ToString("yyyyMMddHHmm")
            };
            
            var invitationFeatureDir =  maker.Solution.FeaturesDir / "Accounts" / "Invitations";
                
            maker.Template(
                "Migration", 
                input, 
                maker.Solution.MigrationsDir / $"{input.Version}_AlterUsersAddInvitation.cs");
            
            maker.Template(
                $"_Invitation.mail.cshtml.stub", 
                input, 
                invitationFeatureDir / "_Invitation.mail.cshtml");
            
            maker.Template(
                $"_New.turbo.cshtml.stub", 
                input, 
                invitationFeatureDir / "_New.turbo.cshtml");
            
            maker.Template(
                $"InvitationNew.stub", 
                input, 
                invitationFeatureDir / "InvitationNew.cs");
            
            maker.Template(
                $"InvitationRegister.stub", 
                input, 
                invitationFeatureDir / "InvitationRegister.cs");

            maker.Template(
                $"New.cshtml.stub", 
                input, 
                invitationFeatureDir / "New.cshtml");

            maker.Template(
                $"Register.cshtml.stub", 
                input, 
                invitationFeatureDir / "Register.cshtml");
        }
    }
}
