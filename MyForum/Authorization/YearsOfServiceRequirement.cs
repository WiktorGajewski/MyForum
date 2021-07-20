using Microsoft.AspNetCore.Authorization;

namespace MyForum.Authorization
{
    public class YearsOfServiceRequirement : IAuthorizationRequirement
    {
        public YearsOfServiceRequirement(int yearsOfServiceRequired)
        {
            YearsOfServiceRequired = yearsOfServiceRequired;
        }

        public int YearsOfServiceRequired { get; set; }
    }
}
