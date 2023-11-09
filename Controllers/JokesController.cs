using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JokesWebApp.Data;
using JokesWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JokesWebApp.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index() 
        {
            return View(await _context.Jokes.ToListAsync());
        }
        public async Task<IActionResult> ShowSearchForm() 
        {
            return View();
        }
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase) 
        {
            return View("Index", await _context.Jokes.Where(j => j.JokeQuestion.Contains(SearchPhrase)).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)  
            {
                return NotFound();
            }

            var joke = await _context.Jokes.FirstOrDefaultAsync(m => m.Id == id);

            if(joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }
        
        [Authorize]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JokeQuestion,JokeAnswer")] Joke joke)
        {
            if(ModelState.IsValid)
            {
                _context.Add(joke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(joke);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id) {
            if(id == null)
            {
                return NotFound();
            }

            var joke = await _context.Jokes.FindAsync(id);
            if(joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JokeQuestion,JokeAnswer")] Joke joke) 
        {
            if(id != joke.Id) 
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if(!JokeExists(joke.Id))
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
            return View(joke);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var joke = await _context.Jokes
                .FirstOrDefaultAsync(m => m.Id == id);

            if(joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var joke = await _context.Jokes.FindAsync(id);
            _context.Jokes.Remove(joke);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool JokeExists(int id)
        {
            return _context.Jokes.Any(e => e.Id == id);
        }
    }
}