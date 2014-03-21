using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using $customNamespace$.Models.Common;
using System.Diagnostics;

namespace $customNamespace$.Models
{
    [DataContract]
    [Serializable]
    public abstract class baseModel : Object
    {
        public baseModel() 
        { 
        
        }

        private static Func<IDataReader, string, bool> readerColumnExists = delegate(IDataReader rdr, string columnName)
        {
            rdr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
            return (rdr.GetSchemaTable().DefaultView.Count > 0);
        };

        private static Func<IDataReader, string, bool> readerHasValue = delegate(IDataReader rdr, string columnName)
        {
            if (baseModel.readerColumnExists(rdr, columnName))
            {
                return !rdr.IsDBNull(rdr.GetOrdinal(columnName));
            }
            else
            {
                return false;
            }
        };

        public static Func<IDataReader, object, object> readerToObject = delegate(IDataReader rdr, object o)
        {
            PropertyInfo[] propertyInfos = baseModel.GetTypeProperties(o);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (baseModel.readerHasValue(rdr, propertyInfo.Name) == true)
                {
                    propertyInfo.SetValue(o, rdr[propertyInfo.Name], null);
                }
            }
            return o;
        };

        private static PropertyInfo[] GetTypeProperties(object o)
        {
            ///TODO: Cache this ?
            return o.GetType().GetProperties();

            //PropertyInfo[] propertyInfos;
            ////propertyInfos = o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static);
            //string key = string.Format("Cache_Type_Properties_{0}", o.ToString());
            //string CacheManagerName = "CacheManagerForBaseModel";
            //ICacheManager _objCacheManager = CacheFactory.GetCacheManager(CacheManagerName);
            //if (!_objCacheManager.Contains(key))
            //{
            //    _objCacheManager.Add(key, o.GetType().GetProperties());
            //}

            //propertyInfos = propertyInfos = (PropertyInfo[])_objCacheManager.GetData(key);
            //return propertyInfos;
        }

        public static MemberInfo GetInfo<P>(Expression<Func<P>> action)
        {
            return ((MemberExpression)action.Body).Member;
        }

        public static MethodInfo GetInfoMethod<P>(Expression<Func<P>> action)
        {
            return ((MethodCallExpression)action.Body).Method;
        }

        public static MethodBase GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            return sf.GetMethod();
        }

        public object DownCast(object source)
        {
            Type destinationType = this.GetType();
            PropertyInfo[] sourceProperties = baseModel.GetTypeProperties(source);
            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                PropertyInfo destinationProperty = destinationType.GetProperty(sourceProperty.Name);
                if (destinationProperty != null)
                {
                    destinationProperty.SetValue(this, sourceProperty.GetValue(source, null), null);
                }
            }
            return this;
        }


        public XmlDocument Serialize()
        {
            return baseModel.Serialize(this);
        }

        public static XmlDocument Serialize(object obj)
        {
            StringWriter writer = null;
            try
            {
                writer = new StringWriter(CultureInfo.InvariantCulture);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(writer.ToString());
                return xDoc;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        public static T Deserialize<T>(XPathNavigator xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(baseModel.StringToUTF8ByteArray(xml.OuterXml));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (T)xs.Deserialize(memoryStream);
        }

        public XmlDocument SerializeWithDataContract()
        {
            XmlDocument xdoc = new XmlDocument();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    DataContractSerializer serializerIIII = new DataContractSerializer(this.GetType());
                    serializerIIII.WriteObject(memoryStream, this);
                    memoryStream.Position = 0;
                    xdoc.LoadXml(reader.ReadToEnd());
                    reader.Close();
                }
            }

            return xdoc;
        }

        public string SerializeToJson()
        {
            return baseModel.SerializeObjectToJson(this);
        }

        public static string SerializeObjectToJson(object obj)
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new IsoDateTimeConverter());
            string result = JsonConvert.SerializeObject(obj);
            return result;
        }

        public static T DeserializeFromJson<T>(string jsonObj)
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new IsoDateTimeConverter());
            T result = JsonConvert.DeserializeObject<T>(jsonObj);
            return result;
        }

        private static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private static Byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    }
}