using RMS.Models;

namespace RMS.FeeRepository
{
    public interface IFeeRepo
    {
        void AddFee(FeeDetails fee);// Add Fee Details
        IEnumerable<FeeDetails> GetFeeList();
        IEnumerable<FeeDetails> GetFeeListByOfStudetId(int id);

        FeeDetails GetFeeDetailsOfSingleStudent(int id); // return data as model
        FeeDetails GetFeeDetailsAsModel();
        FeeDetails GetFeeDetailsAsModelById(int id); //this will return us FeeDetails data as a model
        public bool FeeIdOfStudentExists(int id); // this method will check whether Id of particular student exists in fee table or not

        void FeeToUpdate(FeeDetails ufee);

        void FeeDelete(FeeDetails deleteFee);
        
    }
}
