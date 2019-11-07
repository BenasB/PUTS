using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using PUTSWeb.Models;

namespace PUTSWeb.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public string Group { get; set; }

        public List<ProblemResult> ProblemResults { get; set; } = new List<ProblemResult>();
    }
}
