using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.Repository;
using SimpleWebApp.Services.Interfaces;

namespace SimpleWebApp.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _dbContext;

        public RoleService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Role> GetRole(int id)
        {
            return await _dbContext.Roles                
                .FirstAsync(role => role.Id == id);
        }
    }
}
