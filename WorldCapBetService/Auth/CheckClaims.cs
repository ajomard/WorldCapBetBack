using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WorldCapBetService.Auth
{
    public static class CheckClaims
    {

        public static bool CheckUser(ClaimsIdentity identity, string idUser)
        {

            if (identity != null)
            {
                var userClaimsid = identity.FindFirst("id").Value;
                return userClaimsid == idUser;
            }

            return false;

        }
    }
}
