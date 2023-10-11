using Humanizer;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.Domain.Queries;
using SimpleWebApp.DTOs.User;
using SimpleWebApp.Repository;
using SimpleWebApp.Services.Interfaces;
using System.Data;
using System.Linq.Expressions;

namespace SimpleWebApp.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private const char SortOrderingSymbol = '-';

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> Get(int id)
        {
            return await _dbContext.Users
                .Include(user => user.Roles)
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task AddRole(User user, Role role)
        {
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

            return source;
        }

        public static IReadOnlyDictionary<string, Expression<Func<User, object>>> SortExpressions { get; } =
            new Dictionary<string, Expression<Func<User, object>>>
            {
                {nameof(UserDto.Id).Camelize(), user => user.Id},
                {nameof(UserDto.Name).Camelize(), user => user.Name},
                {nameof(UserDto.Age).Camelize(), user => user.Age},
                {nameof(UserDto.Email).Camelize(), user => user.Email}
            };

        public IQueryable<User> ApplySorting(IQueryable<User> source, IEnumerable<string> sortRules)
        {
            if (sortRules == null)
            {
                return source;
            }

            var sortExpressions = sortRules
              .Select(sortRule => new SortRule
              {
                  IsAscending = sortRule[0] != SortOrderingSymbol,
                  PropertyName = sortRule[0] != SortOrderingSymbol ? sortRule : sortRule.Substring(1)
              })
              .Where(sortRule => SortExpressions.ContainsKey(sortRule.PropertyName))
              .SelectMany(sortRule =>
              {
                  var originalSortExpression = SortExpressions[sortRule.PropertyName];

                  return new[]
                  {
                      new SortExpression<User>
                      {
                          IsAscending = sortRule.IsAscending,
                          Selector = originalSortExpression
                      }
                  };
              });

            var firstRule = sortExpressions.First();
            var orderSource = firstRule.IsAscending
                ? source.OrderBy(firstRule.Selector)
                : source.OrderByDescending(firstRule.Selector);

            return sortExpressions.Skip(1)
                .Aggregate(orderSource, (current, sortExpression) => sortExpression.IsAscending
                ? current.ThenBy(sortExpression.Selector)
                : current.ThenByDescending(sortExpression.Selector));
        }

        public IQueryable<User> ApplyPaging(IQueryable<User> source, int page, int limit)
        {
            return source.Skip(Math.Max(0, page - 1) * limit).Take(limit);
        }
    }
}
