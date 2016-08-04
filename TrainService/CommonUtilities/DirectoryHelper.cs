#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;

#endregion


namespace CommonUtilities
{
    /// <summary>
    /// Utilities of Windows Activity Directory
    /// </summary>
    public static class DirectoryHelper
    {
        public static string GetFullName(string name)
        {
            var user = GetUser(name);
            if (user != null)
            {
                return user.GetDirectoryEntry().Name.Substring(3);
            }

            return name;
        }

        public static string GetEmailAddress(string name)
        {
            var user = GetUser(name);
            if (user != null)
            {
                var mails = user.Properties["mail"];
                if (mails.Count > 0)
                {
                    var mail = mails[0];
                    if (mail != null)
                    {
                        return mail.ToString();
                    }
                }
            }

            return name.Replace(' ', '.') + "@MotorolaSolutions.com";
        }

        private static SearchResult GetUser(string name)
        {
            var de = GetDirectoryEntry();
            var ds = new DirectorySearcher(de)
            {
                Filter = ("(&(objectClass=user)(motcoreid=" + name.ToLower() + "))")
            };
            var user = ds.FindOne();
            if (user == null)
            {
                ds = new DirectorySearcher(de)
                {
                    Filter = ("(&(objectClass=user)(displayname=" + name + "))")
                };
                user = ds.FindOne();
            }

            return user;
        }

        private static DirectoryEntry GetDirectoryEntry()
        {
            var domainName = Environment.UserDomainName;
            var de = new DirectoryEntry("LDAP://" + domainName);

            return de;
        }
    }
}
