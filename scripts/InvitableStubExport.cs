using System;

namespace Scripts;

public class InvitableStubExport : StubExport
{
    public InvitableStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        Func<string, string> tokens = line => line
            // for all files
            .Replace("AltBank", "{{ Solution.Name }}")
            
            .Replace("202112281752", "{{ input.Version }}")
            
            .Replace("CreateCards", "{{ input.Name }}")
            .Replace("AlterCardsAddUserId", "{{ input.Name }}")
            .Replace("TableName", "{{ input.Table }}")
            .Replace("ColumnName", "{{ input.Column }}");
            
        // app stubs
        ExportFile(
            Params.BasedAppDir / "src" / "AltBank" / "Database" / "Migrations" / "202112281752_AlterUsersAddInvitation.cs", 
            "Migration", 
            tokens: tokens);
        
        ExportDir(
            Params.BasedAppDir / "src" / "AltBank" / "Features" / "Accounts" / "Invitations", 
            tokens: tokens);
        
        // tests
    }
}