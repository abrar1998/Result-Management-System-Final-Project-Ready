using iText.Commons.Actions.Contexts;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cmp;
using RMS.DatabaseContext;
using RMS.Models;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

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

        public void FeeDelete(FeeDetails deleteFee)
        {

            context.feeDetails.Remove(deleteFee);
            context.SaveChanges();
         
        }

        public bool FeeIdOfStudentExists(int id)
        {
            return context.feeDetails.Any(fee => fee.StudentId == id);
        }

        public void FeeToUpdate(FeeDetails ufee)
        {
           
            context.feeDetails.Update(ufee);
            context.SaveChanges();
        }

        public FeeDetails GetFeeDetailsAsModel()
        {
            var feedetails = context.feeDetails.FirstOrDefault();
            return feedetails;
        }

        public FeeDetails GetFeeDetailsAsModelById(int id)
        {
            var editfee = context.feeDetails.Where(f=>f.Id == id).FirstOrDefault();
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

        public IEnumerable<FeeDetails> GetFeeListByOfStudetId(int id)
        {
            var data = context.feeDetails.Where(f=>f.StudentId == id).ToList();
            return data;
        }

        public DateOnly? GetFeeRegistrationDate(int id)
        {
            var feeDetails = context.feeDetails
                  .Where(f => f.StudentId == id)
                  .Select(f => f.RegistrationDate)
                  .FirstOrDefault();

            return feeDetails;
        }

        public FeeDetails GetRegistrationDate(int studentId)
        {
            return context.feeDetails
              .Where(f => f.StudentId == studentId)
              .FirstOrDefault();
        }

        /* DateOnly? IFeeRepo.GetFeeRegistrationDate(int studentId)
         {
             throw new NotImplementedException();
         }*/


      



}
}
