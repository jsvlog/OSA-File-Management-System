namespace OSAWebAPI.Models
{
    public class DashboardStats
    {
        public int TotalRecords { get; set; }
        public int ToRegionCount { get; set; }
        public int FromRegionCount { get; set; }
        public int ActionableCount { get; set; }
        public int ThisMonthCount { get; set; }
        public List<TypeCount> ByType { get; set; } = new();
        public List<MunicipalityCount> ByMunicipality { get; set; } = new();
        public List<MonthlyCount> MonthlyTrend { get; set; } = new();
    }
    
    public class TypeCount
    {
        public string? Type { get; set; }
        public int Count { get; set; }
    }
    
    public class MunicipalityCount
    {
        public string? Municipality { get; set; }
        public int Count { get; set; }
    }
    
    public class MonthlyCount
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }
    }
}
