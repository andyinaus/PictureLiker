using System;
using System.Linq;
using System.Security.Claims;

namespace PictureLiker.Extensions
{
    //TODO: Unit Tests
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var nameIdentifierClaim = principal.Claims.FirstOrDefault(c => c.Type.EqualsIgnoreCase(ClaimTypes.NameIdentifier));

            if (nameIdentifierClaim == null) throw new ApplicationException("There is no NameIdentifier(Id) associated with current user.");

            return int.Parse(nameIdentifierClaim.Value);
        }
    }
}
