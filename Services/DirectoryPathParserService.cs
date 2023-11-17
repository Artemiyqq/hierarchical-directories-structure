namespace HierarchicalDirectoryStructure.Services
{
    public class DirectoryPathParserService
    {   // The zero index in the returned list must be the root
        public List<string> Parse(string path)
        {
            string[] pathSegments = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            List<string> directoryNames = new(pathSegments);

            if (directoryNames.Count == 0)
            {
                throw new ArgumentException("Invalid or empty path.", nameof(path));
            }

            return directoryNames;
        }
    }
}
