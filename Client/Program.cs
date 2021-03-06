using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Contoso.Client.Helpers;
using Contoso.Client.Repository.UsersRep;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Contoso.Client {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            // Registering Services
            ConfigureServices(builder.Services);

            builder.Services.AddHttpClient("Contoso.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Contoso.ServerAPI"));

            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }
        /// <summary>
        /// Registering IHTTPService and IUserRepository with an implementation type
        /// This Method is responsible for defining the services that app uses
        /// </summary>
        private static void ConfigureServices(IServiceCollection services) {
            services.AddScoped<IHttpService, HttpService>();

            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}