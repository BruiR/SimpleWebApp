using System.ComponentModel;

namespace SimpleWebApp.DTOs
{
    public class UsersFilterDto
    {
        [DefaultValue(null)]
        public IEnumerable<int>? UserIds { get; set; }

        [DefaultValue(null)]
        public IEnumerable<string>? Names { get; set; }

        [DefaultValue(null)]
        public IEnumerable<int>? Ages { get; set; }

        [DefaultValue(null)]
        public IEnumerable<string>? Emails { get; set; }

        [DefaultValue(null)]
        public IEnumerable<int>? RoleIds { get; set; }
    }
}
