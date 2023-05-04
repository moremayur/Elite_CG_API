using Elite_CG_API.Common;
using Elite_CG_API.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elite_CG_API.DataAccess
{
    public class TimeToMergeProvider
    {   
        public TimeToMergeProvider() {            
        }

        internal async Task<TimeToMerge> GetTimeToMergePRData(DateTime fromDate, DateTime toDate)
        {
            TimeToMerge prList = new TimeToMerge();
            // 2023-04-24
            using (var response = await new CommonData().Client.GetAsync(CommonData.Base_URL + "search/issues?q=repo:" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "+is:pr+is:merged+created:>"+ fromDate.ToString("yyyy-MM-dd") + "&sort=created&order=asc"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();

                var prObj = JObject.Parse(apiResponse);
                var totalPr = Convert.ToString(prObj["total_count"]);                

                var vArray = (JArray)prObj["items"];
                
                if (vArray != null) {
                    List<PullRequest> listPRs = new List<PullRequest>();
                    int cnt_pr = 0;
                    foreach (JObject value in vArray)
                    {
                        PullRequest _prObj = new PullRequest();
                        _prObj.id = Convert.ToString(value["id"]);
                        _prObj.title = Convert.ToString(value["title"]);

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

                        TimeSpan diff = pr_merge - pr_created;
                        _prObj.totalPRMergedHrs = diff.TotalHours;
                        _prObj.totalPRMergedMinutes = diff.TotalMinutes;
                        _prObj.totalPRMergedDays = diff.TotalDays;

                        listPRs.Add(_prObj);
                        cnt_pr++;
                    }
                    prList.listPRs = listPRs;
                    prList.total_count = cnt_pr.ToString();
                }
            }

            return prList;
        }
    }
}
