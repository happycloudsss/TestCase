namespace TestCase.Entities
{
    public class ProjectTestCase : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ModuleId { get; set; }
    }
}