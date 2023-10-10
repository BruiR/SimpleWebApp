using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<PagedResponseDto<UserDto>>> GetUsers([FromQuery] FilterRequestDto<UsersFilterDto> request)
        {
            var users = _context.Users.Include(user => user.Roles).AsQueryable();
            
            int totalItems = users.Count();
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

        // GET: api/Users/5
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
            //!!!!!!!!!!!!!!!!!!!!!!!!!
            await _userService.Update(user);
            _context.Entry(user).Collection(user => user.Roles).Load();
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser(CreateUserDto CreateUserDto)
        {            
            if (await _userService.AnyContainsEmail(CreateUserDto.Email))
            {
                return BadRequest("Данный email уже находится в базе данных");
            }
            var user = _mapper.Map<User>(CreateUserDto);
            await _userService.Create(user);
            if (user == null)
            {
                return Conflict();
            }
            var userDto = _mapper.Map<UserDto>(user);
            //return userDto;
            return Ok(userDto);
        }

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
            if(_userService.HasRole(user, role))
            {
                return Conflict();
            }
            await _userService.AddRole(user, role);
            var UpdateUser = await _userService.Get(usersRoleDto.UserId);
            var userDto = _mapper.Map<UserDto>(UpdateUser);
            return Ok(userDto);
        }

        // DELETE: api/Users/5
        //[HttpDelete("{id}", Name = "Delete")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.Delete(id);
            return NoContent();
        }

    }
}
