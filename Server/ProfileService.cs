using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Contoso.Server {
    /// <summary>
    /// This class configure IdentityServer
    /// by ProfileService Approach
    /// </summary>
    public class ProfileService : IProfileService {
        public ProfileService() { }

        public Task GetProfileDataAsync(ProfileDataRequestContext context) {
            var nameClaim = context.Subject.FindAll(JwtClaimTypes.Name);
            context.IssuedClaims.AddRange(nameClaim);

            var roleClaims = context.Subject.FindAll(JwtClaimTypes.Role);
            context.IssuedClaims.AddRange(roleClaims);

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context) {
            return Task.CompletedTask;
        }
    }
}