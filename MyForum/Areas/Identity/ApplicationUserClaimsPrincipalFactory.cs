using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyForum.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyForum.Areas.Identity
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<MyUser, IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory(
            UserManager<MyUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> options
            ) : base(userManager, roleManager, options)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(MyUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim("RegistrationDate",
                user.RegistrationDate.ToShortDateString()));
            identity.AddClaim(new Claim("PrestigePoints",
                user.PrestigePoints.ToString()));

            return identity;
        }
    }
}
