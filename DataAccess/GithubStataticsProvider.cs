using Elite_CG_API.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Elite_CG_API.Models;

namespace Elite_CG_API.DataAccess
{
    public class GithubStataticsProvider
    {
        private CommonData _commonObj;

        public GithubStataticsProvider()
        {
            _commonObj = new CommonData();
        }

        internal async Task<RepoStatatics> GetStataticsData(DateTime fromDate, DateTime toDate)
        {
            RepoStatatics repoState = new RepoStatatics();

            // Total Commits
            string totalCommitApiUrl = CommonData.Base_URL + "search/commits?sort=committer-date&order=asc&q=repo:" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "+committer-date:>" + fromDate.ToString("yyyy-MM-dd");
            string totalCommitResponse = await _commonObj.HttpCallHandler(totalCommitApiUrl);

            if (!string.IsNullOrEmpty(totalCommitResponse))
            {
                var commitRes = JObject.Parse(totalCommitResponse);
                repoState.total_commits = Convert.ToInt32(Convert.ToString(commitRes["total_count"]));
            }

            // Total Open PR
            string getTotalOpenPRApiUrl = CommonData.Base_URL + "search/issues?per_page=1&q=repo:" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "+is:pr+is:open+created:>" + fromDate.ToString("yyyy-MM-dd");
            string gitTotalOpenPRResponse = await _commonObj.HttpCallHandler(getTotalOpenPRApiUrl);

            if (!string.IsNullOrEmpty(gitTotalOpenPRResponse))
            {
                var openPRResponse = JObject.Parse(gitTotalOpenPRResponse);
                repoState.open_pr = Convert.ToInt32(Convert.ToString(openPRResponse["total_count"]));
            }

            // Total closed PR
            string getTotalclosedPRApiUrl = CommonData.Base_URL + "search/issues?per_page=1&q=repo:" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "+is:pr+is:closed+created:>" + fromDate.ToString("yyyy-MM-dd");
            string gitTotalclosedPRResponse = await _commonObj.HttpCallHandler(getTotalclosedPRApiUrl);

            if (!string.IsNullOrEmpty(gitTotalclosedPRResponse))
            {
                var closedRes = JObject.Parse(gitTotalclosedPRResponse);
                repoState.closed_pr = Convert.ToInt32(Convert.ToString(closedRes["total_count"]));
            }

            // Total Open Issue
            string totalOpenIssueApiUrl = CommonData.Base_URL + "search/issues?per_page=1&q=repo:" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "+is:issue+is:open+created:>" + fromDate.ToString("yyyy-MM-dd");
            string totalOpenIssueRes = await _commonObj.HttpCallHandler(totalOpenIssueApiUrl);

            if (!string.IsNullOrEmpty(totalOpenIssueRes))
            {
                var openIssueRes = JObject.Parse(totalOpenIssueRes);
                repoState.open_issue = Convert.ToInt32(Convert.ToString(openIssueRes["total_count"]));
            }

            // Total Closed Issue
            string totalCloseIssueApiUrl = CommonData.Base_URL + "search/issues?per_page=1&q=repo:" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "+is:issue+is:closed+created:>" + fromDate.ToString("yyyy-MM-dd");
            string totalCloseIssueRes = await _commonObj.HttpCallHandler(totalCloseIssueApiUrl);

            if (!string.IsNullOrEmpty(totalCloseIssueRes))
            {
                var closeIssueRes = JObject.Parse(totalCloseIssueRes);
                repoState.closed_issue = Convert.ToInt32(Convert.ToString(closeIssueRes["total_count"]));
            }

            return repoState;
        }

        internal async Task<List<GitRepoStarContributors>> GetStarContributors()
        {
            List<GitRepoStarContributors> userDetails = new List<GitRepoStarContributors>();

            string starContributeApiUrl = CommonData.Base_URL + "repos/" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "/contributors";
            string starContributeResponse = await _commonObj.HttpCallHandler(starContributeApiUrl);

            if (!string.IsNullOrEmpty(starContributeResponse))
            {
                var starContributors = JsonConvert.DeserializeObject<JArray>(starContributeResponse);

                for (int i = 0; i < starContributors.Count; i++)
                {
                    GitRepoStarContributors user = new GitRepoStarContributors();

                    var userName = Convert.ToString(starContributors[i]["login"]);
                    user.username = userName;
                    user.totalcontributation = Convert.ToInt32(Convert.ToString(starContributors[i]["contributions"]));

                    string userDetailsApiUrl = CommonData.Base_URL + "/users/" + userName;
                    string userDetailsResponse = await _commonObj.HttpCallHandler(userDetailsApiUrl);

                    if (!string.IsNullOrEmpty(userDetailsResponse))
                    {
                        var userObj = JObject.Parse(userDetailsResponse);
                        user.fullname = Convert.ToString(userObj["name"]);
                    }
                    userDetails.Add(user);
                }
            }
            return userDetails;
        }
    }
}
