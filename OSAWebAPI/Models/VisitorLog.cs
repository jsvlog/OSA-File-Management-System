namespace OSAWebAPI.Models
{
    public class VisitorLog
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Purpose { get; set; }
        public string? Office { get; set; }
        public string? Municipality { get; set; }
        public string? Barangay { get; set; }
        public DateTime? EntryTime { get; set; }
    }
}
