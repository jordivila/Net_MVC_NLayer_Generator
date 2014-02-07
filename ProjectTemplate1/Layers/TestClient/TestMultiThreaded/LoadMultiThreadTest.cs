using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.Models;
using $customNamespace$.Tests.Common.Controllers;
using $customNamespace$.UI.Web.Areas.UserAccount;
using $customNamespace$.UI.Web.Areas.UserAccount.Controllers;
using $customNamespace$.Tests.Client.Common;
using $customNamespace$.Tests.Common;

namespace $safeprojectname$.TestMultiThreaded
{
    [TestClass]
    public class LoadMultiThreadTest : TestControllerBase<UserAccountAreaRegistration>
    {
        static EventWaitHandle _mainThreadWaitHandle = new AutoResetEvent(false);
        static int NumTrheads = 10;
        static int NumIterationsPerThread = 1;


        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //    TestBase.MyClassInitialize(testContext);
        //}

        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //    TestBase.MyClassCleanup();
        //}

        //[TestInitialize()]
        //public override void MyTestInitialize()
        //{
        //    base.MyTestInitialize();
        //}

        //[TestCleanup()]
        //public void MyTestCleanup()
        //{

        //}

        [TestMethod]
        public void StressMultiThreadTest()
        {

            Func<int, Thread> createWorker = delegate(int counterThreads)
            {
                Thread t = new Thread(StressMultiThreadWorker);
                t.Name = string.Format("createWorker_{0}", counterThreads);
                ThreadList.ThreadsActive_Add(t);
                t.Start();
                return t;
            };

            DateTime starts = DateTime.Now;
            for (int i = 0; i < NumTrheads; i++) { createWorker(i); }
            //foreach (var item in threadsList) { item.Join(); }
            _mainThreadWaitHandle.WaitOne();
            DateTime ends = DateTime.Now;

            TestResultResume resume = new TestResultResume();
            resume.Errors = (from t in ThreadList.Results where t.Exception != null select t).ToList();
            resume.Success = (from t in ThreadList.Results where t.Exception == null select t).ToList();
            resume.AverageExecutionInSeconds = resume.Success.Count > 0 ? (from s in resume.Success select s.TotalSeconds + 0).Average() : 0;
            baseModel.Serialize(resume).Save(Path.Combine(this.TestContext.ResultsDirectory, string.Format("{0}.xml", this.TestContext.TestName)));
            if (resume.Errors.Count > 0) { Assert.IsTrue(false, resume.ToString()); }
            else
            {
                Console.WriteLine(string.Format("Concurrent Users:{4} , Iterations: {3}, Starts:{0} -- Ends:{1} -- Difference:{2}",
                                                                        starts.ToString("HH:mm:ss"),
                                                                        ends.ToString("HH:mm:ss"),
                                                                        ends.Subtract(starts).TotalSeconds, NumIterationsPerThread * NumTrheads,
                                                                        NumTrheads));
                Console.WriteLine(resume.ToString());
                Assert.IsTrue(true);
            }

        }

        private void StressMultiThreadWorker()
        {
            for (int i = 0; i < NumIterationsPerThread; i++)
            {
                this.StressMultiThreadAction();
            }
            ThreadList.ThreadsActive_Remove(Thread.CurrentThread);

            if ((ThreadList.ThreadsActive.Count == 0) && (ThreadList.Results.Count == (NumTrheads * NumIterationsPerThread)))
            {
                // All Threads are finished. So reanimate main thread
                _mainThreadWaitHandle.Set();
            }
        }

        private void StressMultiThreadAction()
        {
            try
            {
                TestBase.SetHttpContext();

                string UserNameValid = Guid.NewGuid().ToString();
                string UserEmailValid = string.Format("{0}@valid.com", UserNameValid);
                string UserPassword = "123456";
                Guid UserNameValidActivationToken = Guid.Empty;
                Guid CantAccessMyAccountToken = Guid.Empty;
                string UserNameValidUnActivated = Guid.NewGuid().ToString();
                string UserEmailValidUnActivated = string.Format("{0}@valid.com", UserNameValidUnActivated);

                DateTime starts = DateTime.Now;
                UserNameValid = Guid.NewGuid().ToString();
                UserEmailValid = string.Format("{0}@valid.com", UserNameValid);
                CommonTests.Register_Succeed(UserEmailValid, UserPassword, ref UserNameValidActivationToken);
                CommonTests.Register_ActivateAccount(UserNameValidActivationToken);
                CommonTests.CantAccessMyAccount_Succeed(UserEmailValid, ref CantAccessMyAccountToken);
                CommonTests.ResetPassword_Succeed(UserEmailValid, CantAccessMyAccountToken, UserPassword);
                CommonTests.Login_Succeed(UserEmailValid, UserPassword);
                DateTime ends = DateTime.Now;

                ThreadList.Results_Add(new ThreadResult()
                {
                    Starts = starts,
                    Ends = ends,
                    ResultOk = true,
                    ThreadName = Thread.CurrentThread.Name
                });
            }
            catch (Exception ex)
            {
                try
                {
                    ThreadList.Results_Add(new ThreadResult()
                    {
                        Exception = ex,
                        ResultOk = false,
                        ThreadName = Thread.CurrentThread.Name
                    });
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
