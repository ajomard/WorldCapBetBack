using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCapBetService.Data;
using WorldCapBetService.Models.Entities;

namespace WorldCapBetService.Controllers
{
    [Produces("application/json")]
    [Route("api/Teams")]
    public class TeamsController : Controller
    {
        private readonly Context _context;

        public TeamsController(Context context)
        {
            _context = context;
        }

        // GET: api/Teams
        //[Authorize(Policy = "ApiUser")]
        [Authorize(Policy = "ApiAdmin")]
        [HttpGet]
        public IEnumerable<Team> GetTeam()
        {
            return _context.Team.OrderBy(t => t.Name);
        }

        // GET: api/Teams/5
        //[Authorize(Policy = "ApiUser")]
        [Authorize(Policy = "ApiAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = await _context.Team.SingleOrDefaultAsync(m => m.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            return Ok(team);
        }

        // PUT: api/Teams/5
        [Authorize(Policy = "ApiAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam([FromRoute] long id, [FromBody] Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != team.Id)
            {
                return BadRequest();
            }

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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

        // POST: api/Teams
        [Authorize(Policy = "ApiAdmin")]
        [HttpPost]
        public async Task<IActionResult> PostTeam([FromBody] Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Team.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeam", new { id = team.Id }, team);
        }

        // DELETE: api/Teams/5
        [Authorize(Policy = "ApiAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = await _context.Team.SingleOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            //delete associated matches
            var matches = _context.Match.Include("Pronostics").Where(m => m.Team1.Id == team.Id || m.Team2.Id == team.Id);
            _context.Match.RemoveRange(matches);

            _context.Team.Remove(team);
            await _context.SaveChangesAsync();

            return Ok(team);
        }

        private bool TeamExists(long id)
        {
            return _context.Team.Any(e => e.Id == id);
        }
    }
}