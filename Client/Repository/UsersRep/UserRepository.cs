using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Contoso.Client.Helpers;
using Contoso.Shared.Entities;

namespace Contoso.Client.Repository.UsersRep {
    public class UserRepository : IUserRepository {
        private readonly IHttpService _httpService;

        public UserRepository(HttpClient httpClient, IHttpService httpService) {
            _httpService = httpService;
        }

        public async Task<List<ApplicationUser>> GetUsers() {
            var response = await _httpService.Get<List<ApplicationUser>>($"api/user");
            if (!response.Success) {
                throw new ApplicationException(await response.GetBody());

            }

            return response.Response;
        }
    }
}