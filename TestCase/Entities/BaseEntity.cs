namespace TestCase.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string CreatedUser { get; set; } = string.Empty;
        public DateTime CreatedTime { get; set; }
        public string UpdateUser { get; set; } = string.Empty;
        public DateTime UpdatedTime { get; set; }
    }
}