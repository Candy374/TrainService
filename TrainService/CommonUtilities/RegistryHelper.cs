using System.Text.RegularExpressions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities
{
    public class RegistryHelper
    {
        public static void ImportReg(string file)
        {
            var p = ProcessHelper.ExecuteProgram("regedit.exe", "/s \"" + file + "\"");
            p.WaitForExit();
            if (p.ExitCode != 0)
            {
                throw new Exception("Import '" + file + "' failed.");
            }
        }

        /// <summary>
        /// Region = "NA"; "Type"="MOTOTRBO 2.0"
        /// </summary>
        /// <param name="title"></param>
        /// <param name="paths"></param>
        public static void CreatePCRPackages(string title, string[] paths, bool cleanfirstly)
        {
            RegistryKey motorola = CreateMotorolaRegistryKey();
            RegistryKey MotoTRBO = motorola.CreateSubKey("MOTOTRBO CPS");
            if (cleanfirstly)
            {
                MotoTRBO.DeleteSubKeyTree("Packages", false);
            }
            RegistryKey Packages = MotoTRBO.CreateSubKey("Packages");
            RegistryKey guid = Packages.CreateSubKey(Guid.NewGuid().ToString("B"));

            guid.SetValue("Type", "MOTOTRBO 2.0", RegistryValueKind.String);
            guid.SetValue("Region", "NA", RegistryValueKind.String);
            guid.SetValue("Title", title, RegistryValueKind.String);
            for (int i = 1; i <= paths.Length; i++)
            {
                guid.SetValue("Path" + i, paths[i - 1], RegistryValueKind.String);
            }
        }


        private static RegistryKey CreateMotorolaRegistryKey()
        {
            RegistryKey motorola = null;

            if (Environment.Is64BitOperatingSystem)
            {
                motorola = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node", true).CreateSubKey(@"Motorola");
            }
            else
            {
                motorola = Registry.LocalMachine.OpenSubKey(@"SOFTWARE", true).CreateSubKey(@"Motorola");
            }

            return motorola;
        }

        public static void CreateCPSRegistryInfo(string localServerbuildPath)
        {
            CreatePCRCPSRegistryInfo(System.IO.Path.Combine(localServerbuildPath, @"MOTOTRBO_CPS.sln\"));
        }

        public static void RemoveAutoTestSetting(string key)
        {
            var root = Registry.CurrentUser;
            var motorola = root.OpenSubKey("SOFTWARE\\Motorola", true);
            if (motorola == null)
            {
                return;
            }

            var settingKey = motorola.OpenSubKey("AutoTestSettings", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if (settingKey == null)
            {
                return;
            }

            settingKey.DeleteValue(key, false);
        }

        public static void SetAutoTestSettings(Dictionary<string, object> settings)
        {
            var root = Registry.CurrentUser;
            var motorola = root.OpenSubKey("SOFTWARE\\Motorola", true);
            if (motorola == null)
            {
                root.CreateSubKey("SOFTWARE\\Motorola", RegistryKeyPermissionCheck.ReadWriteSubTree);
                motorola = root.OpenSubKey("SOFTWARE\\Motorola", true);
            }
            var settingKey = motorola.CreateSubKey("AutoTestSettings", RegistryKeyPermissionCheck.ReadWriteSubTree);
            foreach (var setting in settings)
            {
                settingKey.SetValue(setting);
                Console.WriteLine("[Set Reg]" + setting.Key + ":" + setting.Value);
            }

        }

        public static void SetAutoTestSetting(string key, object value)
        {
            Dictionary<string, object> setting = new Dictionary<string, object>();
            setting.Add(key, value);
            SetAutoTestSettings(setting);
        }


        public static object GetAutoTestSetting(string key, object defaultValue)
        {
            var root = Registry.CurrentUser;
            var motorola = root.OpenSubKey("SOFTWARE\\Motorola", true);
            if (motorola == null)
            {
                return defaultValue;
            }

            var settingKey = motorola.OpenSubKey("AutoTestSettings", RegistryKeyPermissionCheck.ReadSubTree);
            if (settingKey == null)
            {
                return defaultValue;
            }

            return settingKey.GetValue(key, defaultValue, RegistryValueOptions.None);
        }

        public static void CreatePCRCPSRegistryInfo(string CPSExecutablePath)
        {
            RegistryKey motorola = CreateMotorolaRegistryKey();

            #region MotoTRBO CPS
            RegistryKey MotoTRBO = motorola.CreateSubKey("MOTOTRBO CPS");
            #region values under MotoTRBO CPS key
            MotoTRBO.SetValue("CloneRadioIdentity", 0, RegistryValueKind.DWord);
            MotoTRBO.SetValue("CopyOffset", "0.000000", RegistryValueKind.String);
            MotoTRBO.SetValue("CurrentLanguage", "EN-US", RegistryValueKind.String);
            MotoTRBO.SetValue("CurrentView", "Basic_View", RegistryValueKind.String);
            MotoTRBO.SetValue("DatabasePath", System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Motorola\MOTOTRBO CPS\"), RegistryValueKind.String);
            MotoTRBO.SetValue("DefaultFolderPath", CPSExecutablePath, RegistryValueKind.String);
            MotoTRBO.SetValue("ErrorLogLevel", 1, RegistryValueKind.DWord);
            MotoTRBO.SetValue("InstallPath", CPSExecutablePath, RegistryValueKind.String);
            MotoTRBO.SetValue("MaxEntryMRU", 4, RegistryValueKind.DWord);
            MotoTRBO.SetValue("PlaySound", 0, RegistryValueKind.DWord);
            MotoTRBO.SetValue("ShowContextHelp", 1, RegistryValueKind.DWord);
            MotoTRBO.SetValue("SoftwareSystemKeyLocation", System.IO.Path.Combine(Environment.GetFolderPath(Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.CommonProgramFilesX86 : Environment.SpecialFolder.CommonProgramFiles), @"Motorola\SysKeys\"), RegistryValueKind.String);
            MotoTRBO.SetValue("TcpPort", "50000", RegistryValueKind.String);
            MotoTRBO.SetValue("VAExportLocation", System.IO.Path.Combine(CPSExecutablePath, @"voiceannouncement"), RegistryValueKind.String);
            MotoTRBO.SetValue("VAWorkerLocation", System.IO.Path.Combine(CPSExecutablePath, @"voiceannouncement"), RegistryValueKind.String);
            MotoTRBO.SetValue("ViewFolderPath", System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Motorola\MOTOTRBO CPS\views"), RegistryValueKind.String);
            MotoTRBO.SetValue("XferFolderPath", System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Motorola\CPS_RM\XFER"), RegistryValueKind.String);
            #endregion
            RegistryKey Language = MotoTRBO.CreateSubKey("Language");
            RegistryKey EN_US = Language.CreateSubKey("EN-US");
            #region value under en_us
            EN_US.SetValue("DealerInfoLeft", new string[] { "contact name", "dealer name", "dealer address", "dealer city, state, zip" }, RegistryValueKind.MultiString);
            EN_US.SetValue("DealerInfoMiddle", new string[] { "dealer mission statement" }, RegistryValueKind.MultiString);
            EN_US.SetValue("DealerInfoRight", new string[] { "phone (xxx)xxx-xxxx", "fax (xxx)xxx-xxxx", "email user@domain", "web site www.dealername.com" }, RegistryValueKind.MultiString);
            EN_US.SetValue("DisplayName", "English", RegistryValueKind.String);
            EN_US.SetValue("GUIResource", @".\resources\resource_en-us.resx", RegistryValueKind.String);
            EN_US.SetValue("HelpFileName", @".\resources\help_en-us.chm", RegistryValueKind.String);
            EN_US.SetValue("HelpXMLFileName", @".\resources\help_en-us.xml", RegistryValueKind.String);
            #endregion
            RegistryKey settings = MotoTRBO.CreateSubKey("Settings");
            settings.SetValue("FormWindowState", "Normal", RegistryValueKind.String);

            RegistryKey views = MotoTRBO.CreateSubKey("Views");
            RegistryKey Basic_View = views.CreateSubKey("Basic_View");
            Basic_View.SetValue("ViewName", "Basic", RegistryValueKind.String);
            Basic_View.SetValue("ViewResource", @".\basic_view.lmx", RegistryValueKind.String);
            RegistryKey Expert_View = views.CreateSubKey("Expert_View");
            Expert_View.SetValue("ViewName", "Expert", RegistryValueKind.String);
            Expert_View.SetValue("ViewResource", @".\expert_view.lmx", RegistryValueKind.String);
            #endregion


            Basic_View.Close();
            Expert_View.Close();
            views.Close();
            settings.Close();
            EN_US.Close();
            Language.Close();
            MotoTRBO.Close();
        }


    }
    public static class RegistryExtentions
    {
        public static void SetValue(this RegistryKey key, KeyValuePair<string, object> keyValue)
        {
            if (keyValue.Value == null || keyValue.Value is string)
            {
                key.SetValue(keyValue.Key, keyValue.Value ?? string.Empty, RegistryValueKind.String);
            }
            else if (keyValue.Value is int)
            {
                key.SetValue(keyValue.Key, keyValue.Value, RegistryValueKind.DWord);
            }
            else if (keyValue.Value is long)
            {
                key.SetValue(keyValue.Key, keyValue.Value, RegistryValueKind.QWord);
            }
            else if (keyValue.Value is byte[])
            {
                key.SetValue(keyValue.Key, keyValue.Value, RegistryValueKind.Binary);
            }
        }
    }


}
