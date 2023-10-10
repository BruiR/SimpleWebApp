using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.DTOs;
using SimpleWebApp.DTOs.Role;
using SimpleWebApp.DTOs.User;
using SimpleWebApp.Repository;
using SimpleWebApp.Services.Interfaces;

namespace SimpleWebApp.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UsersController(AppDbContext context, IMapper mapper, IUserService userService, IRoleService roleService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _roleService = roleService;
        }

        //// GET: api/Users
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        //{
        //    var users = await _userService.GetUserList();
        //    if (users == null)
        //    {
        //        return NotFound();
        //    }
        //    var usersDto = _mapper.Map<List<User>, List<UserDto>>(users);               
        //    return usersDto;
        //}

        /// <summary>
        /// Returns users, sorted and filtered list of users.
        /// </summary>
        /// <response code="200">Users have been successfully returned.</response>
        /// <response code="404">No users found.</response>
        /// <response code="500">Unhandled exception has been thrown over the request execution.</response>
        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<UserDto>>> GetUsers([FromQuery] FilterRequestDto<UsersFilterDto> request)
        {
            var users = _context.Users.Include(user => user.Roles).AsQueryable();

            int totalItems = users.Count();

            if (totalItems == 0)
            {
                return NotFound();
            }

            int totalPages = totalItems == 0 ? 1 : (int)Math.Ceiling((double)totalItems / request.Pagination.Limit.Value);
            users = _userService.ApplyFilter(users, request.Filter);
            users = _userService.ApplyPaging(users, request.Pagination.Page.Value, request.Pagination.Limit.Value);

            return new PagedResponseDto<UserDto>
            {
                Items = await _mapper.ProjectTo<UserDto>(users).ToListAsync(),
                PageNumber = request.Pagination.Page.Value,
                Limit = request.Pagination.Limit.Value,
                TotalPages = totalPages
            };
        }

        /// <summary>
        /// Returns user.
        /// </summary>
        /// <response code="200">User has been successfully found.</response>
        /// <response code="404">User with specified id cannot be found.</response>
        /// <response code="500">Unhandled exception has been thrown over the request execution.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {

            var user = await _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        /// <summary>
        /// Updates user and returns an updated user.
        /// </summary>
        /// <response code="200">User has been successfully updated.</response>
        /// <response code="400">Incorrect data was sent.</response>
        /// <response code="500">Unhandled exception has been thrown over the request execution.</response>
        [HttpPut]
        public async Task<ActionResult<UserDto>> PutUser(UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
            {
                return BadRequest();
            }
            if (await _userService.AnyContainsEmailWithoutUser(updateUserDto.Id, updateUserDto.Email))
            {
                return BadRequest("Данный email уже находится в базе данных");
            }
            var user = _mapper.Map<User>(updateUserDto);
            if (!_userService.Contains(user))
            {
                return BadRequest();
            }
            await _userService.Update(user);
            _context.Entry(user).Collection(user => user.Roles).Load();
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }


        /// <summary>
        /// Creates and returns user.
        /// </summary>
        /// <response code="200">User has been successfully updated.</response>
        /// <response code="400">Incorrect data was sent.</response>
        /// <response code="500">Unhandled exception has been thrown over the request execution.</response>
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(CreateUserDto CreateUserDto)
        {
            if (await _userService.AnyContainsEmail(CreateUserDto.Email))
            {
                return BadRequest("Данный email уже находится в базе данных");
            }
            var user = _mapper.Map<User>(CreateUserDto);
            await _userService.Create(user);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        /// <summary>
        /// Add new role to user and returns user.
        /// </summary>
        /// <response code="200">User has been successfully updated.</response>
        /// <response code="404">Incorrect data was sent.</response>
        /// <response code="409">User already has this role.</response>
        /// <response code="500">Unhandled exception has been thrown over the request execution.</response>
        [Route("AddRole")]
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostNewUserRole(AddUsersRoleDto usersRoleDto)
        {
            var user = await _userService.Get(usersRoleDto.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var role = await _roleService.GetRole(usersRoleDto.RoleId);
            if (role == null)
            {
                return NotFound();
            }
            if (_userService.HasRole(user, role))
            {
                return Conflict();
            }
            await _userService.AddRole(user, role);
            var UpdateUser = await _userService.Get(usersRoleDto.UserId);
            var userDto = _mapper.Map<UserDto>(UpdateUser);
            return Ok(userDto);
        }

        /// <summary>
        /// Deletes user.
        /// </summary>
        /// <response code="200">User has been successfully deleted.</response>
        /// <response code="404">User with specified id cannot be found.</response>
        /// <response code="500">Unhandled exception has been thrown over the request execution.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.Get(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userService.Delete(user);
            return Ok();
        }

    }
}
