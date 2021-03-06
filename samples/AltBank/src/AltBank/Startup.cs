using AltBank.Config;
using AltBank.Features;
using AltBank.Features.Home;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Miru.Foundation.Hosting;
using Miru.Pipeline;
using Miru.Queuing;
using Miru.Scopables;
using Miru.Sqlite;
using Miru.Userfy;
using Miru.Userfy.Invitable;

namespace AltBank;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMiru<Startup>()

            .AddDefaultPipeline<Startup>()
                
            .AddEfCoreSqlite<AppDbContext>()
            
            .AddCurrentAttributes<Current, CurrentAttributes>()

            // miru extensions
            .AddUserfy<User, AppDbContext>(
                userfy: cfg =>
                {
                    cfg.AfterLoginFeature = new HomeIndex();
                },
                cookie: cfg =>
                {
                    cfg.Cookie.Name = App.Name;
                    cfg.Cookie.HttpOnly = true;
                    cfg.ExpireTimeSpan = TimeSpan.FromHours(2);
                    cfg.LoginPath = "/Accounts/Login";
                },
                identity: cfg =>
                {
                    cfg.SignIn.RequireConfirmedAccount = false;
                    cfg.SignIn.RequireConfirmedEmail = false;
                    cfg.SignIn.RequireConfirmedPhoneNumber = false;

                    cfg.Password.RequiredLength = 3;
                    cfg.Password.RequireUppercase = false;
                    cfg.Password.RequireNonAlphanumeric = false;
                    cfg.Password.RequireLowercase = false;

                    cfg.User.RequireUniqueEmail = true;
                })
            .AddAuthorizationRules<AuthorizationRulesConfig>()

            .AddUserfyInvitable<User>(opt =>
            {
                // set true if the email will be queued and send later. otherwise, it will be sent now
                opt.SendEmailLater = false;
                
                // customize the invitation email
                opt.MailConfig((mail, user) =>
                {
                    mail.TemplateAt("/Features/Accounts/Invitations", "_Invitation", user);
                });
            })

            .AddMailing(_ =>
            {
                _.EmailDefaults(email => email.From("noreply@company.com", "AltBank"));
            })
            
            // SenderFileStorage saves emails at /storage/temp/emails
            // for sending emails throught smtp, use SmtpSender instead of FileStorageSender:
            .AddSmtpSender()
            //.AddFileStorageSender()

            .AddQueuing(_ =>
            {
                _.UseLiteDb();
            })
            .AddHangfireServer();
            
        services.AddSession();
        services.AddDistributedMemoryCache();
        services.AddMemoryCache();

        // your app services
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // The Middlewares here are configured in order of executation
        // Here, they are organized for Miru defaults. Changing the order might break some functionality 

        if (env.IsDevelopmentOrTest())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseHttpsRedirection();
        }
            
        app.UseStaticFiles();
            
        app.UseRequestLogging();
        app.UseStatusCodePagesWithReExecute("/Error/{0}");
        app.UseExceptionLogging();

        app.UseHangfireDashboard();
        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
            
        app.UseEndpoints(e =>
        {
            e.MapDefaultControllerRoute();
            e.MapRazorPages();
            e.MapHangfireDashboard("/_queue", new DashboardOptions
            {
                AsyncAuthorization = new[] { new HangfireAuthorizationFilter() }
            });
        });
    }
}
