using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DataCenter.Server.Controller
{
    [EnableCors("*", "*", "GET")]
    [RoutePrefix("api")]
    public class FailedController : ApiController
    {
        [Route("Failed/{projectType}")]
        public IEnumerable<object> Get(string projectType)
        {
            var failed = DataService.Instance.GetFailedTestCases(projectType);

            var filtered = failed.Select(history => 
            {
                try
                {
                    var firstBroken = history.FirstBrokenResult != null ? history.FirstBrokenResult : history.EarliestResult;
                    var isTrusted = history.IsFirstBrokenTrusted;
                    var lastestResult = history.LatestResult;

                    var run = DataService.Instance.GetTestRun(firstBroken.TestRun).Key;
                    var assignment = GetAssignment(run, firstBroken, history.Assignment);

                    return new
                    {
                        Scope = history.TestCase.Scope,
                        FullName = history.TestCase.FullName,
                        Identifier = history.TestCase.Identifier,
                        Owner = NameService.GetCoreId(history.TestCase.Owner),
                        OwnerName = NameService.Instance.GetUserName(history.TestCase.Owner),
                        Message = lastestResult != null && !string.IsNullOrEmpty(lastestResult.Message) ? Utility.FormatMessage(lastestResult) : Utility.FormatMessage(firstBroken),
                        History = string.Concat(history.RecentHistory.Select(recent => recent.Passed ? "P" : "F")),
                        AssignToEngineer = assignment != null ? NameService.GetCoreId(assignment.AssignToEngineer) : string.Empty,
                        AssignToEngineerName = assignment != null ? NameService.Instance.GetUserName(assignment.AssignToEngineer) : string.Empty,
                        AssignToChangeset = assignment != null ? assignment.AssignToChangeset : 0,
                        AssignToTime = assignment != null ? assignment.AssignToTime : DateTime.MinValue,
                        Trusted = IsTrusted(isTrusted, run, history.Assignment, firstBroken.ChangeSet),
                        Comment = assignment != null ? assignment.Comment : string.Empty
                    };

                }
                catch (Exception ex)
                {
                    #region Exception
                    if (history != null && history.TestCase != null)
                    {
                        return new
                        {
                            Scope = history.TestCase.Scope,
                            FullName = history.TestCase.FullName,
                            Identifier = history.TestCase.Identifier,
                            Owner = "RXT867",
                            OwnerName = "Li Yang-RXT867",
                            Message = "[Cannot get failed test case info]",
                            History = "F",                            
                            AssignToEngineer = "RXT867",
                            AssignToEngineerName = "Li Yang-RXT867",
                            AssignToChangeset = 0,
                            AssignToTime = DateTime.UtcNow,
                            Trusted = false,
                            Comment = ex.Message
                        };
                    }
                    else
                    {
                        return new
                        {
                            Scope = "[Error]",
                            FullName = "[Unknown]",
                            Identifier = "[Unknown]",
                            Owner = "RXT867",
                            OwnerName = "Li Yang-RXT867",
                            Message = "[Cannot get failed test case info]",
                            History = "F",
                            AssignToEngineer = "RXT867",
                            AssignToEngineerName = "Li Yang-RXT867",
                            AssignToChangeset = 0,
                            AssignToTime = DateTime.UtcNow,
                            Trusted = false,
                            Comment = ex.Message
                        };
                    }
                    #endregion
                }                
            });

            filtered = filtered.Where(h => h != null);
            return filtered;
        }

        private static bool IsTrusted(bool isResultTrusted, TestRun run, AssignTo last, int changeSet)
        {
            if (last != null && last.AssignToTime >= run.StartedTime && last.AssignToTime >= run.CompletedTime)
            {
                // Manually assigned
                return true;
            }
            else
            {
                return isResultTrusted;
            }
        }

        private static AssignTo GetAssignment(TestRun run, TestCaseResult result, AssignTo last)
        {
            if (last != null && last.AssignToTime >= run.StartedTime && last.AssignToTime >= run.CompletedTime)
            {
                return last;
            }
            else
            {
                return new AssignTo() 
                { 
                    AssignToEngineer = run.Owner, 
                    AssignToChangeset = result.ChangeSet, 
                    AssignToTime = run.StartedTime,
                    Comment = string.Format("This failure was introduced by {0} with changeset {1}.", run.Owner, result.ChangeSet)
                };
            }
        }
    }
}
