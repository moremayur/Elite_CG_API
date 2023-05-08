using Elite_CG_API.Models;
using Newtonsoft.Json.Linq;

namespace Elite_CG_API.Common
{
    public class CommonData
    {
        private static IConfiguration AppSetting = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        public static string Base_URL = AppSetting["CommonFields:Base_URL"];
        public static string Repo_Owner = AppSetting["CommonFields:Repo_Owner"];
        public static string Repo_Name = AppSetting["CommonFields:Repo_Name"];
        public static string Token = AppSetting["CommonFields:Token"];

        public HttpClient Client { get; set; }

        public CommonData() {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Token);
            Client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
            Client.DefaultRequestHeaders.Add("User-Agent", Repo_Name);
        }

        internal async Task<string> HttpCallHandler(string webURL)
        {
            using (var response = await Client.GetAsync(webURL))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        internal async Task<TimeToMerge> GetGitHubMergedPullRequests(DateTime fromDate, DateTime toDate)
        {            
            TimeToMerge prList = new TimeToMerge();
            // 2023-04-24
            try
            {
                string webURL = Base_URL + "search/issues?q=repo:" + Repo_Owner + "/" + Repo_Name + "+is:pr+is:merged+created:>" + fromDate.ToString("yyyy-MM-dd") + "&sort=created&order=asc";
                string apiResponse = await HttpCallHandler(webURL);

                if (!string.IsNullOrEmpty(apiResponse))
                {
                    var prObj = JObject.Parse(apiResponse);
                    var totalPr = Convert.ToString(prObj["total_count"]);

                    var vArray = (JArray)prObj["items"];

                    if (vArray != null)
                    {
                        List<PullRequest> listPRs = new List<PullRequest>();
                        foreach (JObject value in vArray)
                        {
                            PullRequest _prObj = new PullRequest();
                            _prObj.id = Convert.ToString(value["id"]);
                            _prObj.title = Convert.ToString(value["title"]);
                            _prObj.prNumber = Convert.ToString(value["number"]);

                            var pr_created = new DateTimeOffset(Convert.ToDateTime(value["created_at"]), TimeSpan.FromHours(0)).ToLocalTime().DateTime;
                            _prObj.created = pr_created;

                            if (toDate != DateTime.MinValue && (pr_created > toDate))
                            {
                                break;
                            }

                            _prObj.updated = new DateTimeOffset(Convert.ToDateTime(value["updated_at"]), TimeSpan.FromHours(0)).ToLocalTime().DateTime;
                            _prObj.closed = new DateTimeOffset(Convert.ToDateTime(value["closed_at"]), TimeSpan.FromHours(0)).ToLocalTime().DateTime;

                            var user = JObject.Parse(Convert.ToString(value["user"]));
                            _prObj.author = Convert.ToString(user["login"]);

                            var pullRequest = JObject.Parse(Convert.ToString(value["pull_request"]));
                            _prObj.url = Convert.ToString(pullRequest["url"]);

                            var pr_merge = new DateTimeOffset(Convert.ToDateTime(pullRequest["merged_at"]), TimeSpan.FromHours(0)).ToLocalTime().DateTime;
                            _prObj.merged = pr_merge;

                            _prObj.totalPRMergedHrs = CalculateTotalTime(pr_created, pr_merge, "HH");
                            _prObj.totalPRMergedMinutes = CalculateTotalTime(pr_created, pr_merge, "MM");
                            _prObj.totalPRMergedDays = CalculateTotalTime(pr_created, pr_merge, "DD");

                            listPRs.Add(_prObj);
                        }
                        prList.listPRs = listPRs;
                        prList.total_count = (listPRs.Count > 0) ? listPRs.Count : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return prList;
        }

        internal static double? CalculateTotalTime(DateTime created_start, DateTime created_end, string timeSelection)
        {
            TimeSpan diff = created_end - created_start;
            if (timeSelection.Equals("HH"))
            {
                return Convert.ToDouble(diff.TotalHours.ToString("0.00"));
            }
            else if (timeSelection.Equals("MM"))
            {
                return Convert.ToDouble(diff.TotalMinutes.ToString("0.00"));
            }
            else if (timeSelection.Equals("DD"))
            {
                return Convert.ToDouble(diff.TotalDays.ToString("0.00"));
            }
            return 0;
        }
    }
}
