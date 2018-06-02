using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorldCapBetService.BLL;

namespace WorldCapBetService.Controllers
{
    [Produces("application/json")]
    [Route("api/Charts")]
    public class ChartsController : Controller
    {
        private readonly Context _context;
        private readonly ChartsManager _chartsManager;

        public ChartsController(Context context)
        {
            _context = context;
            _chartsManager = new ChartsManager(context);
        }

        // GET: api/ChartsAvererage/5
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiUser")]
        [HttpGet("Average/{userId}")]
        public ActionResult GetChartsAvererage([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chart = _chartsManager.GetAverageStatsFromUser(userId);


            if (chart == null)
            {
                return NotFound();
            }

            return Ok(chart);
        }

        // GET: api/ChartsAvererage/5
        [ResponseCache(CacheProfileName = "Never")]
        [Authorize(Policy = "ApiUser")]
        [HttpGet("AverageScore/{userId}")]
        public ActionResult GetChartsAvererageScore([FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var chart = _chartsManager.GetAverageScoreFromUser(userId);


            if (chart == null)
            {
                return NotFound();
            }

            return Ok(chart);
        }
    }
}