using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContinuousIntegration.Interface;
using CI = ContinuousIntegration.DataAccess;
using DataCenter.DataAccess;
using Nebula.Redis;
using Nebula.MongoDB;

namespace DataCenter.Test
{
    [TestClass]
    public class InactiveFullnames
    {
        [TestMethod]
        public void Add()
        {
            var dataCenter = new Repository("mongodb://nebula:4501");

            //var redis = new RedisConnection("nebula:4402");
            //var redisAccessor = new RedisAccessor(redis.DB);
            //var ciRepo = new CI.Repository(redisAccessor);

            //var activenames = redis.DB.SetMembers(CI.TestCaseRedisKey.Set_TestCases).Select(t => t.ToString()).ToArray();
            var testcases = dataCenter.GetAllTestCases();

            var inactive = new List<string>();

            foreach(var t in testcases)
            {
                if (t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.ASTRO25TrunkingChannel")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.ASTROConventionalChannel")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.DVRSConventionalChannel")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.MDCConventionalChannel")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.MixedModeConventionalChannel")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.NoSignallingConventionalChannel")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.TypeIITrunkingChannel")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.ChannelsTest.Test_ChannelName_Invalid")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.ChannelsTest.Test_ChannelName_Valid")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.ChannelsTest.Test_ChannelsChannel_Invalid")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.ChannelsTest.Test_ChannelsChannel_Valid")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.Zones_InternalTest.Test_DynamicZoneEnable_InApplicable4")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.Zones_InternalTest.Test_DynamicZoneEnable_InValid")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.Zones_InternalTest.Test_FPPEnable_Invalid")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.Zones_InternalTest.Test_FPPEnable_Valid")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.Zones_InternalTest.Test_ProtectedZone_Invalid")
                    || t.FullName.StartsWith("ConstraintsTests.APX.Configuration:ASTRO.RadioFeatures.Test.TestCases.Zones.Zones_InternalTest.Test_ProtectedZone_Valid"))
                {
                    inactive.Add(t.FullName);
                }
            }

            foreach(var i in inactive)
            {
                dataCenter.SetInactiveFullname(i);
                Console.WriteLine(i);
            }            
        }
    }
}
