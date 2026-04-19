namespace OSAWebAPI.Models
{
    public class MonitoringSubmission
    {
        public int Id { get; set; }
        public string? DocumentType { get; set; }
        public string? Municipality { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
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
        public List<MunicipalityMonthStatus> Municipalities { get; set; } = new();
    }

    public class MunicipalityMonthStatus
    {
        public string? Municipality { get; set; }
        public List<MonthStatus> Months { get; set; } = new();
    }

    public class MonthStatus
    {
        public int Month { get; set; }
        public string? MonthName { get; set; }
        public string? Status { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public string? Remarks { get; set; }
    }
}