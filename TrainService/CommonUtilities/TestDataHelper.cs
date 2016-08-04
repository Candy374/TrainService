using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilities
{
    public static class TestDataHelper
    {
        private static string _dataServer = null;
        private static string LocalTestDataPath
        {
            get
            {
                if (_dataServer == null)
                {
                    if (Directory.Exists(@"\\Nebula-01\CI.TestBinaries"))
                    {
                        _dataServer = @"\\Nebula-01\CI.TestBinaries";
                    }
                    else
                    {
                        _dataServer = @"\\CMNCPS-36\CI.TestBinaries";
                    }
                }

                return _dataServer;
            }
        }

        /// <summary>
        /// Get local mapped path from specified TFS url
        /// </summary>
        /// <param name="tfsUrl"></param>
        /// <returns>If the specified url is not mapped in local computer, will return null.</returns>
        public static string GetLocalPath(string tfsUrl)
        {
            tfsUrl = tfsUrl.ToUpper();
            if (tfsUrl.StartsWith("$/COMMON_CPS_PLATFRM/IMPLEMENTATION/RM2.0/TESTDATA"))
            {
                return tfsUrl.Replace("$/COMMON_CPS_PLATFRM/IMPLEMENTATION/RM2.0/TESTDATA", _dataServer).Replace('/', '\\');
            }

            if (tfsUrl.StartsWith("-/"))
            {
                return _dataServer + tfsUrl.Substring(1).Replace('/', '\\');
            }

            return null;
        }
    }
}
