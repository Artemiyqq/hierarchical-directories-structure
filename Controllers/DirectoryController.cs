using HierarchicalDirectoryStructure.Infrastructure;
using HierarchicalDirectoryStructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HierarchicalDirectoryStructure.Controllers
{
    public class DirectoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DirectoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string directoryName = "Creating Digital Images")
        {
            Models.Directory? directory = _context.Directories
                .Include(d => d.Children)
                .SingleOrDefault(d => d.Name == directoryName);

            if (directory == null) return NotFound();

            DisplayDirectory displayDirectory = new()
            {
                Name = directory.Name!,
                Children = directory.Children.Select(c => new DisplayDirectory { Name = c.Name! }).ToList()
            };

            return View(displayDirectory);
        }
    }
}