using RMS.Models;

namespace RMS.FeeReceiptRepository
{
    public interface IFeeReceiptService
    {
        byte[] GenerateReceipt(FeeDetails feeDetails);
    }
}
