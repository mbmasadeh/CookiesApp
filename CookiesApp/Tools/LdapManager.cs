using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace CookiesApp.Tools
{
    public class LdapManager
    {
        UserPrincipal user;
        public UserPrincipal GetUserIdentity(string email)
        {
            using (PrincipalContext principal = new PrincipalContext(ContextType.Domain))
            {
                user = UserPrincipal.FindByIdentity(principal, IdentityType.SamAccountName, email);
            }
            return user;
        }
    }
}
