using Miru.Foundation.Hosting;

namespace AltBank;

public class Program
{
    public static async Task Main(string[] args) =>
        await MiruHost
            .CreateMiruWebHost<Startup>(args)
            .RunMiruAsync();
}
