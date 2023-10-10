using SimpleWebApp.Domain.Models;

namespace SimpleWebApp.Services.Interfaces
{
    public interface IAuthorizedPersonService
    {
        Task<AuthorizedPerson> Get(string login, string password);
    }
}
