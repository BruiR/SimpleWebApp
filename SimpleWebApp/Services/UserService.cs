using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.DTOs;
using SimpleWebApp.DTOs.Role;
using SimpleWebApp.Repository;
using SimpleWebApp.Services.Interfaces;
using System.Data;

namespace SimpleWebApp.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> Get(int id)
        {
            return await _dbContext.Users
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Id == id);
                //.FirstAsync(user => user.Id == id);
        }

        //public async Task<List<User>> GetUserList()
        //{
        //    //throw new NotImplementedException();
        //    return await _dbContext.Users.Include(user => user.Roles).ToListAsync();
        //}

        //public async Task<IQueryable<User>> GetUserList()
        //{
        //    //throw new NotImplementedException();
        //    return await _dbContext.Users.Include(user => user.Roles).AsQueryable();
        //}

        public async Task AddRole(User user, Role role)
        {
            ////var user = await _dbContext.Users
            ////    .Include(user => user.Roles)
            ////    //.AsNoTracking()
            ////    .FirstAsync(user => user.Id == userId);
            //var user = await GetUser(userId);
            ////user.Roles.Add(new Role { Id = roleId });
            //var role = await _dbContext.Roles.FindAsync(roleId);
            user.Roles.Add(role);
            await _dbContext.SaveChangesAsync();  
        }


        public async Task Update(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public bool Contains(User user)
        {
            return _dbContext.Users.Contains(user);
        }

        public bool HasRole(User user, Role role)
        {
            return user.Roles.Contains(role);
        }

        public async Task<bool> AnyContainsEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email == email);
        }

        public async Task<bool> AnyContainsEmailWithoutUser(int id, string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email == email && user.Id != id);
        }

        public IQueryable<User> ApplyFilter(IQueryable<User> source, UsersFilterDto filter)
        {
            if (filter == null)
            {
                return source;
            }

            if (filter.UserIds != null)
            {
                source = source.Where(user => filter.UserIds.Contains(user.Id));
            }

            if (filter.Names != null)
            {
                source = source.Where(user => filter.Names.Contains(user.Name));
            }

            if (filter.Ages != null)
            {
                source = source.Where(user => filter.Ages.Contains(user.Age));
            }

            if (filter.Emails != null)
            {
                source = source.Where(user => filter.Emails.Contains(user.Email));
            }

            //if (filter.RoleIds != null)
            //{

            //    source = source.Where(user => filter.RoleIds.Intersect(user.Roles.Select(role => role.Id)) != null);
            //}

            return source;
        }

        public IQueryable<User> ApplyPaging(IQueryable<User> source, int page, int limit)
        {
            return source.Skip(Math.Max(0, page - 1) * limit).Take(limit);
        }
    }
}
