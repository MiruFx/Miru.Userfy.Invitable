using System.CommandLine;
using System.Threading.Tasks;

namespace Miru.Userfy.Invitable.Tests;

public class Program
{
    public static async Task Main(string[] args)
    {
        var rootCommand = new RootCommand
        {
        };

        await rootCommand.InvokeAsync(args);
    }
}