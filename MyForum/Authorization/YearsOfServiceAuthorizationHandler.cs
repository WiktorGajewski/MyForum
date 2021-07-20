using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyForum.Authorization
{
    public class YearsOfServiceAuthorizationHandler :
        AuthorizationHandler<YearsOfServiceRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            YearsOfServiceRequirement requirement)
        {
            if(!context.User.HasClaim(c => c.Type == "RegistrationDate"))
            {
                return Task.CompletedTask;
            }

            var registrationDate = DateTimeOffset.Parse(
                context.User.FindFirst(c => c.Type == "RegistrationDate").Value
            );

            var yearsOfService = Math.Floor( ((DateTimeOffset.Now - registrationDate).TotalDays ) / 365.0);

            if (yearsOfService >= requirement.YearsOfServiceRequired)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
