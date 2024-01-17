using Microsoft.EntityFrameworkCore;
using RMS.DatabaseContext;
using RMS.Models;

namespace RMS.FeeRepository
{
    public class FeeRepo : IFeeRepo
    {
        private readonly AccountContext context;

        public FeeRepo(AccountContext context)
        {
            this.context = context;
        }

        public void AddFee(FeeDetails fee)
        {
            context.feeDetails.Add(fee);
            context.SaveChanges();
        }

        public bool FeeIdOfStudentExists(int id)
        {
            return context.feeDetails.Any(fee => fee.StudentId == id);
        }

        public void FeeToUpdate(FeeDTO fee)
        {
            var dataToUpdate = new FeeDetails()
            {

                TotalFee = fee.TotalFee,
                StudentId = fee.StudentId,
                Description = fee.Description

            };
            context.feeDetails.Update(dataToUpdate);
            context.SaveChanges();
        }

        public FeeDetails GetFeeDetailsAsModel()
        {
            var feedetails = context.feeDetails.FirstOrDefault();
            return feedetails;
        }

        public FeeDetails GetFeeDetailsAsModelById(int id)
        {
            var editfee = context.feeDetails.Where(f=>f.StudentId == id).FirstOrDefault();
            return editfee;
        }

        public FeeDetails GetFeeDetailsOfSingleStudent(int id)
        {
            var feedetails = context.feeDetails.Where(f => f.StudentId == id).FirstOrDefault();
            return feedetails;
        }

        public IEnumerable<FeeDetails> GetFeeList()
        {
            var data = context.feeDetails.ToList();
            return data;
        }


    }
}
