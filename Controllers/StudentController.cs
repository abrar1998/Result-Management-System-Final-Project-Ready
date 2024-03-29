﻿using Microsoft.AspNetCore.Mvc;
using RMS.AccountRepository;
using RMS.DatabaseContext;
using RMS.FeeRepository;
using RMS.Models;
using RMS.StudentCourseRepository;

namespace RMS.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepo repo;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICourseRepo courseRepo;
        private readonly IFeeRepo feeRepo;

        public StudentController(IStudentRepo repo, IWebHostEnvironment webHostEnvironment, ICourseRepo courseRepo, IFeeRepo feeRepo)
        {
            this.repo = repo;
            this.webHostEnvironment = webHostEnvironment;
            this.courseRepo = courseRepo;
            this.feeRepo = feeRepo;
        }

        [HttpGet]
        public IActionResult StudentRegistration()
        {
            ViewBag.data = courseRepo.GetCourseList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StudentRegistration(StudentViewModel svm)
        {
            if (ModelState.IsValid)
            {
                string FileLocation = UploadFile(svm);
             
                string DocumentLocation = UploadDocument(svm);

                var student = new Student()
                {
                    StudentName = svm.StudentName,
                    Parentage = svm.Parentage,
                    StudentAge = svm.StudentAge,
                    StudentEmail = svm.StudentEmail,
                    Adhaar = svm.Adhaar,
                    DOB = svm.DOB,
                    Course = svm.Course,
                    StudentPhone = svm.StudentPhone,
                    StudentPhoto = FileLocation,
                    AddressProof = DocumentLocation,
                    Address = svm.Address,
                    Gender = svm.Gender,
                };

                await repo.RegisterStudent(student);
                return RedirectToAction("Index", "Home");

            }
            return View();
        }



        //for photo
        private string UploadFile(StudentViewModel vm)
        {
            string filename = null;
            if (vm.StudentPhoto != null && vm.StudentPhoto.Length > 0)
            {
                string filedir = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                filename = Guid.NewGuid().ToString() + "-" + vm.StudentPhoto.FileName;
                string filepath = Path.Combine(filedir, filename);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    vm.StudentPhoto.CopyTo(filestream);
                }
            }
            return filename;
        }

        //for address photo
        private string UploadDocument(StudentViewModel vm)
        {
            string filename = null;
            if (vm.AddressProof != null && vm.AddressProof.Length < 1048576)
            {
                string filedir = Path.Combine(webHostEnvironment.WebRootPath, "AddressProof");
                filename = Guid.NewGuid().ToString() + "-" + vm.AddressProof.FileName;
                string filepath = Path.Combine(filedir, filename);
                using (var filestream = new FileStream(filepath, FileMode.Create))
                {
                    vm.AddressProof.CopyTo(filestream);
                }
            }
            return filename;
        }


        [HttpGet]
        public async Task<IActionResult> GetStudentsList()
        {
            var data = await repo.GetAllStudents();
            return View(data);
        }


       

        [HttpGet]
        public async Task<IActionResult> ShowStudents()
        {
            var data = await repo.GetStudentandCourseJoin();
            return View(data);
        }


        [HttpGet]
        public async Task<IActionResult> GetSingleStudent(int id)
        {
            


            //var currentDate = DateOnly.FromDateTime(DateTime.UtcNow.Date);
            DateTime currentDate = DateTime.Now;
           // DateTime curdt = System.Convert.ToDateTime(currentDate);
            // Assuming registrationDate is a DateOnly?
            var registrationDate = feeRepo.GetFeeRegistrationDate(id);
            //DateTime old = System.Convert.ToDateTime(registrationDate.Value.DayNumber);

            if (registrationDate != null)
            {
                //var daysDifference = DaysBetween(registrationDate.Value, currentDate);
                var daysDifference = currentDate.Day - registrationDate.Value.Day;
                Console.WriteLine("Date difference is " + daysDifference);
                // Now you can use the daysDifference as needed
                if (daysDifference > 30)
                {
                    // Enable the button or take other actions
                    ViewBag.Button = true;
                }
                else
                {
                    // Button remains disabled or take other actions
                    ViewBag.Button = false;
                }
            }
           
            //////////////////
            var data = await repo.GetStudentandCourseJoinById(id);
            return View(data);
        }

      






        //// CRUD OPERTAIONS ON SUDENTS

        [HttpGet]
       public async Task<IActionResult> Delete(int id)
       {
            var data = await repo.GetStudentCourseJoinModelById(id);
            return View(data);
        }



        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataTodelete = await repo.GetStudentCourseJoinModelById(id);
            if (dataTodelete != null)
            {
                //Retrieve file path
                var imagepath = Path.Combine(webHostEnvironment.WebRootPath, "Images", dataTodelete.StudentPhoto);
                var addressfile = Path.Combine(webHostEnvironment.WebRootPath, "AddressProof", dataTodelete.AddressProof);

                //delete student from database
                await repo.DeleteStudent(dataTodelete);

                //now delte deleted students photo in wwwroot folder
                if (System.IO.File.Exists(imagepath) && System.IO.File.Exists(addressfile))
                {
                    System.IO.File.Delete(imagepath);
                    System.IO.File.Delete(addressfile);
                }

                return RedirectToAction("ShowStudents", "Student");
            }
            return View();
        }


       

      [HttpGet]
      public async Task<IActionResult> Edit(int id)
      {
            //here we are passing course list in viewbag so that we can edit course of a student as well
            ViewBag.data =  courseRepo.GetCourseList();
            var data = await repo.GetStudentViewEdit(id); // here we are calling this method and because in repo we convert our student model to StudentViewEdit model and then it is returning StudentViewEdit model object which we are passing to the edit view
            return View(data);

      }


        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,StudentName,Parentage,StudentAge,StudentEmail,StudentPhone,Adhaar,DOB,,StudentPhoto, Course, Address, AddressProof, Gender")] StudentViewEdit student, IFormFile? newPhoto, IFormFile? newdocument)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            var oldstudent = await repo.GetStudentCourseJoinModelById(id);
            // oldstudent.StudentName = student.StudentName;


            if (ModelState.IsValid)
            {
                // Handle new photo upload if the user wants to change the photo
                if (newPhoto != null && newPhoto.Length < 1048576)
                {
                    // Handle the new photo upload here
                    // Save the new photo to the wwwroot/Images folder or any other location

                    var uniqueFileName = GetUniqueFileName(newPhoto.FileName);


                    var uploads = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    var filePath = Path.Combine(uploads, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await newPhoto.CopyToAsync(fileStream);
                    }

                    // Update the student's StudentPhoto property with the new file name or path
                    student.StudentPhoto = uniqueFileName;
                    // string oldPhotoPath = oldstudent.StudentPhoto;
                    string oldPhotoPath = Path.Combine(webHostEnvironment.WebRootPath, "Images", oldstudent.StudentPhoto);

                    // You might want to delete the old photo if it's no longer needed
                    // Note: Ensure you have proper error handling and file deletion logic
                    // For example, you might want to check if the old photo exists before deleting
                    // var oldPhotoPath = Path.Combine(uploads, student.StudentPhoto);
                    if (System.IO.File.Exists(oldPhotoPath))
                    {
                        System.IO.File.Delete(oldPhotoPath);
                    }
                }

                if(newdocument !=null && newdocument.Length < 1048576)
                {
                    var DocumentUniqueName = GetUniqueFileName(newdocument.FileName);
                    var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "AddressProof");
                    var DocumentFile = Path.Combine(uploadPath, DocumentUniqueName);

                    using(var filestream = new FileStream(DocumentFile, FileMode.Create))
                    {
                        await newdocument.CopyToAsync(filestream);
                    }

                    student.AddressProof = DocumentUniqueName;

                    if(!String.IsNullOrEmpty(student.AddressProof))
                    {
                        var oldDocument = Path.Combine(webHostEnvironment.WebRootPath, "AddressProof", oldstudent.AddressProof);
                        if(System.IO.File.Exists(oldDocument))
                        {
                            System.IO.File.Delete(oldDocument);
                        }
                    }

                }

                oldstudent.StudentName = student.StudentName;
                oldstudent.StudentId = student.StudentId;

                oldstudent.StudentPhone = student.StudentPhone;
                oldstudent.StudentEmail = student.StudentEmail;
                oldstudent.Course = oldstudent.Course;
                oldstudent.DOB = student.DOB;
                oldstudent.Adhaar = student.Adhaar;
                oldstudent.Course = student.Course;
                oldstudent.Gender = student.Gender;
                //oldstudent.StudentPhoto = student.StudentPhoto;
                if (newPhoto != null)
                {
                    oldstudent.StudentPhoto = student.StudentPhoto;
                }
                else
                {
                    oldstudent.StudentPhoto = oldstudent.StudentPhoto;
                }

                if(newdocument !=null)
                {
                    oldstudent.AddressProof = student.AddressProof;
                }
                else
                {
                    oldstudent.AddressProof = oldstudent.AddressProof;
                }
                

                //now update the student using repository in database
                await repo.UpdateStudent(oldstudent);

                return RedirectToAction("ShowStudents", "Student");
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"Model Error: {error.ErrorMessage}");
                    }
                }
            }

            return View();
        }








        //This function will add new Guid to the new Edited photos
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }


     // Student can download his course only

     [HttpGet]
     public async Task<IActionResult> DownloadCourse(int Id)
     {
         var student = await repo.GetStudentCourseJoinModelById(Id); // get student table with course and store it in it student variable

         if (student != null && student.CourseList != null && !string.IsNullOrEmpty(student.CourseList.CoursePdf))
         {
             var pdfPath = Path.Combine(webHostEnvironment.WebRootPath, "PdfFolder", student.CourseList.CoursePdf);

             if (System.IO.File.Exists(pdfPath))
             {
                 // Retrieve the PDF file content
                 var pdfBytes = System.IO.File.ReadAllBytes(pdfPath);

                 // Return the file using PhysicalFile
                 return PhysicalFile(pdfPath, "application/pdf", $"{student.CourseList.CourseName}_Course.pdf");
             }
         }

         // Handle if the student, course, or PDF is not found
         return NotFound();
     }

    }
}
