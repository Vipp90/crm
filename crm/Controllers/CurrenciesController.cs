#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using crm.Models;
using Newtonsoft.Json;
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;


namespace crm.Controllers
{
    [ApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly CurrencyContext _context;

        public CurrenciesController(CurrencyContext context)
        {
            _context = context;
        }

        // GET: api/Currencies
        [HttpGet]
        
        public string GetCurrency()
        {
            var result = from current in _context.Currency select new { current.id, current.symbol, current.rate, current.updated_at };
            var jsonresult = JsonConvert.SerializeObject(result);
            jsonresult = "{\"currencies\":" + jsonresult;
            return jsonresult;
        }

        // GET: api/Currencies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Currency>> GetCurrency(int id)
        {
            var currency = await _context.Currency.FindAsync(id);

            if (currency == null)
            {
                return NotFound();
            }

            return currency;
        }

        // PUT: api/Currencies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurrency(int id, Currency currency)
        {
            if (id != currency.id)
            {
                return BadRequest();
            }

            _context.Entry(currency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyExists(id))
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

        // POST: api/Currencies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Currency>> PostCurrency(Currency currency)
        {
            _context.Currency.Add(currency);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCurrency", new { id = currency.id }, currency);
        }

        // DELETE: api/Currencies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrency(int id)
        {
            var currency = await _context.Currency.FindAsync(id);
            if (currency == null)
            {
                return NotFound();
            }

            _context.Currency.Remove(currency);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CurrencyExists(int id)
        {
            return _context.Currency.Any(e => e.id == id);
        }
    }
}
