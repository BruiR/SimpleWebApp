using SimpleWebApp.Domain.Models;
using SimpleWebApp.DTOs;

namespace SimpleWebApp.Services.Interfaces
{
    public interface IUserService
    {
        //Task<List<User>> GetUserList(); //Получение списка всех пользователей                                
        Task<User> Get(int id);
        Task Create(User user);
        Task Update(User user);
        Task Delete(User user);
        Task AddRole(User user, Role role); 
        bool Contains(User user);
        bool HasRole(User user, Role role);
        Task<bool> AnyContainsEmail(string email);
        Task<bool> AnyContainsEmailWithoutUser(int id, string email);
        public IQueryable<User> ApplyFilter(IQueryable<User> source, UsersFilterDto filter);
        public IQueryable<User> ApplyPaging(IQueryable<User> source, int page, int limit);
    }
}
