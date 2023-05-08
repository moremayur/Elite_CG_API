namespace Elite_CG_API.Models
{
    public class RepoStatatics
    {
        public int? total_commits { get; set; } = 0;
        public int? open_pr { get; set; } = 0;
        public int? closed_pr { get; set; } = 0;
        public int? open_issue { get; set; } = 0;
        public int? closed_issue { get; set; } = 0;
    }
}
