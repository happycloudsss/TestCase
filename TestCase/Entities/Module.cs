namespace TestCase.Entities
{
    public class Module : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string Owner1 { get; set; } = string.Empty;
        public string Owner2 { get; set; } = string.Empty;
    }
}