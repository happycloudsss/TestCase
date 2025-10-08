namespace TestCase.Entities
{
    public class Attachment : BaseEntity
    {
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public long Size { get; set; }
    }
}