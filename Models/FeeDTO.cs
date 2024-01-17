using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class FeeDTO
    {
      
        public string TotalFee { get; set; }

        public int StudentId { get; set; }
      
        public string Description { get; set; }
    }
}
