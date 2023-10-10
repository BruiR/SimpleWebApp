using SimpleWebApp.Domain.Models;

namespace SimpleWebApp.Services.Interfaces
{
    public interface IRoleService
    {
        Task<Role> GetRole(int id);
    }
}
