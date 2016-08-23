using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIService.Tests
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void WriteLog()
        {
            LoggerContract.Logger.Info("test message","test title");
        }

    }
}
