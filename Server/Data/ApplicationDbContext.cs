using System;
using Contoso.Shared.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Contoso.Server.Data {
    /// <summary>
    /// Using the support for API authorization and 
    /// ASP.NET Core  Identity for authenticating and storing  users combining 
    /// with IdentityServer for implementing OpenIDConnect
    /// </summary>
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser> {

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions) { }
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
                .HasData(new IdentityRole { Name = "User", NormalizedName = "USER", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });
            builder.Entity<IdentityRole>()
                .HasData(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });
        }

        DbSet<ApplicationUser> Usuarios { get; set; }
    }
}