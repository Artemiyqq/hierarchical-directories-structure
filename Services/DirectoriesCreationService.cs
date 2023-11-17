using hierarchical_directory_structure.Models;
using HierarchicalDirectoryStructure.Infrastructure;
using Newtonsoft.Json;

namespace HierarchicalDirectoryStructure.Services
{
    public class DirectoriesCreationService
    {
        private readonly ApplicationDbContext _context;

        public DirectoriesCreationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Must return the name of the root directory
        public string CreateFromPath(List<string> directoriesNames)
        {
            if (directoriesNames.Count == 0)
            {
                throw new ArgumentException("List of directories names is empty.", nameof(directoriesNames));
            }

            List<int> createdDirectoriesIds = new();

            foreach (string directoryName in directoriesNames)
            {
                Models.Directory newDirectory = new() { Name = directoryName };

                _context.Directories.Add(newDirectory);
                _context.SaveChanges();

                createdDirectoriesIds.Add(newDirectory.Id);
            }

            BuilRelationshipsForPath(createdDirectoriesIds);

            return directoriesNames.First();
        }

        private void BuilRelationshipsForPath(List<int> createdDirectoriesIds)
        {
            if (createdDirectoriesIds.Count == 0)
            {
                throw new InvalidOperationException("Not enough directories to build relationships.");
            }

            int parentId = createdDirectoriesIds.First();

            foreach (int directoryId in createdDirectoriesIds.Skip(1))
            {
                Models.Directory directory = _context.Directories.Find(directoryId)!;

                directory.ParentId = parentId;

                _context.SaveChanges();

                parentId = directoryId;
            }
        }

        private void ProcessAndSaveJsonDirectories(List<JsonDirectory> jsonDirectories, int? parentId = null)
        {
            foreach (JsonDirectory jsonDirectory in jsonDirectories)
            {
                Models.Directory newDirectory = new() { Name = jsonDirectory.Name, ParentId = parentId };

                _context.Directories.Add(newDirectory);
                _context.SaveChanges();

                // Recursively process children
                if (jsonDirectory.Children.Count > 0)
                {
                    ProcessAndSaveJsonDirectories(jsonDirectory.Children, newDirectory.Id);
                }
            }
        }

        public string ImportFromJson(string jsonString)
        {
            // Deserialize the JSON string to a list of JsonDirectory
            var jsonDirectories = JsonConvert.DeserializeObject<List<JsonDirectory>>(jsonString);

            // Process the jsonDirectories and update the database, returning the root directory name
            string rootDirectoryName = jsonDirectories[0].Name;

            // Call the recursive method for processing and saving directories
            ProcessAndSaveJsonDirectories(jsonDirectories);

            return rootDirectoryName;
        }
    }
}
