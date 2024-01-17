using RMS.Models;

namespace RMS.FeeRepository
{
    public interface IFeeRepo
    {
        void AddFee(FeeDetails fee);// Add Fee Details
        IEnumerable<FeeDetails> GetFeeList();
        FeeDetails GetFeeDetailsOfSingleStudent(int id);
        FeeDetails GetFeeDetailsAsModel();
        FeeDetails GetFeeDetailsAsModelById(int id);
        public bool FeeIdOfStudentExists(int id);

        void FeeToUpdate(FeeDTO fee);
        
    }
}
