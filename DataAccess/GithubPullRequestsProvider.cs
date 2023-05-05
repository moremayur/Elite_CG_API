using Elite_CG_API.Common;
using Elite_CG_API.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace Elite_CG_API.DataAccess
{
    public class GithubPullRequestsProvider
    {
        private CommonData _commonObj;

        public GithubPullRequestsProvider() {
            _commonObj = new CommonData();
        }

        internal async Task<TimeToMerge> GetTimeToMergePRData(DateTime fromDate, DateTime toDate)
        {
            return await _commonObj.GetGitHubMergedPullRequests(fromDate, toDate);
        }

        internal async Task<List<TimeToFirstComment>> GetTimeToFirstCommentPRData(DateTime fromDate, DateTime toDate)
        {
            List<TimeToFirstComment> prCommentsLists = new List<TimeToFirstComment>();
            TimeToMerge prLists = await _commonObj.GetGitHubMergedPullRequests(fromDate, toDate);

            if (prLists != null && prLists.total_count > 0)
            {
                foreach (PullRequest pr in prLists.listPRs)
                {
                    string prReviewCommentsApiUrl = CommonData.Base_URL + "repos/" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "/pulls/"+ pr.prNumber + "/reviews";
                    string prReviewCommentsResponse = await _commonObj.HttpCallHandler(prReviewCommentsApiUrl);
                    // List<PRComment> prCmts = new List<PRComment>();

                    TimeToFirstComment prCommentsLsts = new TimeToFirstComment();
                    prCommentsLsts.pr_number = pr.prNumber;
                    prCommentsLsts.merged = pr.merged;
                    prCommentsLsts.created = pr.created;

                    bool isCommentFound = false;

                    if (!string.IsNullOrEmpty(prReviewCommentsResponse))
                    {
                        var prReviewComments = JsonConvert.DeserializeObject<JArray>(prReviewCommentsResponse);

                        for (int i = 0; i < prReviewComments.Count; i++)
                        {
                            if (prReviewComments[i]["body"] == null || string.IsNullOrEmpty(Convert.ToString(prReviewComments[i]["body"])))
                                break;

                            var user = JObject.Parse(Convert.ToString(prReviewComments[i]["user"]));
                            prCommentsLsts.comment_owner = Convert.ToString(user["login"]);
                            prCommentsLsts.comment = Convert.ToString(prReviewComments[i]["body"]);
                            var created_comment = new DateTimeOffset(Convert.ToDateTime(prReviewComments[i]["submitted_at"]), TimeSpan.FromHours(0)).ToLocalTime().DateTime;
                            prCommentsLsts.comment_created = created_comment;
                            DateTime pr_created = pr.created ?? DateTime.MinValue;

                            prCommentsLsts.totalHrsForFirstComment = CommonData.CalculateTotalTime(pr_created, created_comment, "HH");
                            prCommentsLsts.totalMinForFirstComment = CommonData.CalculateTotalTime(pr_created, created_comment, "MM");
                            prCommentsLsts.totalDayForFirstComment = CommonData.CalculateTotalTime(pr_created, created_comment, "DD");

                            isCommentFound = true;
                            break;
                        }

                        //foreach (JObject reviewCmt in prReviewComments)
                        //{
                        //    if (reviewCmt["body"] == null || string.IsNullOrEmpty(Convert.ToString(reviewCmt["body"])))
                        //        continue;

                        //    PRComment prc = new PRComment();
                        //    var user = JObject.Parse(Convert.ToString(reviewCmt["user"]));
                        //    prc.comment_owner = Convert.ToString(user["login"]);
                        //    prc.comment = Convert.ToString(reviewCmt["body"]);
                        //    prc.comment_created = new DateTimeOffset(Convert.ToDateTime(reviewCmt["submitted_at"]), TimeSpan.FromHours(0)).ToLocalTime().DateTime;

                        //    prCmts.Add(prc);
                        //}
                    }

                    if (!isCommentFound)
                    {
                        string prCommentsApiUrl = CommonData.Base_URL + "repos/" + CommonData.Repo_Owner + "/" + CommonData.Repo_Name + "/pulls/" + pr.prNumber + "/comments";
                        string prCommentsResponse = await _commonObj.HttpCallHandler(prCommentsApiUrl);

                        if (!string.IsNullOrEmpty(prCommentsResponse))
                        {
                            var prComments = JsonConvert.DeserializeObject<JArray>(prCommentsResponse);

                            for (int i = 0; i < prComments.Count; i++)
                            {
                                if (prComments[i]["body"] == null || string.IsNullOrEmpty(Convert.ToString(prComments[i]["body"])))
                                    break;

                                var user = JObject.Parse(Convert.ToString(prComments[i]["user"]));
                                prCommentsLsts.comment_owner = Convert.ToString(user["login"]);
                                prCommentsLsts.comment = Convert.ToString(prComments[i]["body"]);
                                var created_comment = new DateTimeOffset(Convert.ToDateTime(prComments[i]["created_at"]), TimeSpan.FromHours(0)).ToLocalTime().DateTime;

                                prCommentsLsts.comment_created = created_comment;
                                DateTime pr_created = pr.created ?? DateTime.MinValue;

                                prCommentsLsts.totalHrsForFirstComment = CommonData.CalculateTotalTime(pr_created, created_comment, "HH");
                                prCommentsLsts.totalMinForFirstComment = CommonData.CalculateTotalTime(pr_created, created_comment, "MM");
                                prCommentsLsts.totalDayForFirstComment = CommonData.CalculateTotalTime(pr_created, created_comment, "DD");

                                break;
                            }

                            //foreach (JObject cmt in prComments)
                            //{
                            //    if (cmt["body"] == null || string.IsNullOrEmpty(Convert.ToString(cmt["body"])))
                            //        break;

                            //    PRComment prc = new PRComment();
                            //    var pr_r_array = JObject.Parse(Convert.ToString(cmt["user"]));
                            //    prc.comment_owner = Convert.ToString(pr_r_array["login"]);
                            //    prc.comment = Convert.ToString(cmt["body"]);
                            //    prc.comment_created = new DateTimeOffset(Convert.ToDateTime(cmt["created_at"]), TimeSpan.FromHours(0)).ToLocalTime().DateTime;

                            //    prCmts.Add(prc);
                            //}
                        }
                    }

                    // if (prCmts.Count > 0) { 
                       // TimeToFirstComment prCommentsLsts = new TimeToFirstComment();
                       // prCommentsLsts.pr_number = pr.prNumber;
                       // prCommentsLsts.merged = pr.merged;
                       // prCommentsLsts.created = pr.created;
                       // prCommentsLsts.pr_comments = prCmts;

                    if (!string.IsNullOrEmpty(prCommentsLsts.comment_owner))
                        prCommentsLists.Add(prCommentsLsts);
                    // }
                }
            }

            return prCommentsLists;
        }
    }
}
