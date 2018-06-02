using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCapBetService.Auth;
using WorldCapBetService.BLL;
using WorldCapBetService.Helpers;
using WorldCapBetService.Models;
using WorldCapBetService.Models.Entities;
using WorldCapBetService.ViewModels;

namespace WorldCapBetService.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<User> userManager, Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: api/Users
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiAdmin")]
        [HttpGet]
        public IEnumerable<UserViewModel> GetUser()
        {
            var users = _context.User;
            var result = _mapper.Map<IList<UserViewModel>>(users);

            return result;
        }

        // GET: api/Users/5
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiUser")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (!CheckClaims.CheckUser(identity, id))
            {
                return BadRequest("It's not you :)");
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<UserViewModel>(user);

            return Ok(result);
        }

        // PUT: api/Users/5
        [Authorize(Policy = "ApiUser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] string id, [FromBody] RegistrationViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (!CheckClaims.CheckUser(identity, id))
            {
                return BadRequest("It's not you :)");
            }

            var dbUser = await _userManager.FindByIdAsync(id);
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;

            var result = await _userManager.UpdateAsync(dbUser);
            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            result = await _userManager.RemovePasswordAsync(dbUser);
            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            result = await _userManager.AddPasswordAsync(dbUser, user.Password);
            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            return NoContent();
        }

        // POST: api/Users
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] RegistrationViewModel userVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = new User();
            user.FirstName = userVm.FirstName;
            user.Email = userVm.Email;
            user.UserName = userVm.Email;
            user.LastName = userVm.LastName;
            user.Role = Constants.Strings.JwtClaims.UserAccess;
            var result = await _userManager.CreateAsync(user, userVm.Password);
            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [Authorize(Policy = "ApiAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var ranking = await _context.Rankings.SingleOrDefaultAsync(r => r.User.Id == id);
            if (ranking != null)
            {
                _context.Rankings.Remove(ranking);
            }

            var pronostics = _context.Pronostic.Where(p => p.User.Id == id).ToList();
            if (pronostics.Count > 0)
            {
                _context.Pronostic.RemoveRange(pronostics);
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Users/5
        [Authorize(Policy = "ApiAdmin")]
        [HttpPut("resetpassword/{id}")]
        public async Task<IActionResult> ResetPassword([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser == null)
            {
                return NotFound();
            }
            var result = await _userManager.RemovePasswordAsync(dbUser);
            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            //set temporary password email without @capgemini.com
            var newPassword = dbUser.Email.ToLower();
            newPassword = newPassword.Remove(newPassword.IndexOf('@'));
            result = await _userManager.AddPasswordAsync(dbUser, newPassword);
            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            return Ok(newPassword);
        }
    }
}