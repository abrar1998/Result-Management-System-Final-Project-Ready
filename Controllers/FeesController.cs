using Microsoft.AspNetCore.Mvc;
using RMS.FeeRepository;
using RMS.Models;

namespace RMS.Controllers
{
    public class FeesController : Controller
    {
        private readonly IFeeRepo feeRepo;

        public FeesController(IFeeRepo feeRepo)
        {
            this.feeRepo = feeRepo;
        }

        [HttpGet]
        public IActionResult DepositFee(int id)
        {
         
           ViewBag.StudentId = id;
           return View();
        }


        [HttpPost]
        public IActionResult DepositFee(FeeDTO dto)
        {
            var model = new FeeDetails
            {
                TotalFee = dto.TotalFee,
                Description = dto.Description,
                StudentId  = dto.StudentId,
            };
            feeRepo.AddFee(model);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult CheckFee()
        {

            var data = feeRepo.GetFeeList();
            return View(data);
        }

        [HttpGet]
        public IActionResult GetFeeListOfSignleStudents(int id)
        {

            bool IdExists = feeRepo.FeeIdOfStudentExists(id);
            if (IdExists == true)
            {
                var feedetail = feeRepo.GetFeeList();
                return View(feedetail);

            }
            return RedirectToAction("FeeNotFound", new { id = id });

        }

        [HttpGet]
        public IActionResult FeeNotFound(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpGet]
        /*      public IActionResult EditFee(int id)
              {
                  var feeToedit = feeRepo.GetFeeDetailsAsModelById(id);
                  var newDto = new FeeDTO()
                  {
                      StudentId = feeToedit.StudentId,
                      TotalFee = feeToedit.TotalFee,
                      Description = feeToedit.Description
                  };
                  return View(newDto); 
              }
      */

        public IActionResult EditFee(int id)
        {
            var feeToedit = feeRepo.GetFeeDetailsAsModelById(id);
          
            return View(feeToedit);
        }


        [HttpPost]
        public IActionResult EditFee(int id, FeeDetails fee)
        {
            feeRepo.FeeToUpdate(fee);
            return RedirectToAction("GetSingleStudent", "Student", new { id = id });
        }

        [HttpGet]
        public IActionResult DeleteFee(int id)
        {
            var deldata = feeRepo.GetFeeDetailsAsModelById(id);
            return View(deldata);
        }

        [HttpPost]
        public IActionResult DeleteFee(int id, FeeDetails feedelte)
        {
            var data = feeRepo.GetFeeDetailsAsModelById(id);
            feeRepo.FeeDelete(data);
            return RedirectToAction("GetSingleStudent", "Student", new {id = data.StudentId });
        }


    }
}
