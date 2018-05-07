using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCapBetService.Data;
using WorldCapBetService.Models.Entities;

namespace WorldCapBetService.Controllers
{
    [Produces("application/json")]
    [Route("api/Rankings")]
    public class RankingsController : Controller
    {
        private readonly Context _context;
        private readonly RankingManager _rankingManager;

        public RankingsController(Context context)
        {
            _context = context;
            _rankingManager = new RankingManager(context);
        }

        // GET: api/Rankings
        [HttpGet]
        public IEnumerable<Ranking> GetRankings()
        {
            return _context.Rankings.Include("User").OrderBy(r => r.Rank);
        }

        // GET: api/Rankings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRanking([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ranking = await _context.Rankings.Include("User").SingleOrDefaultAsync(m => m.Id == id);

            if (ranking == null)
            {
                return NotFound();
            }

            return Ok(ranking);
        }

        // PUT: api/Rankings/5
        [Authorize(Policy = "ApiAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRanking([FromRoute] int id, [FromBody] Ranking ranking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ranking.Id)
            {
                return BadRequest();
            }

            _context.Entry(ranking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RankingExists(id))
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

        // POST: api/Rankings
        [Authorize(Policy = "ApiAdmin")]
        [HttpPost]
        public async Task<IActionResult> PostRanking([FromBody] Ranking ranking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Rankings.Add(ranking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRanking", new { id = ranking.Id }, ranking);
        }

        // DELETE: api/Rankings/5
        [Authorize(Policy = "ApiAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRanking([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ranking = await _context.Rankings.SingleOrDefaultAsync(m => m.Id == id);
            if (ranking == null)
            {
                return NotFound();
            }

            _context.Rankings.Remove(ranking);
            await _context.SaveChangesAsync();

            return Ok(ranking);
        }

        private bool RankingExists(int id)
        {
            return _context.Rankings.Any(e => e.Id == id);
        }

        //[Authorize(Policy = "ApiAdmin")]
        [HttpPost("UpdateRankings")]
        public IActionResult UpdateRankings()
        {
            _rankingManager.UpdateRankings();

            return Ok();
        }

    }
}