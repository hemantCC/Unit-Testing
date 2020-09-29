using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalAssignment.Controllers;
using PracticalAssignment.Models.BusinessEntities;

namespace PracticalAssignment.Tests.Controllers
{
    [TestClass]
    public class AccountTest
    {
        //Positive Testing
        [TestMethod]
        public void Login_IsValidUser_ReturnsAction()
        {
            //Arrange
            UserVM Input = new UserVM() { Username = "hemant2@gmail.com", Password = "1234" };
            AccountController account = new AccountController();
            string ExpectedAction = "AllStudents";
            string ExpectedController = "Student";

            //Act
            var Result = account.Login(Input) as RedirectToRouteResult;
            Result.RouteValues["action"].Equals("AllStudents");
            Result.RouteValues["controller"].Equals("Student");

            //Assert
            Assert.AreEqual(ExpectedAction, Result.RouteValues["action"]);
            Assert.AreEqual(ExpectedController, Result.RouteValues["controller"]);

        }


        //Negative Testing
        [TestMethod]
        public void Login_IsInvalidUser_ReturnsAction()
        {
            //Arrange
            UserVM Input = new UserVM() { Username = "Invalid@gmail.com", Password = "1234" }; //Incorrect Output
            AccountController account = new AccountController();
            string ExpectedViewName = "Login";

            //Act
            var Result = account.Login(Input) as ViewResult;

            //Assert
            Assert.AreEqual(ExpectedViewName, Result.ViewName);

        }

    }
}
