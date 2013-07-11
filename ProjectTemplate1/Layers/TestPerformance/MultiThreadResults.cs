using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Serialization;

namespace $safeprojectname$
{
    public static class ThreadList
    {
        private static readonly object syncObject = new object();

        private static List<ThreadResult> _threadsResultList = new List<ThreadResult>();
        public static List<ThreadResult> Results
        {
            get
            {
                return _threadsResultList;
            }
        }
        public static void Results_Add(ThreadResult threadResult)
        {
            lock (syncObject)
            {
                _threadsResultList.Add(threadResult);
            }
        }

        private static List<Thread> _threadsActive = new List<Thread>();
        public static List<Thread> ThreadsActive
        {
            get
            {
                return ThreadList._threadsActive;
            }
        }
        public static void ThreadsActive_Add(Thread t)
        {
            lock (syncObject)
            {
                _threadsActive.Add(t);
            }
        }
        public static void ThreadsActive_Remove(Thread t)
        {
            lock (syncObject)
            {
                _threadsActive.Remove(t);
            }
        }
    }
    public class ThreadResult
    {
        [XmlElement]
        public string ThreadName { get; set; }
        [XmlElement]
        public DateTime Starts { get; set; }
        [XmlElement]
        public DateTime Ends { get; set; }
        [XmlElement]
        public double TotalSeconds
        {
            get
            {
                return Ends.Subtract(Starts).TotalSeconds;
            }
            set
            {

            }
        }
        [XmlElement]
        public bool ResultOk { get; set; }
        [XmlIgnore]
        public Exception Exception { get; set; }
        [XmlElement("Exception")]
        public string ExpcetionMessage
        {
            get
            {
                if (this.Exception != null)
                {
                    string result = string.Empty;
                    result = string.Format("\n\r{0}\n\r {1}\n\r {2}\n\r", this.Exception.Message, this.Exception.Source, this.Exception.StackTrace);
                    if (this.Exception.InnerException != null)
                    {
                        result += string.Format("\n\r{0}\n\r {1}\n\r {2}\n\r", this.Exception.InnerException.Message, this.Exception.InnerException.Source, this.Exception.InnerException.StackTrace);
                    }
                    return result;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {

            }
        }
        [XmlElement]
        public string ResultDescription
        {
            get
            {
                return string.Format("Starts:{0} -- Ends:{1} -- Difference:{2}", Starts.ToString("HH:mm:ss"), Ends.ToString("HH:mm:ss"), Ends.Subtract(Starts).TotalSeconds);
            }
            set
            {

            }
        }
    }
    public class TestResultResume
    {
        public List<ThreadResult> Errors { get; set; }
        public List<ThreadResult> Success { get; set; }
        public double AverageExecutionInSeconds { get; set; }
        public override string ToString()
        {
            return string.Format("Errors: {0} // Success: {1} // Average: {2}", Errors.Count, Success.Count, AverageExecutionInSeconds);
        }
    }
}
