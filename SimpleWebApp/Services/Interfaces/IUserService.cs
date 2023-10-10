using SimpleWebApp.Domain.Models;
using SimpleWebApp.DTOs;

namespace SimpleWebApp.Services.Interfaces
{
    public interface IUserService
    {
        //Task<List<User>> GetUserList(); //Получение списка всех пользователей                                
        Task<User> Get(int id); //Получение пользователя по Id и всех его ролей.
        Task Create(User user); //Создание нового пользователя. 
        Task Update(User user); //Обновление информации о пользователе по Id. 
        Task Delete(int id); //Удаление пользователя по Id. 
        Task AddRole(User user, Role role); //Добавление пользователю по Id новой Роли.
        bool Contains(User user);
        bool HasRole(User user, Role role);
        Task<bool> AnyContainsEmail(string email);
        Task<bool> AnyContainsEmailWithoutUser(int id, string email);
        public IQueryable<User> ApplyFilter(IQueryable<User> source, UsersFilterDto filter);
        public IQueryable<User> ApplyPaging(IQueryable<User> source, int page, int limit);
    }
}
