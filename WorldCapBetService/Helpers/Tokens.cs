using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorldCapBetService.Auth;

namespace WorldCapBetService.Helpers
{
    public static class Tokens
    {
        public static async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string email, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new
            {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                firstName = identity.Claims.Single(c => c.Type == "firstName").Value,
                lastName = identity.Claims.Single(c => c.Type == "lastName").Value,
                auth_token = await jwtFactory.GenerateEncodedToken(email, identity),
                expires_in = (int)jwtOptions.ValidFor.TotalSeconds,
                role = identity.Claims.Single(c => c.Type == Constants.Strings.JwtClaimIdentifiers.Rol).Value
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
