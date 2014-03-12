using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using $customNamespace$.Models;
using $customNamespace$.Models.Configuration;
using $customNamespace$.Models.Configuration.ConnectionProviders;
using $customNamespace$.Models.Logging;

namespace $customNamespace$.DAL.LoggingServices
{
    public class LoggingDAL : BaseDAL, ILoggingDAL
    {
        public LoggingDAL()
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public Guid LoggingExceptionSet(LogEntry logMessage)
        {
            Guid exceptionId = Guid.NewGuid();
            Dictionary<string, object> extendedProperties = new Dictionary<string, object>();
            extendedProperties.Add("Guid", exceptionId.ToString());
            LoggingHelper.Write(logMessage);
            return exceptionId;
        }
        public DataResultLogMessageList LoggingExceptionGetById(Guid guid)
        {
            Database db = DatabaseFactory.CreateDatabase(Info.GetDatabaseName(ApplicationConfiguration.DatabaseNames.Logging));
            DbCommand cmd = db.GetStoredProcCommand("LoggingExceptionGetById");
            db.AddInParameter(cmd, "@exceptionGuid", DbType.Guid, guid);
            XmlDocument xDoc = new XmlDocument();
            Func<IDataReader, LogMessageModel> customConstructor = (rdr) =>
            {
                xDoc.LoadXml((string)rdr["FormattedMessage"]);
                return new LogMessageModel(xDoc.DocumentElement.CreateNavigator());
            };

            List<LogMessageModel> list = this.ExecuteReaderForList<LogMessageModel>(db, null, cmd);

            return new DataResultLogMessageList()
            {
                Data = list,
                Page = 0,
                PageSize = Int16.MaxValue,
                TotalPages = 1,
                TotalRows = list.Count
            };
        }
        public DataResultLogMessageList LoggingExceptionGetAll(DataFilterLogger filter)
        {
            Database db = DatabaseFactory.CreateDatabase(Info.GetDatabaseName(ApplicationConfiguration.DatabaseNames.Logging));
            DbCommand cmd = db.GetStoredProcCommand("LoggingExceptionGetAll");
            db.AddInParameter(cmd, "@filter", DbType.Xml, baseModel.Serialize(filter).DocumentElement.OuterXml);
            XmlDocument xDoc = new XmlDocument();
            Func<IDataReader, LogMessageModel> customConstructor = (rdr) =>
            {
                xDoc.LoadXml((string)rdr["FormattedMessage"]);
                return new LogMessageModel(xDoc.DocumentElement.CreateNavigator());
            };

            DataResultLogMessageList pagedList = (DataResultLogMessageList)this.ExecuteReaderForPagedResult<LogMessageModel>(
                                                                        new DataResultLogMessageList()
                                                                        {
                                                                            PageSize = filter.PageSize,
                                                                            Page = filter.Page
                                                                        }, db, null, cmd, customConstructor);
            return pagedList;
        }
    }
}