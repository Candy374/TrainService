using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;


namespace CommonUtilities
{
    public static class IOHelper
    {
        public static string LastRoboCopyError = string.Empty;

        [Activity]
        public static string GetFileHash(string filePath)
        {
            //创建一个哈希算法对象 
            using (var hash = HashAlgorithm.Create())
            {
                using (var file1 = new FileStream(filePath, FileMode.Open))
                {
                    var hashByte1 = hash.ComputeHash(file1);//哈希算法根据文本得到哈希码的字节数组 

                    var str1 = BitConverter.ToString(hashByte1);//将字节数组装换为字符串 

                    return str1;
                }
            }
        }

        [Activity]
        public static void Copy(string sourceFile, string destName)
        {
            if (sourceFile.Equals(destName, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            var dir = Path.GetDirectoryName(destName);
            CreateDir(dir);
            File.Copy(sourceFile, destName, true);
        }

        /// <summary>
        ///     Copy folder from sourceDir to targetDir via RoboCopy
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="targetDir"></param>
        /// <param name="filter">
        ///     file name filter, use space to split.
        ///     such as: "*.dll *.exe *.pdb"
        ///     If you don't want to set filter,
        ///     please use <c>null</c> or <c>string.Empty</c>
        /// </param>
        /// <returns>Error code</returns>
        [Activity]
        public static bool RoboCopy(string sourceDir, string targetDir, string filter)
        {
            if (!Directory.Exists(targetDir))
            {
                CreateDir(targetDir);
            }

            var roboCopyLog = Path.GetFileName(sourceDir) + DateTime.Now.ToString("HHmmss") +
                              ".RoboCopy.log";
            var args = string.Format("{3} /E /MT:40 /R:20 /W:5 \"{0}\" \"{1}\" /LOG:\"{2}\"",
                sourceDir, targetDir, roboCopyLog, filter ?? "");

            return RoboCopy(args, roboCopyLog);
        }

        private static bool RoboCopy(string args, string roboCopyLog)
        {
            var robocopy = ProcessHelper.ExecuteProgram("Robocopy.exe", args, false, true);
            robocopy.WaitForExit();
            bool isCopySuccess = false;
            switch (robocopy.ExitCode)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    LastRoboCopyError = string.Empty;
                    //Copy succeeded
                    isCopySuccess = true;
                    break;
                default:
                    LastRoboCopyError = BuildRoboCopyErrMsg(robocopy.ExitCode, roboCopyLog);
                    isCopySuccess = false;
                    break;
            }

            try
            {
                File.Delete(roboCopyLog);
            }
            catch (Exception)
            {
                return isCopySuccess;
            }

            return isCopySuccess;
        }

        private static string BuildRoboCopyErrMsg(int p, string logPath)
        {
            var sb = new StringBuilder("Exit Code:" + p + "\n");
            if ((p & 16) > 0)
            {
                sb.AppendLine(RoboCopyErrerCode.E16);
            }

            if ((p & 8) > 0)
            {
                sb.AppendLine(RoboCopyErrerCode.E8);
            }

            if ((p & 4) > 0)
            {
                sb.AppendLine(RoboCopyErrerCode.E4);
            }

            if ((p & 2) > 0)
            {
                sb.AppendLine(RoboCopyErrerCode.E2);
            }

            if ((p & 1) > 0)
            {
                sb.AppendLine(RoboCopyErrerCode.E1);
            }

            sb.AppendLine(GetRoboCopyLog(logPath));

            return sb.ToString();
        }

        private static string GetRoboCopyLog(string logPath)
        {
            if (string.IsNullOrEmpty(logPath))
            {
                return string.Empty;
            }

            try
            {
                if (File.Exists(logPath))
                {
                    var txt = File.ReadAllText(logPath);
                    return txt;
                }
            }
            catch (Exception)
            {

            }

            return string.Empty;
        }

        /// <summary>
        ///     Move folder from sourceDir to targetDir via RoboCopy
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="targetDir"></param>
        /// <param name="filter">
        ///     file name filter, use space to split.
        ///     such as: "*.dll *.exe *.pdb"
        ///     If you don't want to set filter,
        ///     please use <c>null</c> or <c>string.Empty</c>
        /// </param>
        /// <returns>Error code</returns>
        [Activity]
        public static bool RoboMove(string sourceDir, string targetDir, string filter)
        {
            if (!Directory.Exists(targetDir))
            {
                CreateDir(targetDir);
            }

            var roboCopyLog = Path.GetFileName(sourceDir) + DateTime.Now.ToString("HHmmss") +
                             ".RoboMove.log";
            var args = string.Format("{3} /E /MOVE /MT:40 /R:5 /W:2 \"{0}\" \"{1}\" /LOG:\"{2}\"",
                sourceDir, targetDir, roboCopyLog, filter ?? "");

            return RoboCopy(args, roboCopyLog);
        }


        /// <summary>
        /// Mirror sourceDir to targetDir. Use "RoboCopy /MIR" option
        /// </summary>
        /// <param name="sourceDir"></param>
        /// <param name="targetDir"></param>
        /// <returns>Error code</returns>
        [Activity]
        public static bool RoboMirror(string sourceDir, string targetDir)
        {
            if (!Directory.Exists(targetDir))
            {
                CreateDir(targetDir);
            }

            var roboCopyLog = Path.GetFileName(sourceDir) + DateTime.Now.ToString("HHmmss") +
                              ".RoboMirror.log";
            var args = string.Format("/MIR /MT:40 /R:20 /W:5 \"{0}\" \"{1}\" /LOG:\"{2}\"",
                sourceDir, targetDir, roboCopyLog);

            return RoboCopy(args, roboCopyLog);
        }

        /// <summary>
        /// Try delete a directory. The only difference between
        /// <code>IOHelper.DeleteDir</code> and <code>IOHelper.DeleteFolder</code> is
        /// <code>IOHelper.DeleteDir</code> not throw exception when delete folder failed.
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        [Activity]
        public static bool DeleteDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                return true;
            }

