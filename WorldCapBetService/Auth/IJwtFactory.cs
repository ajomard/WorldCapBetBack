using System.Security.Claims;
using System.Threading.Tasks;
using WorldCapBetService.Models.Entities;

namespace WorldCapBetService.Auth
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string email, ClaimsIdentity identity);
        ClaimsIdentity GenerateClaimsIdentity(User user, string id);
    }
}
