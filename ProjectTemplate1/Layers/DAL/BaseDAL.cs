using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using $customNamespace$.Models;
using $customNamespace$.Models.Common;

namespace $customNamespace$.DAL
{
    public abstract class BaseDAL : IDisposable
    {
        public BaseDAL()
        {

        }

        public virtual void Dispose()
        {

        }

        internal int ExecuteReader<T>(Database db, DbTransaction dbTrans, DbCommand cmd, ref T obj) where T : IDataReaderBindable, new()
        {
            Func<IDataReader, T> predicate = delegate(IDataReader dr)
            {
                T instance = new T();
                instance.DataBind(dr);
                return instance;
            };

            return this.ExecuteReader<T>(db, dbTrans, cmd, ref obj, predicate);
        }
        internal int ExecuteReader<T>(Database db, DbTransaction dbTrans, DbCommand cmd, ref T obj, Func<IDataReader, T> customConstructor) where T : new()
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
                    result = customConstructor(rdr);
                }
                obj = result;

                rdr.Close();
                return rdr.RecordsAffected;
            }
            catch (Exception)
            {
                throw;
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
                var listResult = new List<T>();
                while (rdr.Read())
                {
                    listResult.Add((T)baseModel.readerToObject(rdr, new T()));
                }
                return listResult;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (rdr != null) { rdr.Close(); rdr.Dispose(); }
            }
        }
        internal List<T> ExecuteReaderForList<T>(Database db, DbTransaction dbTrans, DbCommand cmd, Func<IDataReader, T> castDelegate) where T : new()
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
                var listResult = new List<T>();
                while (rdr.Read())
                {
                    //listResult.Add((T)baseModel.readerToObject(rdr, new T()));
                    listResult.Add(castDelegate(rdr));
                }
                return listResult;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (rdr != null) { rdr.Close(); rdr.Dispose(); }
            }
        }
        internal IDataResultPaginatedModel<T> ExecuteReaderForPagedResult<T>(IDataResultPaginatedModel<T> resultInstance, Database db, DbTransaction dbTrans, DbCommand cmd, Func<IDataReader, T> customConstructor) where T : new()
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

                var lItems = new List<T>();
                while (rdr.Read())
                {
                    lItems.Add(customConstructor(rdr));
                }

                resultInstance.Data = lItems;

                return resultInstance;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (rdr != null) { rdr.Close(); rdr.Dispose(); }
            }
        }
        internal IDataResultPaginatedModel<T> ExecuteReaderForPagedResult<T>(IDataResultPaginatedModel<T> resultInstance, Database db, DbTransaction dbTrans, DbCommand cmd) where T : new()
        {
            Func<IDataReader, T> defaultContructor = (rdr) => ((T)baseModel.readerToObject(rdr, new T()));
            return this.ExecuteReaderForPagedResult<T>(resultInstance, db, dbTrans, cmd, defaultContructor);
        }

    }
}