            try
            {
                DeleteFolder(dirPath);
            }
            catch (UnauthorizedAccessException)
            {
                //No permission to delete the dir
                return false;
            }
            catch (PathTooLongException)
            {
                // TODO: can do improvement
                return false;
            }
            catch (IOException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        [Activity]
        public static void CreateDir(string dirName)
        {
            dirName = Path.GetFullPath(dirName);
            var parentName = dirName;
            if (dirName.Contains('\\'))
            {
                parentName = dirName.Substring(0, dirName.LastIndexOf('\\'));
            }
            if (!Directory.Exists(parentName))
            {
                CreateDir(parentName);
            }

            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
        }

        [Activity]
        public static string GetValidFileName(string fileName)
        {
            var invChars = Path.GetInvalidPathChars();
            fileName = invChars.Aggregate(fileName, (current, invChar) => current.Replace(invChar, '_'));

            return fileName.Replace(' ', '_');
        }

        [Activity]
        public static void CreateEmptyFile(string fileName, int retryCountRemain = 10)
        {
            try
            {
                File.Create(fileName, 1).Close();
            }
            catch (Exception)
            {
                if (retryCountRemain > 0)
                {
                    retryCountRemain--;
                    Thread.Sleep(100);
                    CreateEmptyFile(fileName, retryCountRemain);
                }
            }
        }

        [Activity]
        public static void CreateTxtFile(string fileName, string content, int retryCountRemain = 10)
        {
            try
            {
                using (var writer = new StreamWriter(fileName, false))
                {
                    writer.Write(content);
                    writer.Flush();
                }
            }
            catch (Exception)
            {
                if (retryCountRemain > 0)
                {
                    retryCountRemain--;
                    Thread.Sleep(100);
                    CreateTxtFile(fileName, content, retryCountRemain);
                }
            }
        }

        [Activity]
        public static void CopyAssemblyAndReferences(string assemblyPath, string destDir)
        {
            if (string.IsNullOrEmpty(assemblyPath))
            {
                throw new ArgumentNullException("assemblyPath");
            }

            if (string.IsNullOrEmpty(destDir))
            {
                throw new ArgumentNullException("destDir");
            }

            var path = Path.GetFullPath(assemblyPath);
            var assemblyName = Path.GetFileName(path);
            var pdbName = Path.GetFileNameWithoutExtension(path) + ".pdb";
            var directoryName = Path.GetDirectoryName(path);
            if (directoryName == null)
            {
                return;
            }
            var basePath = directoryName.ToUpper();
            var targetPath = Path.Combine(destDir, assemblyName);
            CreateDir(destDir);
            Copy(path, targetPath);
            if (File.Exists(path + ".config"))
            {
                Copy(path + ".config", targetPath + ".config");
            }

            var pdbFilePath = Path.Combine(directoryName, pdbName);
            if (File.Exists(pdbFilePath))
            {
                Copy(pdbFilePath, Path.Combine(destDir, pdbName));
            }

            byte[] assemblyBytes = File.ReadAllBytes(targetPath);
            var a = Assembly.LoadFrom(targetPath);
            var references = a.GetReferencedAssemblies();
            foreach (var an in references)
            {
                var assemblies = FindAssembly(basePath, an.Name);
                foreach (var assembly in assemblies)
                {
                    targetPath = Path.Combine(destDir, assembly.Substring(basePath.Length + 1));
                    if (!File.Exists(targetPath))
                    {
                        CopyAssemblyAndReferences(assembly, Path.GetDirectoryName(targetPath));
                    }
                }
            }
        }

        /// <summary>
        /// Remove all content under <para>directoryPath</para>.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <remarks><para>directoryPath</para> itself won't be deleted. Support network path.</remarks>
        public static void ForceClean(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new Exception("'" + directoryPath + "' doesn't exist.");
            }
            foreach (string d in Directory.GetFileSystemEntries(directoryPath))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);
                }
                else
                {
                    DeleteFolder(d);
                }
            }
        }

        /// <summary>
        /// Force delete no matter it is readonly or not.
        ///  The only difference between
        /// <code>IOHelper.DeleteDir</code> and <code>IOHelper.DeleteFolder</code> is
        /// <code>IOHelper.DeleteDir</code> not throw exception when delete folder failed.
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void DeleteFolder(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                foreach (string d in Directory.GetFileSystemEntries(directoryPath))
                {
                    if (File.Exists(d))
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;
                        File.Delete(d);
                    }
                    else
                    {
                        DeleteFolder(d);
                    }
                }

                Directory.Delete(directoryPath);
            }
        }


        /// <summary>
        /// Delete leaf folders which are under <para>rootfolder</para> and fullpath match <para>removefolderRegex</para>.
        /// </summary>
        /// <param name="rootfolder"></param>
        /// <param name="removefolderRegex">RegularExpressions which match the folder path string. </param>
        /// <param name="ignoreRegex">some path will be ignored.</param>
        public static void ForceDeleteLeafFolder(string rootfolder, string removefolderRegex, string ignoreRegex = "")
        {
            if (!Directory.Exists(rootfolder))
            {
                throw new Exception("'" + rootfolder + "' doesn't exist.");
            }
            string[] subfolders = Directory.GetDirectories(rootfolder);

            foreach (string subfolder in subfolders)
            {
                if (Directory.GetDirectories(subfolder).Length == 0)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(subfolder, removefolderRegex, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    {
                        if (string.IsNullOrEmpty(ignoreRegex)
                                || !System.Text.RegularExpressions.Regex.IsMatch(subfolder, ignoreRegex, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                        {
                            DeleteFolder(subfolder);
                        }
                    }
                }
                else
                {
                    ForceDeleteLeafFolder(subfolder, removefolderRegex, ignoreRegex);
                }
            }
        }


        private static string[] FindAssembly(string dir, string name)
        {
            if (!Directory.Exists(dir))
            {
                return null;
            }
            var exts = new[] { ".exe", ".dll", ".ocx" };
            var fileList = new List<string>();
            foreach (var ext in exts)
            {
                var assemblyName = Path.Combine(dir, name + ext);
                if (File.Exists(assemblyName))
                {
                    fileList.Add(assemblyName);
                }
            }

            return fileList.ToArray();
        }





    }


    static class RoboCopyErrerCode
    {
        public const string E0 = "No errors occurred, and no copying was done. The source and destination directory trees are completely synchronized.";
        public const string E1 = "One or more files were copied successfully (that is, new files have arrived).";
        public const string E2 = "Some Extra files or directories were detected. Examine the output log. Some housekeeping may be needed.";
        public const string E4 = "Some Mismatched files or directories were detected. Examine the output log. Housekeeping is probably necessary.";
        public const string E8 = "Some files or directories could not be copied (copy errors occurred and the retry limit was exceeded). Check these errors further.";
        public const string E16 = "Serious error. Robocopy did not copy any files. This is either a usage error or an error due to insufficient access privileges on the source or destination directories.";
    }
}
