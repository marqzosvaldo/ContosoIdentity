using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Contoso.Server.Data;
using Contoso.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Contoso.Server.Controllers {
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationUser>>> Get() {
            var users = await _context.Users.ToListAsync();
            return users;
        }

    }
}