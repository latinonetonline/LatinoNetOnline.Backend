using System.Collections.Generic;

namespace LatinoNetOnline.Backend.Modules.Identities.Web.Dto.Users
{
    record GetAllUserInput
    {
        public string Search { get; set; }
        public IEnumerable<string> Users { get; set; }
        public string Role { get; set; }
    }
}
