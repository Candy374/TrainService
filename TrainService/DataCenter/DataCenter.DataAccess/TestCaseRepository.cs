using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using CI = ContinuousIntegration.Interface;

namespace DataCenter.DataAccess
{
    internal class TestCaseRepository
    {
        protected IMongoCollection<BsonDocument> colTestCases;

        /// <summary>
        /// Fullname, Id
        /// </summary>
        protected Dictionary<string, string> cache_TestCasesMapping = new Dictionary<string, string>();

        /// <summary>
        /// Id - Value
        /// </summary>
        protected Dictionary<string, CI.TestCase> cache_TestCases = new Dictionary<string, CI.TestCase>();

        public TestCaseRepository(IMongoCollection<BsonDocument> testCases)
        {
            this.colTestCases = testCases;
            this.LoadTestCases();
        }

        public void Reload()
        {
            this.LoadTestCases();
        }


        public CI.TestCase[] GetAll()
        {
            return this.cache_TestCases.Values.ToArray();
        }

        public CI.TestCase GetById(string id)
        {
            if (this.cache_TestCases.ContainsKey(id))
            {
                return this.cache_TestCases[id];
            }

            return null;
        }

        public CI.TestCase GetByName(string name)
        {
            if (this.cache_TestCasesMapping.ContainsKey(name))
            {
                var id = this.cache_TestCasesMapping[name];

                if (this.cache_TestCases.ContainsKey(id))
                {
                    return this.cache_TestCases[id];
                }
            }

            return null;
        }

        /// <summary>
        /// Add or update test case (to cache and DB). TestCase.Identifier may be overwritten.
        /// </summary>
        public CI.TestCase AddOrUpdateTestCase(CI.TestCase testcase)
        {
            if (string.IsNullOrEmpty(testcase.FullName))
            {
                throw new Exception("Empty full name.");
            }

            if (this.CheckInCache(testcase.FullName))
            {
                var tc = GetTestCaseByFullnameFromCache(testcase.FullName);
                if (testcase.Owner != tc.Owner || testcase.Scope != tc.Scope)
                {
                    tc.Owner = testcase.Owner;
                    tc.Scope = testcase.Scope;
                    this.SaveTestCase(tc);

                    return tc;
                }
                else
                {
                    // No change
                    return tc;
                }
            }
            else
            {
                var tcInDB = this.GetTestCaseByFullnameFromDB(testcase.FullName);
                {
                    if (tcInDB != null)
                    {
                        testcase.Identifier = tcInDB.Identifier;
                        this.cache_TestCasesMapping.Add(testcase.FullName, testcase.Identifier);

                        if (testcase.Owner != tcInDB.Owner || testcase.Scope != tcInDB.Scope)
                        {
                            tcInDB.Owner = testcase.Owner;
                            tcInDB.Scope = testcase.Scope;

                            this.SaveTestCase(tcInDB);
                        }
                    }
                    else
                    {
                        this.cache_TestCasesMapping.Add(testcase.FullName, Guid.NewGuid().ToString());
                        testcase.Identifier = this.cache_TestCasesMapping[testcase.FullName];

                        this.SaveTestCase(testcase);
                    }

                    if (this.cache_TestCases.ContainsKey(testcase.Identifier))
                    {
                        // Should not, will log
                        this.cache_TestCases.Remove(testcase.Identifier);
                    }

                    this.cache_TestCases.Add(testcase.Identifier, testcase);
                    return testcase;
                }
            }
        }

        #region Helpers

        private void LoadTestCases()
        {
            this.cache_TestCases.Clear();
            this.cache_TestCasesMapping.Clear();

            var wait = this.colTestCases.Find(new BsonDocument()).ToListAsync();
            wait.Wait();

            if (wait.Result != null)
            {
                foreach (var t in wait.Result)
                {
                    var tc = t.ToTestCase();

                    if (!this.cache_TestCases.ContainsKey(tc.Identifier))
                    {
                        this.cache_TestCases.Add(tc.Identifier, tc);
                    }

                    if (!this.cache_TestCasesMapping.ContainsKey(tc.FullName))
                    {
                        this.cache_TestCasesMapping.Add(tc.FullName, tc.Identifier);
                    }
                }
            }
        }

        private bool CheckInCache(string fullname)
        {
            if (this.cache_TestCasesMapping.ContainsKey(fullname))
            {
                var id = this.cache_TestCasesMapping[fullname];
                if (this.cache_TestCases.ContainsKey(id))
                {
                    // Cache mapping exists, got testcase from cache
                    var tc = this.cache_TestCases[id];
                    if (id == tc.Identifier && fullname == tc.FullName)
                    {
                        return true;
                    }
                    else
                    {
                        // Cache error, remove
                        this.cache_TestCasesMapping.Remove(fullname);
                        this.cache_TestCases.Remove(id);

                        return false;
                    }
                }
                else
                {
                    // Cache mapping exists, try to get testcase from DB
                    var tcInDB = GetTestCaseByIdentifierFromDB(id);
                    if (tcInDB == null)
                    {
                        // Cache mapping error, remove mapping
                        this.cache_TestCasesMapping.Remove(fullname);
                        return false;
                    }
                    else
                    {                        
                        if (fullname == tcInDB.FullName)
                        {
                            // Correct testcase loaded, add to cache
                            this.cache_TestCases.Add(id, tcInDB);
                            return true;
                        }
                        else
                        {
                            // Cache mapping error, remove mapping
                            this.cache_TestCasesMapping.Remove(fullname);
                            return false;
                        }
                    }
                }
            }
            else
            {
                // Cache mapping doesn't exist
                return false;
            }
        }

        private CI.TestCase GetTestCaseByFullnameFromDB(string fullname)
        {
            var wait = this.colTestCases.Find(new BsonDocument{ {Constants.TestCase_Fullname, fullname} }).FirstOrDefaultAsync();
            wait.Wait();

            if (wait.Result != null)
            {
                return wait.Result.ToTestCase();
            }
            else
            {
                return null;
            }
        }

        private CI.TestCase GetTestCaseByIdentifierFromDB(string identifier)
        {
            var wait = this.colTestCases.Find(new BsonDocument { { Constants.TestCase_Identifier, identifier } }).FirstOrDefaultAsync();
            wait.Wait();

            if (wait.Result != null)
            {
                return wait.Result.ToTestCase();
            }
            else
            {
                return null;
            }
        }

        private CI.TestCase GetTestCaseByFullnameFromCache(string fullname)
        {
            if (this.cache_TestCasesMapping.ContainsKey(fullname))
            {
                var id = this.cache_TestCasesMapping[fullname];
                if (this.cache_TestCases.ContainsKey(id))
                {
                    return this.cache_TestCases[id];
                }
            }

            return null;
        }

        private void SaveTestCase(CI.TestCase testcase)
        {
            if (testcase != null)
            {
                var filter = Builders<BsonDocument>.Filter.Eq(Constants.TestCase_Identifier, testcase.Identifier);
                var update = Builders<BsonDocument>.Update
                    .Set(Constants.TestCase_Fullname, testcase.FullName)
                    .Set(Constants.TestCase_Owner, testcase.Owner != null ? testcase.Owner : string.Empty)
                    .Set(Constants.TestCase_Scope, !string.IsNullOrEmpty(testcase.Scope) ? testcase.Scope : testcase.FullName.Split(':')[0]);

                var wait = this.colTestCases.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = true });
                wait.Wait();
            }
        }

        #endregion
    }
}
