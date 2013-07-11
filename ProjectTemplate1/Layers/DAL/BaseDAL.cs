using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.Practices.EnterpriseLibrary.Data;
using $customNamespace$.Models;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.UserRequestModel;

namespace $safeprojectname$
{
    public abstract class BaseDAL: IDisposable
    {
        public BaseDAL()
        {

        }

        internal IUserRequestModel<OperationContext, MessageHeaders> UserRequest
        {
            get
            {
                return UserRequestHelper<OperationContext, MessageHeader>.CreateUserRequest() as IUserRequestModel<OperationContext, MessageHeaders>;
            }
        }

        public virtual void Dispose()
        {

        }

        internal int ExecuteReader<T>(Database db, DbTransaction dbTrans, DbCommand cmd, ref T obj) where T : new()
        {
            IDataReader rdr = null;
            try
            {
                if (dbTrans == null)
                {
                    rdr = db.ExecuteReader(cmd);
                }
                else
                {
                    rdr = db.ExecuteReader(cmd, dbTrans);
                }
                T result = default(T);
                if (rdr.Read())
                {
                    result = (T)baseModel.readerToObject(rdr, new T());
                }
                obj = result;

                rdr.Close();
                return rdr.RecordsAffected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (rdr != null) { rdr.Close(); rdr.Dispose(); }
            }
        }
        internal List<T> ExecuteReaderForList<T>(Database db, DbTransaction dbTrans, DbCommand cmd) where T : new()
        {
            IDataReader rdr = null;
            try
            {
                if (dbTrans == null)
                {
                    rdr = db.ExecuteReader(cmd);
                }
                else
                {
                    rdr = db.ExecuteReader(cmd, dbTrans);
                }
                List<T> listResult = new List<T>();
                if (rdr.Read())
                {
                    listResult.Add((T)baseModel.readerToObject(rdr, new T()));
                }
                return listResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (rdr != null) { rdr.Close(); rdr.Dispose(); }
            }
        }
        internal IDataResultPaginatedModel<T> ExecuteReaderForPagedResult<T>(IDataResultPaginatedModel<T> resultInstance, Database db, DbTransaction dbTrans, DbCommand cmd, Func<IDataReader,T> customConstructor)where T : new()
        {
            IDataReader rdr = null;
            try
            {
                if (dbTrans == null)
                {
                    rdr = db.ExecuteReader(cmd);
                }
                else
                {
                    rdr = db.ExecuteReader(cmd, dbTrans);
                }
                rdr.Read();

                resultInstance.TotalRows = Convert.ToInt32(rdr["TotalCount"]);
                resultInstance.TotalPages = Convert.ToInt32(rdr["TotalPages"]);

                rdr.NextResult();

                List<T> lItems = new List<T>();
                while (rdr.Read())
                {
                    lItems.Add(customConstructor(rdr));
                }

                resultInstance.Data = lItems;

                return resultInstance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (rdr != null) { rdr.Close(); rdr.Dispose(); }
            }
        }
        internal IDataResultPaginatedModel<T> ExecuteReaderForPagedResult<T>(IDataResultPaginatedModel<T> resultInstance, Database db, DbTransaction dbTrans, DbCommand cmd)where T : new()
        {
            Func<IDataReader, T> defaultContructor = (rdr) => ((T)baseModel.readerToObject(rdr, new T()));
            return this.ExecuteReaderForPagedResult<T>(resultInstance, db, dbTrans, cmd, defaultContructor);
        }
    }
}