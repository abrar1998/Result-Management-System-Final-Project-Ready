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

        public void FeeDelete(FeeDetails deleteFee)
        {

            context.feeDetails.Remove(deleteFee);
            context.SaveChanges();
          /*  try
            {
                var data = context.feeDetails.Where(f=>f.Id == deleteFee.Id).FirstOrDefault();
                context.feeDetails.Remove(deleteFee);
                context.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var databaseValues = entry.GetDatabaseValues();
                if (databaseValues != null)
                {
                    // Update the entry's original values with the values from the database
                    entry.OriginalValues.SetValues(databaseValues);

                    // Try deleting again
                    context.SaveChanges();
                }
            }
*/
        }

        public bool FeeIdOfStudentExists(int id)
        {
            return context.feeDetails.Any(fee => fee.StudentId == id);
        }

        public void FeeToUpdate(FeeDetails ufee)
        {
           /* var dataToUpdate = new FeeDetails()
            {

                TotalFee = fee.TotalFee,
                StudentId = fee.StudentId,
                Description = fee.Description

            };*/
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


    }
}
