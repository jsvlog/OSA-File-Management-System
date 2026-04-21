namespace OSAWebAPI.Models
{
    public class MonitoringSubmission
    {
        public int Id { get; set; }
        public string? DocumentType { get; set; }
        public string? Municipality { get; set; }
        public string? Barangay { get; set; }
        public int Year { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public string? PdfLink { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MonitoringDocType
    {
        public string Key { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Icon { get; set; } = "";
        public string Color { get; set; } = "";
    }

    public class MonitoringStatusGrid
    {
        public string? DocumentType { get; set; }
        public int Year { get; set; }
        public List<int> YearColumns { get; set; } = new();
        public List<MunicipalityStatusRow> Municipalities { get; set; } = new();
        public bool IsBarangayDocumentType { get; set; }
    }

    public class MunicipalityStatusRow
    {
        public string? Municipality { get; set; }
        public string? Barangay { get; set; }
        public Dictionary<int, MunicipalityStatus> YearStatuses { get; set; } = new();
    }

    public class MunicipalityStatus
    {
        public int SubmissionId { get; set; }
        public string? Municipality { get; set; }
        public string? Barangay { get; set; }
        public int Year { get; set; }
        public string? Status { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string? Remarks { get; set; }
        public string? PdfLink { get; set; }
    }
}