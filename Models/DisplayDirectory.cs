namespace HierarchicalDirectoryStructure.Models
{
    public class DisplayDirectory
    {
        public string Name { get; set; }
        public List<DisplayDirectory> Children { get; set; }
    }
}
