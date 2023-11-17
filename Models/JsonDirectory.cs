namespace hierarchical_directory_structure.Models
{
    public class JsonDirectory
    {
        public string Name { get; set; }
        public List<JsonDirectory> Children { get; set; }
    }
}
