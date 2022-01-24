#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using crm.Models;
using System.Net;
using Newtonsoft.Json;
using System.Web.Helpers;

namespace crm.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly CurrencyContext _context;

        public CurrencyController(CurrencyContext context)
        {
            _context = context;
        }


        public string UpdateCurrencies()
        {
            var rates_json = new WebClient().DownloadString("https://api.exchangerate.host/latest?base=USD");
            var currencies_symbols_json = new WebClient().DownloadString("https://openexchangerates.org/api/currencies.json");
            currencies_symbols_json = "{\"currency\":" + currencies_symbols_json + "}";
            Rates rates = JsonConvert.DeserializeObject<Rates>(rates_json);
            Currencies_Dictionary names = JsonConvert.DeserializeObject<Currencies_Dictionary>(currencies_symbols_json);
            //Currencies_Dictionary names = JsonConvert.DeserializeObject<Currencies_Dictionary>(currencies_json);
            Currency currency;
            bool add;
            string result;
            foreach (var item in rates.Rates_Dictionary)
            {
                add = false;
                currency = _context.Currency.FirstOrDefault(c => c.symbol == item.Key);

                if (currency == null)
                {
                    currency = new Currency();
                    currency.symbol = item.Key;
                    add = true;
                    currency.created_at = rates.Date.DateTime;
                }
                currency.rate = item.Value;
                currency.updated_at = rates.Date.DateTime;
                names.Currencies.TryGetValue(item.Key, out var name);
                currency.name = name;
                try
                {
                    if (add == true)
                    {
                        _context.Currency.Add(currency);
                    }
                    else if (currency.is_sync == true)
                    {
                       _context.Entry(currency).State = EntityState.Modified;
                    }
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                   
                    result = e.ToString();

                    return result;
                }
            }
            result = "Currencies Updated";


            return result;
        }






        // GET: Currency
        public async Task<IActionResult> Index(string sortOrder, string searchString,string filter)
        {

            ViewBag.SymbolSortParm = String.IsNullOrEmpty(sortOrder) ? "symbol_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.RateSortParm = sortOrder == "Rate" ? "rate_desc" : "Rate";
            ViewBag.SyncSortParm = sortOrder == "Is_sync" ? "is_sync_desc" : "Is_sync";
            ViewBag.CreatedSortParm = sortOrder == "Created" ? "created_desc" : "Created";
            ViewBag.UpdatedSortParm = sortOrder == "Updated" ? "updated_desc" : "Updated";
            ViewBag.GhostedSortParm = sortOrder == "Ghosted" ? "ghosted_desc" : "Ghosted";

            var currencies = from s in _context.Currency
                           select s;
            if (!String.IsNullOrEmpty(searchString)&& (filter == "symbol"))
            {
                currencies = currencies.Where(s => s.symbol.Contains(searchString));
                                      
            }

            if (!String.IsNullOrEmpty(searchString) && (filter == "name"))
            {
                currencies = currencies.Where(s => s.name.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "symbol_desc":
                    currencies = currencies.OrderByDescending(s => s.symbol);
                    break;
                case "Name":
                  currencies = currencies.OrderBy(s => s.name);
                   break;
                case "name_desc":
                    currencies = currencies.OrderByDescending(s => s.name);
                    break;
                case "Rate":
                    currencies = currencies.OrderBy(s => s.rate);
                    break;
                case "rate_desc":
                    currencies = currencies.OrderByDescending(s => s.rate);
                    break;
                case "Is_sync":
                    currencies = currencies.OrderBy(s => s.is_sync);
                    break;
                case "is_sync_desc":
                    currencies = currencies.OrderByDescending(s => s.is_sync);
                    break;
                case "Created":
                    currencies = currencies.OrderBy(s => s.created_at);
                    break;
                case "created_desc":
                    currencies = currencies.OrderByDescending(s => s.created_at);
                    break;
                case "Updated":
                    currencies = currencies.OrderBy(s => s.updated_at);
                    break;
                case "updated_desc":
                    currencies = currencies.OrderByDescending(s => s.updated_at);
                    break;
                case "Ghosted":
                    currencies = currencies.OrderBy(s => s.ghosted);
                    break;
                case "ghosted_desc":
                    currencies = currencies.OrderByDescending(s => s.ghosted);
                    break;
                default:
                    currencies = currencies.OrderBy(s => s.symbol);
                    break;
            }

            return View(await currencies.ToListAsync());
                
        }

        // GET: Currency/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currency = await _context.Currency
                .FirstOrDefaultAsync(m => m.id == id);
            if (currency == null)
            {
                return NotFound();
            }

            return View(currency);
        }

        // GET: Currency/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Currency/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,symbol,name,rate,is_sync,created_at,updated_at,ghosted")] Currency currency)
        {
            if (ModelState.IsValid)
            {   currency.created_at = DateTime.Now;
                currency.updated_at = DateTime.Now;
                _context.Add(currency);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(currency);
        }

        // GET: Currency/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currency = await _context.Currency.FindAsync(id);
            if (currency == null)
            {
                return NotFound();
            }
            return View(currency);
        }

        // POST: Currency/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,symbol,name,rate,is_sync,created_at,updated_at,ghosted")] Currency currency)
        {
            if (id != currency.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(currency);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurrencyExists(currency.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(currency);
        }

        // GET: Currency/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currency = await _context.Currency
                .FirstOrDefaultAsync(m => m.id == id);
            if (currency == null)
            {
                return NotFound();
            }

            return View(currency);
        }

        // POST: Currency/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var currency = await _context.Currency.FindAsync(id);
            currency.ghosted = true;
            _context.Entry(currency).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrencyExists(int id)
        {
            return _context.Currency.Any(e => e.id == id);
        }
    }
}
