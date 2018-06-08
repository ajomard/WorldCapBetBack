using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCapBetService.Auth;
using WorldCapBetService.BLL;
using WorldCapBetService.Models.Entities;
using WorldCapBetService.ViewModels;

namespace WorldCapBetService.Controllers
{
    [Produces("application/json")]
    [Route("api/Pronostics")]
    public class PronosticsController : Controller
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public PronosticsController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Pronostics
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiUser")]
        [HttpGet]
        public IList<PronosticViewModel> GetPronostic()
        {
            var pronostics = _context.Pronostic.Include("Match").Include("User");

            var result = _mapper.Map<IList<PronosticViewModel>>(pronostics);
            return result;
        }

        // GET: api/Pronostics
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiUser")]
        [HttpGet("match/{id}")]
        public IList<PronosticViewModel> GetPronosticForMatch([FromRoute] long id)
        {
            var pronostics = _context.Pronostic.Include("Match").Include("User").Where(p => p.Match.Id == id);

            var result = _mapper.Map<IList<PronosticViewModel>>(pronostics);
            return result;
        }

        // GET: api/Pronostics/5
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiUser")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPronostic([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pronostic = await _context.Pronostic.Include("Match").Include("User").SingleOrDefaultAsync(m => m.Id == id);

            if (pronostic == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<PronosticViewModel>(pronostic);

            return Ok(result);
        }

        // PUT: api/Pronostics/5
        [Authorize(Policy = "ApiUser")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPronostic([FromRoute] long id, [FromBody] Pronostic pronostic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pronostic.Id)
            {
                return BadRequest();
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (!CheckClaims.CheckUser(identity, pronostic.User.Id))
            {
                return BadRequest("It's not you :)");
            }


            if (pronostic.Match.Date <= DateTime.Now)
            {
                return BadRequest("Cannot bet on already played match");
            }

            _context.Entry(pronostic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PronosticExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pronostics
        [Authorize(Policy = "ApiUser")]
        [HttpPost]
        public async Task<IActionResult> PostPronostic([FromBody] Pronostic pronostic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (!CheckClaims.CheckUser(identity, pronostic.User.Id))
            {
                return BadRequest("It's not you :)");
            }


            var isAlreadyBet = _context.Pronostic.Any(p => p.Match.Id == pronostic.Match.Id && p.User.Id == pronostic.User.Id);
            if (isAlreadyBet)
            {
                return BadRequest("A bet already exists for this match/user");
            }
            if (pronostic.Match.Date <= DateTime.Now)
            {
                return BadRequest("Cannot bet on already played match");
            }
            _context.Entry(pronostic.User).State = EntityState.Unchanged;
            _context.Entry(pronostic.Match).State = EntityState.Unchanged;
            _context.Pronostic.Add(pronostic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPronostic", new { id = pronostic.Id }, pronostic);
        }

        // DELETE: api/Pronostics/5
        [Authorize(Policy = "ApiAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePronostic([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pronostic = await _context.Pronostic.SingleOrDefaultAsync(m => m.Id == id);
            if (pronostic == null)
            {
                return NotFound();
            }

            _context.Pronostic.Remove(pronostic);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool PronosticExists(long id)
        {
            return _context.Pronostic.Any(e => e.Id == id);
        }


    }
}