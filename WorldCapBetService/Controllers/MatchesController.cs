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
    [Route("api/Matches")]
    public class MatchesController : Controller
    {
        private readonly Context _context;
        private readonly MatchManager _matchDAO;
        private readonly IMapper _mapper;

        public MatchesController(Context context, IMapper mapper)
        {
            _context = context;
            _matchDAO = new MatchManager(context, mapper);
            _mapper = mapper;
        }

        // GET: api/Matches
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiAdmin")]
        [HttpGet]
        public IList<MatchViewModel> GetMatch()
        {
            var matches = _context.Match.Include("Team1").Include("Team2").ToList();
            var result = _mapper.Map<IList<MatchViewModel>>(matches);

            foreach (var match in result)
            {
                match.Date = match.Date.ToUniversalTime();
            }

            return result;
        }

        // GET: api/Matches/5
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatch([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var match = await _context.Match.SingleOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<MatchViewModel>(match);
            result.Date = result.Date.ToUniversalTime();

            return Ok(result);
        }

        // PUT: api/Matches/5
        [Authorize(Policy = "ApiAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch([FromRoute] long id, [FromBody] Match match)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != match.Id)
            {
                return BadRequest();
            }
            match.Date = match.Date.ToLocalTime();
            _context.Entry(match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(id))
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

        // POST: api/Matches
        [Authorize(Policy = "ApiAdmin")]
        [HttpPost]
        public async Task<IActionResult> PostMatch([FromBody] Match match)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Entry(match.Team1).State = EntityState.Unchanged;
            _context.Entry(match.Team2).State = EntityState.Unchanged;
            match.Date = match.Date.ToLocalTime();
            _context.Match.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMatch", new { id = match.Id }, match);
        }

        // DELETE: api/Matches/5
        [Authorize(Policy = "ApiAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var match = await _context.Match.Include("Pronostics").SingleOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            _context.Match.Remove(match);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool MatchExists(long id)
        {
            return _context.Match.Any(e => e.Id == id);
        }

        // GET: api/Pronostic/5
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiUser")]
        [HttpGet("Pronostic/{id}")]
        public IActionResult GetMatchesWithPronosticFromUser([FromRoute] string id)
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

            var result = _matchDAO.GetMatchesWithPronosticFromUser(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/Pronostic/5
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiUser")]
        [HttpGet("TodayPronostic/{id}")]
        public IActionResult GetTodayMatchesWithPronosticFromUser([FromRoute] string id)
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

            var result = _matchDAO.GetTodayMatchesWithPronosticFromUser(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }



    }
}