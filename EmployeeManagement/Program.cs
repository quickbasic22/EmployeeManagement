using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using NLog;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagement.Security;
using Microsoft.AspNetCore.Mvc.Authorization;

internal class Program
{
    private static void Main(string[] args)
    {
        var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug("init main");
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseNLog();

            builder.Services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.EnableEndpointRouting = false;
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            var connectionString = builder.Configuration.GetConnectionString("MyConnection") ?? throw new InvalidOperationException("Connection string 'EmployeeManagementContextConnection' not found.");

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;

                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });


            builder.Services.AddControllersWithViews().AddXmlSerializerFormatters();

            builder.Logging.AddDebug();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role"));

                options.AddPolicy("EditRolePolicy",
                    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("Admin"));
            });


            builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

            builder.Services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            builder.Services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            builder.Services.AddSingleton<DataProtectionPurposeStrings>();

            // override appsettings
            // builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // Override the value of the "MyKey" setting
            //builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
            //    {
            //        { "MyKey", "My string setting in console." }
            //    });


            var app = builder.Build();



            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            //    app.MapControllerRoute(
            //name: "default",
            //pattern: "{controller=Home}/{action=Index}/{id?}");

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });


            //var text = "<hr />";
            //byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
            //bod.



            //app.MapGet("/", async (context, next) =>
            //{
            //    await context.Response.WriteAsync(builder.Configuration["MyKey"]);
            //    await context.Response.WriteAsync("");
            //    await context.Response.WriteAsync(builder.Configuration["MyUserSecret"]);
            //    await context.Response.WriteAsync("");
            //    await context.Response.WriteAsync(builder.Configuration["MyDateTime"]);
            //    next();
            //});

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Hello from 1st middleware.");
            //    await next();
            //});


            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("from orginial middleware.");
            //    await next();
            //});

            //int index = 1;
            //int[] array = new int[2];
            //string message = string.Format("This is the value at index [{0}]: {1}", index, array);


            //        app.Use(async (context, next) =>
            //{
            //await next();
            //await context.Response.WriteAsync("Hello from third middleware");
            //await context.Response.WriteAsync("Hello from third middleware");
            //    app.Logger.LogInformation(message);
            //});

            //app.Use(async (context, next) =>
            //{
            //    await next();
            //    await context.Response.WriteAsync("Hello from second middleware");
            //    await context.Response.WriteAsync("Hello from second middleware");
            //    app.Logger.LogInformation($"third middleware ran {app.Logger.ToString()}", new int[] { 2, 22 });
            //});
            //app.Use(async (context, next) =>
            //{
            //    await next();
            //    await context.Response.WriteAsync("Hello from first middleware");
            //    await context.Response.WriteAsync("Hello from first middleware");
            //    app.Logger.LogInformation($"third middleware ran {args.Length}", new int[] { 1, 11 });
            //});

            app.Run();
        }
        catch (Exception exception)
        {
            logger.Error(exception, "Stopped program because of exception");
            throw;
        }
        finally
        {
            NLog.LogManager.Shutdown();
        }
    }
}