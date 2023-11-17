using HierarchicalDirectoryStructure.Infrastructure;
using HierarchicalDirectoryStructure.Models;
using HierarchicalDirectoryStructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace HierarchicalDirectoryStructure.Controllers
{
    public class DirectoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DirectoryPathParserService _directoryPathParserService;
        private readonly DirectoriesCreationService _directoriesCreationService;

        public DirectoryController(ApplicationDbContext context,
                                   DirectoryPathParserService folderPathParserService,
                                   DirectoriesCreationService directoryCreationService)
        {
            _context = context;
            _directoryPathParserService = folderPathParserService;
            _directoriesCreationService = directoryCreationService;
        }

        public IActionResult Index(string directoryName = "Creating Digital Images")
        {
            Models.Directory? directory = _context.Directories
                .Include(d => d.Children)
                .FirstOrDefault(d => d.Name == directoryName);

            if (directory == null) return NotFound();

            DisplayDirectory displayDirectory = new()
            {
                Name = directory.Name!,
                Children = directory.Children.Select(c => new DisplayDirectory { Name = c.Name! }).ToList()
            };

            return View(displayDirectory);
        }

        public IActionResult ImportDirectoryStructure(string path)
        {
            try
            {
                List<string> directoriesNames = _directoryPathParserService.Parse(path);
                string rootDirectoryName = _directoriesCreationService.CreateFromPath(directoriesNames);

                string redirectUrl = Url.Action("Index", new { directoryName = rootDirectoryName })!;

                return Json(new { success = true, redirectUrl });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error importing directory structure." });
            }
        }

        [HttpPost]
        public IActionResult ImportFromJson(IFormFile jsonFile)
        {
            try
            {
                using var reader = new StreamReader(jsonFile.OpenReadStream());
                var jsonString = reader.ReadToEnd();

                var updatedRootDirectory = _directoriesCreationService.ImportFromJson(jsonString);

                return Json(new { success = true, redirectUrl = Url.Action("Index", new { directoryName = updatedRootDirectory }) });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error importing from JSON." });
            }
        }
    }
}