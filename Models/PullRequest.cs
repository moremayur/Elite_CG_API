namespace Elite_CG_API.Models
{
    public class PullRequest
    {
        public string? id { get; set; }
        public string? prNumber { get; set; }
        public string? title { get; set; }
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }
        public DateTime? merged { get; set; }
        public DateTime? closed { get; set; }
        public string? author { get; set; }
        public string? url { get; set; }
        public double? totalPRMergedHrs { get; set; } = 0;
        public double? totalPRMergedMinutes { get; set; } = 0;
        public double? totalPRMergedDays { get; set; } = 0;
    }
}
