using SimpleWebApp.Domain.Models;

namespace SimpleWebApp.Services.Interfaces
{
    public interface IJWTManagerService
    {
        TokenResponse Authenticate(AuthorizedPerson authorizedPerson);
    }
}
