using System.Linq;
using System.Threading.Tasks;
using Contoso.Server.Data;
using Contoso.Server.Email;
using Contoso.Shared.Entities;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Contoso.Server {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) {

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DockerConnectionLocal")));
            services.AddAuthentication().AddFacebook(facebookOptions => {
                facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                // ask permissions
                facebookOptions.Scope.Add("pages_manage_posts");
                facebookOptions.Scope.Add("user_birthday");
                facebookOptions.SaveTokens = true;

                facebookOptions.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents() {
                    OnRemoteFailure = LoginFailureHandler => {
                        var authProperties = facebookOptions.StateDataFormat.Unprotect(LoginFailureHandler.Request.Query["state"]);
                        LoginFailureHandler.Response.Redirect("/Identity/Account/Login");
                        return Task.FromResult(0);
                    }
                };
            });
            // Identity with default UI,
            // Roles Management wich adds role-related services and 
            // ApplicationDbContext to store
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // IdentityServer with an additional AddAPIAuthorization helper method that
            // sets up some default  ASP.NET Core Conventions
            // and Custom ApplicationUser Model 
            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            // AddIdentityServerJwt helper method that configures the app to validate JWT tokens
            // produced by IdentityServer
            services.AddAuthentication()
                .AddIdentityServerJwt();
            // Register the Profile Service 
            services.AddTransient<IProfileService, ProfileService>();

            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseWebAssemblyDebugging();
            } else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            // The IdentityServer middleware that exposes the OpenID Connect endpoints
            app.UseIdentityServer();

            // Authentication middleware that is responsible for validating the request credentials and setting
            // the user on the request  context
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}