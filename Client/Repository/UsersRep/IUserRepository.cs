using System.Collections.Generic;
using System.Threading.Tasks;
using Contoso.Shared.Entities;
namespace Contoso.Client.Repository.UsersRep {
    public interface IUserRepository {
        Task<List<ApplicationUser>> GetUsers();
    }
}