using System.ComponentModel.DataAnnotations;

namespace MoneyService.Models.Transactions
{
    public class RefillModel
    {
        public long accountNumber { get; set; }
        [Required]
        public decimal amount { get; set; }
    }
}