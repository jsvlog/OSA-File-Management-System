namespace OSAWebAPI.Models
{
    public class InventoryModel
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Location { get; set; }
        public string? Remarks { get; set; }
        public string? ScannedCopy { get; set; }
    }
}