using System;
using $customNamespace$.Models.Enumerations;

namespace $customNamespace$.Models.Configuration.ConnectionProviders
{
    public static class Info
    {
        public static string GetDatabaseName(ApplicationConfiguration.DatabaseNames connectionString)
        {
            string cnnProvider = connectionString.ToEnumMemberString();

            return cnnProvider;
        }
    }
}
