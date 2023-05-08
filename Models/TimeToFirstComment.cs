namespace Elite_CG_API.Models
{
    public class TimeToFirstComment
    {
        public string? pr_number { get; set; }
        public DateTime? created { get; set; }
        public DateTime? merged { get; set; }
        // public List<PRComment>? pr_comments { get; set; }

        public string? comment_owner { get; set; }
        public string? comment { get; set; }
        public DateTime? comment_created { get; set; }
        public double? totalHrsForFirstComment { get; set; } = 0;
        public double? totalMinForFirstComment { get; set; } = 0;
        public double? totalDayForFirstComment { get; set; } = 0;
    }
}
