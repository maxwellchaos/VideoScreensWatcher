using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoScreensWatcher.Data;
using VideoScreensWatcher.Models;

namespace VideoScreensWatcher.Controllers
{
    public class ComputersController : Controller
    {
        private readonly VideoScreensWatcherContext _context;

        public ComputersController(VideoScreensWatcherContext context)
        {
            _context = context;
        }

        // GET: Computers
        public async Task<IActionResult> Index()
        {
            return _context.Computer != null ?
                        View(await _context.Computer.ToListAsync()) :
                        Problem("Entity set 'VideoScreensWatcherContext.Computer'  is null.");
        }

        // GET: Computers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Computer == null)
            {
                return NotFound();
            }

            var computer = await _context.Computer
                .Include(comp => comp.Logs.OrderByDescending(log => log.OnlineDateTime))
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computer == null)
            {
                return NotFound();
            }

            return View(computer);
        }

        // GET: Computers/Block/5
        public async Task<IActionResult> Block(int? id)
        {
            if (id == null || _context.Computer == null)
            {
                return NotFound();
            }

            var computer = await _context.Computer.FirstOrDefaultAsync(m => m.Id == id);

            Computer.UpdateStatus(_context, computer, Statuses.Blocked);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Computers/Unblock/5
        public async Task<IActionResult> Unblock(int? id)
        {
            if (id == null || _context.Computer == null)
            {
                return NotFound();
            }

            var computer = await _context.Computer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (computer == null)
            {
                return NotFound();
            }
            Computer.UpdateStatus(_context, computer, Statuses.Unblocked);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private bool ComputerExists(int id)
        {
          return (_context.Computer?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
