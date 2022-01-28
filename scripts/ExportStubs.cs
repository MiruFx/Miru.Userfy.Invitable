using System.IO;
using Miru.Core;

namespace Scripts;

public class ExportStubs
{
    public static void Export()
    {
        var rootDir = new SolutionFinder().FromCurrentDir().Solution.RootDir;
        
        var param = new StubParams()
        {
            BasedAppDir = rootDir / "samples" / "AltBank",
            StubDir = rootDir / "src" / "Miru.Userfy.Invitable" / "Templates"
        };
        
        Directories.DeleteIfExists(param.StubDir);
        Directory.CreateDirectory(param.StubDir);
           
        new InvitableStubExport(param).Export();
    }
}