namespace Elite_CG_API.Models
{
    public class TimeToMerge
    {
        public int total_count { get; set; }
        public List<PullRequest>? listPRs { get; set; }
    }
}
