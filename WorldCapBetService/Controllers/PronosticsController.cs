﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCapBetService.Models;

namespace WorldCapBetService.Controllers
{
    [Produces("application/json")]
    [Route("api/Pronostics")]
    public class PronosticsController : Controller
    {
        private readonly Context _context;

        public PronosticsController(Context context)
        {
            _context = context;
        }

        // GET: api/Pronostics
        [HttpGet]
        public IEnumerable<Pronostic> GetPronostic()
        {
            return _context.Pronostic;
        }

        // GET: api/Pronostics/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPronostic([FromRoute] long id)
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

            return Ok(pronostic);
        }

        // PUT: api/Pronostics/5
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
        [HttpPost]
        public async Task<IActionResult> PostPronostic([FromBody] Pronostic pronostic)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Pronostic.Add(pronostic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPronostic", new { id = pronostic.Id }, pronostic);
        }

        // DELETE: api/Pronostics/5
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

            return Ok(pronostic);
        }

        private bool PronosticExists(long id)
        {
            return _context.Pronostic.Any(e => e.Id == id);
        }
    }
}