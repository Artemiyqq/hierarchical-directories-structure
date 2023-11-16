namespace HierarchicalDirectoryStructure.Models
{
    public class Directory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentId { get; set; }

        public Directory? Parent { get; set; }
        public List<Directory> Children { get; set; } = new List<Directory>();
    }
}
