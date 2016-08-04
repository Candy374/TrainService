using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities
{
    public static class VisualStudioHelper
    {
        /// <summary>
        /// Run test methods using VsTest.Console.exe
        /// </summary>
        /// <param name="targetVisualStudioVersion">Which version of Visual Studio do you want to use</param>
        /// <param name="testDllPath">the full path of the test DLL</param>
        /// <param name="testMethods">
        /// You can use 
        /// <code>CommonUtilities.LoadAssembly(path).GetAllTestMethods()</code>
        /// to get methods.</param>
        /// <param name="allowNewerVersion">
        /// Default is false. If set it to <code>true</code>,
        /// the mathod may return a path from a newer version Visual Studio,
        /// when the target version of Visual Stuido not existed. 
        /// </param>
        public static void RunTests(VsVersion targetVisualStudioVersion, string testDllPath, IEnumerable<MethodInfo> testMethods, bool allowNewerVersion = false)
        {
            if (testMethods == null)
            {
                throw new ArgumentNullException("testMethods");
            }
            var filters = testMethods.BuildVsTestFilter();
            RunTests(targetVisualStudioVersion, testDllPath, filters, allowNewerVersion);
        }

        /// <summary>
        /// Run test methods using VsTest.Console.exe
        /// </summary>
        /// <param name="targetVisualStudioVersion">Which version of Visual Studio do you want to use</param>
        /// <param name="testDllPath">the full path of the test DLL</param>
        /// <param name="filters">Method name list</param>
        /// <param name="allowNewerVersion">
        /// Default is false. If set it to <code>true</code>,
        /// the mathod may return a path from a newer version Visual Studio,
        /// when the target version of Visual Stuido not existed. 
        /// </param>
        public static void RunTests(VsVersion targetVisualStudioVersion, string testDllPath, IEnumerable<string> filters, bool allowNewerVersion = false)
        {
            foreach (var filter in filters)
            {
                var cmd = BuildVsTestCommand(testDllPath, filter);
                var vsTestConsole = GetPath(VsPathOptions.VsTest_Console_exe, targetVisualStudioVersion, allowNewerVersion);
                var vsTest = ProcessHelper.ExecuteProgram(vsTestConsole, cmd, true, true);
                Console.WriteLine("Start run test via {0}(PID={1}) at {2}", vsTest.ProcessName, vsTest.Id, vsTest.StartTime.ToString("HH:mm:ss"));
                vsTest.WaitForExit();
                Console.WriteLine("{0} existed at {1}. Cost {2} seconds", vsTest.ProcessName, vsTest.ExitTime.ToString("HH:mm:ss"), (vsTest.ExitTime - vsTest.StartTime).TotalSeconds);
            }
        }


        private static string BuildVsTestCommand(string testDllFullPath, string filter)
        {
            return "\"{0}\" /TestCaseFilter:{1} {2}".FormatedWith(testDllFullPath, filter, " /inIsolation");
        }


        private static string[] BuildVsTestFilter(this IEnumerable<MethodInfo> testCases)
        {
            var filters = testCases.Join("|", m => "Name={0}".FormatedWith(m.Name))
                 .CutWithMaxLength('|', 30000);

            return filters;
        }

        /// <summary>
        /// Get Path for installed Visual Studio in local computer
        /// </summary>
        /// <param name="option">The path that you want to get</param>
        /// <param name="targetVisualStudioVersion">The Visual Studio version that you want to get path from.</param>
        /// <param name="allowNewerVersion">
        /// Default is false. If set it to <code>true</code>,
        /// the mathod may return a path from a newer version Visual Studio,
        /// when the target version of Visual Stuido not existed. 
        /// </param>
        /// <returns></returns>
        public static string GetPath(VsPathOptions option, VsVersion targetVisualStudioVersion, bool allowNewerVersion = false)
        {
            string path = null;
            switch (option)
            {
                case VsPathOptions.VisualStudioInstallation:
                    var toolPath = GetCommonToolsPath(targetVisualStudioVersion);
                    if (!string.IsNullOrEmpty(toolPath))
                    {
                        var dirInfo = new DirectoryInfo(toolPath);
                        path = dirInfo.Parent.Parent.FullName;
                    }
                    break;
                case VsPathOptions.VsTest_Console_exe:
                    path = GetVsTestConsolePath(targetVisualStudioVersion);
                    break;
                case VsPathOptions.CommonTools:
                    path = GetCommonToolsPath(targetVisualStudioVersion);
                    break;
                case VsPathOptions.VisualStudioSDK:
                    path = GetVsSDKPath(targetVisualStudioVersion);
                    break;
                default:
                    throw new ArgumentException("Unsupported args");
            }

            if (string.IsNullOrEmpty(path) && allowNewerVersion)
            {
                int num = (int)targetVisualStudioVersion + 1;
                if (((VsVersion)num).ToString() != num.ToString())
                {
                    return GetPath(option, (VsVersion)num, allowNewerVersion);
                }
            }

            return path;
        }

        private static string GetVsTestConsolePath(VsVersion targetVisualStudioVersion)
        {
            var vsTestConsolePath = Path.Combine(GetCommonToolsPath(targetVisualStudioVersion), @"..\IDE\CommonExtensions\Microsoft\TestWindow\VSTest.Console.exe");
            if (string.IsNullOrEmpty(vsTestConsolePath) || !File.Exists(vsTestConsolePath))
            {
                return string.Empty;
            }

            return vsTestConsolePath;
        }

        private static string GetCommonToolsPath(VsVersion targetVisualStudioVersion)
        {
            string vsPath = Environment.GetEnvironmentVariable(GetVsNumber(targetVisualStudioVersion, "VS{0}") + "COMNTOOLS");
            if (string.IsNullOrEmpty(vsPath) || !Directory.Exists(vsPath))
            {
                return string.Empty;
            }

            return vsPath;
        }

        private static string GetVsSDKPath(VsVersion targetVisualStudioVersion)
        {
            string vsPath = Environment.GetEnvironmentVariable(GetVsNumber(targetVisualStudioVersion, "VSSDK{0}") + "Install");
            if (string.IsNullOrEmpty(vsPath) || !Directory.Exists(vsPath))
            {
                return string.Empty;
            }

            return vsPath;
        }

        private static string GetVsNumber(VsVersion targetVisualStudioVersion, string format)
        {
            switch (targetVisualStudioVersion)
            {
                case VsVersion.VS2010:
                    return format.FormatedWith("100");
                case VsVersion.VS2012:
                    return format.FormatedWith("110");
                case VsVersion.VS2013:
                    return format.FormatedWith("120");
                case VsVersion.VS2015:
                    return format.FormatedWith("140");
                case VsVersion.NewerThan2015:
                    return format.FormatedWith("150");
                default:
                    return string.Empty;
            }
        }

        public enum VsVersion
        {
            VS2010 = 1,
            VS2012 = 2,
            VS2013 = 3,
            VS2015 = 4,
            NewerThan2015 = 5
        }

        public enum VsPathOptions
        {
            VisualStudioInstallation,
            VsTest_Console_exe,
            CommonTools,
            VisualStudioSDK
        }
    }
}
