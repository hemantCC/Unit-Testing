using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracticalAssignment.Controllers;
using PracticalAssignment.MappingProfiles;
using PracticalAssignment.Models.BusinessEntities;

namespace PracticalAssignment.Tests.Controllers
{
    [TestClass]
    public class StudentTest
    {
        public StudentTest()
        {
            //Initialise Mapper Profile
            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfiles>());
            
        }


        [TestMethod]
        public async Task AllStudents_ListsStudents_ReturnsViewResult()
        {
            //Arrange
            StudentController studentController = new StudentController();
            string ExpectedView = "AllStudents";

            //Act
            var ResultStudents = await studentController.AllStudents() as ViewResult;

            //Assert
            Assert.IsInstanceOfType(ResultStudents, typeof(ViewResult));
            Assert.AreEqual(ExpectedView, ResultStudents.ViewName);

        }

        //Checks if we get values 
        [TestMethod]
        public async Task AllStudents_ListsStudents_ReturnsViewResultValue()
        {
            //Arrange
            StudentController studentController = new StudentController();

            //Act
            var Result = await studentController.AllStudents() as ViewResult;
            List<StudentVM> students = (List<StudentVM>)Result.Model;

            //Assert
            Assert.IsNotNull(students);

        }

        //Positive Testing for Add Operation
        [TestMethod]
        public  async Task AddStudent_CorrectInput_ReturnsViewResult()
        {
            //Arrange
            StudentController studentController = new StudentController();
            StudentVM InputStudent = new StudentVM() { Name = "Hemant", Age = 23, Standard = "8" }; //Correct Input
            string ExpectedAction = "AllStudents";
            string ExpectedController = "Student";

            //Act
            var Result =  await studentController.AddStudent(InputStudent) as RedirectToRouteResult;
            Result.RouteValues["action"].Equals("AllStudents");
            Result.RouteValues["controller"].Equals("Student");

            //Assert
            Assert.AreEqual(ExpectedAction, Result.RouteValues["action"]);
            Assert.AreEqual(ExpectedController, Result.RouteValues["controller"]);

        }


        //Negative Testing for Add Operation
        [TestMethod]
        public async Task AddStudent_IncorrectInput_ReturnsViewResult()
        {
            //Arrange
            StudentController studentController = new StudentController();
            StudentVM InputStudent = new StudentVM() { Name = "Hemant", Age = 23 }; //Incorrect Input
            string ExpectedViewName = "AddStudent";

            //Act
            var Result = await studentController.AddStudent(InputStudent) as ViewResult;

            //Assert
            Assert.AreEqual(ExpectedViewName, Result.ViewName);

        }

        //Positive Testing for Update Operation
        [TestMethod]
        public async Task UpdateStudent_CorrectInput_ReturnsViewResult()
        {
            //Arrange
            StudentController studentController = new StudentController();
            StudentVM InputStudent = new StudentVM() { Id = 1, Name = "Hemant", Age = 20, Standard = "11" }; //Correct Input
            string ExpectedAction = "AllStudents";
            string ExpectedController = "Student";

            //Act
            var Result = await studentController.UpdateStudent(InputStudent) as RedirectToRouteResult;
            Result.RouteValues["action"].Equals("AllStudents");
            Result.RouteValues["controller"].Equals("Student");

            //Assert
            Assert.AreEqual(ExpectedAction, Result.RouteValues["action"]);
            Assert.AreEqual(ExpectedController, Result.RouteValues["controller"]);

        }

        //Negative Testing for Update Operation
        [TestMethod]
        public async Task UpdateStudent_IncorrectInput_ReturnsViewResult()
        {
            //Arrange
            StudentController studentController = new StudentController();
            StudentVM InputStudent = new StudentVM() { Name = "Hemant", Age = 20, Standard="11" }; //Incorrect Input
            string ExpectedViewName = "UpdateStudent";

            //Act
            var Result = await studentController.UpdateStudent(InputStudent) as ViewResult;

            //Assert
            Assert.AreEqual(ExpectedViewName, Result.ViewName);

        }

        //Positive Testing for Delete Operation
        [TestMethod]
        public async Task DeleteConfirmed_CorrectInput_ReturnsViewResult()
        {
            //Arrange
            StudentController studentController = new StudentController();
            int InputId = 14; //Correct Input
            string ExpectedAction = "AllStudents";
            string ExpectedController = "Student";

            //Act
            var Result = await studentController.DeleteConfirmed(InputId) as RedirectToRouteResult;
            Result.RouteValues["action"].Equals("AllStudents");
            Result.RouteValues["controller"].Equals("Student");

            //Assert
            Assert.AreEqual(ExpectedAction, Result.RouteValues["action"]);
            Assert.AreEqual(ExpectedController, Result.RouteValues["controller"]);

        }


        //Negative Testing for Delete Operation
        [TestMethod]
        public async Task DeleteConfirmed_IncorrectInput_ReturnsViewResult()
        {
            //Arrange
            StudentController studentController = new StudentController();
            int InputId = 100;      //Incorrect Input
            int ExpectedStatusCode = 404;

            //Act
            var Result = await studentController.DeleteConfirmed(InputId) as HttpNotFoundResult;

            //Assert
            Assert.AreEqual(ExpectedStatusCode, Result.StatusCode);

        }




    }
}
