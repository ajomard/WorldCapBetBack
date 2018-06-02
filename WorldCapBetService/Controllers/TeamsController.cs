using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCapBetService.BLL;
using WorldCapBetService.Models.Entities;
using WorldCapBetService.ViewModels;

namespace WorldCapBetService.Controllers
{
    [Produces("application/json")]
    [Route("api/Teams")]
    public class TeamsController : Controller
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public TeamsController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Teams
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiAdmin")]
        [HttpGet]
        public IEnumerable<TeamViewModel> GetTeam()
        {
            var teams = _context.Team.OrderBy(t => t.Name);

            var result = _mapper.Map<IList<TeamViewModel>>(teams);
            return result;
        }

        // GET: api/Teams/5
        [ResponseCache(CacheProfileName = "Never")]
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


            var result = _mapper.Map<TeamViewModel>(team);

            return Ok(result);
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

            return Ok();
        }

        private bool TeamExists(long id)
        {
            return _context.Team.Any(e => e.Id == id);
        }
    }
}