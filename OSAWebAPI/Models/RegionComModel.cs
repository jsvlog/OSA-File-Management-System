namespace OSAWebAPI.Models
{
    public class RegionComModel
    {
        public int? Id { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string? TypeOfDocs { get; set; }
        public string? Addressee { get; set; }
        public string? SubjectParticulars { get; set; }
        public string? Details { get; set; }
        public string? RefNumber { get; set; }
        public string? Municipality { get; set; }
        public string? Barangay { get; set; }
        public string? ReceivedFrom { get; set; }
        public DateTime? DateSentOutToTeam { get; set; }
        public string? Receiver { get; set; }
        public string? Location { get; set; }
        public bool? ActionableDoc { get; set; }
        public DateTime? DateDeadline { get; set; }
        public string? Remarks { get; set; }
        public string? TrackingCode { get; set; }
        public string? Direction { get; set; }
        public string? NumberOfCopies { get; set; }
        public DateTime? DateSignBySA { get; set; }
        public DateTime? DateReceiveByRegion { get; set; }
        public DateTime? DateSentOutToRegion { get; set; }
        public string? LbcRefNumber { get; set; }
        public string? ScannedCopy { get; set; }
        public bool? IsHighlighted { get; set; }
        public bool? Visib { get; set; }
    }
}
