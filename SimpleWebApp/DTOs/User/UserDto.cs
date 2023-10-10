using SimpleWebApp.DTOs.Role;

namespace SimpleWebApp.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}
