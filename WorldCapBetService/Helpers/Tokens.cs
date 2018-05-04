using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorldCapBetService.Auth;

namespace WorldCapBetService.Helpers
{
    public class Tokens
    {
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                username = identity.Claims.Single(c => c.Type == "username").Value,
                firstName = identity.Claims.Single(c => c.Type == "firstName").Value,
                lastName = identity.Claims.Single(c => c.Type == "lastName").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(userName, identity),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds,
                role = identity.Claims.Single(c => c.Type == Constants.Strings.JwtClaimIdentifiers.Rol).Value
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
