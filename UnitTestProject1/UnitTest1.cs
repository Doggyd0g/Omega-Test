using Microsoft.VisualStudio.TestTools.UnitTesting;
using PP_Omega;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
       
        public void AuthorizationWithValidData()
        {
            //var loginerWin = new PP_Omega.LoginerWin();
            bool test_result = PP_Omega.Loginer.IsAuthorizated("Test", "1111");
            Assert.IsTrue(test_result);
        }

        [TestMethod]
        public void AuthorizationWithInvalidData()
        {
            //var loginerWin = new PP_Omega.LoginerWin();
            bool test_result = PP_Omega.Loginer.IsAuthorizated("", "");
            Assert.IsFalse(test_result);
        }
    }
}
