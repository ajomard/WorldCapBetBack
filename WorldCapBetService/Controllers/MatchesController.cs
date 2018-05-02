using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCapBetService.Data;
using WorldCapBetService.Models;
using WorldCapBetService.Models.Entities;

namespace WorldCapBetService.Controllers
{
    [Produces("application/json")]
    [Route("api/Matches")]
    public class MatchesController : Controller
    {
        private readonly Context _context;

        public MatchesController(Context context)
        {
            _context = context;
        }

        // GET: api/Matches
        [HttpGet]
        public IEnumerable<Match> GetMatch()
        {
            var matches = _context.Match.Include("Team1").Include("Team2");

            return matches;
        }

        // GET: api/Matches/5
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

            return Ok(match);
        }

        // PUT: api/Matches/5
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
        [HttpPost]
        public async Task<IActionResult> PostMatch([FromBody] Match match)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Entry(match.Team1).State = EntityState.Unchanged;
            _context.Entry(match.Team2).State = EntityState.Unchanged;
            _context.Match.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMatch", new { id = match.Id }, match);
        }

        // DELETE: api/Matches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch([FromRoute] long id)
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

            _context.Match.Remove(match);
            await _context.SaveChangesAsync();

            return Ok(match);
        }

        private bool MatchExists(long id)
        {
            return _context.Match.Any(e => e.Id == id);
        }

        // GET: api/Pronostic/5
        [HttpGet("Pronostic/{id}")]
        public async Task<IActionResult> GetMatchesWithPronosticFromUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = from matches in _context.Match.Include("Team1").Include("Team2")
                        from pronostics in _context.Pronostic.Include("Match").Include("User").Where(prono => prono.Match.Id == matches.Id && prono.User.Id == id).DefaultIfEmpty()
                        select new {
                            matches.Id,
                            matches.Date,
                            matches.Team1,
                            matches.Team2,
                            matches.ScoreTeam1,
                            matches.ScoreTeam2,
                            pronostic = new
                            {
                                scoreTeam1 = pronostics != null ? pronostics.ScoreTeam1 : null,
                                scoreTeam2 = pronostics != null ? pronostics.ScoreTeam2 : null
                            }
                        };


            var result = await query.ToListAsync();



            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}