using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DataCenter.Server.Controller
{
    [EnableCors("*", "*", "GET")]
    [RoutePrefix("api")]
    public class FailedDateOfDistributionController : ApiController
    {
        [Route("FailedDateOfDistribution/{projectType}/{scopeClassifyRule}")]
        public HttpResponseMessage Get(string projectType, string scopeClassifyRule)
        {
            var failed = DataService.Instance.GetFailedTestCases(projectType);
            var data = new Dictionary<string, Dictionary<string, int>>();
            var scopeList = new List<string>();
            var returnValue = string.Empty;
            var rulesDic = GetScopeClassifyRuleDic(scopeClassifyRule);

            foreach (var h in failed)
            {
                try
                {
                    var r = h.FirstBrokenResult != null ? h.FirstBrokenResult : h.EarliestResult;
                    var lastestR = h.LatestResult;
                    if (!string.IsNullOrEmpty(r.TestRun))
                    {
                        var run = DataService.Instance.GetTestRun(r.TestRun).Key;
                        string assignTo;
                        var assignTime = GetAssignTime(run, h.Assignment,out assignTo);
                        if (assignTo.ToUpper().Equals("NEBULA"))
                        {
                            continue;
                        }
                        var date = (assignTime.Year - 2000).ToString() + assignTime.ToString("MMdd");
                        var scope = ClassifyScope(r.TestCase.Scope, rulesDic);
                        if (!data.ContainsKey(date))
                        {
                            data.Add(date, new Dictionary<string, int>());
                        }

                        if (!data[date].ContainsKey(scope))
                        {
                            data[date].Add(scope, 1);
                            if (!scopeList.Contains(scope))
                            {
                                scopeList.Add(scope);
                            }
                        }
                        else
                        {
                            data[date][scope]++;
                        }
                    }
                }
                catch (Exception e)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(
                            e.Message + "<br/>" + e.StackTrace,
                            Encoding.UTF8,
                            "text/html"
                        )
                    };
                }


            } // end of foreach (var h in failed.Key)

            SortedList<string, SortedList<string, int>> summaries = new SortedList<string, SortedList<string, int>>();

            foreach (var k in data.Keys)
            {
                summaries.Add(k, new SortedList<string, int>());
                foreach (var s in data[k].Keys)
                {
                    summaries[k].Add(s, data[k][s]);
                }
            }

            returnValue = BuildJson(summaries, scopeList.OrderBy(s => s));
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    returnValue,
                    Encoding.UTF8,
                    "text/html"
                )
            };
        }

        private static Dictionary<string, string> GetScopeClassifyRuleDic(string scopeClassifyRule)
        {
            var rulesDic = new Dictionary<string, string>();
            var rulesArray = string.IsNullOrWhiteSpace(scopeClassifyRule)
                ? null
                : scopeClassifyRule.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var r in rulesArray)
            {
                var args = r.Split('=');
                if (args.Length == 2)
                {
                    var key = args[0].ToUpper();
                    if (!rulesDic.ContainsKey(key))
                    {
                        rulesDic.Add(key, args[1]);
                    }
                }
            }

            return rulesDic;
        }

        private string ClassifyScope(string scope, Dictionary<string, string> rules)
        {
            if (rules == null)
            {
                return scope;
            }

            var s = scope.ToUpper();
            foreach (var r in rules.Keys)
            {
                if (s.Contains(r))
                {
                    return rules[r];
                }
            }

            return scope;
        }

        private string BuildJson(SortedList<string, SortedList<string, int>> summaries, IEnumerable<string> scopeList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var time in summaries.Keys)
            {
                sb.Append(string.Format("{{\"date\":\"{0}\"", time)); // "date":"1120"
                var scopes = summaries[time];
                foreach (var scope in scopeList)
                {
                    if (scopes.ContainsKey(scope))
                    {
                        sb.Append(string.Format(",\"{0}\":{1}", Format(scope), scopes[scope])); // ,"BLL":100
                    }
                    else
                    {
                        sb.Append(string.Format(",\"{0}\":0", Format(scope))); // ,"BLL":0
                    }
                }

                sb.Append("},");
            }

            if (sb[sb.Length - 1] == ',')
            {
                // Replace the last ',' to ']'
                sb[sb.Length - 1] = ']';
            }
            else
            {
                sb.Append(']');
            }
            return sb.ToString();
        }

        private string Format(string str)
        {
            StringBuilder sb = new StringBuilder(str.Length);
            foreach (var c in str)
            {
                if (c < '0' || (c > '9' && c < 'A') || (c > 'Z' && c < 'a') || c > 'z')
                {
                    sb.Append('_');
                }
                else
                {
                    sb.Append(c);
                }
            }

            var result = sb.ToString();
            if (result[0] < 'A')
            {
                result = '_' + result.Substring(1);
            }

            return result;
        }

        private static DateTime GetAssignTime(ContinuousIntegration.Interface.TestRun run,
            ContinuousIntegration.Interface.AssignTo last,out string assignTo)
        {
            if (last != null && last.AssignToTime >= run.StartedTime && last.AssignToTime >= run.CompletedTime)
            {
                assignTo = NameService.GetCoreId(last.AssignToEngineer);
                return last.AssignToTime;
            }
            else
            {
                assignTo = run.Owner;
                return run.StartedTime;
            }
        }

        class FailedDateSummary
        {
            public string Date;
            public int BLL;
            public int RMC_ASTRO;
            public int RMC_PCR;


        }

    }
}
