using AltBank.Domain;
using AltBank.Tests.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miru.Core;
using Miru.PageTesting;
using Miru.PageTesting.Chrome;
using Miru.Testing;
using OpenQA.Selenium.Chrome;

namespace AltBank.PageTests.Config
{
    public class PageTestsConfig : ITestConfig
    {
        public void ConfigureTestServices(IServiceCollection services)
        {
            // import services from AltBank.Tests
            services.AddFrom<TestsConfig>();
            
            services.AddPageTesting<User>(options =>
            {
                if (OS.IsWindows)
                    options.UseChrome(new ChromeOptions().Incognito());
                else
                    options.UseChrome(new ChromeOptions().Incognito().Headless());
            });
        }

        public void ConfigureRun(TestRunConfig run)
        {
            run.PageTestingDefault();
        }
        
        public IHostBuilder GetHostBuilder() => 
            TestMiruHost.CreateMiruHost<Startup>();
    }
}
