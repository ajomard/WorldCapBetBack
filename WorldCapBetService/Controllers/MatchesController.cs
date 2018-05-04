using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly MatchDAO _matchDAO;

        public MatchesController(Context context)
        {
            _context = context;
            _matchDAO = new MatchDAO(context);
        }

        // GET: api/Matches
        [Authorize(Policy = "ApiUser")]
        [HttpGet]
        public IEnumerable<Match> GetMatch()
        {
            var matches = _context.Match.Include("Team1").Include("Team2");

            return matches;
        }

        // GET: api/Matches/5
        [Authorize(Policy = "ApiUser")]
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

            return Ok(match);
        }

        private bool MatchExists(long id)
        {
            return _context.Match.Any(e => e.Id == id);
        }

        // GET: api/Pronostic/5
        [Authorize(Policy = "ApiUser")]
        [HttpGet("Pronostic/{id}")]
        public IActionResult GetMatchesWithPronosticFromUser([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _matchDAO.GetMatchesWithPronosticFromUser(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

     
    }
}