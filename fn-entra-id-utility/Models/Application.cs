using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fn_entra_id_utility.Models
{
    public class Application
    {
        public string Id { get; set; }
        public string? ApplicationId { get; set; }
        public string ApplicationName { get; set; }

        public string Owner { get; set; }
        public List<AppRole> Roles { get; set; }
        public bool SecretToBeExpired { get; set; }
    }
    public class PasswordCredentials
    {
        public string DisplayName { get; set; }
        public string EndDate { get; set; }
    }
    public class AppRole
    {
        public Guid? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Value { get; set; }
        public string ApplicationId { get; set; }
        public IEnumerable<string> AllowedMemberTypes { get; set; }
    }
}
