using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.Domain.Entities;
using Data.Domain.Interfaces.Repositories;
using Data.Persistence;

namespace Cinema.Controllers
{
    public class CinemaInfoesController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ICinemaRepository _cinemaRepository;

        public CinemaInfoesController(DatabaseContext context, ICinemaRepository cinemaRepository)
        {
            _context = context;
            _cinemaRepository = cinemaRepository;
        }

        // GET: CinemaInfoes
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Cinemas.ToListAsync());
            return View(_cinemaRepository.getAllCinemas());
        }

        // GET: CinemaInfoes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaInfo = await _context.Cinemas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cinemaInfo == null)
            {
                return NotFound();
            }

            return View(cinemaInfo);
        }

        // GET: CinemaInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CinemaInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LocationId,Email,PhoneNumber")] CinemaInfo cinemaInfo)
        {
            if (ModelState.IsValid)
            {
                cinemaInfo.Id = Guid.NewGuid();
                _context.Add(cinemaInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cinemaInfo);
        }

        // GET: CinemaInfoes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaInfo = await _context.Cinemas.FindAsync(id);
            if (cinemaInfo == null)
            {
                return NotFound();
            }
            return View(cinemaInfo);
        }

        // POST: CinemaInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,LocationId,Email,PhoneNumber")] CinemaInfo cinemaInfo)
        {
            if (id != cinemaInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cinemaInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CinemaInfoExists(cinemaInfo.Id))
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
            return View(cinemaInfo);
        }

        // GET: CinemaInfoes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cinemaInfo = await _context.Cinemas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cinemaInfo == null)
            {
                return NotFound();
            }

            return View(cinemaInfo);
        }

        // POST: CinemaInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cinemaInfo = await _context.Cinemas.FindAsync(id);
            _context.Cinemas.Remove(cinemaInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CinemaInfoExists(Guid id)
        {
            return _context.Cinemas.Any(e => e.Id == id);
        }
    }
}
