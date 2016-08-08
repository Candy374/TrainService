using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContinuousIntegration.Interface;

namespace DataCenter.Server
{
    static class Utility
    {
        public static string FormatMessage(TestCaseResult result)
        {
            var sb = new StringBuilder();
            const string lineStart = "    ";
            AppendLine(sb, "", "Full Name: ");
            AppendLine(sb, lineStart, result.TestCase.FullName);
            AppendLine(sb, "", "Message Version: ");
            AppendLine(sb, lineStart, result.ChangeSet.ToString());
            sb.AppendLine("Message:");
            var lines = result.Message.Split('\n');
            foreach (var line in lines)
            {
                if (line.TrimStart().StartsWith("at ") && line.Contains(" in "))
                {
                    //Stack trace
                    var placeHolder = line.Substring(0, line.Length - line.TrimStart().Length);
                    AppendLine(sb, lineStart, line.Substring(0, line.IndexOf(" in ")));
                    AppendLine(sb, placeHolder + lineStart + lineStart, line.Substring(line.IndexOf(" in ") + 1));
                }
                else
                {
                    AppendLine(sb, lineStart, line);
                }
            }

            return sb.ToString().Replace("&", "&amp;")
                                .Replace("<", "&lt;")
                                .Replace(">", "&gt;")
                                .Replace("\"", "&quot;")
                                .Replace(" ", "&nbsp;")
                                .Replace("©", "&copy;")
                                .Replace("®", "&reg;")
                                .Replace("\n", "<br/>");
        }

        public static string FormatMessage(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return string.Empty;
            }
            var sb = new StringBuilder();
            const string lineStart = "    ";
            var lines = msg.Split('\n');
            foreach (var line in lines)
            {
                if (line.TrimStart().StartsWith("at ") && line.Contains(" in "))
                {
                    //Stack trace
                    var placeHolder = line.Substring(0, line.Length - line.TrimStart().Length);
                    AppendLine(sb, lineStart, line.Substring(0, line.IndexOf(" in ")));
                    AppendLine(sb, placeHolder + lineStart + lineStart, line.Substring(line.IndexOf(" in ") + 1));
                }
                else
                {
                    AppendLine(sb, lineStart, line);
                }
            }

            return sb.ToString().Replace("&", "&amp;")
                                .Replace("<", "&lt;")
                                .Replace(">", "&gt;")
                                .Replace("\"", "&quot;")
                                .Replace(" ", "&nbsp;")
                                .Replace("©", "&copy;")
                                .Replace("®", "&reg;")
                                .Replace("\n", "<br/>");
        }

        private static void AppendLine(StringBuilder sb, string placeHolder, string line)
        {
            if (line.Length <= 130)
            {
                sb.AppendLine(placeHolder + line);
                return;
            }

            var sub1 = line.Substring(0, 130);
            if (sub1.Contains(". "))
            {
                sub1 = sub1.Substring(0, sub1.LastIndexOf(". ") + 2);
            }
            else if (sub1.Contains(": "))
            {
                sub1 = sub1.Substring(0, sub1.LastIndexOf(": ") + 2);
            }
            else if (sub1.Contains(' '))
            {
                sub1 = sub1.Substring(0, sub1.LastIndexOf(' ') + 1);
            }

            sb.AppendLine(placeHolder + sub1);
            AppendLine(sb, placeHolder, line.Substring(sub1.Length));
        }
    }
}
