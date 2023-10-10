using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.Repository;
using SimpleWebApp.Services.Interfaces;

namespace SimpleWebApp.Services
{
    public class AuthorizedPersonService : IAuthorizedPersonService
    {
        private readonly AppDbContext _dbContext;

        public AuthorizedPersonService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Contains(AuthorizedPerson authorizedPerson)
        {
            return await _dbContext.AuthorizedPersons.AnyAsync(person => person.Login == authorizedPerson.Login && person.Password == authorizedPerson.Password);
        }

        public async Task<AuthorizedPerson> Get(string login, string password)
        {
            return await _dbContext.AuthorizedPersons
                .FirstOrDefaultAsync(person => person.Login == login && person.Password == password);
        }
    }
}
