using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using PracticalAssignment.Filters;
using PracticalAssignment.Models.BusinessEntities;
using PracticalAssignment.Models.DataEntities;

namespace PracticalAssignment.Controllers
{
    [BasicAuthentication]
    public class StudentController : Controller
    {
        /// <summary>
        /// Gets All recors of students
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AllStudents()
        {
            using (var _context = new DbUnitTestingAssignmentEntities())
            {
                List<tblStudent> DomainStudents = await _context.tblStudents.ToListAsync();

                //Mappging into DTO Model
                List<StudentVM> StudentsDTO = Mapper.Map<List<tblStudent>, List<StudentVM>>(DomainStudents);
                return View("AllStudents", StudentsDTO);
            }
        }

        public ActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> AddStudent([Bind(Include = "Id,Name,Age,Standard")]StudentVM studentVM)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    using (var _context = new DbUnitTestingAssignmentEntities())
                    {
                        //Mappging into Domain Model
                        tblStudent student = Mapper.Map<StudentVM, tblStudent>(studentVM);
                        _context.tblStudents.Add(student);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("AllStudents", "Student");
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Input not provided as per Requirement!");
                return View("AddStudent", studentVM);
            }
            return View("AddStudent", studentVM);
        }

        public async Task<ActionResult> UpdateStudent(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var _context = new DbUnitTestingAssignmentEntities())
            {
                tblStudent DomainStudent = await _context.tblStudents.FindAsync(id);

                //Mapping record into its DTO
                StudentVM StudentDTO = Mapper.Map<tblStudent, StudentVM>(DomainStudent);
                if (StudentDTO == null)
                {
                    return HttpNotFound();
                }
                return View(StudentDTO);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> UpdateStudent([Bind(Include = "Id,Name,Age,Standard")] StudentVM studentVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var _context = new DbUnitTestingAssignmentEntities())
                    {
                        tblStudent student = Mapper.Map<StudentVM, tblStudent>(studentVM);
                        _context.Entry(student).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("AllStudents", "Student");
                    }
                }
            }
            catch (Exception)
            {
                return View("UpdateStudent", studentVM);
            }
            return View("UpdateStudent", studentVM);
        }


        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var _context = new DbUnitTestingAssignmentEntities())
            {
                tblStudent DomainStudent = await _context.tblStudents.FindAsync(id);


                //Mappging into DTO Model
                StudentVM StudentDTO = Mapper.Map<tblStudent, StudentVM>(DomainStudent);
                if (StudentDTO == null)
                {
                    return HttpNotFound();
                }
                return View(StudentDTO);
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using (var _context = new DbUnitTestingAssignmentEntities())
                {
                    tblStudent DomainStudent = await _context.tblStudents.FindAsync(id);
                    _context.tblStudents.Remove(DomainStudent);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AllStudents", "Student");
                }
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }

    }
}