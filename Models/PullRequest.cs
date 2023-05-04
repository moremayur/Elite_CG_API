namespace Elite_CG_API.Models
{
    public class PullRequest
    {
        public string? id { get; set; }
        public string? title { get; set; }
        public DateTime? created { get; set; }
        public DateTime? updated { get; set; }
        public DateTime? merged { get; set; }
        public DateTime? closed { get; set; }
        public string? author { get; set; }
        public string? url { get; set; }
        public double? totalPRMergedHrs { get; set; }
        public double? totalPRMergedMinutes { get; set; }
        public double? totalPRMergedDays { get; set; }
    }
}
