using Microsoft.AspNetCore.Mvc;
using RMS.FeeReceiptRepository;
using RMS.FeeRepository;
using RMS.Models;

namespace RMS.Controllers
{
    public class FeesController : Controller
    {
        private readonly IFeeRepo feeRepo;
        private readonly IFeeReceiptService feeReceiptService;

        public FeesController(IFeeRepo feeRepo, IFeeReceiptService feeReceiptService)
        {
            this.feeRepo = feeRepo;
            this.feeReceiptService = feeReceiptService;
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
            // Generate and save the receipt, and get the file path
            var receiptFilePath = feeReceiptService.GenerateReceipt(model);
            // Return a downloadable file response
            return File(receiptFilePath, "application/pdf", "FeeReceipt.pdf");
            //return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult DownloadReceipt()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CheckFee()
        {

            var data = feeRepo.GetFeeList();
            return View(data);
        }

        [HttpGet]
        public IActionResult GetFeeListOfSingleStudent(int id)
        {

            bool IdExists = feeRepo.FeeIdOfStudentExists(id);
            if (IdExists == true)
            {
                var feedetail = feeRepo.GetFeeListByOfStudetId(id);
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
        public IActionResult EditFee(int id)
        {
            var feeToedit = feeRepo.GetFeeDetailsAsModelById(id);
            
            return View(feeToedit);
        }


        [HttpPost]
        public IActionResult EditFee(int id, FeeDetails fee)
        {
            feeRepo.FeeToUpdate(fee);
            return RedirectToAction("GetSingleStudent", "Student", new { id = fee.StudentId });
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
